using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class ProductBlockDto
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal NetPrice { get; set; }
        public int Stock { get; set; }
    }
}
