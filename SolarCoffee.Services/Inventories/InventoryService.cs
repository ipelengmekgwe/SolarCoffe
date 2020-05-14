using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _solarDb;
        private readonly ILogger<InventoryService> _logger;
        public InventoryService(SolarDbContext solarDb, ILogger<InventoryService> logger)
        {
            _solarDb = solarDb;
            _logger = logger;
        }

        public List<ProductInventory> GetProductInventories()
        {
            return _solarDb.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public ProductInventory GetProductById(int id)
        {
            return _solarDb.ProductInventories.Include(p => p.Product).FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Returns Snapshot history for the previous 6 hours
        /// </summary>
        /// <param name="productInventory"></param>
        /// <returns></returns>
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _solarDb.ProductInventorySnapshots
                .Include(p => p.Product)
                .Where(p => p.SnapshotTime > earliest && !p.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Update number of units available of the provided product id
        /// Adjust QuantityOnHand by adjustment value
        /// </summary>
        /// <param name="id">productId</param>
        /// <param name="adjustment">number of units added / removed from inventory</param>
        /// <returns>ServiceResponse<ProductInventory></returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            var now = DateTime.UtcNow;

            try
            {
                var inventory = _solarDb.ProductInventories
                    .Include(pi => pi.Product)
                    .First(pi => pi.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError(ex.StackTrace);
                }

                _solarDb.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Time = now,
                    Data = inventory,
                    Message = $"Product {id} inventory adjusted"
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Time = now,
                    Data = null,
                    Message = ex.StackTrace
                };
            }
        }

        private void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;
            var spanshot = new ProductInventorySnapshot
            {
                SnapshotTime = now,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            _solarDb.Add(spanshot);
        }
    }
}
