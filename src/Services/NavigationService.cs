using System.Windows;

namespace Wrecept.Services;

public class NavigationService : INavigationService
{
    public void ShowInvoiceListView()
    {
        MessageBox.Show("Számlák kezelése – még nincs megvalósítva", "Információ");
    }

    public void ShowMasterDataView()
    {
        MessageBox.Show("Törzsadatok kezelése – még nincs megvalósítva", "Információ");
    }

    public void ShowFilterByDateView()
    {
        MessageBox.Show("Dátum alapú keresés – még nincs megvalósítva", "Információ");
    }

    public void ShowFilterBySupplierView()
    {
        MessageBox.Show("Szállító alapú keresés – még nincs megvalósítva", "Információ");
    }

    public void ShowFilterByProductGroupView()
    {
        MessageBox.Show("Termékcsoport keresés – még nincs megvalósítva", "Információ");
    }

    public void ShowFilterByProductView()
    {
        MessageBox.Show("Termék keresés – még nincs megvalósítva", "Információ");
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
