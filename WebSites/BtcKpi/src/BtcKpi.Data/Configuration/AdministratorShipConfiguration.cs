using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class AdministratorShipConfiguration : EntityTypeConfiguration<AdministratorShip>
    {
        public AdministratorShipConfiguration()
        {
            ToTable("dbo.AdministratorShips");
        }
    }
}
