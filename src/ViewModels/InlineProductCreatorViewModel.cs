using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Wrecept.ViewModels;

public partial class InlineProductCreatorViewModel : InlineCreatorViewModel<Product>
{
    private readonly IProductService _productService;
    private readonly IProductGroupService _groupService;
    private readonly IUnitService _unitService;
    private readonly ITaxRateService _taxService;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private List<ProductGroup> _groups = new();

    [ObservableProperty]
    private ProductGroup? _selectedGroup;

    [ObservableProperty]
    private List<Unit> _units = new();

    [ObservableProperty]
    private Unit? _selectedUnit;

    [ObservableProperty]
    private List<TaxRate> _taxRates = new();

    [ObservableProperty]
    private TaxRate? _selectedTaxRate;

    [ObservableProperty]
    private decimal _unitPriceNet;

    public InlineProductCreatorViewModel(
        IProductService productService,
        IProductGroupService groupService,
        IUnitService unitService,
        ITaxRateService taxService,
        string name)
    {
        _productService = productService;
        _groupService = groupService;
        _unitService = unitService;
        _taxService = taxService;
        _name = name;
        _ = LoadLookupDataAsync();
    }

    private async Task LoadLookupDataAsync()
    {
        Groups = await _groupService.GetAllAsync();
        Units = await _unitService.GetAllAsync();
        TaxRates = await _taxService.GetAllAsync();
        SelectedGroup = Groups.FirstOrDefault();
        SelectedUnit = Units.FirstOrDefault();
        SelectedTaxRate = TaxRates.FirstOrDefault();
    }

    protected override async Task<Product> CreateEntityAsync()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Group = SelectedGroup!,
            TaxRate = SelectedTaxRate!,
            DefaultUnit = SelectedUnit!
        };
        await _productService.SaveAsync(product);
        return product;
    }
}
