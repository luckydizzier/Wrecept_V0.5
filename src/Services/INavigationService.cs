using System;

namespace Wrecept.Services;

public interface INavigationService
{
    void ShowInvoiceListView();
    void ShowSupplierView();
    void ShowProductView();
    void ShowSettingsView();
    void ShowFilterByDateView(Action<DateOnly?, DateOnly?> applyFilter);
    void ShowFilterBySupplierView(Action<Guid?> applyFilter);
    void ShowFilterByProductGroupView(Action<Guid?> applyFilter);
    void ShowFilterByProductView(Action<Guid?> applyFilter);
    void ShowHelpView();
    void ShowAboutDialog();
    void ShowOnboardingOverlay();
    void ExitApplication();
}
