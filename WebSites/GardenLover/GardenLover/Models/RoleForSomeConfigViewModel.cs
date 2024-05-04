using GardenLover.EF;
using System.Collections.Generic;

namespace GardenLover.Models
{
    public class RoleForSome_ConfigVM
    {
        public aRoleForAll RoleForAll { get; set; }

        public List<List<RankAndChecked>> allRankAndChecked { get; set; }
        public List<List<DepartmentAndChecked>> allDepartmentAndChecked { get; set; }
        public List<List<BranchAndChecked>> allBranchAndChecked { get; set; }
        public List<List<CompanyAndChecked>> allCompanyAndChecked { get; set; }
        public List<CorporationAndChecked> allCorporationAndChecked { get; set; }
    }
}