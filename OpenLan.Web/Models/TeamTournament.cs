using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class TeamTournament
    {
        [Key]
        public int Id { get; set; }

        public virtual Team Team { get; set; }

        public virtual Tournament Tournament { get; set; }
    }
}