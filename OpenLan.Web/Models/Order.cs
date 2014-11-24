using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace OpenLan.Web.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        [BindNever]
        public List<OrderDetail> OrderDetails { get; set; }
    }
}