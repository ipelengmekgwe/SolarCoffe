using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Products;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public ActionResult GetProducts()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetProducts();

            var productModels = products.Select(ProductMapper.SerializeProductModel);
            return Ok(productModels);
        }

        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving product");
            var archiveResult = _productService.ArchiveProduct(id);
            return Ok(archiveResult);
        }

        [HttpPost("/api/product")]
        public ActionResult AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Adding a new product");
            var newProduct = ProductMapper.SerializeProductModel(product);
            var newProductResponse = _productService.CreateProduct(newProduct);
            return Ok(newProductResponse);
        }
    }
}
