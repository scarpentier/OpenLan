using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Product Product { get; set; }

        public Order Order { get; set; }
    }
}