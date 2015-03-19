using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        /// <summary>
        /// User that initiated the order
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        public decimal Total { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }
}