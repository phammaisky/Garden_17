using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Data.Repositories
{
    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Test GetTestByName(string categoryName)
        {
            var category = this.DbContext.Tests.Where(c => c.Name == categoryName).FirstOrDefault();

            return category;
        }

        public override void Update(Test entity)
        {
            base.Update(entity);
        }
    }

    public interface ITestRepository : IRepository<Test>
    {
        Test GetTestByName(string categoryName);
    }
}
