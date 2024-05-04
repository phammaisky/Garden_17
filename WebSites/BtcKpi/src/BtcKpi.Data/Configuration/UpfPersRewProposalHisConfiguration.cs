using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfPersRewProposalHisConfiguration : EntityTypeConfiguration<UpfPersRewProposalHis>
    {
        public UpfPersRewProposalHisConfiguration()
        {
            ToTable("kpi.UpfPersRewProposalHis");
        }
    }
}
