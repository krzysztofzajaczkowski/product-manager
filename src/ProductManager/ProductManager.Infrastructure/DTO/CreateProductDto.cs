using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.DTO
{
    public class CreateProductDto
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
