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


    private class FailingRepo : IInvoiceRepository
    {
        public Task AddAsync(Invoice entity) => throw new InvalidOperationException("fail");
        public Task DeleteAsync(Guid id) => throw new NotImplementedException();
        public Task<List<Invoice>> FindAsync(System.Linq.Expressions.Expression<Func<Invoice, bool>> predicate) => throw new NotImplementedException();
        public Task<List<Invoice>> GetAllAsync() => throw new NotImplementedException();
        public Task<List<Invoice>> GetByProductGroupIdAsync(Guid groupId) => throw new NotImplementedException();
        public Task<List<Invoice>> GetByProductIdAsync(Guid productId) => throw new NotImplementedException();
        public Task<List<Invoice>> GetBySupplierIdAsync(Guid supplierId) => throw new NotImplementedException();
        public Task<Invoice?> GetByIdAsync(Guid id) => throw new NotImplementedException();
        public Task UpdateAsync(Invoice entity) => throw new NotImplementedException();
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
