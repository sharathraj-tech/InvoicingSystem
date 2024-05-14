using InvoicingSystem.Models;
using InvoicingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }

        [HttpPost("generate-invoice")]
        public IActionResult GenerateInvoice(int customerId, PaymentMethod paymentMethod)
        {
            try
            {
                var invoice = _invoiceService.GenerateInvoice(customerId, paymentMethod);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-customer-invoice")]
        public IActionResult GenerateCustomerInvoice(int customerId)
        {
            try
            {
                var invoice = _invoiceService.GenerateCustomerInvoice(customerId);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("apply-discount-to-product")]
        public IActionResult ApplyDiscountToProduct(int customerId, int productId,decimal discountPercentage)
        {
            try
            {
                _invoiceService.AddDiscountToProduct(customerId, productId, discountPercentage);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("apply-discount-to-cart")]
        public IActionResult ApplyDiscountToCart(int customerId, decimal discountPercentage)
        {
            try
            {
                _invoiceService.AddDiscountToCart(customerId, discountPercentage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("apply-tax")]
        public IActionResult ApplyTaxToCart(int customerId, decimal taxPercentage)
        {
            try
            {
                _invoiceService.ApplyTax(customerId, taxPercentage);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
