using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GardenLover.EF;

namespace GardenLover
{
    public class DepartmentForAllsController : Controller
    {
        private CompanyEntities db = new CompanyEntities();

        // GET: DepartmentForAlls
        public async Task<ActionResult> Index()
        {
            var cDepartmentForAlls = db.cDepartmentForAlls.Include(c => c.cBranch).Include(c => c.cCompany).Include(c => c.cCorporation);
            return View(await cDepartmentForAlls.ToListAsync());
        }

        // GET: DepartmentForAlls/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cDepartmentForAll cDepartmentForAll = await db.cDepartmentForAlls.FindAsync(id);
            if (cDepartmentForAll == null)
            {
                return HttpNotFound();
            }
            return View(cDepartmentForAll);
        }

        // GET: DepartmentForAlls/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.cBranches, "Id", "BranchName");
            ViewBag.CompanyId = new SelectList(db.cCompanies, "Id", "CompanyName");
            ViewBag.CorporationId = new SelectList(db.cCorporations, "Id", "CorporationName");
            return View();
        }

        // POST: DepartmentForAlls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CorporationId,CompanyId,BranchId,DepartmentName,Seq,AutoCreate")] cDepartmentForAll cDepartmentForAll)
        {
            if (ModelState.IsValid)
            {
                db.cDepartmentForAlls.Add(cDepartmentForAll);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(db.cBranches, "Id", "BranchName", cDepartmentForAll.BranchId);
            ViewBag.CompanyId = new SelectList(db.cCompanies, "Id", "CompanyName", cDepartmentForAll.CompanyId);
            ViewBag.CorporationId = new SelectList(db.cCorporations, "Id", "CorporationName", cDepartmentForAll.CorporationId);
            return View(cDepartmentForAll);
        }

        // GET: DepartmentForAlls/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cDepartmentForAll cDepartmentForAll = await db.cDepartmentForAlls.FindAsync(id);
            if (cDepartmentForAll == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.cBranches, "Id", "BranchName", cDepartmentForAll.BranchId);
            ViewBag.CompanyId = new SelectList(db.cCompanies, "Id", "CompanyName", cDepartmentForAll.CompanyId);
            ViewBag.CorporationId = new SelectList(db.cCorporations, "Id", "CorporationName", cDepartmentForAll.CorporationId);
            return View(cDepartmentForAll);
        }

        // POST: DepartmentForAlls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CorporationId,CompanyId,BranchId,DepartmentName,Seq,AutoCreate")] cDepartmentForAll cDepartmentForAll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cDepartmentForAll).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.cBranches, "Id", "BranchName", cDepartmentForAll.BranchId);
            ViewBag.CompanyId = new SelectList(db.cCompanies, "Id", "CompanyName", cDepartmentForAll.CompanyId);
            ViewBag.CorporationId = new SelectList(db.cCorporations, "Id", "CorporationName", cDepartmentForAll.CorporationId);
            return View(cDepartmentForAll);
        }

        // GET: DepartmentForAlls/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cDepartmentForAll cDepartmentForAll = await db.cDepartmentForAlls.FindAsync(id);
            if (cDepartmentForAll == null)
            {
                return HttpNotFound();
            }
            return View(cDepartmentForAll);
        }

        // POST: DepartmentForAlls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            cDepartmentForAll cDepartmentForAll = await db.cDepartmentForAlls.FindAsync(id);
            db.cDepartmentForAlls.Remove(cDepartmentForAll);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
