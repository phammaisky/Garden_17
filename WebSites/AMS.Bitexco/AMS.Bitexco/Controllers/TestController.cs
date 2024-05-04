using System.Data;
using System.Linq;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        private AMSEntities db = new AMSEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult AutoCompleteCountry(string term)
        {
            var result = (from r in db.ToolCategories
                          where r.ToolCatName.ToLower().Contains(term.ToLower())
                          select new { r.ToolCatName }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}