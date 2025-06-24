using System;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceServiceTests
{
    [Fact]
    public async Task SaveAsync_ShouldAddNewInvoice_WhenIdEmpty()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var invoice = new Invoice { SerialNumber = "1" };

        await service.SaveAsync(invoice);

        Assert.Single(await repo.GetAllAsync());
    }

    [Fact]
    public async Task SaveAsync_ShouldUpdateInvoice_WhenIdExists()
    {
        var repo = new InMemoryInvoiceRepository();
        var invoice = new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" };
        await repo.AddAsync(invoice);
        var service = new DefaultInvoiceService(repo);

        invoice.SerialNumber = "2";
        await service.SaveAsync(invoice);

        var stored = await repo.GetByIdAsync(invoice.Id);
        Assert.Equal("2", stored?.SerialNumber);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveInvoice()
    {
        var repo = new InMemoryInvoiceRepository();
        var invoice = new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" };
        await repo.AddAsync(invoice);
        var service = new DefaultInvoiceService(repo);

        await service.DeleteAsync(invoice.Id);

        Assert.Empty(await repo.GetAllAsync());
    }
}
