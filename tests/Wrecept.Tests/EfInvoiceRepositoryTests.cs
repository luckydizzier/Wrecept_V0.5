using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class EfInvoiceRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistInvoice()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<WreceptDbContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new WreceptDbContext(options);
        await context.Database.EnsureCreatedAsync();
        await SeedDataService.SeedAsync(context);

        var repo = new EfInvoiceRepository(context);
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            SerialNumber = "INV-TEST",
            TransactionNumber = "TINV-1",
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            Supplier = await context.Suppliers.FirstAsync(),
            PaymentMethod = await context.PaymentMethods.FirstAsync()
        };

        await repo.AddAsync(invoice);
        var fromDb = await context.Invoices.FirstOrDefaultAsync(i => i.Id == invoice.Id);
        Assert.NotNull(fromDb);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSeededInvoice()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<WreceptDbContext>()
            .UseSqlite(connection)
            .Options;
        await using var context = new WreceptDbContext(options);
        await context.Database.EnsureCreatedAsync();
        await SeedDataService.SeedAsync(context);

        var repo = new EfInvoiceRepository(context);
        var all = await repo.GetAllAsync();

        Assert.NotEmpty(all);
    }
}
