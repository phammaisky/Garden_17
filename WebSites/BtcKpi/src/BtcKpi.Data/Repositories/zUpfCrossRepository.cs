using System;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class zUpfCrossRepository : RepositoryBase<zUpfCross>, IzUpfCrossRepository
    {
        public zUpfCrossRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int InsertHistory(int id, byte? action, string description, DateTime insertTime, int userId)
        {
            string sql = string.Format(@"INSERT INTO [kpi].[zUpfCross] ([Action] ,[Descriptions] ,[ID] ,[Year] ,[Month] ,[Status] ,[DependWeight] ,[DependScore] ,[SelfWeight] ,[SelfScore] ,[TotalScore] ,[Created] ,[CreatedBy] ,[DeleteFlg] ,[Deleted] ,[DeletedBy] ,[Updated] ,[UpdateBy])
	                                        SELECT {1} AS [Action] ,N'{2}' AS [Descriptions] ,[ID] ,[Year] ,[Month] ,[Status] ,[DependWeight] ,[DependScore] ,[SelfWeight] ,[SelfScore] ,[TotalScore] ,[Created] ,[CreatedBy] ,[DeleteFlg] ,[Deleted] ,[DeletedBy] ,'{3}' AS [Updated] ,{4} AS [UpdateBy]
		                                        FROM kpi.UpfCross WHERE ID = {0}", id, action, description, insertTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), userId);
            return DbContext.Database.ExecuteSqlCommand(sql);
        }
    }

    public interface IzUpfCrossRepository : IRepository<zUpfCross>
    {
        int InsertHistory(int id, byte? action, string description, DateTime insertTime, int userId);
    }
}
