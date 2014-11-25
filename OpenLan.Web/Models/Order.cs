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

        /// <summary>
        /// User that initiated the order
        /// </summary>
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        [BindNever]
        public virtual List<OrderDetail> OrderDetails { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }
}