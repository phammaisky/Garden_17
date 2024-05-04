using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model.Enum
{
    public static class BtcConst
    {
        public static string UpfCrossSpecialUsersCode = "upf_special_users";
        public static string EmailConfigType = "email_config";
        public static string EmailFromName = "email_from_name";
        public static string EmailFromEmail = "email_from_email";
        public static string EmailPassword = "email_password";
        public static string EmailMailServer = "email_server";
        public static string EmailPort = "email_port";
        public static string EmailSSL = "email_ssl";
        public static string KpiMailType = "kpis_emails";
        public static string KpiEmailCreate = "email_create";
        public static string KpiEmailCreateToCreater = "email_create_tocreater";
        public static string KpiEmailApprove = "email_approve";
        public static string IpfScheduleTypeYear = "Năm";
        public static string IpfScheduleTypeCyclical = "Định kỳ";
    }
    public enum BtcKpiActiveFlag
    {
        IsActive = 1,
        NotActive = 0
    }

    public enum BtcKpiDeleteFlag
    {
        Deleted = 1,
        NotDelete = 0
    }

    public enum IpfScheduleType
    {
        Year = 0,       // Năm
        Cyclical = 1    // Định kỳ
    }

    public enum QuarterType
    {
        Quarter1 = 1,       // Quý 1
        Quarter2 = 2,       // Quý 2
        Quarter3 = 3,       // Quý 3
        Quarter4 = 4,       // Quý 4
    }

    public enum IpfWorkType
    {
        CompleteWork = 0,   // Hoàn thành công việc
        Competency = 1      // Năng lực
    }

    public enum KpiStatus
    {
        Draft = 0,    // Nháp
        Complete = 1,      // Hoàn thành
        Approved = 2      // Đã duyệt
    }

    public enum DepartmentStatus
    {
        DraftApproved = 0,  // Luu nhap
        NotApproved = 1,  // Chua duyet
        Approved = 2,      // QL Da duyet
        BODApproved = 3,      // BOD Da duyet
        Refuse = 4      // Tu choi
    }

    public enum UpfCrossStatus
    {
        New = 0,       // Chưa phản hồi
        Responded = 1    // Đã phản hồi
    }

    public enum PerformanceTypeFB
    {
        FB = 0,       // Chưa phản hồi
        NonFB = 1    // Đã phản hồi
    }
}
