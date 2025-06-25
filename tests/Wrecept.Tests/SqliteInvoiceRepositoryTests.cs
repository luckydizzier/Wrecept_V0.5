using System;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class SqliteInvoiceRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnInvoices_WithValidGuid()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        var dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
        var factory = new SqliteConnectionFactory(dbPath);

        await SqliteMigrator.EnsureCreatedAsync(dbPath).ConfigureAwait(false);
        await using (var conn = factory.CreateConnection())
        {
            await conn.ExecuteAsync("INSERT INTO Suppliers (Id, Name, Address, TaxId, BankAccountNumber) VALUES (@Id, 'A', 'B', 'T', 'BA')",
                new { Id = Guid.NewGuid().ToString() }).ConfigureAwait(false);

            var supplierId = await conn.QuerySingleAsync<string>("SELECT Id FROM Suppliers LIMIT 1").ConfigureAwait(false);

            await conn.ExecuteAsync("INSERT INTO PaymentMethods (Id, Label) VALUES (@Id, 'Cash')",
                new { Id = Guid.NewGuid().ToString() }).ConfigureAwait(false);
            var payId = await conn.QuerySingleAsync<string>("SELECT Id FROM PaymentMethods LIMIT 1").ConfigureAwait(false);

            await conn.ExecuteAsync("INSERT INTO Invoices (Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes) VALUES (@Id, 'INV-1', '2024-01-01', @Sup, @Pay, '')",
                new { Id = Guid.NewGuid().ToString(), Sup = supplierId, Pay = payId }).ConfigureAwait(false);
        }

        var repo = new SqliteInvoiceRepository(factory);
        var invoices = await repo.GetAllAsync().ConfigureAwait(false);

        Assert.NotEmpty(invoices);
        Assert.All(invoices, i => Assert.NotEqual(Guid.Empty, i.Id));
    }
}
