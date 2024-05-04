using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        BtcKpiEntities dbContext;

        public BtcKpiEntities Init()
        {
            return dbContext ?? (dbContext = new BtcKpiEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
