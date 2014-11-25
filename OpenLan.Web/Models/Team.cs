using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Team
    {
        [ScaffoldColumn(false)]
        public int TeamId { get; set; }

        /// <summary>
        /// Name of the team
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// URL to the team's website
        /// </summary>
        [DataType(DataType.Url)]
        public string Url { get; set; }

        /// <summary>
        /// Team's tagline
        /// </summary>
        public string Tagline { get; set; }

        /// <summary>
        /// Generated token to join a team
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// User that owns the team
        /// </summary>
        [Required]
        public ApplicationUser Owner { get; set; }

        /// <summary>
        /// Team's members
        /// </summary>
        public virtual List<ApplicationUser> Members { get; set; }

        /// <summary>
        /// Tournaments in which the team is registered
        /// </summary>
        public List<Tournament> Tournaments { get; set; }
    }
}