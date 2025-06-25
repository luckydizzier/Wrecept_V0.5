using System.Configuration;
using System.Data;
using System.Windows;

namespace Wrecept
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var settings = Infrastructure.SettingsService.Load();
            ApplyTheme(settings.Theme);
        }

        public static void ApplyTheme(string theme)
        {
            var dict = new ResourceDictionary { Source = new Uri($"Themes/{theme}.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }

}
