using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _solarDb;

        public ProductService(SolarDbContext solarDb)
        {
            _solarDb = solarDb;
        }

        /// <summary>
        /// Archives a product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<Product> ArchiveProduct(int id)
        {
            try
            {
                var product = GetProduct(id);
                product.IsArchived = true;
                _solarDb.SaveChanges();

                return new ServiceResponse<Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = "Archived product"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = ex.StackTrace

                };
            }
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Product> CreateProduct(Product product)
        {
            try
            {
                _solarDb.Add(product);
                var newInvertory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };

                _solarDb.Add(newInvertory);
                _solarDb.SaveChanges();

                return new ServiceResponse<Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = "Saved new product"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = ex.StackTrace
                };

            }
        }

        /// <summary>
        /// Retrives a product by id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            return _solarDb.Products.Find(id);
        }

        /// <summary>
        /// Retrives all products from the database
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            return _solarDb.Products.ToList();
        }
    }
}
