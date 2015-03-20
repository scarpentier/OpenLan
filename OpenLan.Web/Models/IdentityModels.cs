using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Framework.OptionsModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OpenLan.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}