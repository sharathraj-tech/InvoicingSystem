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
                    new Product { Id = 1, Name = "Smartphone", Description = "A high-end smartphone with advanced features.", Price = 599.99m, Quantity = 20, CategoryId = 1 },
                    new Product { Id = 2, Name = "Blender", Description = "A powerful blender for smoothies and shakes.", Price = 49.99m, Quantity = 15, CategoryId = 2 },
                    new Product { Id = 3, Name = "Fantasy Novel", Description = "An epic fantasy novel with captivating storytelling.", Price = 15.99m, Quantity = 30, CategoryId = 3 },
                    new Product { Id = 4, Name = "Yoga Mat", Description = "A premium yoga mat for comfortable and stable practice.", Price = 29.99m, Quantity = 25, CategoryId = 4 },
                    new Product { Id = 5, Name = "Football", Description = "A high-quality football for recreational play.", Price = 19.99m, Quantity = 10, CategoryId = 5 }
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
