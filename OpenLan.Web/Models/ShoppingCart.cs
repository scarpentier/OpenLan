using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenLan.Web.Models
{
    public partial class ShoppingCart
    {
        OpenLanContext _db;
        string ShoppingCartId { get; set; }

        public ShoppingCart(OpenLanContext db)
        {
            _db = db;
        }

        public static ShoppingCart GetCart(OpenLanContext db, HttpContext context)
        {
            var cart = new ShoppingCart(db);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public void AddToCart(Product product)
        {
            var cartItem = _db.CartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _db.CartItems.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.Id == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _db.CartItems.Remove(cartItem);
                }
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _db.CartItems.Where(cart => cart.CartId == ShoppingCartId);
            _db.CartItems.RemoveRange(cartItems);
        }

        public List<CartItem> GetCartItems()
        {
            var cartItems = _db.CartItems.Where(cart => cart.CartId == ShoppingCartId).ToList();
            //TODO: Auto population of the related product data not available until EF feature is lighted up.
            foreach (var cartItem in cartItems)
            {
                cartItem.Product = _db.Products.Single(a => a.Id == cartItem.Id);
            }

            return cartItems;
        }

        public int GetCount()
        {
            int sum = 0;
            //https://github.com/aspnet/EntityFramework/issues/557
            // Get the count of each item in the cart and sum them up
            var cartItemCounts = (from cartItems in _db.CartItems
                                  where cartItems.CartId == ShoppingCartId
                                  select (int?)cartItems.Count);

            cartItemCounts.ForEachAsync(carItemCount =>
            {
                if (carItemCount.HasValue)
                {
                    sum += carItemCount.Value;
                }
            });

            // Return 0 if all entries are null
            return sum;
        }

        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total

            // TODO Collapse to a single query once EF supports querying related data
            decimal total = 0;
            foreach (var item in _db.CartItems.Where(c => c.CartId == ShoppingCartId))
            {
                var product = _db.Products.Single(a => a.Id == item.Id);
                total += item.Count * product.Price;
            }

            return total;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var product = _db.Products.Single(a => a.Id == item.ProductId);

                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    UnitPrice = product.Price,
                    Quantity = item.Count,
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * product.Price);

                _db.OrderDetails.Add(orderDetail);
            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return order.Id;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContext context)
        {
            var sessionCookie = context.Request.Cookies.Get("Session");
            string cartId = null;

            if (string.IsNullOrWhiteSpace(sessionCookie))
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();

                // Send cart Id as a cookie to the client.
                context.Response.Cookies.Append("Session", cartId);
            }
            else
            {
                cartId = sessionCookie;
            }

            return cartId;
        }
    }
}