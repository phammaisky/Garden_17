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
    public interface IPerformanceService
    {
        bool CheckCreatePerformance(PerformanceLSFB performanceLsfb, int userId);
        Company GetCompanyById(int? companyId);
        Department GetDepartmentById(int? departmentId);
        Projects GetProjectById(int? projectId);
        TypePerformance GetTypePerformanceById(int? typePerformanceId);
        List<Projects> GetListProjects();
        List<TypePerformance> GetListTypePerformance(int projectId);

        bool InsertPerformance(PerformanceLSFB performanceInsert, ref string updateMsg);
        bool UpdatePerformance(PerformanceLSFB performanceUpdate, ref string updateMsg);

        List<PerformanceInfo> GetPerformanceByConditions(string projectId, string typePerformanceId, string years, string typeFbId);

        PerformanceLSFB GetPerformanceById(int id);
    }

    public class PerformanceService : IPerformanceService
    {
        private readonly IPerformanceRepository performanceRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IProjectsRepository projectsRepository;
        private readonly ITypePerformanceRepository typePerformanceRepository;
        private readonly IUnitOfWork unitOfWork;

        public PerformanceService(IPerformanceRepository performanceRepository, ICompanyRepository companyRepository, IDepartmentRepository departmentRepository,
            IProjectsRepository projectsRepository, ITypePerformanceRepository typePerformanceRepository, IUnitOfWork unitOfWork)
        {
            this.performanceRepository = performanceRepository;
            this.companyRepository = companyRepository;
            this.departmentRepository = departmentRepository;
            this.projectsRepository = projectsRepository;
            this.typePerformanceRepository = typePerformanceRepository;
            this.unitOfWork = unitOfWork;
        }

        public bool CheckCreatePerformance(PerformanceLSFB performanceLsfb, int userId)
        {
            return performanceRepository.CheckCreatePerformance(performanceLsfb, userId);
        }

        public Company GetCompanyById(int? companyId)
        {
            return companyRepository.GetCompanyById(companyId);
        }

        public Department GetDepartmentById(int? departmentId)
        {
            return departmentRepository.GetDepartmentById(departmentId);
        }

        public List<Projects> GetListProjects()
        {
            return projectsRepository.GetListProjects();
        }

        public List<TypePerformance> GetListTypePerformance(int projectId)
        {
            return typePerformanceRepository.GetListTypePerformance(projectId);
        }

        public Projects GetProjectById(int? projectId)
        {
            return projectsRepository.GetProjectById(projectId);
        }

        public TypePerformance GetTypePerformanceById(int? typePerformanceId)
        {
            return typePerformanceRepository.GetTypePerformanceById(typePerformanceId);
        }

        public bool InsertPerformance(PerformanceLSFB performanceInsert, ref string updateMsg)
        {
            performanceRepository.Add(performanceInsert);
            unitOfWork.Commit();

            return true;
        }
        public bool UpdatePerformance(PerformanceLSFB performanceUpdate, ref string updateMsg)
        {
            performanceRepository.Update(performanceUpdate);
            unitOfWork.Commit();

            return true;
        }

        public List<PerformanceInfo> GetPerformanceByConditions(string projectId, string typePerformanceId, string years, string typeFbId)
        {
            return performanceRepository.GetPerformanceByConditions(BtcHelper.RemoveComman(projectId), BtcHelper.RemoveComman(typePerformanceId), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(typeFbId));
        }

        public PerformanceLSFB GetPerformanceById(int id)
        {
            return performanceRepository.GetById(id);
        }

    }
}
