using InvoicingSystem.Models;

namespace InvoicingSystem.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>();

        public ProductService()
        {
            _products = new List<Product>()
            {
                    new Product{ Id = 1,Name = "Organic Cotton Tote Bag",Description = "A spacious and durable tote bag made from 100% organic cotton, perfect for your everyday errands.",Price = 19.99m,Quantity = 50,CategoryId = 1},
                    new Product{ Id = 2,Name = "Smartphone Stand with Wireless Charger",
                    Description = "A sleek and stylish smartphone stand featuring a built-in wireless charger, perfect for keeping your device powered up while watching videos or video calling.",Price = 29.99m,Quantity = 100,CategoryId = 2 },
                    new Product{ Id = 3,Name = "Organic Lavender Essential Oil",Description = "Pure and natural lavender essential oil, sourced from organic farms, ideal for aromatherapy and relaxation.",Price = 12.99m,Quantity = 30,CategoryId = 3 },
                    new Product{ Id = 4,Name = "Stainless Steel Water Bottle",Description = "An eco-friendly and durable water bottle made from high-quality stainless steel, perfect for staying hydrated on the go.",Price = 24.99m,Quantity = 75,CategoryId = 4 },
                    new Product { Id = 5, Name = "Handmade Ceramic Mug", Description = "A beautifully crafted ceramic mug, hand-painted with intricate designs, ideal for enjoying your morning coffee or tea.", Price = 15.99m, Quantity = 40, CategoryId = 5 }
            };
        }
        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Id = GenerateProductId();

            _products.Add(product);
        }

        public void UpdateProduct(int id, Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                throw new ArgumentNullException(nameof(updatedProduct));
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {id} not found.");
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;
            existingProduct.CategoryId = updatedProduct.CategoryId;
        }

        public void DeleteProduct(int id)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {id} not found.");
            }

            _products.Remove(existingProduct);
        }

        private int GenerateProductId()
        {
            return _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        }

        public bool ProductExists(int productId)
        {
            return _products.Any(p => p.Id == productId);
        }
        public int GetRemainigProductQuantity(int productId)
        {
            try
            {
                var product = _products.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {
                    throw new ArgumentException($"No Product with ID {productId} found.");
                }
                else
                {
                    return product.Quantity;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


    }
}
