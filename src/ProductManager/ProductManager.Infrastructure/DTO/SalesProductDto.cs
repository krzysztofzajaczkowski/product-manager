using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class SalesProductDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public decimal Cost { get; set; }
        public int TaxPercentage { get; set; }
        public decimal NetPrice { get; set; }
    }
}
