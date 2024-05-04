using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfPersRewProposalConfiguration : EntityTypeConfiguration<UpfPersRewProposal>
    {
        public UpfPersRewProposalConfiguration()
        {
            ToTable("kpi.UpfPersRewProposal");
        }
    }
}
