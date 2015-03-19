using System;
using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OpenLan.Web.Components
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryComponent : ViewComponent
    {
        private readonly OpenLanContext db;

        public CartSummaryComponent(OpenLanContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartItems = await GetCartItems();

            ViewBag.CartCount = cartItems.Count();
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());

            return View();
        }

        private async Task<IOrderedEnumerable<string>> GetCartItems()
        {
            var cart = ShoppingCart.GetCart(db, Context);

            return (await cart.GetCartItems())
                .Select(ci => ci.Product.Name)
                .OrderBy(x => x);
        }
    }
}