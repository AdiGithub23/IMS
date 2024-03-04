﻿using IMS.CoreBusiness;
using IMS.Plugins.EfCoreSqlServer;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {
        private readonly IProductRepository productRepository;
        //private readonly IMSContext db;
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductTransactionEFCoreRepository(
            IProductRepository productRepository,
            //IMSContext db)
            IDbContextFactory<IMSContext> contextFactory)
        {
            using var db = contextFactory.CreateDbContext();
            this.productRepository = productRepository;
            this.contextFactory = contextFactory;
        }

        //public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        //{
        //    var prod = await this.productRepository.GetProductByIdAsync(product.ProductID);
        //    if (prod != null)
        //    {
        //        foreach (var pi in prod.ProductInventories)
        //        {
        //            if (pi.Inventory != null)
        //            {
        //                //add inventory transaction
        //                await this.inventoryTransactionRepository.ProduceAsync(
        //                    productionNumber, pi.Inventory, pi.InventoryQuantity * quantity, doneBy, -1);

        //                //decrease the inventories
        //                var inv = await this.inventoryRepository.GetInventoriesByIdAsync(pi.InventoryId);
        //                inv.Quantity -= pi.InventoryQuantity * quantity;
        //                await this.inventoryRepository.UpdateInventoryAsync(inv);
        //            }

        //        }
        //    }

        //    //add product transaction
        //    this.db.ProductTransactions.Add(new ProductTransaction
        //    {
        //        ProductionNumber = productionNumber,
        //        ProductId = product.ProductID,
        //        QauntityBefore = product.Quantity,
        //        ActivityType = ProductTransactionType.ProduceProduct,
        //        QauntityAfter = product.Quantity + quantity,
        //        TransactionDate = DateTime.Now,
        //    });

        //    await this.db.SaveChangesAsync();

        //}

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice)
        {
            using var db = contextFactory.CreateDbContext();

            db.ProductTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SONumber = salesOrderNumber,
                ProductId = product.ProductID,
                QauntityBefore = product.Quantity,
                QauntityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.Now,
                UnitPrice = unitPrice
            });

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            using var db = contextFactory.CreateDbContext();

            var query = from pt in db.ProductTransactions
                        join prod in db.Products on pt.ProductId equals prod.ProductID
                        where
                        (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                        (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || pt.ActivityType == transactionType)
                        select pt;

            return await query.Include(x => x.Product).ToListAsync();
        }
    }
}
