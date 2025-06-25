using Dapper;

namespace Wrecept.Infrastructure;

public static class SeedDataService
{
    public static async Task SeedAsync(SqliteConnectionFactory factory)
    {
        await using var conn = factory.CreateConnection();
        await conn.OpenAsync();

        long count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Suppliers");
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Suppliers (Id, Name, Address, TaxId, BankAccountNumber) VALUES (@Id, @Name, @Address, @TaxId, @BankAccountNumber)",
                new { Id = Guid.NewGuid().ToString(), Name = "Minta Kft.", Address = "Budapest", TaxId = "11111111-1-11", BankAccountNumber = "11111111-11111111" });
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM ProductGroups");
        Guid groupId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO ProductGroups (Id, Name) VALUES (@Id, @Name)", new { Id = groupId, Name = "Általános" });
        }
        else
        {
            groupId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM ProductGroups LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM TaxRates");
        Guid vatId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO TaxRates (Id, Label, Percentage) VALUES (@Id, @Label, @Percentage)", new { Id = vatId, Label = "ÁFA 27%", Percentage = 27 });
        }
        else
        {
            vatId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM TaxRates LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Units");
        Guid unitId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Units (Id, Name, Symbol) VALUES (@Id, @Name, @Symbol)", new { Id = unitId, Name = "darab", Symbol = "db" });
        }
        else
        {
            unitId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Units LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Products");
        Guid productId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Products (Id, Name, ProductGroupId, TaxRateId, DefaultUnitId) VALUES (@Id, @Name, @GroupId, @TaxId, @UnitId)",
                new { Id = productId, Name = "Teszt termék", GroupId = groupId, TaxId = vatId, UnitId = unitId });
        }
        else
        {
            productId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Products LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM PaymentMethods");
        Guid payId = Guid.NewGuid();
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO PaymentMethods (Id, Label) VALUES (@Id, @Label)", new { Id = payId, Label = "Készpénz" });
        }
        else
        {
            payId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM PaymentMethods LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM Invoices");
        Guid invoiceId = Guid.NewGuid();
        Guid supplierId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Suppliers LIMIT 1");
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO Invoices (Id, SerialNumber, IssueDate, SupplierId, PaymentMethodId, Notes) VALUES (@Id, @Serial, @Date, @Sup, @Pay, '')",
                new { Id = invoiceId, Serial = "INV-001", Date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"), Sup = supplierId, Pay = payId });
        }
        else
        {
            invoiceId = await conn.QuerySingleAsync<Guid>("SELECT Id FROM Invoices LIMIT 1");
        }

        count = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM InvoiceItems");
        if (count == 0)
        {
            await conn.ExecuteAsync("INSERT INTO InvoiceItems (Id, InvoiceId, ProductId, Quantity, UnitId, UnitPriceNet, VatRatePercent) VALUES (@Id, @InvId, @ProdId, 1, @UnitId, 100, 27)",
                new { Id = Guid.NewGuid(), InvId = invoiceId, ProdId = productId, UnitId = unitId });
        }
    }
}
