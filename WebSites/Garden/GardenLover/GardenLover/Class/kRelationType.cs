using IQWebApp_Blank.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQWebApp_Blank.Models
{
    public class kRelationType
    {
        GoodJobEntities dbJob = new GoodJobEntities();

        public long StepId { get; set; }
        public long SendToRelationTypeId { get; set; }
        public string SendToRelationValue { get; set; }

        public void StepSendTo(cUserInfo reporter, long stepSeq)
        {
            long stepForAllId = dbJob.kStepForAlls.FirstOrDefault(x => x.Seq == stepSeq).Id;

            var relationType = dbJob.kRelationTypes.FirstOrDefault(x => x.UserId == reporter.UserId);

            long? relationTypeId = relationType != null
                ? relationType.StepRelationTypeId
                : dbJob.kRelationTypeForAlls.FirstOrDefault(x => x.IsDefaultStepRelationType).Id;
            //Neu ko co ? Thi doc Default. Va phai setup san Default tu dau.

            kStep step = null;

            //User
            if (relationTypeId == 7)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.UserId.ToString().ToLower() && x.StepForAllId == stepForAllId);

            //Rank
            if (relationTypeId == 6)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.RankId.ToString() && x.StepForAllId == stepForAllId);

            //Department
            if (relationTypeId == 5)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.DepartmentId.ToString() && x.StepForAllId == stepForAllId);

            //Branch
            if (relationTypeId == 4)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.BranchId.ToString() && x.StepForAllId == stepForAllId);

            //Company
            if (relationTypeId == 3)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.CompanyId.ToString() && x.StepForAllId == stepForAllId);

            //Corp
            if (relationTypeId == 2)
                step = dbJob.kSteps.FirstOrDefault(x => x.StepRelationTypeId == relationTypeId && x.StepRelationValue == reporter.CorporationId.ToString() && x.StepForAllId == stepForAllId);

            if (step != null)
            {
                StepId = step.Id;
                SendToRelationTypeId = step.SendToRelationTypeId;
                SendToRelationValue = step.SendToRelationValue;

                if (SendToRelationValue == "Manager")
                {
                    SendToRelationValue = reporter.ManagerId.ToString().ToLower();
                    SendToRelationTypeId = 7;
                }
                else
                if (SendToRelationValue == "HR-Manager")
                {
                    SendToRelationValue = reporter.ManagerId.ToString().ToLower();
                    SendToRelationTypeId = 7;
                }
            }
        }
    }
}