using System;
using System.Threading.Tasks;

namespace Wrecept.Services;

public interface INavigationService
{
    void SetHost(ViewModels.MainWindowViewModel host);
    Task ShowInvoiceListViewAsync();
    void ShowSupplierView();
    void ShowProductView();
    void ShowUnitView();
    void ShowProductGroupView();
    void ShowTaxRateView();
    void ShowSettingsView();
    void ShowFilterByDateView();
    void ShowFilterBySupplierView();
    void ShowFilterByProductGroupView();
    void ShowFilterByProductView();
    void ShowHelpView();
    void ShowAboutDialog();
    void ShowOnboardingOverlay();
    void CloseCurrentView();
    void ExitApplication();
}
