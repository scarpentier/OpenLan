using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        /// <summary>
        /// User that initiated the transaction
        /// </summary>
        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public List<Ticket> Tickets { get; set; }
    }
}