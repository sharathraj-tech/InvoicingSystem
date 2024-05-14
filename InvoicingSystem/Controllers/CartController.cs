using InvoicingSystem.Models;
using InvoicingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public CartController(CartService cartService, ProductService productService, CustomerService customerService)
        {
            _cartService = cartService;
            _productService = productService;
            _customerService = customerService;
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCart(int customerId)
        {
            var cart = _cartService.GetCart(customerId);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost("add")]
        public IActionResult AddToCart(int customerId,int productId, int quantity=1)
        {
            try
            {
                var availableProductCount = _productService.GetRemainigProductQuantity(productId);

                if (!_customerService.CustomerExists(customerId))
                {
                    return BadRequest($"No Customer Details found with given ID: {customerId}");
                }

                if (!_productService.ProductExists(productId))
                {
                    return BadRequest($"No Product found with given ID: {productId}");
                }

                
                if((availableProductCount - quantity)<0)
                {
                    return BadRequest($"Requested quantity not available for the product. Only {availableProductCount} are in stock.");
                }

                var productDetails=_productService.GetProductById(productId);

                _cartService.AddToCart(customerId,productId,quantity,productDetails.Price);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{customerId}/update/{productId}")]
        public IActionResult UpdateCartItemQuantity(int customerId, int productId, [FromQuery] int quantity)
        {
            try
            {
                _cartService.UpdateCartItemQuantity(customerId, productId, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{customerId}/remove/{productId}")]
        public IActionResult RemoveFromCart(int customerId, int productId, [FromQuery] int quantity = 1)
        {
            _cartService.RemoveFromCart(customerId, productId, quantity);
            return Ok();
        }

        [HttpDelete("{customerId}/clear")]
        public IActionResult ClearCart(int customerId)
        {
            _cartService.ClearCart(customerId);
            return Ok();
        }
    }
}
