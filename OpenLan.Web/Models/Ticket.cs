using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLan.Web.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public ApplicationUser Owner { get; set; }

        public Transaction transaction { get; set; }

        public string Tagline { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ApplicationUser ContactUser { get; set; }
    }
}