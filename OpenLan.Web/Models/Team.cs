using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLan.Web.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ApplicationUser ContactUser { get; set; }
    }
}