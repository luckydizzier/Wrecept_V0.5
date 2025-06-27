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
        var invoice = new Invoice { SerialNumber = "1", TransactionNumber = "T1" };

        await service.SaveAsync(invoice);

        Assert.Single(await repo.GetAllAsync());
    }

    [Fact]
    public async Task SaveAsync_ShouldUpdateInvoice_WhenIdExists()
    {
        var repo = new InMemoryInvoiceRepository();
        var invoice = new Invoice { Id = Guid.NewGuid(), SerialNumber = "1", TransactionNumber = "T1" };
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
        var invoice = new Invoice { Id = Guid.NewGuid(), SerialNumber = "1", TransactionNumber = "T1" };
        await repo.AddAsync(invoice);
        var service = new DefaultInvoiceService(repo);

        await service.DeleteAsync(invoice.Id);

        Assert.Empty(await repo.GetAllAsync());
    }


    private class FailingRepo : IInvoiceRepository
    {
        public Task AddAsync(Invoice entity) => throw new InvalidOperationException("fail");
        public Task DeleteAsync(Guid id) => Task.CompletedTask;
        public Task<List<Invoice>> FindAsync(System.Linq.Expressions.Expression<Func<Invoice, bool>> predicate) => Task.FromResult(new List<Invoice>());
        public Task<List<Invoice>> GetAllAsync() => Task.FromResult(new List<Invoice>());
        public Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId) => Task.FromResult(new List<Invoice>());
        public Task<List<Invoice>> GetByProductIdAsync(Guid productId) => Task.FromResult(new List<Invoice>());
        public Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId) => Task.FromResult(new List<Invoice>());
        public Task<Invoice?> GetByIdAsync(Guid id) => Task.FromResult<Invoice?>(null);
        public Task UpdateAsync(Invoice entity) => Task.CompletedTask;
    }

    [Fact]
    public async Task SaveAsync_ShouldWrapException_WhenRepositoryFails()
    {
        var repo = new FailingRepo();
        var service = new DefaultInvoiceService(repo);
        var invoice = new Invoice();

        await Assert.ThrowsAsync<ServiceException>(() => service.SaveAsync(invoice));
    }
}
