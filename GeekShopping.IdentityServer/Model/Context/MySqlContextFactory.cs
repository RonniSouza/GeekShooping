using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GeekShopping.IdentityServer.Model.Context
{
    public class MySQLContextFactory : IDesignTimeDbContextFactory<MySQLContext>
    {
        public MySQLContext CreateDbContext(string[] args)
        {
            // 1. Carrega appsettings.json manualmente
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. Lê a connection string igual ao Program.cs
            var connectionString = config.GetConnectionString("MySQlConnectionString");

            // 3. Constrói as opções para o EF
            var optionsBuilder = new DbContextOptionsBuilder<MySQLContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 44))
            );

            // 4. Retorna o contexto configurado
            return new MySQLContext(optionsBuilder.Options);
        }
    }
}
