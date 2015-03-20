using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string CartId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}