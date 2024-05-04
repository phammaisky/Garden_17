using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class UpfCrossInfo 
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public int ID { get; set; }
        public int UpfCrossID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public Nullable<int> FromDepartment { get; set; }
        public Nullable<int> ToDepartment { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<byte> Month { get; set; }
        public string Objective { get; set; }
        public string ContentsRequested { get; set; }
        public string ExpectedTimeOfCompletion { get; set; }
        public string ExpectedResult { get; set; }
        public string TimeOfCompletion { get; set; }
        public string Result { get; set; }
        public string PlanToDo { get; set; }
        public string ExplainationForResults { get; set; }
        public string Solutions { get; set; }
        public string Timeline { get; set; }
        public string AssessmentByCouncil { get; set; }
        public Nullable<decimal> TotalScore { get; set; }
        public Nullable<decimal> FromScore { get; set; }
        public Nullable<decimal> ToScore { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> ToUser { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<decimal> ScoreAssessed { get; set; }
        public Nullable<decimal> ScoreAssessment { get; set; }
        public Nullable<decimal> ScoreBOD { get; set; }
        public Nullable<decimal> ScoreAverage { get; set; }
        public string Rated { get; set; }
        public Nullable<byte> DependWeight { get; set; }
        public Nullable<decimal> DependScore { get; set; }
        public Nullable<byte> FromWeight { get; set; }
        public Nullable<byte> ToWeight { get; set; }
        public Nullable<byte> TotalDependWeight { get; set; }
        public Nullable<byte> TotalAssessmentWeight { get; set; }
        public Nullable<int> CountAverage { get; set; }
        public Nullable<decimal> PerformancePoint { get; set; }
        public Nullable<byte> BodDependWeight { get; set; }
        public Nullable<decimal> BodDependScore { get; set; }
        public Nullable<byte> PerformanceWeight { get; set; }
        public Nullable<byte> LeasingWeight { get; set; }
        public Nullable<decimal> LeasingScore { get; set; }
        public Nullable<byte> FbWeight { get; set; }
        public Nullable<decimal> FbScore { get; set; }
        public Nullable<byte> OpsWeight { get; set; }
        public Nullable<decimal> OpsScore { get; set; }
        public Nullable<byte> MktWeight { get; set; }
        public Nullable<decimal> MktScore { get; set; }
        public Nullable<byte> AccWeight { get; set; }
        public Nullable<decimal> AccScore { get; set; }
        public Nullable<byte> HrWeight { get; set; }
        public Nullable<decimal> HrScore { get; set; }
        public Nullable<byte> EplWeight { get; set; }
        public Nullable<decimal> EplScore { get; set; }
        public Nullable<byte> ItWeight { get; set; }
        public Nullable<decimal> ItScore { get; set; }
        public Nullable<byte> DesignWeight { get; set; }
        public Nullable<decimal> DesignScore { get; set; }
        public Nullable<byte> CrmWeight { get; set; }
        public Nullable<decimal> CrmScore { get; set; }
        public Nullable<byte> LegalWeight { get; set; }
        public Nullable<decimal> LegalScore { get; set; }
        public Nullable<byte> GcWeight { get; set; }
        public Nullable<decimal> GcScore { get; set; }
        public Nullable<byte> CashiersWeight { get; set; }
        public Nullable<decimal> CashiersScore { get; set; }
        public Nullable<byte> TechWeight { get; set; }
        public Nullable<decimal> TechScore { get; set; }
        public Nullable<byte> SfWeight { get; set; }
        public Nullable<decimal> SfScore { get; set; }
        public Nullable<byte> CcWeight { get; set; }
        public Nullable<decimal> CcScore { get; set; }
        public string RatedBod { get; set; }
        public Nullable<decimal> January { get; set; }
        public Nullable<decimal> February { get; set; }
        public Nullable<decimal> March { get; set; }
        public Nullable<decimal> April { get; set; }
        public Nullable<decimal> May { get; set; }
        public Nullable<decimal> June { get; set; }
        public Nullable<decimal> July { get; set; }
        public Nullable<decimal> August { get; set; }
        public Nullable<decimal> September { get; set; }
        public Nullable<decimal> October { get; set; }
        public Nullable<decimal> November { get; set; }
        public Nullable<decimal> December { get; set; }
        public string RatedJanuary { get; set; }
        public string RatedFebruary { get; set; }
        public string RatedMarch { get; set; }
        public string RatedApril { get; set; }
        public string RatedMay { get; set; }
        public string RatedJune { get; set; }
        public string RatedJuly { get; set; }
        public string RatedAugust { get; set; }
        public string RatedSeptember { get; set; }
        public string RatedOctober { get; set; }
        public string RatedNovember { get; set; }
        public string RatedDecember { get; set; }
        public Nullable<decimal> TotalYearPoint { get; set; }
        public string TotalYearRated { get; set; }
        public Nullable<int> CountFromDepartment { get; set; }
        public Nullable<int> CountToDepartment { get; set; }
        public Nullable<byte> TotalFromWeight { get; set; }
        public Nullable<byte> TotalToWeight { get; set; }
    }
}
