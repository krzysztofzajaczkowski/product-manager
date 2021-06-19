using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Infrastructure.Database;
using Xunit;

namespace ProductManager.UnitTests
{
    public class SQLiteConnectionFactoryTests : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _dbFileName;

        public SQLiteConnectionFactoryTests()
        {
            _dbFileName = "file.db";
            _connectionString = $"DataSource={_dbFileName}";
        }

        [Fact]
        public void WhenCreatingSQLiteConnectionFactory_ConnectionStringFieldShouldBeEqualToPassedParameter()
        {
            var testConnectionString = "testConnString";
            var sut = new SQLiteConnectionFactory(testConnectionString);

            var connectionStringField = sut.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "_connectionString" && x.IsPrivate);

            var value = (string) connectionStringField.GetValue(sut);

            value.Should().Be(testConnectionString);
        }

        [Fact]
        public async Task WhenCallingCreate_FactoryShouldReturnOpenedIDbConnection()
        {
            var sut = new SQLiteConnectionFactory(_connectionString);

            var conn = await sut.CreateAsync();

            var caughtState = conn.State;

            conn.Close();

            caughtState.Should().Be(ConnectionState.Open);
        }

        [Fact]
        public async Task WhenCallingCreate_FactoryShouldCreateFileWithSpecifiedPath()
        {
            var sut = new SQLiteConnectionFactory(_connectionString);

            var conn = await sut.CreateAsync();

            conn.Close();

            File.Exists(_dbFileName).Should().BeTrue();
        }


        public void Dispose()
        {
            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
        }
    }
}
