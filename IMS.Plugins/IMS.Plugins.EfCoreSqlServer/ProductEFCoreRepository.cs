using IMS.CoreBusiness;
using IMS.Plugins.EfCoreSqlServer;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        //private readonly IMSContext db;
        //public ProductEFCoreRepository(IMSContext db)
        //{
        //    this.db = db;
        //}
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Products.Add(product);
            FLagInventoryUnchanges(product, db);

            await db.SaveChangesAsync();

            //if(product.ProductInventories != null &&
            //   product.ProductInventories.Count > 0)
            //{
            //    foreach(var prodInv in  product.ProductInventories)
            //    {
            //        if(prodInv.Inventory != null)
            //        {
            //            this.db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
            //        }
            //    }
            //}

        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var db = contextFactory.CreateDbContext();
            //return await this.db.Products.Include(x => x.ProductInventories)
            //    .ThenInclude(x => x.Inventory)
            //    .FirstOrDefaultAsync(x => x.ProductID == productId);

            //return await this db.Products.FirstOrDefaultAsync(x => x.ProductID == productId);
            return await Task.FromResult(db.Products.FirstOrDefault(x => x.ProductID == productId));
        }
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = contextFactory.CreateDbContext();
            return await db.Products.Where(x => x.ProductName.ToLower()
                .IndexOf(name.ToLower()) >= 0).ToListAsync();

        }

        public async Task UpdateProductAsync(Product product)
        {
            using var db = contextFactory.CreateDbContext();
            //var prod = await this.db.Products
            //    .Include(x => x.ProductInventories)
            //    .FirstOrDefaultAsync(x => x.ProductID == product.ProductID);
            var prod = await db.Products.FirstOrDefaultAsync(x => x.ProductID == product.ProductID);
                       
            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;

                FLagInventoryUnchanges(product, db);

                await db.SaveChangesAsync();
            }
        }

        private void FLagInventoryUnchanges(Product product, IMSContext db)
        {
            //if (product.ProductInventories != null &&
            //                   product.ProductInventories.Count > 0)
            //{
            //    foreach (var prodInv in product.ProductInventories)
            //    {
            //        if (prodInv.Inventory != null)
            //        {
            //            this.db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
            //        }
            //    }
            //}
        }
    }
}
