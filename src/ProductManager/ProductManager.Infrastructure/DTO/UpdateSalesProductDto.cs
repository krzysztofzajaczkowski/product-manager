using System;

namespace ProductManager.Infrastructure.DTO
{
    public class UpdateSalesProductDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public decimal Cost { get; set; }
        public int TaxPercentage { get; set; }
        public decimal NetPrice { get; set; }
    }
}