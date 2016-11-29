using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MileStone2A.Models
{
    public class ShoppingCart
    {
        BikeStoreEntities storeDb = new BikeStoreEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            var cartItem = storeDb.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
            && c.ProductId == product.ProductID);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.ProductID,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDb.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            storeDb.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = storeDb.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

            int itemCount = 0;
            if(cartItem != null)
            {
                if(cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDb.Carts.Remove(cartItem);
                }
                storeDb.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDb.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDb.Carts.Remove(cartItem);
            }
            // Save changes
            storeDb.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return storeDb.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDb.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in storeDb.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.ListPrice).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.ListPrice,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Product.ListPrice);

                storeDb.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            storeDb.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDb.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDb.SaveChanges();
        }

    }
}