using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public TicketType TicketType { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// Origin order for the ticket
        /// </summary>
        public virtual Order Order { get; set; }

        public virtual string UserOwnerId { get; set; }

        public virtual ApplicationUser UserOwner { get; set; }

        public string UserAssignedId { get; set; }

        /// <summary>
        /// User for which the ticket is applied
        /// </summary>
        public virtual ApplicationUser UserAssigned { get; set; }

        public int? SeatId { get; set; }

        public virtual Seat Seat { get; set; }
    }

    public enum TicketType
    {
        BYOC,
        BYOCVIP,
        Console,
        Visitor
    }
}