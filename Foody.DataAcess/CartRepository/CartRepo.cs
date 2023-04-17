using Foody.DataAcess.CategoryRepository;
using Foody.DTOs;
using Foody.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.CartRepository
{
    public class CartRepo : ICartRepo
    {
        private readonly IProductRepo _productRepo;
        private readonly AppDbContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepo(IProductRepo productRepo, AppDbContext context, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _productRepo = productRepo;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        private string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userID = _userManager.GetUserId(user);

            return userID;
        }
        public Response<int> AddToCart(int productId, int qty)
        {
            string userId = GetUserId();
            using var scope = _context.Database.BeginTransaction();
            try
            {
                var newCart = GetCart(userId);
                if (newCart == null)
                {
                    newCart = new ShoppingCart
                    {
                        Status = "Active",
                        CustomerId = userId,
                    };
                    _context.ShoppingCarts.Add(newCart);
                }

                _context.SaveChanges();

                var CartDetail = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == newCart.Id && x.ProductId == productId);

                if (CartDetail != null)
                {
                    CartDetail.Quantity += qty;
                }
                else
                {
                    var prod = _context.Products.Find(productId);
                    if (prod != null)
                    {
                        if(qty > prod.Count)
                        {
                            throw new Exception($"the selected product has just {prod.Count} in stock, Kindly reduce the quantity!");
                        }
                        CartDetail = new CartDetail
                        {
                            ProductId = productId,
                            Quantity = qty,
                            Price = prod.Price,
                            ShoppingCartId = newCart.Id,
                            ProductName = prod.Name
                        };
                        _context.CartDetails.Add(CartDetail);
                        prod.Count = prod.Count - qty;
                        _context.Products.Update(prod);
                    }

                }
                _context.SaveChanges();

                scope.Commit();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return new Response<int>
            {
                Message = "Item added successfull",
                Data = GetCartItemCount(userId),
                IsSuccessful = true,
                StatusCode = 200
            };
        }

        public Task<decimal> CalculateTotalAmount(int cartId)
        {
            throw new NotImplementedException();
        }

        public Response<string> ClearCart(int cartId)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _context.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.CustomerId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<Response<int>> RemoveFromCart(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");

                var cartItem = _context.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("No items in cart");
                else if (cartItem.Quantity == 1)
                    _context.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return new Response<int>
            {
                Message = "Item Removed Successfully",
                Data = GetCartItemCount(userId),
                IsSuccessful = true,
                StatusCode = 200
            };

        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    CustomerId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderItem
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    _context.OrderItems.Add(orderDetail);
                }
                _context.SaveChanges();

                // removing the cartdetails
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public int GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = (from cart in _context.ShoppingCarts
                        join cartDetail in _context.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        select new { cartDetail.Id }
                        ).ToList();
            return data.Count;
        }
        public ShoppingCart GetCart(string userId)
        {
            var cart = _context.ShoppingCarts.FirstOrDefault(x => x.CustomerId == userId);
            return cart;
        }
       
    }
}
