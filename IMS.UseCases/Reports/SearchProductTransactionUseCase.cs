using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
    public class SearchProductTransactionUseCase : ISearchProductTransactionUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;

        public SearchProductTransactionUseCase(IProductTransactionRepository productTransactionRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
        }
        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductTransactionType? transactionType)
        {

            if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

            return await this.productTransactionRepository.GetProductTransactionsAsync(
                productName, dateFrom, dateTo, transactionType);
        }
    }
}
