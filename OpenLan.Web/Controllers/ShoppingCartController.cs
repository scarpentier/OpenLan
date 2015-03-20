using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using OpenLan.Web.Models;
using OpenLan.Web.ViewModels;

namespace OpenLan.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly OpenLanContext _dbContext;

        public ShoppingCartController(OpenLanContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /ShoppingCart/

        public async Task<IActionResult> Index()
        {
            var cart = ShoppingCart.GetCart(_dbContext, Context);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems(),
                CartTotal = await cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public async Task<IActionResult> AddToCart(string id)
        {
            // Retrieve the product from the database
            var addedProduct = _dbContext.Products
                .Single(product => product.Id == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(_dbContext, Context);

            cart.AddToCart(addedProduct);

            await _dbContext.SaveChangesAsync(Context.RequestAborted);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var formParameters = await Context.Request.ReadFormAsync();
            var requestVerification = formParameters["RequestVerificationToken"];
            string cookieToken = null;
            string formToken = null;

            if (!string.IsNullOrWhiteSpace(requestVerification))
            {
                var tokens = requestVerification.Split(':');

                if (tokens != null && tokens.Length == 2)
                {
                    cookieToken = tokens[0];
                    formToken = tokens[1];
                }
            }

            var antiForgery = Context.RequestServices.GetService<AntiForgery>();
            antiForgery.Validate(Context, new AntiForgeryTokenSet(formToken, cookieToken));

            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(_dbContext, Context);

            // Get the name of the product to display confirmation
            var cartItem = await _dbContext.CartItems
                .Where(item => item.Id == id)
                .Include(c => c.Product)
                .SingleOrDefaultAsync();

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            await _dbContext.SaveChangesAsync(Context.RequestAborted);

            string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = removed + cartItem.Product.Name +
                    " has been removed from your shopping cart.",
                CartTotal = await cart.GetTotal(),
                CartCount = await cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }
    }
}