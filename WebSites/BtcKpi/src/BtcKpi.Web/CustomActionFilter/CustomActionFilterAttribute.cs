using System.Web;
using System.Web.Mvc;
using BtcKpi.Web.Controllers;
using BtcKpi.Web.ViewModels;

namespace BtcKpi.Web.CustomActionFilter
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller is BaseController)
            {
                BaseController ctr = (BaseController)filterContext.Controller;
                string action = filterContext.ActionDescriptor.ActionName;
                string controller = filterContext.Controller.GetType().Name;

                if (!(action.Contains("Create") || action.Contains("View") || action.Contains("Edit") || action.Contains("Delete") ||
                    action.Contains("Comment") || action.Contains("Approve")))
                {
                    //Clear session here
                    //HttpContext.Current.Session[string.Format("ipf-{0}", ctr.CurrentUser.UserId)] = null;
                }
            }
        }
    }
}