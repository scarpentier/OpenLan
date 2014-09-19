using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Microsoft.AspNet.Identity.EntityFramework;

using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    public class RolesController : Controller
    {
        private OpenLanContext _db = new OpenLanContext();

        public ActionResult Index()
        {
            var rolesList = new List<RoleViewModel>();
            foreach (var role in this._db.Roles)
            {
                var roleModel = new RoleViewModel(role);
                rolesList.Add(roleModel);
            }
            return this.View(rolesList);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create(string message = "")
        {
            this.ViewBag.Message = message;
            return this.View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include =
            "RoleName,Description")]RoleViewModel model)
        {
            string message = "That role name has already been used";
            if (this.ModelState.IsValid)
            {
                var role = new ApplicationRole(model.RoleName, model.Description);
                ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(this._db));
                if (this._db.RoleExists(_roleManager, model.RoleName))
                {
                    return this.View(message);
                }
                else
                {
                    this._db.CreateRole(_roleManager, model.RoleName, model.Description);
                    return this.RedirectToAction("Index", "Roles");
                }
            }
            return this.View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            // It's actually the Role.Name tucked into the id param:
            var role = this._db.Roles.First(r => r.Name == id);
            var roleModel = new EditRoleViewModel(role);
            return this.View(roleModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include =
            "RoleName,OriginalRoleName,Description")] EditRoleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var role = this._db.Roles.First(r => r.Name == model.OriginalRoleName);
                role.Name = model.RoleName;
                role.Description = model.Description;
                this._db.Entry(role).State = EntityState.Modified;
                this._db.SaveChanges();
                return this.RedirectToAction("Index");
            }
            return this.View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = this._db.Roles.First(r => r.Name == id);
            var model = new RoleViewModel(role);
            if (role == null)
            {
                return this.HttpNotFound();
            }
            return this.View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var role = this._db.Roles.First(r => r.Name == id);
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this._db));
            this._db.DeleteRole(this._db, userManager, role.Id);
            return this.RedirectToAction("Index");
        }
    }
}