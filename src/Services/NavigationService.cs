using System;
using System.Windows;
using System.Threading.Tasks;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;

namespace Wrecept.Services;

public class NavigationService : INavigationService
{
    protected virtual void ShowDialog(Window view) => view.ShowDialog();
    public async Task ShowInvoiceListViewAsync()
    {
        var invoices = await WreceptAppContext.InvoiceService.GetAllAsync();
        var list = new System.Collections.ObjectModel.ObservableCollection<Wrecept.Core.Domain.Invoice>(invoices);
        var current = list.FirstOrDefault() ?? new Wrecept.Core.Domain.Invoice();
        var vm = new Wrecept.ViewModels.InvoiceEditorViewModel(current, false, WreceptAppContext.InvoiceService, list);
        var view = new Wrecept.Views.InvoiceEditorWindow
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        view.Loaded += (_, _) => vm.OnLoaded();
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowSupplierView()
    {
        var vm = new Wrecept.ViewModels.SupplierListViewModel(WreceptAppContext.SupplierService);
        var view = new Wrecept.Views.MasterData.SupplierView
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowProductView()
    {
        var vm = new Wrecept.ViewModels.ProductListViewModel(WreceptAppContext.ProductService);
        var view = new Wrecept.Views.MasterData.ProductView
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowSettingsView()
    {
        var vm = new Wrecept.ViewModels.SettingsViewModel(WreceptAppContext.SettingsService);
        var view = new Wrecept.Views.Settings.SettingsWindow
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowFilterByDateView(Func<DateOnly?, DateOnly?, Task> applyFilter)
    {
        var vm = new Wrecept.ViewModels.DateFilterViewModel(applyFilter);
        var dlg = new Wrecept.Views.Filters.DateFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(dlg);
        }
    }

    public void ShowFilterBySupplierView(Func<Guid?, Task> applyFilter)
    {
        var vm = new Wrecept.ViewModels.SupplierFilterViewModel(applyFilter, WreceptAppContext.SupplierService);
        var dlg = new Wrecept.Views.Filters.SupplierFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(dlg);
        }
    }

    public void ShowFilterByProductGroupView(Func<Guid?, Task> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductGroupFilterViewModel(applyFilter, WreceptAppContext.ProductGroupService);
        var dlg = new Wrecept.Views.Filters.ProductGroupFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(dlg);
        }
    }

    public void ShowFilterByProductView(Func<Guid?, Task> applyFilter)
    {
        var vm = new Wrecept.ViewModels.ProductFilterViewModel(applyFilter, WreceptAppContext.ProductService);
        var dlg = new Wrecept.Views.Filters.ProductFilterDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(dlg);
        }
    }

    public void ShowHelpView()
    {
        var view = new Wrecept.Views.Help.HelpWindow { Owner = Application.Current.MainWindow };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowAboutDialog()
    {
        var view = new Wrecept.Views.Help.AboutWindow { Owner = Application.Current.MainWindow };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(view);
        }
    }

    public void ShowOnboardingOverlay()
    {
        var overlay = new Wrecept.Views.OnboardingOverlay { Owner = Application.Current.MainWindow };
        using (new Infrastructure.InputLockScope())
        {
            ShowDialog(overlay);
        }
    }

    public void ExitApplication()
    {
        Infrastructure.AppContext.FeedbackService.Exit();
        Application.Current.Shutdown();
    }
}
