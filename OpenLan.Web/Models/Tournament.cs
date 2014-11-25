using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the tournament
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Summary of what the tournament is about
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Picture to accompany the tournament
        /// </summary>
        [DataType(DataType.Url)]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Url to the actual external tournament management system (BinaryBeast)
        /// </summary>
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public virtual List<TeamTournament> Teams { get; set; }
    }
}