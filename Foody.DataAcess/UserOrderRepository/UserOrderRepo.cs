using Foody.DTOs;
using Foody.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.UserOrderRepository
{
    public class UserOrderRepo : IUserOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderRepo(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<Order>> PlaceOrderAsync(PlaceOrderRequestModel PlaceOrderRequestModel)
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = await _context.ShoppingCarts
                                       .Include(sc => sc.CartDetails)
                                       .ThenInclude(ci => ci.Product)
                                       .FirstOrDefaultAsync(sc => sc.CustomerId == userId);
            var scope = _context.Database.BeginTransaction();

            var Address = new Address
            {
                State = PlaceOrderRequestModel.State,
                City = PlaceOrderRequestModel.City,
                Town = PlaceOrderRequestModel.Town,
                ZipCode = PlaceOrderRequestModel.ZipCode,
                CustomerId = userId,
                Street = PlaceOrderRequestModel.Street,
               // Orders = new List<Order>()
            };


            var checkout = new CheckOut
            {
               PaymentMethod = PlaceOrderRequestModel.PaymentMethod,
               TotalPrice = PlaceOrderRequestModel.TotalPrice,
            };

            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = shoppingCart.TotalPrice,
                OrderStatusId = 5,
                OrderItems = new List<OrderItem>(),
                ShippingAddress = Address,
                CheckOut = checkout
            };

            foreach (var cartItem in shoppingCart.CartDetails)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.Product.Id,
                    Product = cartItem.Product,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                order.OrderItems.Add(orderItem);
            }
            

            _context.Orders.Add(order);
             var i =  await _context.SaveChangesAsync();
            scope.Commit();
            if (i > 0)
            {
                //send a mail to the customer
                return new Response<Order> { Data = order, Message = "Order successfully placed", IsSuccessful = true };
            }

            return new Response<Order> {  Message = "Order failed to be placed", IsSuccessful = false };
        }


        public async Task<Response<string>> CancelOrder(int orderId)
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .SingleOrDefaultAsync(o => o.Id == orderId && o.CustomerId == userId);

            if (order == null)
            {
                return new Response<string>
                {Message = $"Order with ID {orderId} not found or does not belong to user {userId}",
                IsSuccessful =false,
                };
            }
            if(order.OrderStatus.StatusId != 5)
            {
                return new Response<string>
                {
                    Message = $"Order with ID {orderId} cannot be cancelled because  it's status is not placed",
                    IsSuccessful = false,
                };
            }
            order.DateCancelled = DateTime.Now;
            order.OrderStatus.StatusId = 4;

            foreach (var orderItem in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(orderItem.ProductId);
                product.Count += orderItem.Quantity;

                _context.Products.Update(product);
            }

           var i = await _context.SaveChangesAsync();
            if(i> 0)
            {
                return new Response<string> { IsSuccessful = true, Message = $"Successfully cancelled order with ID {orderId}" };
            }
            return new Response<string> { IsSuccessful = false, Message = $"Order cancelation failed" };
        }

    }
}
