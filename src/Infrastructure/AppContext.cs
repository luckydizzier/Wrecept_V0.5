namespace Wrecept.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.Services;

public static class AppContext
{
    private static readonly Dictionary<Type, object> _services;
    public static string DatabasePath { get; }
    public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

    static AppContext()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dir);
        DatabasePath = Path.Combine(dir, "wrecept.db");
        Console.WriteLine($"Database path: {DatabasePath}");

        SqliteMigrator.EnsureCreatedAsync(DatabasePath).GetAwaiter().GetResult();
        var connectionFactory = new SqliteConnectionFactory(DatabasePath);
        SeedDataService.SeedAsync(connectionFactory).GetAwaiter().GetResult();

        var invoiceRepo = new SqliteInvoiceRepository(connectionFactory);
        var invoiceItemRepo = new InMemoryInvoiceItemRepository();
        var productRepo = new SqliteProductRepository(connectionFactory);
        var productGroupRepo = new InMemoryProductGroupRepository();
        var supplierRepo = new SqliteSupplierRepository(connectionFactory);
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
        DialogService = new KeyboardDialogService();
        NavigationService = new NavigationService();

        _services = new Dictionary<Type, object>
        {
            [typeof(IInvoiceService)] = InvoiceService,
            [typeof(IInvoiceItemService)] = InvoiceItemService,
            [typeof(IProductService)] = ProductService,
            [typeof(IProductGroupService)] = ProductGroupService,
            [typeof(ISupplierService)] = SupplierService,
            [typeof(IPaymentMethodService)] = PaymentMethodService,
            [typeof(ITaxRateService)] = TaxRateService,
            [typeof(IUnitService)] = UnitService,
            [typeof(IKeyboardDialogService)] = DialogService,
            [typeof(INavigationService)] = NavigationService
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
    public static IKeyboardDialogService DialogService { get; }
    public static INavigationService NavigationService { get; }

    public static T GetService<T>() where T : class
    {
        return _services[typeof(T)] as T
               ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
    }
}
