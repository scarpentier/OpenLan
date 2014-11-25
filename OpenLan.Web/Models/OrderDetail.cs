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

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}