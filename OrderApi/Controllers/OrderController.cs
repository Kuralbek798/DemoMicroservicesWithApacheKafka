using Microsoft.AspNetCore.Mvc;
using OrderApi.OrderServices;
using Shared;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Starts the consuming service.
        /// </summary>
        /// <returns>No content if successful.</returns>
        [HttpGet("start-consuming-service")]
        public async Task<IActionResult> StartService()
        {
            try
            {
                await _orderService.StartConsumingService();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while starting the consuming service.");
            }
        }

        /// <summary>
        /// Retrieves the list of products.
        /// </summary>
        /// <returns>The list of products.</returns>
        [HttpGet("get-products")]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _orderService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the products.");
            }
        }

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="order">The order details.</param>
        /// <returns>Created response with order details or bad request.</returns>
        [HttpPost("add-order")]
        public IActionResult AddOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }

            try
            {
                _orderService.AddOrder(order);
                return CreatedAtAction(nameof(AddOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the order.");
            }
        }

        /// <summary>
        /// Retrieves the order summaries.
        /// </summary>
        /// <returns>The list of order summaries.</returns>
        [HttpGet("order-summary")]
        public IActionResult GetOrderSummaries()
        {
            try
            {
                var orderSummaries = _orderService.GetOrderSummaries();
                return Ok(orderSummaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the order summaries.");
            }
        }
    }
}

