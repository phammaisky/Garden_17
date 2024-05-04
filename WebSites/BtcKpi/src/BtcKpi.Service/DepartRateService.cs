using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Service.Common;

namespace BtcKpi.Service
{
    // operations you want to expose
    public interface IDepartRateService
    {
        List<UpfRate> GetUpfRateList();
    }

    public class DepartRateService : IDepartRateService
    {
        private readonly IDepartmentRateRepository departmentRateRepository;

        public DepartRateService(IDepartmentRateRepository departmentRateRepository)
        {
            this.departmentRateRepository = departmentRateRepository;
        }

        public List<UpfRate> GetUpfRateList()
        {
            return departmentRateRepository.GetUpfRateList();
        }
    }
}
