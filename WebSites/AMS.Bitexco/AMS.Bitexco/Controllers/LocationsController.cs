using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class LocationsController : Controller
    {
        private AMSEntities db = new AMSEntities();

        // GET: Locations
        public ActionResult Index()
        {
            var locations = db.Locations;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_LocationList", locations.ToList());
            }
            else
            {
                return View(locations.ToList());
            }
        }
        public ActionResult LocationSelector(string type)
        {
            var locations = db.Locations;
            if(!String.IsNullOrEmpty(type))
            {
                if (type == "ToSearch")
                { return PartialView("_LocationSelectorToSearch", locations.ToList()); }
                  
                return PartialView("_LocationSelector", locations.ToList());           
            }
            return PartialView("_LocationSelector", locations.ToList());           
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }
        
            return PartialView("_Create");
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentId,ShortName,LocationName,Description")] Location location)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (ModelState.IsValid)
            {
                if (location.ParentId == 0)
                    location.ParentId = null;

                db.Locations.Add(location);
                db.SaveChanges();
                return Json(new { success = true });
            }
           
            return PartialView("_Create");
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentId,,ShortName,LocationName,Description")] Location location)
        {
            if (ModelState.IsValid)
            {
                if (location.ParentId == 0)
                    location.ParentId = null;
                else
                {
                    if (!checkValidParentSelected(location.ParentId, location.Id))
                    {
                        ModelState.AddModelError(string.Empty, "Chọn cây địa điểm không phù hợp!");
                        return PartialView("_Edit", location);
                    }
                }

                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", location);
        }


        public bool checkValidParentSelected(int? parentId, int locationId)
        {
            using(AMSEntities dbAm = new AMSEntities())
            {
                var location = dbAm.Locations.Where(lc => lc.Id == locationId).FirstOrDefault();
                if (location != null)
                {
                    if (location.LocationChild.Where(lcc => lcc.Id == parentId).Count() > 0)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            if (location.HistoryUses.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các thiết bị đang sử dụng DM này");
                ViewBag.UnableDelete = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa danh mục địa điểm?");
                ViewBag.UnableDelete = false;
            }
            return PartialView("_Delete");
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {   
            Location location = db.Locations.Find(id);
            if (location.HistoryUses.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các thiết bị đang sử dụng DM này");
                return PartialView("_Delete", location);
            }
            else
            {
                db.Locations.Remove(location);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được danh mục địa điểm!");
                    return PartialView("_Delete", location);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
