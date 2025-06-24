namespace Wrecept.Services;

public interface INavigationService
{
    void ShowInvoiceListView();
    void ShowMasterDataView();
    void ShowFilterByDateView();
    void ShowFilterBySupplierView();
    void ShowFilterByProductGroupView();
    void ShowFilterByProductView();
    void ShowHelpView();
    void ShowAboutDialog();
    void ExitApplication();
}
