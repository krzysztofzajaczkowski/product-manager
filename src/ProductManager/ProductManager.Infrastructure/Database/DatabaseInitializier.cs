using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductManager.Core.Domain;
using ProductManager.Infrastructure.Helper;
using ProductManager.Infrastructure.Services;

namespace ProductManager.Infrastructure.Database
{
    public class DatabaseInitializier
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DatabaseInitializier(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task SeedDatabaseAsync()
        {
            try
            {
                SqlMapper.AddTypeHandler(new GuidTypeHandler());
                SqlMapper.RemoveTypeMap(typeof(Guid));
                SqlMapper.RemoveTypeMap(typeof(Guid?));
                using var conn = _dbConnectionFactory.Create();

                // create tables
                conn.Execute(@"CREATE TABLE Users (
	                                        Id TEXT primary key,
	                                        Name nvarchar(30) unique not null,
	                                        Email nvarchar(60) unique not null,
	                                        Password nvarchar(255) not null
                                        );

                                        CREATE TABLE Roles (
	                                        Id TEXT primary key,
	                                        Name nvarchar(30) unique not null
                                        );

                                        CREATE TABLE UserRoles (
	                                        UserId TEXT not null,
	                                        RoleId TEXT not null,
	                                        PRIMARY KEY(UserId, RoleId),
	                                        CONSTRAINT fk_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users (Id),
	                                        CONSTRAINT fk_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles (Id)
                                        );");

                conn.Execute(@"CREATE TABLE CatalogProducts (
	                                        Id TEXT,
	                                        Sku nvarchar(255) unique not null,
	                                        Name nvarchar(255) unique not null,
	                                        Description TEXT,
                                            PRIMARY KEY(Id, Sku)
                                        );

                                        CREATE TABLE WarehouseProducts (
	                                        Id TEXT primary key,
	                                        Sku nvarchar(255) unique not null,
                                            Stock integer not null,
                                            Weight real,
                                            CONSTRAINT fk_WarehouseProducts_CatalogProducts FOREIGN KEY (Sku) REFERENCES CatalogProducts(Sku) on delete cascade
                                        );

                                        CREATE TABLE SalesProducts (
	                                        Id TEXT primary key,
	                                        Sku nvarchar(255) unique not null,
                                            Cost real not null,
                                            TaxPercentage integer not null,
                                            NetPrice real not null,
                                            CONSTRAINT fk_SalesProducts_CatalogProducts FOREIGN KEY (Sku) REFERENCES CatalogProducts(Sku) on delete cascade
                                        );
                                        ");

                // seed database

                var roles = new List<Role>
                {
                    new Role("CatalogManager"),
                    new Role("SalesManager"),
                    new Role("WarehouseManager"),
                    new Role("user"),
                    new Role("admin")
                };
                conn.Execute("INSERT INTO Roles(Id,Name) VALUES (@Id, @Name);", roles);

                var users = new List<User>
                {
                    new User(Guid.NewGuid(), "admin", "admin@admin.com", "secret", roles.ToArray())
                };

                var usersWithHashedPassword = users.Select(u => new
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Password = PasswordHelper.CalculateHash(u.Password)
                });

                conn.Execute(
                    "INSERT INTO Users(Id, Name, Email, Password) VALUES (@Id, @Name, @Email, @Password);", usersWithHashedPassword);

                var userRoles = users.SelectMany(u =>
                {
                    return u.Roles.Select(r => new
                    {
                        UserId = u.Id,
                        RoleId = r.Id
                    });
                }).ToList();

                conn.Execute(
                    "INSERT INTO UserRoles(UserId, RoleId) VALUES (@UserId, @RoleId);", userRoles);

                var rand = new Random(1111);
                var products = new List<Product>();
                for (var i = 0; i < 30; ++i)
                {
                    var cost = rand.Next(100, 10000) / 100.0;
                    var tax = rand.Next(8, 30);
                    var netPrice = cost * ((100 + tax) / 100.0);
                    var stock = rand.Next(0, 100);
                    var weight = rand.Next(100, 10000) / 100.0;
                    var sku = rand.Next(111111111, 999999999).ToString();
                    products.Add(new Product(Guid.NewGuid(), $"{sku}{i}", $"Prod{i}", $"Desc{i}", Guid.NewGuid(), stock, weight, Guid.NewGuid(),
                        (decimal)cost, tax, (decimal)netPrice));
                }

                conn.Execute(
                    "INSERT INTO CatalogProducts(Id, Sku, Name, Description) VALUES (@Id, @Sku, @Name, @Description);",
                    products.Select(p => p.CatalogProduct).ToList());

                conn.Execute(
                    "INSERT INTO WarehouseProducts(Id, Sku, Stock, Weight) VALUES (@Id, @Sku, @Stock, @Weight);",
                    products.Select(p => p.WarehouseProduct).ToList());

                conn.Execute(
                    "INSERT INTO SalesProducts(Id, Sku, Cost, TaxPercentage, NetPrice) VALUES (@Id, @Sku, @Cost, @TaxPercentage, @NetPrice);",
                    products.Select(p => p.SalesProduct).ToList());
            }
            catch (Exception e)
            {
                // Database already initialized
            }

            return Task.CompletedTask;
        }
    }
}
