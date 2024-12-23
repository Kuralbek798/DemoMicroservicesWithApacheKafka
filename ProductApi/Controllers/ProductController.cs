using Microsoft.AspNetCore.Mvc;
using ProductApi.ProductServices;
using Shared;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Adds a new product to the catalog.
        /// </summary>
        /// <param name="product">The product details.</param>
        /// <returns>Created response with product details or bad request.</returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            try
            {
                await _productService.AddProduct(product);
                return CreatedAtAction(nameof(AddProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                // Log exception (not shown for brevity)
                return StatusCode(500, "An error occurred while adding the product.");
            }
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>No content if successful, not found if the product does not exist.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception (not shown for brevity)
                return StatusCode(500, "An error occurred while deleting the product.");
            }
        }
    }
}

