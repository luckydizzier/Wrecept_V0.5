namespace Wrecept.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.Infrastructure;

public static class AppContext
{
    private static readonly Dictionary<Type, object> _services = new();
    private static bool _initialized;
    public static string DatabasePath { get; private set; } = string.Empty;
    public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

    public static void Initialize()
    {
        if (_initialized)
            return;

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dir);
        DatabasePath = Path.Combine(dir, "wrecept.db");
        Console.WriteLine($"Database path: {DatabasePath}");

        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        try
        {
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
            SettingsService = new JsonSettingsService();

            _services[typeof(IInvoiceService)] = InvoiceService;
            _services[typeof(IInvoiceItemService)] = InvoiceItemService;
            _services[typeof(IProductService)] = ProductService;
            _services[typeof(IProductGroupService)] = ProductGroupService;
            _services[typeof(ISupplierService)] = SupplierService;
            _services[typeof(IPaymentMethodService)] = PaymentMethodService;
            _services[typeof(ITaxRateService)] = TaxRateService;
            _services[typeof(IUnitService)] = UnitService;
            _services[typeof(IKeyboardDialogService)] = DialogService;
            _services[typeof(INavigationService)] = NavigationService;
            _services[typeof(ISettingsService)] = SettingsService;

            _initialized = true;
        }
        catch (SqliteException ex)
        {
            Console.Error.WriteLine($"SQLite init error: {ex.Message}");
            throw;
        }
    }

    public static IInvoiceService InvoiceService { get; private set; } = null!;
    public static IInvoiceItemService InvoiceItemService { get; private set; } = null!;
    public static IProductService ProductService { get; private set; } = null!;
    public static IProductGroupService ProductGroupService { get; private set; } = null!;
    public static ISupplierService SupplierService { get; private set; } = null!;
    public static IPaymentMethodService PaymentMethodService { get; private set; } = null!;
    public static ITaxRateService TaxRateService { get; private set; } = null!;
    public static IUnitService UnitService { get; private set; } = null!;
    public static IKeyboardDialogService DialogService { get; private set; } = null!;
    public static INavigationService NavigationService { get; private set; } = null!;
    public static ISettingsService SettingsService { get; private set; } = null!;
    public static Action<string>? StatusMessageSetter { get; set; }

    public static void SetStatus(string message) => StatusMessageSetter?.Invoke(message);

    public static T GetService<T>() where T : class
    {
        return _services[typeof(T)] as T
               ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
    }
}
