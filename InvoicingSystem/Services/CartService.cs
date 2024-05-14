using InvoicingSystem.Models;

namespace InvoicingSystem.Services
{
    public class CartService
    {
        private readonly Dictionary<int, Cart> _carts = new Dictionary<int, Cart>();
        private int _nextCartId = 1;

        public Cart GetCart(int customerId)
        {
            if (_carts.TryGetValue(customerId, out Cart cart))
            {
                return cart;
            }
            else
            {
                throw new ArgumentException($"Cart not found for Customer ID: {customerId}");
            }
        }

        public void AddToCart(int customerId, int productId, int quantity, decimal productUnitPrice)
        {
            try
            {
                if (quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
                }

                if (!_carts.TryGetValue(customerId, out Cart cart))
                {
                    cart = new Cart { Id = _nextCartId++, CustomerId = customerId, Items = new List<CartItem>() };
                    _carts.Add(customerId, cart);
                }

                var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.Price = existingItem.Quantity * productUnitPrice;
                }
                else
                {
                    cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity, Discount = 0, Price = (quantity * productUnitPrice) });
                }
                UpdateCartTotal(cart);
            }
            catch (Exception ex)
            {

                throw new ArgumentException($"Could not add item to cart. ERROR: " + ex.Message);
            }

        }

        public void RemoveFromCart(int customerId, int productId, int quantity = 1)
        {
            if (!_carts.TryGetValue(customerId, out Cart cart))
            {
                return;
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (item.Quantity > quantity)
                {
                    item.Quantity -= quantity;
                }
                else
                {
                    cart.Items.Remove(item);
                }
            }
        }

        public void ClearCart(int customerId)
        {
            _carts.Remove(customerId);
        }

        public List<CartItem> GetCartItems(int customerId)
        {
            if (!_carts.TryGetValue(customerId, out Cart cart))
            {
                return new List<CartItem>();
            }

            return cart.Items.ToList();
        }

        public void UpdateCartItemQuantity(int customerId, int productId, int quantity)
        {
            if (!_carts.TryGetValue(customerId, out Cart cart))
            {
                return;
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            UpdateCartTotal(cart);
        }

        public void UpdateCartTotal(Cart cart)
        {
            decimal subTotal = cart.Items.Sum(item => item.Price);
            decimal discount = cart.Discount != 0 ? cart.Discount : cart.Items.Sum(item => item.Discount);
            cart.Subtotal = subTotal;
            cart.Total = (subTotal - discount) + cart.Tax;
        }

    }
}
