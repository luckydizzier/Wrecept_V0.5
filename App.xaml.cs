using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Wrecept.Infrastructure;

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
            Infrastructure.AppContext.Initialize();
            var settings = Infrastructure.AppContext.SettingsService.LoadAsync().GetAwaiter().GetResult();
            try
            {
                ApplyTheme(settings.Theme);
            }
            catch
            {
                ApplyTheme("Light");
            }
        }

        public static void ApplyTheme(string theme)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Wrecept;component/Themes/{theme}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }

}
