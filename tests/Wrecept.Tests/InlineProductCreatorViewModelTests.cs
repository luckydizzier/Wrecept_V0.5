using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InlineProductCreatorViewModelTests
{
    [Fact]
    public void SaveCommand_ShouldCreateProduct()
    {
        var repo = new Wrecept.Core.Repositories.InMemoryProductRepository();
        var service = new DefaultProductService(repo);
        var groupService = new DefaultProductGroupService(new Wrecept.Core.Repositories.InMemoryProductGroupRepository());
        var unitService = new DefaultUnitService(new Wrecept.Core.Repositories.InMemoryUnitRepository());
        var taxService = new DefaultTaxRateService(new Wrecept.Core.Repositories.InMemoryTaxRateRepository());

        var vm = new InlineProductCreatorViewModel(service, groupService, unitService, taxService, "Test");
        Product? created = null;
        vm.Saved += p => created = p;

        vm.SaveCommand.Execute(null);

        Assert.NotNull(created);
        Assert.Equal("Test", created!.Name);
    }
}
