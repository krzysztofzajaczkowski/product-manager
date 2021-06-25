using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class ProductStock : ValueObject
    {
        public int Stock { get; protected set; }

        public ProductStock(int stock)
        {
            if (stock < 0)
            {
                throw new InvalidStockException($"Stock of product can not be less than 0.");
            }
            Stock = stock;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Stock;
        }
    }
}
