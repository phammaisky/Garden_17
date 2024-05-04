using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class FunctionRepository : RepositoryBase<Function>, IFunctionRepository
    {
        public FunctionRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IFunctionRepository : IRepository<Function>
    {

    }
}
