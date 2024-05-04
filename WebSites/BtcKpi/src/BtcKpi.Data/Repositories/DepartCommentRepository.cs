using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class DepartCommentRepository : RepositoryBase<UpfComment>, IDepartCommentRepository
    {
        public DepartCommentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfComment> GetByUpfId(int upfId)
        {
            //var queryTable = (from c in DbContext.UpfComments.Where(t => t.UpfID == upfId & t.DeleteFlg == 0)
            //    join u in DbContext.Users on c.CreatedBy equals u.ID
            //    join ad in DbContext.AdministratorShips on u.AdministratorshipID equals ad.ID
            //    join de in DbContext.Departments on u.DepartmentID equals de.Id
            //    select new
            //    {
            //        ID = c.ID,
            //        UpfID = c.UpfID,
            //        Comment = c.Comment,
            //        Created = c.CreatedDate,
            //        CreatedBy = c.CreatedBy,
            //        DeleteFlg = c.DeleteFlg,
            //        Deleted = c.Deleted,
            //        DeletedBy = c.DeletedBy,
            //        UserName = u.UserName,
            //        AdminDepartName = ad.Name + " - " + de.Name
            //    }).ToList();
            //var items = from c in queryTable
            //    select new UpfComment
            //    {
            //        ID = c.ID,
            //        UpfID = c.UpfID,
            //        Comment = c.Comment,
            //        CreatedDate = c.Created,
            //        CreatedBy = c.CreatedBy,
            //        DeleteFlg = c.DeleteFlg,
            //        Deleted = c.Deleted,
            //        DeletedBy = c.DeletedBy,
            //        UserName = c.UserName,
            //        AdminDepartName = c.AdminDepartName
            //    };
            List<UpfComment> items = this.DbContext.UpfComments.Where(t => t.UpfID == upfId && t.DeleteFlg == 0).ToList();
            if (items.Any())
            {
                foreach (UpfComment item in items)
                {
                    User user = this.DbContext.Users.FirstOrDefault(t => t.ID == item.CreatedBy && t.DeleteFlg == 0);
                    AdministratorShip administratorShip = this.DbContext.AdministratorShips.FirstOrDefault(t => t.ID == user.AdministratorshipID && t.DeleteFlg == 0);
                    Department department = this.DbContext.Departments.FirstOrDefault(t => t.Id == user.DepartmentID && t.DeleteFlg == 0);
                    if (user != null)
                    {
                        item.UserName = user.UserName;
                    }
                    if (administratorShip != null && department != null)
                    {
                        item.AdminDepartName = administratorShip.Name + " - " + department.Name;
                    }
                }
                return items;
            }
            return new List<UpfComment>();
        }

    }

    public interface IDepartCommentRepository : IRepository<UpfComment>
    {
        List<UpfComment> GetByUpfId(int upfId);
    }
}
