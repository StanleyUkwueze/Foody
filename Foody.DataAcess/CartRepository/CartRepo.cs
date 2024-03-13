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
            var successCount = 0;
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
                    if (_context.SaveChanges() > 0)
                    {
                        successCount++;
                    }
                }

                var CartDetail = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == newCart.Id && x.ProductId == productId);

                var prod = _context.Products.Find(productId);
              if(prod != null)
              {
                if (CartDetail != null)
                {
                    if (qty > prod.Count)
                    {
                        return new Response<int> { IsSuccessful = false, Message = $"The selected product has {prod.Count} quantity in stock, Kindly reduce the quantity!", StatusCode = 400 };
                    }
                    CartDetail.Quantity += qty;
                }
                else
                {
                     
                    if (prod != null)
                    {
                        if(qty > prod.Count || prod.Count <= 0)
                        {
                            return new Response<int> { IsSuccessful = false, Message = $"The selected product has just {prod.Count} in stock, Kindly reduce the quantity!", StatusCode = 400 };
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
                        newCart.TotalPrice = CartDetail.Price*qty;

                        _context.ShoppingCarts.Update(newCart);
                        _context.Products.Update(prod);

                        if (_context.SaveChanges() > 0)
                        {
                            successCount++;
                        }
                    }

                }
               

                if(newCart != null)
                {
                        if (successCount >= 1)
                        {
                            scope.Commit();
                            return new Response<int> { Message = "Item added successfully", Data = GetCartItemCount(userId), IsSuccessful = true, StatusCode = 200 };
                        }
                   
                        else
                        { 
                            return new Response<int> { Message = $"Item not added successfully", IsSuccessful = false, StatusCode = 400 };

                        }
                }
                else
                {
                    if (successCount >= 1)
                    {
                        scope.Commit();
                        return new Response<int> { Message = "Item added successfully", Data = GetCartItemCount(userId), IsSuccessful = true, StatusCode = 200 };
                    }
                }
              }
              else
              {
                return new Response<int> { Message = $"Item not added successfully: No product was found with the provided Id: {productId}", IsSuccessful = false, StatusCode = 400 };

              }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return new Response<int>
            {
                Message = "Oops! Item could not be added to the Cart",
                IsSuccessful = false,
                StatusCode = 500
            };
        }

        public Task<decimal> CalculateTotalAmount(int cartId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> ClearCart(int cartId)
        {
            var cart = _context.ShoppingCarts.FirstOrDefault(x => x.Id == cartId);

            if(cart != null)
            {
                if (cart.CartDetails.Count > 0)
                {
                    cart.CartDetails.RemoveRange(0, cart.CartDetails.Count);
                   await _context.SaveChangesAsync();

                    return new Response<string>
                    { Message = "Cart successfully cleared", IsSuccessful = true, StatusCode = 200 };
                }
                return new Response<string>
                { Message = "No item found on the cart", IsSuccessful = false, StatusCode = 204 };
            }
            return new Response<string>
            { Message = "This is an empty cart", IsSuccessful = false, StatusCode = 400 };

        }

        public async Task<Response<ShoppingCart>> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null) return new Response<ShoppingCart> { Message = "Invalid userid", IsSuccessful = false, StatusCode = 400 };



            var shoppingCart = await _context.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.CustomerId == userId).FirstOrDefaultAsync();
            if(shoppingCart == null) return new Response<ShoppingCart> { Message = "User has no shoppingCart at the moment.", IsSuccessful = false, StatusCode = 400 };

            return new Response<ShoppingCart> {  IsSuccessful = true, StatusCode = 200, Data = shoppingCart };

        }
        public async Task<Response<int>> RemoveFromCart(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId)) return new Response<int> { Message = "user is not logged-in", IsSuccessful = false };
                    
                var cart = GetCart(userId);
                if (cart is null)
                    return new Response<int> { Message = "USser has no cart tied to him.", IsSuccessful = false };
                var cartItem = await _context.CartDetails
                                  .FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null) return new Response<int> { Message = "This shoppingcart is empty", IsSuccessful = false };
                else if (cartItem.Quantity == 1)
                    _context.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
               await _context.SaveChangesAsync();
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

        public async Task<Response<bool>> DoCheckout()
        {
            
            try
            {
                 var transaction = _context.Database.BeginTransaction();
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return new Response<bool> { Message = "user is not logged-in", IsSuccessful = false };
                var cart = GetCart(userId);
                if (cart is null) return new Response<bool> { Message = "USser has no cart tied to him.", IsSuccessful = false };

                var cartDetail = await _context.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToListAsync();
                if (cartDetail.Count == 0)
                    return new Response<bool> { Message = "This shoppingcart is empty", IsSuccessful = false };
                //var cust = _userManager.FindByEmailAsync(userId).Result;
                var orderStatus = await _context.OrderStatus
       .FirstOrDefaultAsync(x => x.StatusName == "Pending");
                var order = new Order
                {
                    CustomerId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = orderStatus.Id 
                };
                
                await _context.Orders.AddAsync(order);
                
               await _context.SaveChangesAsync();
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
                return new Response<bool> { Message = "Successfully Checked out", IsSuccessful = true, StatusCode =200};
            }
            catch (Exception ex)
            {
                return new Response<bool> { Message = $"The following Exception Occurred: ${ex.Message.ToString()}", IsSuccessful = false, StatusCode = 400 };

            }
           
        }
        public int GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
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
