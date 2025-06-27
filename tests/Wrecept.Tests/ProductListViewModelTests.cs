using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Wrecept.Infrastructure;
using Wrecept.Services;
using Xunit;

namespace Wrecept.Tests;

public class ProductListViewModelTests
{
    private static (
        IProductGroupService groupService,
        ITaxRateService taxService,
        IUnitService unitService,
        IStatusService statusService) SetupServices(bool includeGroups, bool includeTaxes, bool includeUnits)
    {
        var groupRepo = new InMemoryProductGroupRepository();
        var taxRepo = new InMemoryTaxRateRepository();
        var unitRepo = new InMemoryUnitRepository();
        var groupService = new DefaultProductGroupService(groupRepo);
        var taxService = new DefaultTaxRateService(taxRepo);
        var unitService = new DefaultUnitService(unitRepo);
        if (includeGroups)
            groupService.SaveAsync(new ProductGroup { Name = "G" }).Wait();
        if (includeTaxes)
            taxService.SaveAsync(new TaxRate { Label = "T" }).Wait();
        if (includeUnits)
            unitService.SaveAsync(new Unit { Name = "U", Symbol = "u" }).Wait();
        return (groupService, taxService, unitService, new StatusService());
    }

    [Fact]
    public async Task AddCommand_ShouldAddProduct_WhenDependenciesExist()
    {
        var (groupService, taxService, unitService, status) = SetupServices(true, true, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        var vm = new ProductListViewModel(productService, groupService, taxService, unitService, status);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Single(vm.Products);
        Assert.Equal(vm.Products[0], vm.SelectedProduct);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenGroupMissing()
    {
        var (groupService, taxService, unitService, status) = SetupServices(false, true, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        status.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService, groupService, taxService, unitService, status);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenTaxRateMissing()
    {
        var (groupService2, taxService2, unitService2, status2) = SetupServices(true, false, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        status2.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService, groupService2, taxService2, unitService2, status2);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenUnitMissing()
    {
        var (groupService3, taxService3, unitService3, status3) = SetupServices(true, true, false);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        status3.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService, groupService3, taxService3, unitService3, status3);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }
}
