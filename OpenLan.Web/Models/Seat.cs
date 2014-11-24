using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        /// <summary>
        /// Seat name
        /// </summary>
        [Required]
        public string Name { get; set; }

        public int PosX { get; set; }

        public int PosY { get; set; }

        public SeatStatus Status { get; set; }

        public Ticket Ticket { get; set; }
    }

    public enum SeatStatus
    {
        Free,
        Occupied,
        Reserved,
        NotAvailable
    }
}