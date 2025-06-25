using Dapper;

namespace Wrecept.Infrastructure;

public static class SeedDataService
{
    public static async Task SeedAsync(SqliteConnectionFactory factory)
    {
        await using var conn = factory.CreateConnection();
        await conn.OpenAsync().ConfigureAwait(false);

        long count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Suppliers").ConfigureAwait(false);
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Suppliers (Id, Name, Address, TaxId, BankAccountNumber) VALUES (@Id, @Name, @Address, @TaxId, @BankAccountNumber)",
                new { Id = Guid.NewGuid().ToString(), Name = "Minta Kft.", Address = "Budapest", TaxId = "11111111-1-11", BankAccountNumber = "11111111-11111111" }).ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM ProductGroups").ConfigureAwait(false);
        Guid groupId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO ProductGroups (Id, Name) VALUES (@Id, @Name)", new { Id = groupId.ToString(), Name = "Általános" }).ConfigureAwait(false);
        }
        else
        {
            groupId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM ProductGroups LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM TaxRates").ConfigureAwait(false);
        Guid vatId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO TaxRates (Id, Label, Percentage) VALUES (@Id, @Label, @Percentage)", new { Id = vatId.ToString(), Label = "ÁFA 27%", Percentage = 27 }).ConfigureAwait(false);
        }
        else
        {
            vatId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM TaxRates LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Units").ConfigureAwait(false);
        Guid unitId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Units (Id, Name, Symbol) VALUES (@Id, @Name, @Symbol)", new { Id = unitId.ToString(), Name = "darab", Symbol = "db" }).ConfigureAwait(false);
        }
        else
        {
            unitId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Units LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Products").ConfigureAwait(false);
        Guid productId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync(
                "INSERT INTO Products (Id, Name, ProductGroupId, TaxRateId, DefaultUnitId) VALUES (@Id, @Name, @GroupId, @TaxId, @UnitId)",
                new
                {
                    Id = productId.ToString(),
                    Name = "Teszt termék",
                    GroupId = groupId.ToString(),
                    TaxId = vatId.ToString(),
                    UnitId = unitId.ToString()
                }).ConfigureAwait(false);
        }
        else
        {
            productId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Products LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM PaymentMethods").ConfigureAwait(false);
        Guid payId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO PaymentMethods (Id, Label) VALUES (@Id, @Label)", new { Id = payId.ToString(), Label = "Készpénz" }).ConfigureAwait(false);
        }
        else
        {
            payId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM PaymentMethods LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Invoices").ConfigureAwait(false);
        Guid invoiceId = Guid.NewGuid();
        Guid supplierId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Suppliers LIMIT 1").ConfigureAwait(false);
        if (count == 0)
        {
            await conn.ExecuteAsync(
                "INSERT INTO Invoices (Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes) VALUES (@Id, @Serial, @Date, @Sup, @Pay, '')",
                new
                {
                    Id = invoiceId.ToString(),
                    Serial = "INV-001",
                    Date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                    Sup = supplierId.ToString(),
                    Pay = payId.ToString()
                }).ConfigureAwait(false);
        }
        else
        {
            invoiceId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Invoices LIMIT 1").ConfigureAwait(false);
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM InvoiceItems").ConfigureAwait(false);
        if (count == 0)
        {
            await conn.ExecuteAsync(
                "INSERT INTO InvoiceItems (Id, InvoiceId, ProductId, Quantity, UnitId, UnitPriceNet, VatRatePercent) VALUES (@Id, @InvId, @ProdId, 1, @UnitId, 100, 27)",
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    InvId = invoiceId.ToString(),
                    ProdId = productId.ToString(),
                    UnitId = unitId.ToString()
                }).ConfigureAwait(false);
        }
    }
}
