using System;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class zUpfCrossDetailRepository : RepositoryBase<zUpfCrossDetail>, IzUpfCrossDetailRepository
    {
        public zUpfCrossDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public int InsertHistory(int id, byte? action, string description, DateTime insertTime, int userId)
        {
            string sql = string.Format(@"INSERT INTO [kpi].[zUpfCrossDetail] ([Action] ,[Descriptions] ,[ID] ,[UpfCrossID] ,[FromDepartment] ,[ContentsRequested] ,[ExpectedTimeOfCompletion] ,[ExpectedResult] ,[FromWeight] ,[ToDepartment] ,[TimeOfCompletion] ,[Result] ,[FromScore] ,[PlanToDo] ,[ExplainationForResults] ,[Solutions] ,[Timeline] ,[ToWeight] ,[ToScore] ,[AssessmentByCouncil] ,[TotalScore] ,[Status] ,[Created] ,[CreatedBy] ,[DeleteFlg] ,[Deleted] ,[DeletedBy] ,[Updated] ,[UpdateBy])
	                                        SELECT {1} AS [Action] ,N'{2}' AS [Descriptions] ,[ID] ,[UpfCrossID] ,[FromDepartment] ,[ContentsRequested] ,[ExpectedTimeOfCompletion] ,[ExpectedResult] ,[FromWeight] ,[ToDepartment] ,[TimeOfCompletion] ,[Result] ,[FromScore] ,[PlanToDo] ,[ExplainationForResults] ,[Solutions] ,[Timeline] ,[ToWeight] ,[ToScore] ,[AssessmentByCouncil] ,[TotalScore] ,[Status] ,[Created] ,[CreatedBy] ,[DeleteFlg] ,[Deleted] ,[DeletedBy] ,'{3}' AS [Updated] ,{4} AS [UpdateBy]
		                                        FROM kpi.UpfCrossDetail WHERE ID = {0}", id, action, description, insertTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), userId);
            return DbContext.Database.ExecuteSqlCommand(sql);
        }
    }

    public interface IzUpfCrossDetailRepository : IRepository<zUpfCrossDetail>
    {
        int InsertHistory(int id, byte? action, string description, DateTime insertTime, int userId);
    }
}
