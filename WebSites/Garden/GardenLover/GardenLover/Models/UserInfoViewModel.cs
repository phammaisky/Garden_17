using IQWebApp_Blank.EF;
using System;

namespace IQWebApp_Blank.Models
{
    public class UserInfoVM
    {
        public Guid UserId { get; set; }
        public bool IsNew { get; set; }

        public cUserInfo UserInfo { get; set; }
        public vLoginUser LoginUser { get; set; }
    }

    public class DepartmentVM
    {
        public long Id { get; set; }
        public string DepartmentName { get; set; }
    }

    public class RankVM
    {
        public long Id { get; set; }
        public string RankName { get; set; }
    }

    public class GradeVM
    {
        public long Id { get; set; }
        public string GradeName { get; set; }
    }

    public class ManagerVM
    {
        public Guid UserId { get; set; }
        public string FullNameAndRank { get; set; }
    }
}