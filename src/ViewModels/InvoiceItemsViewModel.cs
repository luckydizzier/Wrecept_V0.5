using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class InvoiceItemsViewModel : ObservableObject
{
    public Invoice Invoice { get; }
    public ObservableCollection<InvoiceItemRowViewModel> Rows { get; }
    public InvoiceItemRowViewModel Entry { get; }
    public InlineProductCreatorViewModel? ProductCreator { get; private set; }

    public IRelayCommand AddItemCommand { get; }

    private readonly IProductService _productService;
    private readonly IProductGroupService _groupService;
    private readonly IUnitService _unitService;
    private readonly ITaxRateService _taxService;

    public InvoiceItemsViewModel(Invoice invoice)
        : this(invoice,
            Infrastructure.AppContext.ProductService,
            Infrastructure.AppContext.ProductGroupService,
            Infrastructure.AppContext.UnitService,
            Infrastructure.AppContext.TaxRateService)
    {
    }

    public InvoiceItemsViewModel(
        Invoice invoice,
        IProductService productService,
        IProductGroupService groupService,
        IUnitService unitService,
        ITaxRateService taxService)
    {
        _productService = productService;
        _groupService = groupService;
        _unitService = unitService;
        _taxService = taxService;

        Invoice = invoice;
        Rows = new ObservableCollection<InvoiceItemRowViewModel>();
        Entry = new InvoiceItemRowViewModel { IsPlaceholder = true };
        Rows.Add(Entry);
        foreach (var item in invoice.Items)
        {
            Rows.Add(new InvoiceItemRowViewModel(item));
        }
        AddItemCommand = new RelayCommand(AddItem);
    }

    private void AddItem()
    {
        if (!Entry.Validate())
            return;

        var model = Entry.ToModel();
        Invoice.Items.Add(model);
        Rows.Add(new InvoiceItemRowViewModel(model));
        Entry.Clear();
        OnPropertyChanged(nameof(Rows));
    }

    public bool TryOpenProductCreator()
    {
        var name = Entry.ProductName.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var existing = _productService.GetAllAsync().Result
            .Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (existing)
            return false;

        ProductCreator = new InlineProductCreatorViewModel(
            _productService,
            _groupService,
            _unitService,
            _taxService,
            name);
        ProductCreator.Saved += OnProductSaved;
        ProductCreator.Canceled += OnProductCanceled;
        OnPropertyChanged(nameof(ProductCreator));
        return true;
    }

    private void OnProductSaved(Product product)
    {
        Entry.ProductName = product.Name;
        ProductCreator = null;
        OnPropertyChanged(nameof(ProductCreator));
    }

    private void OnProductCanceled()
    {
        ProductCreator = null;
        OnPropertyChanged(nameof(ProductCreator));
    }
}
