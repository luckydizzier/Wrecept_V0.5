using System;
using System.Windows;

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
        var view = new Wrecept.Views.MasterData.SupplierView { DataContext = vm };
        view.ShowDialog();
    }

    public void ShowProductView()
    {
        var vm = new Wrecept.ViewModels.ProductListViewModel(WreceptAppContext.ProductService);
        var view = new Wrecept.Views.MasterData.ProductView { DataContext = vm };
        view.ShowDialog();
    }

    public void ShowSettingsView()
    {
        var vm = new Wrecept.ViewModels.SettingsViewModel();
        var view = new Wrecept.Views.Settings.SettingsWindow { DataContext = vm };
        view.ShowDialog();
    }

    public void ShowFilterByDateView(Action<DateOnly?, DateOnly?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.DateFilterViewModel(applyFilter);
        var dlg = new Wrecept.Views.Filters.DateFilterDialog { DataContext = vm };
        dlg.ShowDialog();
    }

    public void ShowFilterBySupplierView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.SupplierFilterViewModel(applyFilter, WreceptAppContext.SupplierService);
        var dlg = new Wrecept.Views.Filters.SupplierFilterDialog { DataContext = vm };
        dlg.ShowDialog();
    }

    public void ShowFilterByProductGroupView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductGroupFilterViewModel(applyFilter, WreceptAppContext.ProductGroupService);
        var dlg = new Wrecept.Views.Filters.ProductGroupFilterDialog { DataContext = vm };
        dlg.ShowDialog();
    }

    public void ShowFilterByProductView(Action<Guid?> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductFilterViewModel(applyFilter, WreceptAppContext.ProductService);
        var dlg = new Wrecept.Views.Filters.ProductFilterDialog { DataContext = vm };
        dlg.ShowDialog();
    }

    public void ShowHelpView()
    {
        MessageBox.Show("Súgó – még nincs megvalósítva", "Információ");
    }

    public void ShowAboutDialog()
    {
        MessageBox.Show("Wrecept – még nincs Névjegy", "Információ");
    }

    public void ExitApplication()
    {
        Application.Current.Shutdown();
    }
}
