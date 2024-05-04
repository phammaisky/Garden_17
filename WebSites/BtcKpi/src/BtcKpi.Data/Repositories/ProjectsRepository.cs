using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class ProjectsRepository : RepositoryBase<Projects>, IProjectsRepository
    {
        public ProjectsRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Projects> GetListProjects()
        {
            return DbContext.Projects.Where(w => w.DeleteFlg == 0).ToList();
        }

        public Projects GetProjectById(int? projectId)
        {
            return DbContext.Projects.FirstOrDefault(w => w.DeleteFlg == 0 && w.Id == projectId);
        }
    }

    public interface IProjectsRepository : IRepository<Projects>
    {
        List<Projects> GetListProjects();
        Projects GetProjectById(int? projectId);
    }
}
