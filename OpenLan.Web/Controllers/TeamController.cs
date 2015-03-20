using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly OpenLanContext db;

        public TeamController(OpenLanContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            Team team = null;

            // Get current user
            var username = User.Identity.Name;
            var user = db.Users.Include(x => x.Team).Single(x => x.UserName == User.Identity.Name); // Might be null

            // Make sure user is authenticated and has a team
            if (user != null && user.Team != null)
            {
                team = user.Team;
            }

            return View(team);
        }

        public IActionResult IndexPartial()
        {
            return PartialView(db.Teams);
        }

        public IActionResult ShowToken(int? id)
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
            return this.PartialView(team);
        }

        public ActionResult Start()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            // Redirect to team management if user is already part of a team
            var currentUser = db.Users.Single(x => x.UserName == User.Identity.Name);
            if (currentUser.TeamId != null)
                return RedirectToAction("Manage", "Team");

            return View();
        }

        // GET: /Team/Create
        public ActionResult CreatePartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // POST: /Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(new [] { "Id","Name"})] Team team)
        {
            if (ModelState.IsValid)
            {
                // Make sure the team is not already there
                if (db.Teams.FirstOrDefault(x => x.Name == team.Name) != null)
                {
                    ModelState.AddModelError("Name", "Team name already exists");
                    return View("Start", team);
                }

                // Add current user as leader and member
                var currentUser = db.Users.Single(x => x.UserName == User.Identity.Name);
                currentUser.Team = team;
                team.OwnerUser = currentUser;
                team.Members = new List<ApplicationUser> { currentUser };
                
                db.Teams.Add(team);
                db.SaveChanges();

                return RedirectToAction("CreateDone");
            }

            return PartialView("CreatePartial", team);
        }

        public ActionResult CreateDone()
        {
            var user = db.Users.Include(x => x.Team).Single(x => x.UserName == User.Identity.Name);
            return View(user.Team);
        }

        // GET: /Team/Join
        public ActionResult JoinPartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // GET: /Team/Join/password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(new[] { "Token" })] string token)
        {
            // Get User
            var user = db.Users.Single(x => x.UserName == User.Identity.Name);

            // TODO: Qu'est-ce qui arrive quand ce user est déjà membre d'une équipe?

            // Get Team
            var team = db.Teams.FirstOrDefault(t => t.Token == token);
            if (team == null)
            {
                ModelState.AddModelError("Token", "This token does not exist");
                return View("Start");
            }

            // Add user to team and save changes
            team.Members.Add(user);
            db.SaveChanges();

            return RedirectToAction("Index", "TeamStunt");
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