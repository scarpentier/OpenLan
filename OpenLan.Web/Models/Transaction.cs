using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLan.Web.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public String  Code { get; set; }

        public DateTime TimeStarted { get; set; }

        public DateTime TimeEnded { get; set; }

        public String Status { get; set; }

        public virtual ApplicationUser ContactUser { get; set; }
    }
}