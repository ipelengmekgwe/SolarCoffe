using System;
using System.Collections.Generic;
using System.Text;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Products
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
        ServiceResponse<Product> CreateProduct(Product product);
        ServiceResponse<Product> ArchiveProduct(int id);
    }
}
