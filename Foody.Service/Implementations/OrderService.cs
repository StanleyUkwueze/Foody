using Foody.DataAcess;
using Foody.DataAcess.CategoryRepository;
using Foody.DataAcess.UnitOfWork;
using Foody.DataAcess.UserOrderRepository;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUserOrderRepo _userOrderRepo;
        private readonly AppDbContext _context;
        private readonly IProductRepo _productRepo;
        private readonly IUnitOfWork _unitOfWork;


        public OrderService(IUserOrderRepo userOrderRepo, AppDbContext context, IProductRepo productRepo, IUnitOfWork unitOfWork)
        {
            _userOrderRepo = userOrderRepo;
            _context = context;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<string>> ReInitiateOrder(int orderId)
        {
            var successCount = 0;
           var orderToReinitiate = await _userOrderRepo.GetUserOrderAsync(orderId);

      
            if (orderToReinitiate == null) 
                return new Response<string> { IsSuccessful = false, Message = "Order is not found" };

            if(orderToReinitiate.OrderStatusId != 4) 
                return new Response<string> { IsSuccessful = false, Message = "Order cannot be reinitiated: It is not in a cancelled state" };

            foreach(var orderitem in orderToReinitiate.OrderItems)
            {
                var product = _unitOfWork.ProductRepo.GetFirstOrDefauly(x => x.Id == orderitem.Id);

                if(product == null || product.Count < orderitem.Quantity)
                {
                    return new Response<string> 
                    { IsSuccessful = false, Message = "This Order cannot be reinitiated as one or more the order item is out of stock" };
                }
            }

            //var originalCheckOut = _context.CheckOuts.Find(orderToReinitiate.CheckOutId);
            //_context.CheckOuts.Add(originalCheckOut);

            var newOrder = new Order
            {
                CustomerId = orderToReinitiate.CustomerId,
                IsDeleted = false,
                CheckOutId = orderToReinitiate.CheckOutId,
                AddressId = orderToReinitiate.AddressId,
                TotalPrice = orderToReinitiate.TotalPrice,
                OrderStatusId = orderToReinitiate.OrderStatusId,
                ShippingAddress = orderToReinitiate.ShippingAddress,
            };

            foreach(var orderItem in orderToReinitiate.OrderItems)
            {
                var product = _context.Products.Find(orderItem.ProductId);

                if(product != null)
                {
                    product.Count -= orderItem.Quantity;

                    _context.Products.Update(product);

                    var newOrderItem = new OrderItem
                    {
                        Quantity = orderItem.Quantity,
                        ProductId = orderItem.ProductId,
                        Price = orderItem.Price,   
                    };
                    newOrder.OrderItems.Add(newOrderItem);
                }
            }

            _context.Orders.Remove(orderToReinitiate);
            _context.SaveChanges();

            _context.Orders.Add(newOrder);
           if( await _context.SaveChangesAsync() > 0)
            {
                successCount++;
            }

            newOrder.OrderStatusId = 6;
            newOrder.CheckOutId = orderToReinitiate.CheckOutId;
            _context.Orders.Update(newOrder);
            if (await _context.SaveChangesAsync() > 0)
            {
                successCount++;
            }

            if (successCount >= 2)
                return new Response<string> { Message = $"Order with OrderId:{orderId} has been successfully reinitiated", IsSuccessful = true, StatusCode = 200 };
            
            return new Response<string> { Message = $"Order with OrderId:{orderId} could not be reinitiated", IsSuccessful = true };


        }

        
    }
}
