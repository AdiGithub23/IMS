using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        private readonly IProductRepository productRepository;

        public ProductTransactionRepository(
            IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }        
        
        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice)
        {
            this._productTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SONumber = salesOrderNumber,
                ProductId = product.ProductID,
                QauntityBefore = product.Quantity,
                QauntityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.Now,
                UnitPrice = unitPrice
            });

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            var query = from it in this._productTransactions
                        join inv in products on it.ProductId equals inv.ProductID
                        where
                        (string.IsNullOrWhiteSpace(productName) || inv.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new ProductTransaction
                        {
                            Product = inv,
                            ProductTransactionId = it.ProductTransactionId,
                            SONumber = it.SONumber,
                            ProductionNumber = it.ProductionNumber,
                            ProductId = it.ProductId,
                            QauntityBefore = it.QauntityBefore,
                            ActivityType = it.ActivityType,
                            QauntityAfter = it.QauntityAfter,
                            TransactionDate = it.TransactionDate,
                            //DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice
                        };
            return query;
        }

        //public Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
