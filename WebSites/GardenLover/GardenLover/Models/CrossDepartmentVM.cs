using GardenLover.EF;
using System.Collections.Generic;

namespace GardenLover.Models
{
    public class CrossDepartmentVM
    {
        public ckReport Report { get; set; }
        public List<ckJobVM> allJobVM { get; set; }
    }

    public class ckJobVM
    {
        public ckJob Job { get; set; }
        public List<ckJobDetail> allJobDetail { get; set; }
    }
}