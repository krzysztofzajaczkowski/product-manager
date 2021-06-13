using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class ProductDto
    {
        public Guid CatalogId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid SalesId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int TaxPercentage { get; set; }
        public decimal NetPrice { get; set; }
        public int Stock { get; set; }
        public double Weight { get; set; }
    }
}
