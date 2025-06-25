using System;

namespace Wrecept.Services;

public interface INavigationService
{
    void ShowInvoiceListView();
    void ShowMasterDataView();
    void ShowFilterByDateView(Action<DateOnly?, DateOnly?> applyFilter);
    void ShowFilterBySupplierView();
    void ShowFilterByProductGroupView();
    void ShowFilterByProductView();
    void ShowHelpView();
    void ShowAboutDialog();
    void ExitApplication();
}
