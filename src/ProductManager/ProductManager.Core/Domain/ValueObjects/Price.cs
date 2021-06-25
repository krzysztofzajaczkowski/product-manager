using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class Price : ValueObject
    {
        public decimal Cost { get; protected set; }
        public decimal NetPrice { get; protected set; }
        public int TaxPercentage { get; protected set; }

        public Price(decimal cost, decimal netPrice, int taxPercentage)
        {
            if (cost <= 0)
            {
                throw new InvalidCostException($"Cost of product can not be less than or equal to 0.");
            }

            if (netPrice <= 0)
            {
                throw new InvalidNetPriceException($"Net price of product can not be less than or equal to 0.");
            }

            if (netPrice <= cost)
            {
                throw new InvalidNetPriceException($"Net price of product can not be less than or equal to cost.");
            }

            if (taxPercentage < 0)
            {
                throw new InvalidTaxPercentageException($"Tax percentage for product can not be less than 0.");
            }

            if (taxPercentage >= 100)
            {
                throw new InvalidTaxPercentageException($"Tax percentage for product can not be greater than or equal to 100.");
            }

            Cost = cost;
            NetPrice = netPrice;
            TaxPercentage = taxPercentage;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Cost;
            yield return NetPrice;
            yield return TaxPercentage;
        }
    }
}
