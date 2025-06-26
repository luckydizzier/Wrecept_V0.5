using System;
using System.Threading.Tasks;

namespace Wrecept.Services;

public interface INavigationService
{
    Task ShowInvoiceListViewAsync();
    void ShowSupplierView();
    void ShowProductView();
    void ShowSettingsView();
    void ShowFilterByDateView(Func<DateOnly?, DateOnly?, Task> applyFilter);
    void ShowFilterBySupplierView(Func<Guid?, Task> applyFilter);
    void ShowFilterByProductGroupView(Func<Guid?, Task> applyFilter);
    void ShowFilterByProductView(Func<Guid?, Task> applyFilter);
    void ShowHelpView();
    void ShowAboutDialog();
    void ShowOnboardingOverlay();
    void ExitApplication();
}
