using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLan.Web.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}