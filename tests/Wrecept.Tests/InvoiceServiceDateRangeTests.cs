using System;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceServiceDateRangeTests
{
    [Fact]
    public async Task GetByDateRange_ShouldReturnMatches()
    {
        var repo = new InMemoryInvoiceRepository();
        await repo.AddAsync(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1", TransactionNumber = "T1", IssueDate = new DateOnly(2024,1,10) });
        await repo.AddAsync(new Invoice { Id = Guid.NewGuid(), SerialNumber = "2", TransactionNumber = "T2", IssueDate = new DateOnly(2024,2,10) });
        var service = new DefaultInvoiceService(repo);

        var result = await service.GetByDateRange(new DateOnly(2024,2,1), new DateOnly(2024,2,28));

        Assert.Single(result);
        Assert.Equal("2", result[0].SerialNumber);
    }
}
