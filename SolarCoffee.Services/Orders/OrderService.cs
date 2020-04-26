using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventories;
using SolarCoffee.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _solarDb;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDbContext solarDb, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _solarDb = solarDb;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            var now = DateTime.UtcNow;

            _logger.LogInformation("Generate new order");
            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProduct(item.Product.Id);
                var inventoryId = _inventoryService.GetProductById(item.Product.Id).Id;
                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }
            
            try
            {
                _solarDb.SalesOrders.Add(order);
                _solarDb.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Time = now,
                    Data = true,
                    Message = "Open order created"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Time = now,
                    Data = false,
                    Message = ex.StackTrace
                };
            }
        }

        public List<SalesOrder> GetSalesOrders()
        {
            return _solarDb.SalesOrders
                .Include(c => c.Customer)
                .ThenInclude(a => a.PrimaryAddress)
                .Include(s => s.SalesOrderItems)
                .ThenInclude(p => p.Product)
                .ToList();
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;

            try
            {
                var salesOrder = _solarDb.SalesOrders.Find(id);

                if (salesOrder == null)
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = false,
                        Time = now,
                        Data = false,
                        Message = "Sale Order not found!"
                    };
                }

                salesOrder.IsPaid = true;
                salesOrder.UpdatedOn = now;

                _solarDb.Update(salesOrder);
                _solarDb.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Time = now,
                    Data = true,
                    Message = $"Order {salesOrder.Id} closed: Invoice paid in full."
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Time = now,
                    Data = false,
                    Message = ex.StackTrace
                };
            }
        }
    }
}
