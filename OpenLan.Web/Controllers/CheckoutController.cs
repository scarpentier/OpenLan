using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private const string PromoCode = "FREE";
        private readonly OpenLanContext _dbContext;

        public CheckoutController(OpenLanContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /Checkout/

        public IActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressAndPayment(Order order)
        {
            var formCollection = await Context.Request.ReadFormAsync();

            try
            {
                if (string.Equals(formCollection.GetValues("PromoCode").FirstOrDefault(), PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.UserId = Context.User.Identity.GetUserId();
                    order.OrderDate = DateTime.Now;

                    //Add the Order
                    await _dbContext.Orders.AddAsync(order, Context.RequestAborted);

                    //Process the order
                    var cart = ShoppingCart.GetCart(_dbContext, Context);
                    await cart.CreateOrder(order);

                    // Save all changes
                    await _dbContext.SaveChangesAsync(Context.RequestAborted);

                    return RedirectToAction("Complete", new { id = order.Id });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete

        public async Task<IActionResult> Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = await _dbContext.Orders.AnyAsync(
                o => o.Id == id &&
                o.UserId == Context.User.Identity.GetUserId());

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}