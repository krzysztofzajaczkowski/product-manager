using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class ProductWeight : ValueObject
    {
        public double Weight { get; protected set; }

        public ProductWeight(double weight)
        {
            if (weight < 0)
            {
                throw new InvalidWeightException($"Weight of product can not be less than 0.");
            }
            Weight = weight;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Weight;
        }
    }
}
