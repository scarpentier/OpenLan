using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Framework.OptionsModel;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's birthdate
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// User's team
        /// </summary>
        public Team Team { get; set; }

        /// <summary>
        /// User's website
        /// </summary>
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}