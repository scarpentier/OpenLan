using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Clan
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the clan
        /// </summary>
        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// URL to the clan's website
        /// </summary>
        [DataType(DataType.Url)]
        public string Url { get; set; }

        [DataType(DataType.Url)]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Clan's tagline
        /// </summary>
        public string Tagline { get; set; }

        /// <summary>
        /// Generated token to join a clan
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// User that owns the clan
        /// </summary>
        public virtual ApplicationUser OwnerUser { get; set; }

        /// <summary>
        /// Clan's members
        /// </summary>
        public virtual ICollection<ApplicationUser> Members { get; set; }

        /// <summary>
        /// Tournaments in which the clan is registered
        /// </summary>
        public virtual ICollection<TeamTournament> Tournaments { get; set; }

        public Clan()
        {
            DateCreated = DateTime.UtcNow;
            Token = Guid.NewGuid().ToString();
        }
    }
}