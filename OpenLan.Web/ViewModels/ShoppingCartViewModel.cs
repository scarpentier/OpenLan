using System.Collections.Generic;
using OpenLan.Web.Models;

namespace OpenLan.Web.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ICollection<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}