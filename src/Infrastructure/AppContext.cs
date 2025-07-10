namespace Wrecept.Infrastructure;

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Services;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.Core.Plugins;

public static class AppContext
{
    private static bool _initialized;
    public static string DatabasePath { get; private set; } = string.Empty;
    public static string? CustomDatabasePath { get; set; }
    public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

    public static Exception? LastError { get; private set; }
    public static bool DatabaseAvailable => LastError is null;
    public static bool InputLocked { get; set; }

    public static bool Initialize(IServiceCollection services)
    {
        if (_initialized)
            return LastError is null;

        var dir = AppDirectories.GetWritableAppDataDirectory();
        DatabasePath = CustomDatabasePath ?? Path.Combine(dir, "wrecept.db");
        Console.WriteLine($"Database path: {DatabasePath}");


        try
        {
            var options = new DbContextOptionsBuilder<WreceptDbContext>()
                .UseSqlite($"Data Source={DatabasePath}")
                .Options;
            var dbContext = new WreceptDbContext(options);
            dbContext.Database.EnsureCreated();
            SchemaUpgradeService.EnsureLatestSchema(dbContext);
            SeedDataService.SeedAsync(dbContext).GetAwaiter().GetResult();

            SetupSqliteServices(services, dbContext);

            _initialized = true;
            LastError = null;
            return true;
        }
        catch (SqliteException ex)
        {
            Console.Error.WriteLine($"SQLite init error: {ex.Message}");
            SetupInMemoryServices(services);
            _initialized = true;
            LastError = ex;
            return false;
        }
    }

    public static bool TryRecoverDatabase(IServiceCollection services)
    {
        var backup = DatabasePath + ".bak";
        try
        {
            if (File.Exists(DatabasePath))
            {
                File.Copy(DatabasePath, backup, true);
                File.Delete(DatabasePath);
            }
            var options = new DbContextOptionsBuilder<WreceptDbContext>()
                .UseSqlite($"Data Source={DatabasePath}")
                .Options;
            var dbContext = new WreceptDbContext(options);
            dbContext.Database.EnsureCreated();
            SchemaUpgradeService.EnsureLatestSchema(dbContext);
            SeedDataService.SeedAsync(dbContext).GetAwaiter().GetResult();
            SetupSqliteServices(services, dbContext);
            LastError = null;
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Recovery failed: {ex.Message}");
            LastError = ex;
            SetupInMemoryServices(services);
            return false;
        }
    }

    public static bool IsDatabaseLocked(SqliteException ex) => ex.SqliteErrorCode is 5 or 6;

    public static bool IsDatabaseCorrupt(SqliteException ex) => ex.SqliteErrorCode is 11 or 26;

    private static readonly List<IMenuPlugin> _menuPlugins = new();
    public static IReadOnlyList<IMenuPlugin> MenuPlugins => _menuPlugins;
    public static Action<string>? StatusMessageSetter { get; set; }

    public static void SetStatus(string message) => StatusMessageSetter?.Invoke(message);

    private static void SetupSqliteServices(IServiceCollection services, WreceptDbContext dbContext)
    {

        var invoiceRepo = new EfInvoiceRepository(dbContext);
        var invoiceItemRepo = new EfInvoiceItemRepository(dbContext);
        var productRepo = new EfProductRepository(dbContext);
        var productGroupRepo = new EfProductGroupRepository(dbContext);
        var supplierRepo = new EfSupplierRepository(dbContext);
        var paymentMethodRepo = new EfPaymentMethodRepository(dbContext);
        var taxRateRepo = new EfTaxRateRepository(dbContext);
        var unitRepo = new EfUnitRepository(dbContext);

        RegisterServices(services, invoiceRepo, invoiceItemRepo, productRepo, productGroupRepo,
            supplierRepo, paymentMethodRepo, taxRateRepo, unitRepo);

        LoadMenuPlugins();
    }

    private static void SetupInMemoryServices(IServiceCollection services)
    {
        var invoiceRepo = new InMemoryInvoiceRepository();
        var invoiceItemRepo = new InMemoryInvoiceItemRepository();
        var productRepo = new InMemoryProductRepository();
        var productGroupRepo = new InMemoryProductGroupRepository();
        var supplierRepo = new InMemorySupplierRepository();
        var paymentMethodRepo = new InMemoryPaymentMethodRepository();
        var taxRateRepo = new InMemoryTaxRateRepository();
        var unitRepo = new InMemoryUnitRepository();

        RegisterServices(services, invoiceRepo, invoiceItemRepo, productRepo, productGroupRepo,
            supplierRepo, paymentMethodRepo, taxRateRepo, unitRepo);

        LoadMenuPlugins();
    }

    private static void RegisterServices(
        IServiceCollection services,
        IInvoiceRepository invoiceRepo,
        IInvoiceItemRepository invoiceItemRepo,
        IProductRepository productRepo,
        IProductGroupRepository productGroupRepo,
        ISupplierRepository supplierRepo,
        IPaymentMethodRepository paymentMethodRepo,
        ITaxRateRepository taxRateRepo,
        IUnitRepository unitRepo)
    {
        services.AddSingleton<IInvoiceService>(new DefaultInvoiceService(invoiceRepo));
        services.AddSingleton<IInvoiceItemService>(new DefaultInvoiceItemService(invoiceItemRepo));
        services.AddSingleton<IProductService>(new DefaultProductService(productRepo));
        services.AddSingleton<IProductGroupService>(new DefaultProductGroupService(productGroupRepo));
        services.AddSingleton<ISupplierService>(new DefaultSupplierService(supplierRepo));
        services.AddSingleton<IPaymentMethodService>(new DefaultPaymentMethodService(paymentMethodRepo));
        services.AddSingleton<ITaxRateService>(new DefaultTaxRateService(taxRateRepo));
        services.AddSingleton<IUnitService>(new DefaultUnitService(unitRepo));
        services.AddSingleton<IKeyboardDialogService, KeyboardDialogService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISettingsService, JsonSettingsService>();
        services.AddSingleton<IPriceHistoryService, JsonPriceHistoryService>();
        services.AddSingleton<IStatusService, StatusService>();
        services.AddSingleton<IFeedbackService, FeedbackService>();
        services.AddSingleton<IFocusService, FocusService>();
        services.AddSingleton<CommandManagerService>();
    }

    private static void LoadMenuPlugins()
    {
        _menuPlugins.Clear();
        _menuPlugins.AddRange(PluginLoader.LoadPlugins());
    }
}
