using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductManager.Core.Domain;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Database;

namespace ProductManager.Infrastructure.Repositories
{
    public class SQLiteProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public SQLiteProductRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Product> GetProductAsync(string sku)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var product = (await connection.QueryAsync(
                @"SELECT 
                        cp.Id as CatalogId,
                        wp.Id as WarehouseId,
                        sp.Id as SalesId,
                        cp.Sku,
                        Name,
                        Description,
                        Stock,
                        Weight,
                        Cost,
                        TaxPercentage,
                        NetPrice
                    FROM CatalogProducts cp
                    INNER JOIN WarehouseProducts wp ON cp.Sku = wp.Sku
                    INNER JOIN SalesProducts sp ON cp.Sku = sp.Sku
                    WHERE cp.Sku = @Sku;", new
                {
                    Sku = sku
                })).Select(row => new Product(Guid.Parse(row.CatalogId), (string)row.Sku, (string)row.Name, (string)row.Description, Guid.Parse(row.WarehouseId),
                (int)row.Stock, (double)row.Weight, Guid.Parse(row.SalesId), (decimal)row.Cost, (int)row.TaxPercentage, (decimal)row.NetPrice)).FirstOrDefault();

            return product;
        }

        public async Task AddAsync(Product product)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            await connection.ExecuteAsync(
                "INSERT INTO CatalogProducts(Id, Sku, Name, Description) VALUES (@Id, @Sku, @Name, @Description);",
                new
                {
                    product.CatalogProduct.Id,
                    product.CatalogProduct.Sku.Sku,
                    product.CatalogProduct.Name.Name,
                    product.CatalogProduct.Description.Description
                });

            await connection.ExecuteAsync(
                "INSERT INTO WarehouseProducts(Id, Sku, Stock, Weight) VALUES (@Id, @Sku, @Stock, @Weight);",
                new
                {
                    product.WarehouseProduct.Id,
                    product.WarehouseProduct.Sku.Sku,
                    product.WarehouseProduct.Stock.Stock,
                    product.WarehouseProduct.Weight.Weight
                });

            await connection.ExecuteAsync(
                "INSERT INTO SalesProducts(Id, Sku, Cost, TaxPercentage, NetPrice) VALUES (@Id, @Sku, @Cost, @TaxPercentage, @NetPrice);",
                new
                {
                    product.SalesProduct.Id,
                    product.SalesProduct.Sku.Sku,
                    product.SalesProduct.Price.Cost,
                    product.SalesProduct.Price.TaxPercentage,
                    product.SalesProduct.Price.NetPrice,
                });
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            await connection.ExecuteAsync(@"
                UPDATE CatalogProducts
                SET
                    Name = @Name,
                    Description = @Description
                WHERE
                    Sku = @Sku",
                new
                {
                    updatedProduct.CatalogProduct.Sku.Sku,
                    updatedProduct.CatalogProduct.Name.Name,
                    updatedProduct.CatalogProduct.Description.Description
                });

            await connection.ExecuteAsync(@"
                UPDATE WarehouseProducts
                SET
                    Stock = @Stock,
                    Weight = @Weight
                WHERE
                    Sku = @Sku",
                new
                {
                    updatedProduct.WarehouseProduct.Sku.Sku,
                    updatedProduct.WarehouseProduct.Stock.Stock,
                    updatedProduct.WarehouseProduct.Weight.Weight
                });

            await connection.ExecuteAsync(@"
                UPDATE SalesProducts
                SET
                    Cost = @Cost,
                    TaxPercentage = @TaxPercentage,
                    NetPrice = @NetPrice
                WHERE
                    Sku = @Sku",
                new
                {
                    updatedProduct.SalesProduct.Sku.Sku,
                    updatedProduct.SalesProduct.Price.Cost,
                    updatedProduct.SalesProduct.Price.TaxPercentage,
                    updatedProduct.SalesProduct.Price.NetPrice,
                });

        }

        public async Task DeleteAsync(string sku)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            await connection.ExecuteAsync("DELETE FROM CatalogProducts WHERE Sku = @Sku;", new
            {
                Sku = sku
            });
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var products = (await connection.QueryAsync(
                @"SELECT 
                        cp.Id as CatalogId,
                        wp.Id as WarehouseId,
                        sp.Id as SalesId,
                        cp.Sku,
                        Name,
                        Description,
                        Stock,
                        Weight,
                        Cost,
                        TaxPercentage,
                        NetPrice
                    FROM CatalogProducts cp
                    INNER JOIN WarehouseProducts wp ON cp.Sku = wp.Sku
                    INNER JOIN SalesProducts sp ON cp.Sku = sp.Sku;")).Select(row => new Product(Guid.Parse(row.CatalogId), (string)row.Sku, (string)row.Name, (string)row.Description, Guid.Parse(row.WarehouseId),
                (int)row.Stock, (double)row.Weight, Guid.Parse(row.SalesId), (decimal)row.Cost, (int)row.TaxPercentage, (decimal)row.NetPrice)).ToList();

            return products;
        }
    }
}
