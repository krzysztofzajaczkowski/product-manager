using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class StockKeepingUnit : ValueObject
    {
        private readonly Regex _skuRegex = new Regex(@"^[a-zA-Z0-9]+$");
        public string Sku { get; protected set; }

        public StockKeepingUnit(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku) || !_skuRegex.IsMatch(sku))
            {
                throw new InvalidSkuException("Product sku should contain only alphanumeric characters.");
            }
            Sku = sku;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Sku;
        }
    }
}
