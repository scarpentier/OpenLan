using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using Stripe;

namespace OpenLan.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly OpenLanContext _dbContext;

        public CheckoutController(OpenLanContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /Checkout/

        public async Task<IActionResult> AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(_dbContext, Context);
                        
            return View(Convert.ToInt32(await cart.GetTotal() * 100));
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressAndPayment(Order order)
        {
            var cart = ShoppingCart.GetCart(_dbContext, Context);
            var formCollection = await Context.Request.ReadFormAsync();
                
            var charge = new StripeChargeCreateOptions
            {
                Amount = Convert.ToInt32(await cart.GetTotal() * 100),
                Currency = "cad",
                Card = new StripeCreditCardOptions
                {
                    TokenId = formCollection.GetValues("stripeToken").FirstOrDefault()
                }
            };

            // TODO: Read this from configuration
            var chargeService = new StripeChargeService("sk_test_VD5Xw4EehScXQdYV0fujg6Nr");
            var stripeCharge = chargeService.Create(charge);

            order.UserId = Context.User.Identity.GetUserId();
            order.OrderDate = DateTime.Now;

            // Add the Order
            await _dbContext.Orders.AddAsync(order, Context.RequestAborted);

            // Process the order
            await _dbContext.SaveChangesAsync(Context.RequestAborted);

            // Get the order's content
            foreach (var item in await cart.GetCartItems())
            {
                // If the product is a ticket, add it to the user's account
                TicketType tt;
                switch (item.ProductId) {
                    case "lanbyoc":
                        tt = TicketType.BYOC;
                        break;
                    case "lanbyocvip":
                        tt = TicketType.BYOCVIP;
                        break;
                    case "lanconsole":
                        tt = TicketType.Console;
                        break;
                    case "lanvisitor":
                        tt = TicketType.Visitor;
                        break;
                    default:
                        continue;
                }

                var ticket = new Ticket
                {
                    OrderId = order.Id,
                    TicketType = tt,
                    UserOwnerId = User.Identity.GetUserId()
                };
                await _dbContext.Tickets.AddAsync(ticket);
            }

            // Save all changes
            await _dbContext.SaveChangesAsync(Context.RequestAborted);

            // Empty the cart
            cart.EmptyCart();

            return RedirectToAction("Complete", new { id = order.Id });
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