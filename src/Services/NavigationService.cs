using System;
using System.Windows;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;

namespace Wrecept.Services;

public class NavigationService : INavigationService
{
    public void ShowInvoiceListView()
    {
        MessageBox.Show("Számlák kezelése – még nincs megvalósítva", "Információ");
    }

    public void ShowSupplierView()
    {
        var vm = new Wrecept.ViewModels.SupplierListViewModel(WreceptAppContext.SupplierService);
        var view = new Wrecept.Views.MasterData.SupplierView
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        view.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowProductView()
    {
        var vm = new Wrecept.ViewModels.ProductListViewModel(WreceptAppContext.ProductService);
        var view = new Wrecept.Views.MasterData.ProductView
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        view.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowSettingsView()
    {
        var vm = new Wrecept.ViewModels.SettingsViewModel(WreceptAppContext.SettingsService);
        var view = new Wrecept.Views.Settings.SettingsWindow
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        view.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowFilterByDateView(Action<DateOnly?, DateOnly?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.DateFilterViewModel(applyFilter);
        var dlg = new Wrecept.Views.Filters.DateFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        dlg.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowFilterBySupplierView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.SupplierFilterViewModel(applyFilter, WreceptAppContext.SupplierService);
        var dlg = new Wrecept.Views.Filters.SupplierFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        dlg.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowFilterByProductGroupView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductGroupFilterViewModel(applyFilter, WreceptAppContext.ProductGroupService);
        var dlg = new Wrecept.Views.Filters.ProductGroupFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        dlg.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowFilterByProductView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductFilterViewModel(applyFilter, WreceptAppContext.ProductService);
        var dlg = new Wrecept.Views.Filters.ProductFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        Infrastructure.AppContext.InputLocked = true;
        dlg.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowHelpView()
    {
        Infrastructure.AppContext.InputLocked = true;
        MessageBox.Show("Súgó – még nincs megvalósítva", "Információ");
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowAboutDialog()
    {
        Infrastructure.AppContext.InputLocked = true;
        MessageBox.Show("Wrecept – még nincs Névjegy", "Információ");
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ShowOnboardingOverlay()
    {
        var overlay = new Wrecept.Views.OnboardingOverlay { Owner = Application.Current.MainWindow };
        Infrastructure.AppContext.InputLocked = true;
        overlay.ShowDialog();
        Infrastructure.AppContext.InputLocked = false;
    }

    public void ExitApplication()
    {
        Application.Current.Shutdown();
    }
}
