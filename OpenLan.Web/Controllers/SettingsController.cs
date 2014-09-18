using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private OpenLanContext db = new OpenLanContext();

        // GET: /Settings/
        public ActionResult Index()
        {
            return View(this.db.Settings.ToList());
        }

        // GET: /Settings/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: /Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Key,Value,Description")] Setting setting)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Settings.Add(setting);
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(setting);
        }

        // GET: /Settings/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = this.db.Settings.Find(id);
            if (setting == null)
            {
                return this.HttpNotFound();
            }
            return this.View(setting);
        }

        // POST: /Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Key,Value,Description")] Setting setting)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(setting).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }
            return this.View(setting);
        }

        // GET: /Settings/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = this.db.Settings.Find(id);
            if (setting == null)
            {
                return this.HttpNotFound();
            }
            return this.View(setting);
        }

        // POST: /Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Setting setting = this.db.Settings.Find(id);
            this.db.Settings.Remove(setting);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
