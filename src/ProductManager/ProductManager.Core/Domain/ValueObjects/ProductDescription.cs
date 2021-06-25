using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class ProductDescription : ValueObject
    {
        private readonly Regex _descriptionRegex = new Regex(@"^[a-zA-Z0-9 ]+$");
        public string Description { get; protected set; }

        public ProductDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description) || !_descriptionRegex.IsMatch(description))
            {
                throw new InvalidDescriptionException("Product description should contain only alphanumeric characters and spaces.");
            }
            Description = description;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
        }
    }
}
