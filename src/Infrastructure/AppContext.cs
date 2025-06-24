namespace Wrecept.Infrastructure;

using Wrecept.Core.Repositories;
using Wrecept.Core.Services;

public static class AppContext
{
    private static readonly Dictionary<Type, object> _services;

    static AppContext()
    {
        var invoiceRepo = new InMemoryInvoiceRepository();
        var invoiceItemRepo = new InMemoryInvoiceItemRepository();
        var productRepo = new InMemoryProductRepository();
        var productGroupRepo = new InMemoryProductGroupRepository();
        var supplierRepo = new InMemorySupplierRepository();
        var paymentMethodRepo = new InMemoryPaymentMethodRepository();
        var taxRateRepo = new InMemoryTaxRateRepository();
        var unitRepo = new InMemoryUnitRepository();

        InvoiceService = new DefaultInvoiceService(invoiceRepo);
        InvoiceItemService = new DefaultInvoiceItemService(invoiceItemRepo);
        ProductService = new DefaultProductService(productRepo);
        ProductGroupService = new DefaultProductGroupService(productGroupRepo);
        SupplierService = new DefaultSupplierService(supplierRepo);
        PaymentMethodService = new DefaultPaymentMethodService(paymentMethodRepo);
        TaxRateService = new DefaultTaxRateService(taxRateRepo);
        UnitService = new DefaultUnitService(unitRepo);

        _services = new Dictionary<Type, object>
        {
            [typeof(IInvoiceService)] = InvoiceService,
            [typeof(IInvoiceItemService)] = InvoiceItemService,
            [typeof(IProductService)] = ProductService,
            [typeof(IProductGroupService)] = ProductGroupService,
            [typeof(ISupplierService)] = SupplierService,
            [typeof(IPaymentMethodService)] = PaymentMethodService,
            [typeof(ITaxRateService)] = TaxRateService,
            [typeof(IUnitService)] = UnitService
        };
    }

    public static IInvoiceService InvoiceService { get; }
    public static IInvoiceItemService InvoiceItemService { get; }
    public static IProductService ProductService { get; }
    public static IProductGroupService ProductGroupService { get; }
    public static ISupplierService SupplierService { get; }
    public static IPaymentMethodService PaymentMethodService { get; }
    public static ITaxRateService TaxRateService { get; }
    public static IUnitService UnitService { get; }

    public static T GetService<T>() where T : class
    {
        return _services[typeof(T)] as T
               ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
    }
}
