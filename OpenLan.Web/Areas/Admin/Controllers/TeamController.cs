using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenLan.Web.Models;

namespace OpenLan.Web.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize("ManageTeams")]
    public class TeamController : Controller
    {
        private readonly OpenLanContext db;

        public TeamController(OpenLanContext context)
        {
            db = context;
        }

        // GET: /Team/
        public IActionResult Index()
        {
            return View(db.Teams.OrderByDescending(x => x.Members));
        }

         // GET: /Team/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Team team = db.Teams.SingleOrDefault(x => x.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: /Team/Create
        public ActionResult CreatePartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // GET: /Team/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Team team = db.Teams.Single(x => x.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(new [] { "Id","Name","Token","Tagline","Url"})] Team team)
        {
            if (ModelState.IsValid)
            {
                var t = db.Teams.Single(x => x.Id == team.Id);

                t.Name = team.Name;
                t.Token = team.Token;
                t.Tagline = team.Tagline;
                t.Url = team.Url;

                db.Entry(t).SetState(Microsoft.Data.Entity.EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(team);
        }

        // GET: /Team/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Team team = db.Teams.Single(x => x.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Single(x => x.Id == id);

            // Kick everyone from that team
            // TODO: Do with CASCADE NULL instead
            team.Members.ToList().ForEach(x => x.Team = null);

            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }

        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll()
        {
            foreach (var team in db.Teams)
                db.Teams.Remove(team);

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