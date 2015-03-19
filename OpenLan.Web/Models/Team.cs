using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

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

        [DataType(DataType.Url)]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Team's tagline
        /// </summary>
        public string Tagline { get; set; }

        /// <summary>
        /// Generated token to join a team
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// User that owns the team
        /// </summary>
        public virtual ApplicationUser OwnerUser { get; set; }

        /// <summary>
        /// Team's members
        /// </summary>
        public virtual List<ApplicationUser> Members { get; set; }

        /// <summary>
        /// Tournaments in which the team is registered
        /// </summary>
        public virtual List<TeamTournament> Tournaments { get; set; }
    }
}