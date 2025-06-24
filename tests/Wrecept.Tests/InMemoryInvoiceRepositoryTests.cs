using System.Collections.Generic;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Xunit;

namespace Wrecept.Tests;

public class InMemoryInvoiceRepositoryTests
{
    [Fact]
    public async Task Constructor_ShouldSeedData()
    {
        var seed = new List<Invoice> { new() { Id = Guid.NewGuid(), SerialNumber = "1" } };
        var repo = new InMemoryInvoiceRepository(seed);

        var all = await repo.GetAllAsync();

        Assert.Single(all);
    }
}
