using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;
        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product() {ProductID=1, ProductName="Bike", Quantity=10, Price=150},
                new Product() {ProductID=2, ProductName="Car", Quantity=5, Price=25000},
            };
        }

        //public async Task<bool> ExistsAsync(Product product)
        //{
        //    return await Task.FromResult(_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)));
        //}

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _products.Max(x => x.ProductID);
            product.ProductID = maxId + 1;

            _products.Add(product);
            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            //return await Task.FromResult(_products.FirstOrDefault(x => x.ProductID == productId));
            var prod = _products.FirstOrDefault(x => x.ProductID == productId);
            var newProd = new Product();

            if (prod != null)
            {
                newProd.ProductID = prod.ProductID;
                newProd.ProductName = prod.ProductName;
                newProd.Price = prod.Price;
                newProd.Quantity = prod.Quantity;
                
            }

            return await Task.FromResult(newProd);
        }
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);
            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductID != product.ProductID &&
                                 x.ProductName.ToLower() == product.ProductName.ToLower()))
            { return Task.CompletedTask; }

            var prod = _products.FirstOrDefault(x => x.ProductID == product.ProductID);
            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
            }

            return Task.CompletedTask;
        }
    }
}


