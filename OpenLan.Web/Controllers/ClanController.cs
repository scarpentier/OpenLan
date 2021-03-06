﻿using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenLan.Web.Models;
using System.Threading.Tasks;
using System.Security.Principal;

namespace OpenLan.Web.Controllers
{
    public class ClanController : Controller
    {
        private readonly OpenLanContext db;

        public ClanController(OpenLanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var clan = (await db.Users.Include(x => x.Clan).SingleAsync(x => x.UserName == User.Identity.Name)).Clan;
                ViewBag.UserClan = clan;
            }

            return View(db.Clans);
        }

        public async Task<IActionResult> Manage()
        {
            // Get current user
            var user = await db.Users
                .Include(x => x.Clan)
                .ThenInclude(x => x.Tournaments)
                .ThenInclude(x => x.Tournament)
                .SingleAsync(x => x.UserName == User.Identity.Name);

            // Make sure user is authenticated and has a clan
            if (user == null || user.Clan == null)
            {
                return RedirectToAction("Start");
            }

            return View(user.Clan);
        }

        public async Task<IActionResult> ShowToken(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Clan clan = await db.Clans.SingleOrDefaultAsync(x => x.Id == id);
            if (clan == null)
            {
                return HttpNotFound();
            }
            return this.PartialView(clan);
        }

        public async Task<IActionResult> Start()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            // Redirect to clan management if user is already part of a clan
            var currentUser = await db.Users.SingleAsync(x => x.UserName == User.Identity.Name);
            if (currentUser.ClanId != null)
                return RedirectToAction("Manage", "Clan");

            return View();
        }

        // GET: /Clan/Create
        public async Task<IActionResult> CreatePartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // POST: /Clan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(new [] { "Id","Name"})] Clan clan)
        {
            if (ModelState.IsValid)
            {
                // Make sure the clan is not already there
                if (await db.Clans.FirstOrDefaultAsync(x => x.Name == clan.Name) != null)
                {
                    ModelState.AddModelError("Name", "Clan name already exists");
                    return View("Start", clan);
                }

                // Add current user as leader and member
                var currentUser = await db.Users.SingleAsync(x => x.UserName == User.Identity.Name);
                currentUser.Clan = clan;
                clan.OwnerUserId = currentUser.Id;
                clan.Members = new List<ApplicationUser> { currentUser };
                
                await db.Clans.AddAsync(clan);
                await db.SaveChangesAsync();

                return RedirectToAction("CreateDone");
            }

            return PartialView("CreatePartial", clan);
        }

        public async Task<IActionResult> CreateDone()
        {
            var user = await db.Users.Include(x => x.Clan).SingleAsync(x => x.UserName == User.Identity.Name);
            return View(user.Clan);
        }

        // GET: /Clan/Join
        public async Task<IActionResult> JoinPartial()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            return PartialView();
        }

        // GET: /Clan/Join/password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join([Bind(new[] { "Token" })] string token)
        {
            // Get User
            var user = await db.Users.SingleAsync(x => x.UserName == User.Identity.Name);

            // TODO: Qu'est-ce qui arrive quand ce user est déjà membre d'une équipe?

            // Get Clan
            var clan = await db.Clans.FirstOrDefaultAsync(t => t.Token == token);
            if (clan == null)
            {
                ModelState.AddModelError("Token", "This token does not exist");
                return View("Start");
            }

            // Add user to clan and save changes
            clan.Members.Add(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "ClanStunt");
        }

        // GET: /Clan/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Make sure the user is logged in
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // Get the current user object
            var user = await db.Users.Include(x => x.Clan).SingleAsync(x => x.UserName == User.Identity.Name);

            // Make sure the current user is the clan's leader
            if (user.Clan.OwnerUserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(403);

            return View(user.Clan);
        }

        // POST: /Clan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(new[] { "Id", "Name", "PictureUrl", "Tag", "Tagline", "Url" })] Clan clan)
        {
            if (ModelState.IsValid)
            {
                var t = await db.Clans.SingleAsync(x => x.Id == clan.Id);

                t.Name = clan.Name;
                t.Token = clan.Token;
                t.PictureUrl = clan.PictureUrl;
                t.Tag = clan.Tag;
                t.Tagline = clan.Tagline;
                t.Url = clan.Url;

                db.Entry(t).SetState(Microsoft.Data.Entity.EntityState.Modified);
                await db.SaveChangesAsync();
                return RedirectToAction("Manage");
            }
            return View(clan);
        }

        public async Task<IActionResult> TransferLeadership()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Leave()
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