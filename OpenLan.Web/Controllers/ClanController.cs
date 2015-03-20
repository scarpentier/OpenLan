using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    public class ClanController : Controller
    {
        private readonly OpenLanContext db;

        public ClanController(OpenLanContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            Clan clan = null;

            // Get current user
            var username = User.Identity.Name;
            var user = db.Users.Include(x => x.Clan).Single(x => x.UserName == User.Identity.Name); // Might be null

            // Make sure user is authenticated and has a clan
            if (user != null && user.Clan != null)
            {
                clan = user.Clan;
            }

            return View(clan);
        }

        public IActionResult IndexPartial()
        {
            return PartialView(db.Clans);
        }

        public IActionResult ShowToken(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Clan clan = db.Clans.SingleOrDefault(x => x.Id == id);
            if (clan == null)
            {
                return HttpNotFound();
            }
            return this.PartialView(clan);
        }

        public ActionResult Start()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            // Redirect to clan management if user is already part of a clan
            var currentUser = db.Users.Single(x => x.UserName == User.Identity.Name);
            if (currentUser.ClanId != null)
                return RedirectToAction("Manage", "Clan");

            return View();
        }

        // GET: /Clan/Create
        public ActionResult CreatePartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // POST: /Clan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(new [] { "Id","Name"})] Clan clan)
        {
            if (ModelState.IsValid)
            {
                // Make sure the clan is not already there
                if (db.Clans.FirstOrDefault(x => x.Name == clan.Name) != null)
                {
                    ModelState.AddModelError("Name", "Clan name already exists");
                    return View("Start", clan);
                }

                // Add current user as leader and member
                var currentUser = db.Users.Single(x => x.UserName == User.Identity.Name);
                currentUser.Clan = clan;
                clan.OwnerUser = currentUser;
                clan.Members = new List<ApplicationUser> { currentUser };
                
                db.Clans.Add(clan);
                db.SaveChanges();

                return RedirectToAction("CreateDone");
            }

            return PartialView("CreatePartial", clan);
        }

        public ActionResult CreateDone()
        {
            var user = db.Users.Include(x => x.Clan).Single(x => x.UserName == User.Identity.Name);
            return View(user.Clan);
        }

        // GET: /Clan/Join
        public ActionResult JoinPartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // GET: /Clan/Join/password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(new[] { "Token" })] string token)
        {
            // Get User
            var user = db.Users.Single(x => x.UserName == User.Identity.Name);

            // TODO: Qu'est-ce qui arrive quand ce user est déjà membre d'une équipe?

            // Get Clan
            var clan = db.Clans.FirstOrDefault(t => t.Token == token);
            if (clan == null)
            {
                ModelState.AddModelError("Token", "This token does not exist");
                return View("Start");
            }

            // Add user to clan and save changes
            clan.Members.Add(user);
            db.SaveChanges();

            return RedirectToAction("Index", "ClanStunt");
        }

        public ActionResult Leave()
        {
            // TODO: Implement
            throw new NotImplementedException();
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