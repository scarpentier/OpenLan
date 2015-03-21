using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Seat name
        /// </summary>
        [Required]
        public string Name { get; set; }

        public int PosX { get; set; }

        public int PosY { get; set; }

        public SeatStatus Status { get; set; }

        public int? TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public enum SeatStatus
    {
        Free,
        Occupied,
        Reserved,
        NotAvailable
    }
}