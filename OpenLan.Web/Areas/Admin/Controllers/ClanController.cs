using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenLan.Web.Models;

namespace OpenLan.Web.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("ManageClans")]
    public class ClanController : Controller
    {
        private readonly OpenLanContext db;

        public ClanController(OpenLanContext context)
        {
            db = context;
        }

        // GET: /Clan/
        public IActionResult Index()
        {
            return View(db.Clans.OrderByDescending(x => x.Members));
        }

         // GET: /Clan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Clan Clan = db.Clans.SingleOrDefault(x => x.Id == id);
            if (Clan == null)
            {
                return HttpNotFound();
            }
            return View(Clan);
        }

        // GET: /Clan/Create
        public ActionResult CreatePartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // GET: /Clan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Clan Clan = db.Clans.Single(x => x.Id == id);
            if (Clan == null)
            {
                return HttpNotFound();
            }
            return View(Clan);
        }

        // POST: /Clan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(new [] { "Id","Name","Token","Tagline","Url"})] Clan Clan)
        {
            if (ModelState.IsValid)
            {
                var t = db.Clans.Single(x => x.Id == Clan.Id);

                t.Name = Clan.Name;
                t.Token = Clan.Token;
                t.Tagline = Clan.Tagline;
                t.Url = Clan.Url;

                db.Entry(t).SetState(Microsoft.Data.Entity.EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(Clan);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Clan Clan = db.Clans.Single(x => x.Id == id);
            if (Clan == null)
            {
                return HttpNotFound();
            }
            return View(Clan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clan Clan = db.Clans.Single(x => x.Id == id);

            // Kick everyone from that Clan
            // TODO: Do with CASCADE NULL instead
            Clan.Members.ToList().ForEach(x => x.Clan = null);

            db.Clans.Remove(Clan);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }

        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll()
        {
            foreach (var Clan in db.Clans)
                db.Clans.Remove(Clan);

            db.SaveChanges();
            return View("Data");
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