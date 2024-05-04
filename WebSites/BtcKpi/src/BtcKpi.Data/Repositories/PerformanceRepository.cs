using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class PerformanceRepository : RepositoryBase<PerformanceLSFB>, IPerformanceRepository
    {
        public PerformanceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public bool CheckCreatePerformance(PerformanceLSFB performanceLsfb, int userId)
        {
            List<PerformanceLSFB> items = new List<PerformanceLSFB>();
            if (performanceLsfb.Type == 0)
            {
                items = this.DbContext.PerformanceLsfbs.Where(t => t.ProjectId == performanceLsfb.ProjectId && t.TypePerformanceId == performanceLsfb.TypePerformanceId && t.Type == performanceLsfb.Type && t.Year == performanceLsfb.Year && t.Month == performanceLsfb.Month && t.CreatedBy == userId && t.DeleteFlg == 0).ToList();
            }
            else if (performanceLsfb.Type == 1)
            {
                items = this.DbContext.PerformanceLsfbs.Where(t => t.ProjectId == performanceLsfb.ProjectId && t.TypePerformanceId == performanceLsfb.TypePerformanceId && t.Type == performanceLsfb.Type && t.Year == performanceLsfb.Year && t.QuarterId == performanceLsfb.QuarterId && t.CreatedBy == userId && t.DeleteFlg == 0).ToList();
            }
            return items.Count > 0 ? false : true;
        }

        public List<PerformanceInfo> GetPerformanceByConditions(string projectId, string typePerformanceId, string years, string typeFbId)
        {
            List<PerformanceLSFB> queryItems = this.DbContext.PerformanceLsfbs.Where(w => w.DeleteFlg == 0).ToList();
            if (!string.IsNullOrEmpty(projectId))
            {
                queryItems = queryItems.Where(w => w.ProjectId == int.Parse(projectId)).ToList();
            }
            if (!string.IsNullOrEmpty(typePerformanceId))
            {
                queryItems = queryItems.Where(w => w.TypePerformanceId == int.Parse(typePerformanceId)).ToList();
            }
            if (!string.IsNullOrEmpty(years))
            {
                queryItems = queryItems.Where(w => w.Year == int.Parse(years)).ToList();
            }
            if (!string.IsNullOrEmpty(typeFbId))
            {
                queryItems = queryItems.Where(w => w.TypeFB == int.Parse(typeFbId)).ToList();
            }
            var items = from d in queryItems
                select new PerformanceInfo()
                {
                    Id = d.Id, ProjectId = d.ProjectId, TypePerformanceId = d.TypePerformanceId,
                    Year = d.Year, Month = d.Month, Type = d.Type, QuarterId = d.QuarterId,
                    OfficeArea = d.OfficeArea, OfficeMonthMoney = d.OfficeMonthMoney,
                    OfficeTY = d.OfficeTY, OfficeLY = d.OfficeLY, RetailArea = d.RetailArea,
                    RetailMonthMoney = d.RetailMonthMoney, RetailTY = d.RetailTY,
                    RetailLY = d.RetailLY, NewArea = d.NewArea, NewMonthMoney = d.NewMonthMoney,
                    NewTotalRev = d.NewTotalRev, TotalMonthMoney = d.TotalMonthMoney,
                    TotalRevTY = d.TotalRevTY, TotalRevLY = d.TotalRevLY, TotalGrossTY = d.TotalGrossTY,
                    TotalGrossLY = d.TotalGrossLY, TypeFB = d.TypeFB, SalesLineToLine = d.SalesLineToLine,
                    SalesAll = d.SalesAll, SalesCashFlowTY = d.SalesCashFlowTY, SalesCashFlowLY = d.SalesCashFlowLY,
                    RevLineTOSNoMG = d.RevLineTOSNoMG, RevLineTOSWithMG = d.RevLineTOSWithMG, RevLineNoMG = d.RevLineNoMG,
                    RevLineWithMG = d.RevLineWithMG, RevAllTOSNoMG = d.RevAllTOSNoMG, RevAllTOSWithMG = d.RevAllTOSWithMG,
                    RevAllNoMG = d.RevAllNoMG, RevAllWithMG = d.RevAllWithMG, RevAllLY = d.RevAllLY,
                    RevAllOPMonthMoney = d.RevAllOPMonthMoney, RevTotalNoMG = d.RevTotalNoMG,
                    RevTotalWithMG = d.RevTotalWithMG, RevTotalLY = d.RevTotalLY, ComProfitTY = d.ComProfitTY,
                    ComProfitLY = d.ComProfitLY, RevLSArea = d.RevLSArea, RevLSMonthMoney = d.RevLSMonthMoney,
                    RevLSRev = d.RevLSRev, RevLSOPMonthMoney = d.RevLSOPMonthMoney, BusinessProfit = d.BusinessProfit,
                    RevAllOccRate = d.RevAllOccRate, RevNormalLine = d.RevNormalLine, RevNormalAll = d.RevNormalAll,
                    RevAllRev = d.RevAllRev, Note = d.Note, CreatedDate = d.CreatedDate, CreatedBy = d.CreatedBy,
                    DeleteFlg = d.DeleteFlg, ApprovedBy = d.ApprovedBy, StatusId = d.StatusId, Comment = d.Comment
                };

            var performanceInfos = items.ToList();
            if (performanceInfos.Any())
            {
                return performanceInfos.ToList();
            }
            return new List<PerformanceInfo>();
        }
    }

    public interface IPerformanceRepository : IRepository<PerformanceLSFB>
    {
        bool CheckCreatePerformance(PerformanceLSFB performanceLsfb, int userId);

        List<PerformanceInfo> GetPerformanceByConditions(string projectId, string typePerformanceId, string years, string typeFbId);
    }
}
