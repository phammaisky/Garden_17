using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class PerformanceInfo
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public byte? Month { get; set; }
        public string MonthStr { get; set; }
        public int? Type { get; set; }
        public int? ProjectId { get; set; }
        public int? TypePerformanceId { get; set; }
        public int? QuarterId { get; set; }
        public byte? OfficeArea { get; set; }
        public byte? OfficeMonthMoney { get; set; }
        public byte? OfficeTY { get; set; }
        public byte? OfficeLY { get; set; }
        public byte? RetailArea { get; set; }
        public byte? RetailMonthMoney { get; set; }
        public byte? RetailTY { get; set; }
        public byte? RetailLY { get; set; }
        public byte? NewArea { get; set; }
        public byte? NewMonthMoney { get; set; }
        public byte? NewTotalRev { get; set; }
        public byte? TotalMonthMoney { get; set; }
        public byte? TotalRevTY { get; set; }
        public byte? TotalRevLY { get; set; }
        public byte? TotalGrossTY { get; set; }
        public byte? TotalGrossLY { get; set; }
        public int? TypeFB { get; set; }
        public byte? SalesLineToLine { get; set; }
        public byte? SalesAll { get; set; }
        public byte? SalesCashFlowTY { get; set; }
        public byte? SalesCashFlowLY { get; set; }
        public byte? RevLineTOSNoMG { get; set; }
        public byte? RevLineTOSWithMG { get; set; }
        public byte? RevLineNoMG { get; set; }
        public byte? RevLineWithMG { get; set; }
        public byte? RevAllTOSNoMG { get; set; }
        public byte? RevAllTOSWithMG { get; set; }
        public byte? RevAllNoMG { get; set; }
        public byte? RevAllWithMG { get; set; }
        public byte? RevAllLY { get; set; }
        public byte? RevAllOPMonthMoney { get; set; }
        public byte? RevTotalNoMG { get; set; }
        public byte? RevTotalWithMG { get; set; }
        public byte? RevTotalLY { get; set; }
        public byte? RevTotalMonthMoney { get; set; }
        public byte? ComProfitTY { get; set; }
        public byte? ComProfitLY { get; set; }
        public byte? RevLSArea { get; set; }
        public byte? RevLSMonthMoney { get; set; }
        public byte? RevLSRev { get; set; }
        public byte? RevLSOPMonthMoney { get; set; }
        public byte? BusinessProfit { get; set; }
        public byte? RevAllOccRate { get; set; }
        public byte? RevNormalLine { get; set; }
        public byte? RevNormalAll { get; set; }
        public byte? RevAllRev { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public byte? DeleteFlg { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public int? StatusId { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public string ApprovedName { get; set; }
        [NotMapped]
        public string StatusName
        {
            get
            {
                string statusName = "";
                if (StatusId == 0)
                {
                    statusName = "Chưa duyệt";
                }
                else if (StatusId == 1)
                {
                    statusName = "Đã duyệt";
                }
                else if (StatusId == 2)
                {
                    statusName = "Từ chối";
                }

                return statusName;
            }
            set { StatusName = value; }
        }
        [NotMapped]
        public bool CheckExistsPerformance { get; set; }
        [NotMapped]
        public int ShowFormByProjType { get; set; }
        [NotMapped]
        public string ProjectName { get; set; }
        [NotMapped]
        public string TypePerformanceName { get; set; }
    }
}
