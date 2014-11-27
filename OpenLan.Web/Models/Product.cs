using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Url)]
        public string PictureUrl { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

        [ScaffoldColumn(false)]
        [BindNever]
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// TODO: Temporary hack to populate the orderdetails until EF does this automatically. 
        /// </summary>
        public Product()
        {
            OrderDetails = new List<OrderDetail>();
            Created = DateTime.UtcNow;
        }
    }
}