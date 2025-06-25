using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Wrecept.Infrastructure;

namespace Wrecept
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _logPath = string.Empty;

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupGlobalHandlers();

            var ok = Infrastructure.AppContext.Initialize();
            if (!ok && Infrastructure.AppContext.LastError is not null)
            {
                LogException(Infrastructure.AppContext.LastError);
                MessageBox.Show(
                    "A SQLite adatbázis nem elérhető, az adatok nem kerülnek mentésre.",
                    "Indítási hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            var settings = Infrastructure.AppContext.SettingsService.LoadAsync().GetAwaiter().GetResult();
            try
            {
                ApplyTheme(settings.Theme);
            }
            catch
            {
                ApplyTheme("Light");
            }

            base.OnStartup(e);

            if (settings.ShowOnboarding)
            {
                var overlay = new Views.OnboardingOverlay { Owner = Current.MainWindow };
                overlay.ShowDialog();
                settings.ShowOnboarding = false;
                _ = Infrastructure.AppContext.SettingsService.SaveAsync(settings);
            }
        }

        private void SetupGlobalHandlers()
        {
            var appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var dir = Path.Combine(appData, "Wrecept");
            Directory.CreateDirectory(dir);
            _logPath = Path.Combine(dir, "errors.log");

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show("Váratlan hiba történt. Részletek az errors.log fájlban.",
                "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void OnDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                LogException(ex);
        }

        private void LogException(Exception ex)
        {
            var line = $"{DateTime.UtcNow:o} | {ex}\n";
            File.AppendAllText(_logPath, line);
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
