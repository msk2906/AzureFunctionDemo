using System.Collections.Generic;
using System.Linq;
using AzureFunctionDemo;

namespace ProductWebAPI.Services
{
    public static class InMemoryProductService
    {
        private static List<Product> products = new List<Product>
        {
            new Product { ProductID = 1, ProductName = "Laptop", Price = 1000, Quantity = 10 },
            new Product { ProductID = 2, ProductName = "Smartphone", Price = 800, Quantity = 20 }
        };

        public static List<Product> GetAll()
        {
            return products;
        }

        public static Product GetById(int id) => products.FirstOrDefault(p => p.ProductID == id);

        public static void Add(Product product)
        {
            product.ProductID = products.Max(p => p.ProductID) + 1;
            products.Add(product);
        }

        public static bool Update(Product updatedProduct)
        {
            var existingProduct = products.FirstOrDefault(p => p.ProductID == updatedProduct.ProductID);
            if (existingProduct == null) return false;

            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;
            return true;
        }

        public static bool Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return false;

            products.Remove(product);
            return true;
        }
    }
}
