﻿using Foody.DTOs;
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
        private string GetUserId()
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId ?? string.Empty;
        }

        private async Task<ShoppingCart> GetUserCart(string userId )
        {
         
            var shoppingCart = await _context.ShoppingCarts
                                       .Include(sc => sc.CartDetails)
                                       .ThenInclude(ci => ci.Product)
                                       .FirstOrDefaultAsync(sc => sc.CustomerId == userId);
            return shoppingCart;
        }

        public async Task<Order> GetUserOrderAsync(int orderId)
        {
            var userId = GetUserId();
            var order = await _context.Orders.Include(x=>x.OrderItems)
                         .FirstOrDefaultAsync(x=>x.Id == orderId && x.CustomerId == userId );
            return order;
        }

        public async Task<Response<Order>> PlaceOrderAsync(PlaceOrderRequestModel PlaceOrderRequestModel)
        {
            var userId = GetUserId();
            
            var shoppingCart = await GetUserCart(userId);

            var scope = _context.Database.BeginTransaction();

            var Address = new Address
            {
                State = PlaceOrderRequestModel.State,
                City = PlaceOrderRequestModel.City,
                Town = PlaceOrderRequestModel.Town,
                ZipCode = PlaceOrderRequestModel.ZipCode,
                CustomerId = userId,
                Street = PlaceOrderRequestModel.Street,
            };
             _context.Addresses.Add(Address);
            await Save();

            var orderstatus = _context.OrderStatus.FirstOrDefault(o => o.StatusName == "Placed");
            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = shoppingCart.TotalPrice,
                OrderStatusId = orderstatus.Id,
                ShippingAddress = Address, 
                PaymentMethod = PlaceOrderRequestModel.PaymentMethod,
            };
           

            if(shoppingCart.CartDetails.Count > 0)
            {

           
                foreach (var cartItem in shoppingCart.CartDetails)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.Product.Id,
                        Product = cartItem.Product,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Product.Price
                    };
                    order.TotalPrice += orderItem.Price;
                    order.OrderItems.Add(orderItem);
                }

                   var i =  await Add(order);

                if (i)
                {
                    //remove items from the cart
                    _context.CartDetails.RemoveRange(shoppingCart.CartDetails);
                    _context.SaveChanges();
                    scope.Commit();

                    // notify the admin via email service
                    return new Response<Order> { Data = order, Message = "Order successfully placed", IsSuccessful = true, StatusCode = 200 };
                }

                return new Response<Order> {  Message = "Order failed to be placed", IsSuccessful = false };
            }
            return new Response<Order> { Message = "Kindly add items to your shopping cart before attempting placing an order", IsSuccessful = false };
        }


        public async Task<Response<string>> CancelOrder(int orderId)
        {
            var userId = GetUserId();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(s=>s.OrderStatus)
                .SingleOrDefaultAsync(o => o.Id == orderId && o.CustomerId == userId);

            if (order == null)
            {
                return new Response<string>
                {Message = $"Order with ID {orderId} not found or does not belong to user {userId}",
                IsSuccessful =false,
                };
            }
            if(order.OrderStatusId != 5)
            {
                return new Response<string>
                {
                    Message = $"Order with ID {orderId} cannot be cancelled because  it's status is not placed",
                    IsSuccessful = false,
                };
            }
            order.DateCancelled = DateTime.Now;
            order.OrderStatusId = 4;
            order.IsDeleted = true;
            _context.Orders.Update(order);

            foreach (var orderItem in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(orderItem.ProductId);
                product.Count += orderItem.Quantity;

               await Update(order);
            }

            if (await Save())
            {
                // notify the admin via email service that this order has been cancelled
                return new Response<string> { IsSuccessful = true, Message = $"Successfully cancelled order with ID {orderId}", StatusCode = 200 };
            }
            return new Response<string> { IsSuccessful = false, Message = $"Order cancelation failed" };
        }

        public async Task<Response<string>> MarkOrderAsDelivered(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return new Response<string> { IsSuccessful = false, Message = $"Order with the provided id: {orderId} is not found." };


            if(order.OrderStatusId == 4 || order.IsDeleted) return new Response<string> { Message = "This order was cancelled", IsSuccessful=false, StatusCode = 400 };

            if(order.OrderStatusId == 2) return new Response<string> { Message = "This order had already been marked delered", IsSuccessful = false,StatusCode = 4000  };
            order.Delivered = DateTime.Now;
            order.OrderStatusId = 2;

          var update = await Update(order);
         
           if (update)
            {
                return new Response<string> { IsSuccessful = true, Message = "Order Successfully Marked as Delivered", StatusCode = 200 };
            }
            return new Response<string> { IsSuccessful = false, Message = "Marking order as Delivered failed", StatusCode = 500 };
        }

        public async Task<bool> Remove(Order entity)
        {
            _context.Orders.Remove(entity);
            return await Save();
        }

        public async Task<bool> Add(Order entity)
        {
            _context.Orders.Add(entity);
           return await Save();
        }
        public async Task<bool> Update(Order entity)
        {
            _context.Orders.Update(entity);
           return await Save();
        }
        public async Task<bool> Save()
        {
          return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
