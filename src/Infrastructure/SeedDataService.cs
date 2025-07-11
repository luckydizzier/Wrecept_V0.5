using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;

namespace Wrecept.Infrastructure;

public static class SeedDataService
{
    public static async Task SeedAsync(WreceptDbContext db)
    {
        if (!await db.Suppliers.AnyAsync())
        {
            db.Suppliers.Add(new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "Minta Kft.",
                Address = "Budapest",
                TaxId = "11111111-1-11",
                BankAccountNumber = "11111111-11111111"
            });
        }

        Guid groupId;
        if (!await db.ProductGroups.AnyAsync())
        {
            groupId = Guid.NewGuid();
            db.ProductGroups.Add(new ProductGroup { Id = groupId, Name = "Általános" });
        }
        else
        {
            groupId = await db.ProductGroups.Select(pg => pg.Id).FirstAsync();
        }

        Guid vatId;
        if (!await db.TaxRates.AnyAsync())
        {
            vatId = Guid.NewGuid();
            db.TaxRates.Add(new TaxRate { Id = vatId, Label = "ÁFA27%", Percentage = 27 });
        }
        else
        {
            vatId = await db.TaxRates.Select(tr => tr.Id).FirstAsync();
        }

        Guid unitId;
        if (!await db.Units.AnyAsync())
        {
            unitId = Guid.NewGuid();
            db.Units.Add(new Unit { Id = unitId, Name = "darab", Symbol = "db" });
        }
        else
        {
            unitId = await db.Units.Select(u => u.Id).FirstAsync();
        }

        Guid productId;
        if (!await db.Products.AnyAsync())
        {
            productId = Guid.NewGuid();
            db.Products.Add(new Product
            {
                Id = productId,
                Name = "Teszt termék",
                Group = await db.ProductGroups.FindAsync(groupId)
                    ?? throw new InvalidOperationException("Group missing"),
                TaxRate = await db.TaxRates.FindAsync(vatId)
                    ?? throw new InvalidOperationException("VAT missing"),
                DefaultUnit = await db.Units.FindAsync(unitId)
                    ?? throw new InvalidOperationException("Unit missing")
            });
        }
        else
        {
            productId = await db.Products.Select(p => p.Id).FirstAsync();
        }

        Guid payId;
        if (!await db.PaymentMethods.AnyAsync())
        {
            payId = Guid.NewGuid();
            db.PaymentMethods.Add(new PaymentMethod { Id = payId, Label = "Készpénz" });
        }
        else
        {
            payId = await db.PaymentMethods.Select(pm => pm.Id).FirstAsync();
        }

        Guid invoiceId;
        Guid supplierId = await db.Suppliers.Select(s => s.Id).FirstAsync();
        if (!await db.Invoices.AnyAsync())
        {
            invoiceId = Guid.NewGuid();
            db.Invoices.Add(new Invoice
            {
                Id = invoiceId,
                SerialNumber = "INV-001",
                TransactionNumber = "TXN-001",
                IssueDate = DateOnly.FromDateTime(DateTime.Today),
                Supplier = await db.Suppliers.FindAsync(supplierId)
                    ?? throw new InvalidOperationException("Supplier missing"),
                CalculationMode = CalculationMode.Net,
                PaymentMethod = await db.PaymentMethods.FindAsync(payId)
                    ?? throw new InvalidOperationException("Payment method missing"),
                Notes = string.Empty
            });
        }
        else
        {
            invoiceId = await db.Invoices.Select(i => i.Id).FirstAsync();
        }

        if (!await db.InvoiceItems.AnyAsync())
        {
            var item = new InvoiceItem
            {
                Id = Guid.NewGuid(),
                Product = await db.Products.FindAsync(productId)
                    ?? throw new InvalidOperationException("Product missing"),
                Quantity = 1,
                Unit = await db.Units.FindAsync(unitId)
                    ?? throw new InvalidOperationException("Unit missing"),
                UnitPriceNet = 100,
                VatRatePercent = 27
            };
            var invoice = await db.Invoices.FindAsync(invoiceId)
                ?? throw new InvalidOperationException("Invoice missing");
            invoice.Items.Add(item);
            db.InvoiceItems.Add(item);
        }

        await db.SaveChangesAsync();
    }
}
