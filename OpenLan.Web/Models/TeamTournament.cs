using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class TeamTournament
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ClanId { get; set; }

        public virtual Clan Clan { get; set; }

        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<ApplicationUser> AssignedMembers { get; set; }
    }
}