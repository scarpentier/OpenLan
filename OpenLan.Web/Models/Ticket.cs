using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Origin order for the ticket
        /// </summary>
        public virtual Order Order { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// User for which the ticket is applied
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        public int SeatId { get; set; } // TODO: Change for nullable type

        /// <summary>
        /// Seat for which the ticket is applied
        /// </summary>
        public virtual Seat Seat { get; set; }
    }
}