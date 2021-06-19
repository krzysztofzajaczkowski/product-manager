using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductManager.Core.Domain;
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

        public async Task SeedDatabaseAsync()
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            using var conn = await _dbConnectionFactory.CreateAsync();

            // create tables
            await conn.ExecuteAsync(@"CREATE TABLE Users (
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

            await conn.ExecuteAsync(@"CREATE TABLE CatalogProducts (
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
                new Role("WarehouseManager")
            };
            await conn.ExecuteAsync("INSERT INTO Roles(Id,Name) VALUES (@Id, @Name);", roles);

            var users = new List<User>
            {
                new User(Guid.NewGuid(), "admin", "admin@admin.com", "secret", roles.ToArray())
            };

            await conn.ExecuteAsync(
                "INSERT INTO Users(Id, Name, Email, Password) VALUES (@Id, @Name, @Email, @Password);", users);

            var userRoles = users.SelectMany(u =>
            {
                return u.Roles.Select(r => new
                {
                    UserId = u.Id,
                    RoleId = r.Id
                });
            }).ToList();

            await conn.ExecuteAsync(
                "INSERT INTO UserRoles(UserId, RoleId) VALUES (@UserId, @RoleId);", userRoles);

        }
    }
}
