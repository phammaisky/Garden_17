using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class AdministratorShipRepository : RepositoryBase<AdministratorShip>, IAdministratorShipRepository
    {
        public AdministratorShipRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IAdministratorShipRepository : IRepository<AdministratorShip>
    {

    }
}
