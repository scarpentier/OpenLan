using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        /// <summary>
        /// Origin order for the ticket
        /// </summary>
        [Required]
        public Order Order { get; set; }

        /// <summary>
        /// User for which the ticket is applied
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Seat for which the ticket is applied
        /// </summary>
        public Seat Seat { get; set; }
    }
}