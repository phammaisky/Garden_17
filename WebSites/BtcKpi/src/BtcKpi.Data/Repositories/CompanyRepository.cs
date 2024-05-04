using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Company GetCompanyById(int? companyId)
        {
            var item = this.DbContext.Companies.FirstOrDefault(w => w.DeleteFlg == 0 && w.Id == companyId);
            return item;
        }
    }

    public interface ICompanyRepository : IRepository<Company>
    {
        Company GetCompanyById(int? companyId);
    }
}
