using IQWebApp_Blank.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace IQWebApp_Blank.Models
{
    public class func
    {
        public static bool CheckRole(cUserInfo userInfo, kReport report, string roleType)
        {
            GoodJobEntities dbJob = new GoodJobEntities();
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

        public static void SendEmail(string title, string content, string emailTo, string replyTo)
        {
            try
            {
                CompanyEntities dbCompany = new CompanyEntities();

                var email = dbCompany.sConfigs.FirstOrDefault(x => x.ConfigName == "KpiEmail").Value;
                var password = dbCompany.sConfigs.FirstOrDefault(x => x.ConfigName == "KpiPassword").Value;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;

                message.From = new MailAddress(email, "KPI online");
                message.ReplyTo = new MailAddress(replyTo);
                message.To.Add(new MailAddress(emailTo));

                message.Subject = title;
                message.Body = content;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 465;
                smtp.Host = "mail.bitexcojsc.com.vn";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }

    }
}