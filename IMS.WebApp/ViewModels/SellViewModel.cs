using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class SellViewModel
    {
        [Required]
        public string SalesOrderNumber { get; set; } = string.Empty;

        [Required]
        //[Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You have to select a Product.")]
        public int ProductId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than 1.")]
        [Sell_EnsureEnoughProductQuantity]
        public int QuantityToSell { get; set; }

        [Required]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "UnitPrice has to be greater than 0.")]
        public double UnitPrice { get; set; }

        public Product? Product { get; set; } = null;
    }
}
