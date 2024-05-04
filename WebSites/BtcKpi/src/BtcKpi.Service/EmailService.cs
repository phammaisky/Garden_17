using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using BtcKpi.Model.Enum;
using BtcKpi.Service.Common;

namespace BtcKpi.Service
{
    public interface IEmailService
    {
        string SendEmail(EmailModel email);
        string SendMailCreateIpf(Ipf ipf);
        string SendMailApproveIpf(Ipf ipf);
        string SendMailCreateUpf(Upf upf);
        string SendMailApproveUpf(Upf upf);
    }
    public class EmailService : IEmailService
    {
        private readonly ISysConfigRepository sysConfigRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public EmailService(ISysConfigRepository sysConfigRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.sysConfigRepository = sysConfigRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }
        public string SendEmail(EmailModel email)
        {
            string errorMessage = "";
            var emailConfigs = sysConfigRepository.GetByType(BtcConst.EmailConfigType);
            if(emailConfigs == null)
                emailConfigs = new List<SysConfig>();
            var fromNameObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailFromName);
            var fromEmailObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailFromEmail);
            var passwordObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailPassword);
            var mailServerObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailMailServer);
            var portObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailPort);
            var sslObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.EmailSSL);

            var message = new MailMessage();

            message.From = new MailAddress((fromNameObj == null ? "" : fromNameObj.Values) + " <" + (fromEmailObj == null ? "" : fromEmailObj.Values) + ">");
            message.To.Add(new MailAddress(email.ToName + " <" + email.ToEmail + ">"));
            if (!string.IsNullOrEmpty(email.CC))
            {
                if (email.CC.Contains(";"))
                {
                    foreach (var cc in email.CC.Split(';').ToList())
                    {
                        message.CC.Add(cc);
                    }
                }
            }
            if (!string.IsNullOrEmpty(email.BCC))
            {
                if (email.BCC.Contains(";"))
                {
                    foreach (var bcc in email.BCC.Split(';').ToList())
                    {
                        message.Bcc.Add(bcc);
                    }
                }
            }

            message.Subject = email.Subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = email.Message;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                smtp.Host = mailServerObj == null ? "" : mailServerObj.Values;
                smtp.Port = portObj == null ? 587 : Int32.Parse(portObj.Values);
                smtp.Credentials = new System.Net.NetworkCredential(fromEmailObj == null ? "" : fromEmailObj.Values, passwordObj == null ? "" : passwordObj.Values);
                smtp.EnableSsl = (sslObj != null && sslObj.Values != "0");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    errorMessage = ex.ToString() + " Message: " + ex.Message;
                }
            }

            return errorMessage;
        }

        public string SendMailCreateIpf(Ipf ipf)
        {
            string errorMessage = "";
            EmailModel emailModel = new EmailModel();
            User createUser = userRepository.GetById((int) ipf.CreatedBy);
            User approveUser = userRepository.GetById((int)ipf.ApproveBy);
            var emailConfigs = sysConfigRepository.GetByType(BtcConst.KpiMailType);
            if (emailConfigs == null)
                emailConfigs = new List<SysConfig>();
            var mailCreateObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailCreate);
            var mailCreateToCreaterObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailCreateToCreater);

            //Gửi email cho người duyệt
            if (approveUser != null && !string.IsNullOrEmpty(approveUser.Email))
            {
                emailModel.ToEmail = approveUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateObj == null) ? "" : mailCreateObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", approveUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(ipf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)ipf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", ipf.ScheduleName);
                }

                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            //Gửi email cho người tạo
            if (createUser != null && !string.IsNullOrEmpty(createUser.Email))
            {
                emailModel.ToEmail = createUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateToCreaterObj == null) ? "" : mailCreateToCreaterObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", createUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Tên người duyệt
                    if (approveUser != null) body = body.Replace("[ApproveUser]", approveUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(ipf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)ipf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", ipf.ScheduleName);
                }
                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            return errorMessage;
        }

        public string SendMailApproveIpf(Ipf ipf)
        {
            string errorMessage = "";
            EmailModel emailModel = new EmailModel();
            User createUser = userRepository.GetById((int)ipf.CreatedBy);
            User approveUser = userRepository.GetById((int)ipf.ApproveBy);
            var emailConfigs = sysConfigRepository.GetByType(BtcConst.KpiMailType);
            if (emailConfigs == null)
                emailConfigs = new List<SysConfig>();
            var mailCreateToCreaterObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailApprove);

            //Gửi email cho người tạo
            if (createUser != null && !string.IsNullOrEmpty(createUser.Email))
            {
                emailModel.ToEmail = createUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateToCreaterObj == null) ? "" : mailCreateToCreaterObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", createUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Tên người duyệt
                    if (approveUser != null) body = body.Replace("[ApproveUser]", approveUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(ipf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)ipf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", ipf.ScheduleName);
                }
                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            return errorMessage;
        }

        public string SendMailCreateUpf(Upf upf)
        {
            string errorMessage = "";
            EmailModel emailModel = new EmailModel();
            User createUser = userRepository.GetById((int)upf.CreatedBy);
            User approveUser = userRepository.GetById((int)upf.ApproveBy);
            var emailConfigs = sysConfigRepository.GetByType(BtcConst.KpiMailType);
            if (emailConfigs == null)
                emailConfigs = new List<SysConfig>();
            var mailCreateObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailCreate);
            var mailCreateToCreaterObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailCreateToCreater);

            //Gửi email cho người duyệt
            if (approveUser != null && !string.IsNullOrEmpty(approveUser.Email))
            {
                emailModel.ToEmail = approveUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateObj == null) ? "" : mailCreateObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", approveUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(upf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)upf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", upf.ScheduleName);
                }

                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            //Gửi email cho người tạo
            if (createUser != null && !string.IsNullOrEmpty(createUser.Email))
            {
                emailModel.ToEmail = createUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateToCreaterObj == null) ? "" : mailCreateToCreaterObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", createUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Tên người duyệt
                    if (approveUser != null) body = body.Replace("[ApproveUser]", approveUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(upf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)upf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", upf.ScheduleName);
                }
                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            return errorMessage;
        }

        public string SendMailApproveUpf(Upf upf)
        {
            string errorMessage = "";
            EmailModel emailModel = new EmailModel();
            User createUser = userRepository.GetById((int)upf.CreatedBy);
            User approveUser = userRepository.GetById((int)upf.ApproveBy);
            var emailConfigs = sysConfigRepository.GetByType(BtcConst.KpiMailType);
            if (emailConfigs == null)
                emailConfigs = new List<SysConfig>();
            var mailCreateToCreaterObj = emailConfigs.FirstOrDefault(p => p.Code == BtcConst.KpiEmailApprove);

            //Gửi email cho người tạo
            if (createUser != null && !string.IsNullOrEmpty(createUser.Email))
            {
                emailModel.ToEmail = createUser.Email;
                emailModel.Subject = "KPIs - " + createUser.FullName;
                string body = (mailCreateToCreaterObj == null) ? "" : mailCreateToCreaterObj.Values;
                if (!string.IsNullOrEmpty(body))
                {
                    //Tên người nhận mail
                    body = body.Replace("[FullName]", createUser.FullName);

                    //Tên người tạo
                    body = body.Replace("[FromUser]", createUser.FullName);

                    //Tên người duyệt
                    if (approveUser != null) body = body.Replace("[ApproveUser]", approveUser.FullName);

                    //Loại
                    body = body.Replace("[ScheduleType]", BtcHelper.ConvertIpfScheduleType(upf.ScheduleType));

                    //Năm
                    body = body.Replace("[Year]", ((int)upf.Year).ToString());

                    //Kỳ
                    body = body.Replace("[ScheduleName]", upf.ScheduleName);
                }
                emailModel.Message = body;
                errorMessage += SendEmail(emailModel);
            }

            return errorMessage;
        }

        private string createEmailBody(string userName, string message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/htmlTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{message}", message);
            return body;
        }
    }
}
