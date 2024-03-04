using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Must be greater than or equal to 0")]
        public int Quantity { get; set; }
        [Range(5, int.MaxValue, ErrorMessage = "Must be greater than or equal to 10")]
        public double Price { get; set; }
        
    }
}
