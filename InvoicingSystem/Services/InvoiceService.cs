using InvoicingSystem.Models;

namespace InvoicingSystem.Services
{
    public class InvoiceService
    {
        private readonly List<Invoice> _invoices = new List<Invoice>();
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly CartService _cartService;
        public InvoiceService(ProductService productService,
            CustomerService customerService,
            CartService cartService)
        {
            _productService = productService;
            _customerService = customerService;
            _cartService = cartService;
        }

        public Invoice GenerateInvoice(int customerId, PaymentMethod paymentMethod=PaymentMethod.CreditCard)
        {
            try
            {
                var cartDetails = _cartService.GetCart(customerId);
                if(cartDetails==null)
                {
                    throw new ArgumentException("Cart details not found for the customer!");
                }
                var customerDetails = _customerService.GetCustomerById(customerId);
                var invoiceItems = cartDetails.Items.Select(cartItem => new InvoiceItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = _productService.GetProductById(cartItem.ProductId).Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = _productService.GetProductById(cartItem.ProductId).Price,
                    Discount = cartItem.Discount,
                }).ToList();

                var invoice = new Invoice
                {
                    CustomerId = customerId,
                    CustomerName = customerDetails.Name,
                    CustomerEmail = customerDetails.Email,
                    CustomerContactNumber = customerDetails.ContactNumber,
                    CustomerAddress = customerDetails.Address,
                    PaymentMethod = paymentMethod,
                    Items = invoiceItems,
                    Subtotal = cartDetails.Subtotal,
                    Total = cartDetails.Total,
                    Discount = cartDetails.Discount != 0 ? cartDetails.Discount : cartDetails.Items.Sum(item => item.Discount),
                    Tax = cartDetails.Tax,
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                };

                _invoices.Add(invoice);
                //_cartService.ClearCart(customerId);

                return invoice;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public List<Invoice> GenerateCustomerInvoice(int customerId)
        {
            var invoiceDetails=_invoices.Where(x=>x.CustomerId== customerId).ToList();
            if(invoiceDetails.Count>0)
            {
                return invoiceDetails;
            }
            else
            {
                throw new ArgumentException("No invoice details found for the custoemr.");
            }
        }

        public List<Invoice> GenerateCustomerInvoiceById(Guid guid)
        {
            var invoiceDetails = _invoices.Where(x => x.Id == guid).ToList();
            if (invoiceDetails.Count > 0)
            {
                return invoiceDetails;
            }
            else
            {
                throw new ArgumentException($"No invoice details found for the ID {guid}.");
            }
        }


        public void AddDiscountToProduct(int customerId, int productId, decimal discountPercentage)
        {
            try
            {
                if (discountPercentage <=0 || discountPercentage > 100)
                {
                    throw new ArgumentException("Discount given cannot be less than zero and greater than 100.");
                }
                var cartItems = _cartService.GetCartItems(customerId);
                var cartItem = cartItems.FirstOrDefault(p => p.ProductId == productId);
                if (cartItem != null)
                {
                    decimal unitPrice = cartItem.Price;
                    decimal discount = discountPercentage / 100;
                    decimal discountAmount = unitPrice * discount;
                    decimal finalPrice = unitPrice - discountAmount;
                    if (discountAmount < 0)
                    {
                        throw new ArgumentException("Discount amount cannot be negative.");
                    }
                    if (finalPrice < 0)
                    {
                        throw new ArgumentException("Discount given cannot be more than the actual price.");
                    }
                    cartItem.Discount = discountAmount;
                    _cartService.UpdateCartTotal(_cartService.GetCart(customerId));
                }
                else
                {
                    throw new ArgumentException($"No product found in the cart for ID :{productId}");
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);
            }
        }

        public void AddDiscountToCart(int customerId, decimal discountPercentage)
        {
            try
            {
                if (discountPercentage <=0 || discountPercentage>100)
                {
                    throw new ArgumentException("Discount given cannot be less than zero and greater than 100.");
                }
                var cartItems = _cartService.GetCart(customerId);
                if (cartItems != null)
                {
                    decimal totalCartValue = cartItems.Total;
                    decimal discount = discountPercentage / 100;
                    decimal discountAmount = totalCartValue * discount;
                    decimal finalPrice = totalCartValue - discountAmount;
                    if (discountAmount < 0)
                    {
                        throw new ArgumentException("Discount amount cannot be negative.");
                    }
                    if (finalPrice < 0)
                    {
                        throw new ArgumentException("Discount given cannot be more than the actual price.");
                    }
                    cartItems.Discount = discountAmount;
                    _cartService.UpdateCartTotal(cartItems);
                }
                else
                {
                    throw new ArgumentException($"No product found in the cart for ID :{customerId}");
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);
            }
        }

        public void ApplyTax(int customerId, decimal taxPercentage)
        {
            try
            {
                if(taxPercentage <=0 || taxPercentage>100)
                {
                    throw new ArgumentException("Tax % cannot be less than zero and greater than 100.");
                }
                var cartItems = _cartService.GetCart(customerId);
                if (cartItems != null)
                {
                    decimal totalCartValue = cartItems.Total;
                    decimal taxValue = taxPercentage / 100;
                    decimal totalTaxAmount = totalCartValue * taxValue;
                    cartItems.Tax = totalTaxAmount;
                    _cartService.UpdateCartTotal(cartItems);
                }
                else
                {
                    throw new ArgumentException($"No data found in the cart for ID :{customerId}");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
