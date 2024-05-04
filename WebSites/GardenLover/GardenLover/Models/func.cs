using _IQwinwin;
using GardenLover.EF;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace GardenLover.Models
{
    public class func
    {
        public static bool CheckRole(cUserInfo userInfo, kReport report, string roleType)
        {
            GoodJobEntities dbGoodJob = new GoodJobEntities();
            CompanyEntities dbCompany = new CompanyEntities();

            bool result = false;

            cUserInfo reporter = null;

            if (report != null)
                reporter = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId == report.UserId);
            else
                reporter = userInfo;

            if (roleType == "mark")
            {
                if (userInfo.UserId == reporter.ManagerId)
                {
                    if (report.SendToRelationValue.ToLower() == userInfo.UserId.ToString().ToLower())
                    {
                        result = true;
                    }
                }
            }
            else if (roleType == "confirm")
            {
                var allRoleForSomeId = dbCompany.aRoleForSomeAllows.Where(x => x.RoleForAllId == 3 && x.AllowRelationTypeId == 6 && x.AllowRelationValue == reporter.RankId.ToString()).Select(x => x.RoleForSomeId);

                var role = dbCompany.aRoles.FirstOrDefault(x => allRoleForSomeId.Any(y => y == x.RoleForSomeId) && x.RoleRelationValue == userInfo.UserId.ToString().ToLower());

                if (role != null)
                {
                    if (report.SendToRelationValue.ToLower() == userInfo.UserId.ToString().ToLower())
                    {
                        result = true;
                    }
                }
            }
            else if (roleType == "bod")
            {
                if (userInfo.cRank.RankForAllId == 2)
                {
                    result = true;
                }
            }

            return result;
        }

        public static string ReadConfig(string configName)
        {
            CompanyEntities dbCompany = new CompanyEntities();
            return dbCompany.sConfigs.FirstOrDefault(x => x.ConfigName == configName).Value;
        }
        public static void SendEmail(string title, string content, string emailTo, string replyTo)
        {
            try
            {
                CompanyEntities dbCompany = new CompanyEntities();

                var email = ReadConfig("KpiEmail");
                var password = ReadConfig("KpiEmailPassword");

                var smtpServer = ReadConfig("KpiEmailSmtp");
                var smtpPort = ReadConfig("KpiEmailPort");
                var ssl = ReadConfig("KpiEmailSsl") == "1" ? true : false;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;

                message.From = new MailAddress(email, "KPI online");
                message.ReplyTo = new MailAddress(replyTo);
                message.To.Add(new MailAddress(emailTo));

                message.Subject = title;
                message.Body = content;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = System.Convert.ToInt32(smtpPort);
                smtp.Host = smtpServer;
                smtp.EnableSsl = ssl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                sys.Write("D:\\Error\\Error_GardenLover.log", ex);
            }
        }
    }
}