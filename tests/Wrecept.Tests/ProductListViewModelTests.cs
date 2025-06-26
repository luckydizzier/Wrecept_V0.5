using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class ProductListViewModelTests
{
    private static void SetupServices(bool includeGroups, bool includeTaxes, bool includeUnits)
    {
        var groupRepo = new InMemoryProductGroupRepository();
        var taxRepo = new InMemoryTaxRateRepository();
        var unitRepo = new InMemoryUnitRepository();
        AppContext.ProductGroupService = new DefaultProductGroupService(groupRepo);
        AppContext.TaxRateService = new DefaultTaxRateService(taxRepo);
        AppContext.UnitService = new DefaultUnitService(unitRepo);
        if (includeGroups)
            AppContext.ProductGroupService.SaveAsync(new ProductGroup { Name = "G" }).Wait();
        if (includeTaxes)
            AppContext.TaxRateService.SaveAsync(new TaxRate { Label = "T" }).Wait();
        if (includeUnits)
            AppContext.UnitService.SaveAsync(new Unit { Name = "U", Symbol = "u" }).Wait();
    }

    [Fact]
    public async Task AddCommand_ShouldAddProduct_WhenDependenciesExist()
    {
        SetupServices(true, true, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        var vm = new ProductListViewModel(productService);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Single(vm.Products);
        Assert.Equal(vm.Products[0], vm.SelectedProduct);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenGroupMissing()
    {
        SetupServices(false, true, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        AppContext.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenTaxRateMissing()
    {
        SetupServices(true, false, true);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        AppContext.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }

    [Fact]
    public async Task AddCommand_ShouldShowStatus_WhenUnitMissing()
    {
        SetupServices(true, true, false);
        var productService = new DefaultProductService(new InMemoryProductRepository());
        string? message = null;
        AppContext.StatusMessageSetter = m => message = m;
        var vm = new ProductListViewModel(productService);

        await vm.AddCommand.ExecuteAsync(null);

        Assert.Empty(vm.Products);
        Assert.Equal("Nincs rögzített termékcsoport, adókulcs vagy mértékegység.", message);
    }
}
