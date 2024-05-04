using IQWebApp_Blank.EF;
using System;
using System.Collections.Generic;

namespace IQWebApp_Blank.Models
{
    public class UserRole_ConfigVM
    {
        public aRoleForAll RoleForAll { get; set; }

        public List<RankAndChecked> allRankAndChecked { get; set; }
        public List<DepartmentAndChecked> allDepartmentAndChecked { get; set; }
        public List<BranchAndChecked> allBranchAndChecked { get; set; }
        public List<CompanyAndChecked> allCompanyAndChecked { get; set; }
        public List<CorporationAndChecked> allCorporationAndChecked { get; set; }
    }

    public class RankAndChecked
    {
        public cCorporation Corporation { get; set; }
        public cCompany Company { get; set; }
        public cBranch Branch { get; set; }
        public cRank Rank { get; set; }
        public bool Checked { get; set; }
    }
    public class DepartmentAndChecked
    {
        public cCorporation Corporation { get; set; }
        public cCompany Company { get; set; }
        public cBranch Branch { get; set; }
        public cDepartment Department { get; set; }
        public bool Checked { get; set; }
    }
    public class BranchAndChecked
    {
        public cCorporation Corporation { get; set; }
        public cCompany Company { get; set; }
        public cBranch Branch { get; set; }
        public bool Checked { get; set; }
    }
    public class CompanyAndChecked
    {
        public cCorporation Corporation { get; set; }
        public cCompany Company { get; set; }
        public bool Checked { get; set; }
    }
    public class CorporationAndChecked
    {
        public cCorporation Corporation { get; set; }
        public bool Checked { get; set; }
    }
}