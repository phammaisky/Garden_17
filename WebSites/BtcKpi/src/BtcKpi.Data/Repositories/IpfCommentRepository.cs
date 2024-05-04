using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfCommentRepository : RepositoryBase<IpfComment>, IIpfCommentRepository
    {
        public IpfCommentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<IpfComment> GetByIpfId(int ipfId)
        {
            var queryTable = (from c in DbContext.IpfComments.Where(t => t.IpfID == ipfId & t.DeleteFlg == 0)
                join u in DbContext.Users on c.CreatedBy equals u.ID
                select new
                {
                    ID = c.ID,
                    IpfID = c.IpfID,
                    Comment = c.Comment,
                    Created = c.Created,
                    CreatedBy = c.CreatedBy,
                    DeleteFlg = c.DeleteFlg,
                    Deleted = c.Deleted,
                    DeletedBy = c.DeletedBy,
                    UserName = u.UserName
                }).ToList();
            var items = from c in queryTable
                select new IpfComment
                {
                    ID = c.ID,
                    IpfID = c.IpfID,
                    Comment = c.Comment,
                    Created = c.Created,
                    CreatedBy = c.CreatedBy,
                    DeleteFlg = c.DeleteFlg,
                    Deleted = c.Deleted,
                    DeletedBy = c.DeletedBy,
                    UserName = c.UserName
                };
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<IpfComment>();
        }

    }

    public interface IIpfCommentRepository : IRepository<IpfComment>
    {
        List<IpfComment> GetByIpfId(int ipfId);
    }
}
