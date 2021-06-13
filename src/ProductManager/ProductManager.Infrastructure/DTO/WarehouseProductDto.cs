using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class WarehouseProductDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
        public double Weight { get; set; }
    }
}
