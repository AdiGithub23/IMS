using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        //Task<bool> ExistsAsync(Product product);
        Task AddProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task UpdateProductAsync(Product product);
    }
}
