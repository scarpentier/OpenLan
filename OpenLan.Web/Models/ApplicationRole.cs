using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenLan.Web.Models
{
    public class ApplicationRole: IdentityRole
    {
        public virtual string Description { get; set; }

        public ApplicationRole() : base() { }

        public ApplicationRole(string name, string description) : base(name)
        {
            this.Description = description;
        }

        public virtual ICollection<ApplicationUserRole> RoleUsers { get; set; }
    }
}