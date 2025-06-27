using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Infrastructure;
using Wrecept.Services;
using Wrecept.Views.Dialogs;
using Wrecept.ViewModels;

namespace Wrecept
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _logPath = string.Empty;
        private IServiceProvider? _services;

        public static IServiceProvider Services => ((App)Current)._services!;

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupGlobalHandlers();

            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var ok = Infrastructure.AppContext.Initialize(services);
            if (!ok && Infrastructure.AppContext.LastError is SqliteException se)
            {
                LogException(se);
                if (Infrastructure.AppContext.IsDatabaseLocked(se))
                {
                    MessageBox.Show(
                        "A SQLite adatbázis zárolt. Zárd be a másik példányt vagy fájlt.",
                        "Adatbázis zárolva", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (Infrastructure.AppContext.IsDatabaseCorrupt(se))
                {
                    var dialog = services.BuildServiceProvider().GetRequiredService<IKeyboardDialogService>();
                    var confirm = dialog.Confirm(
                        "Adatbázis sérült. Újra legyen létrehozva? (I: Igen, N: Nem)");
                    if (confirm && Infrastructure.AppContext.TryRecoverDatabase(services))
                    {
                        ok = true;
                    }
                    else
                    {
                        MessageBox.Show(
                            "A SQLite adatbázis nem elérhető, az adatok nem kerülnek mentésre.",
                            "Indítási hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "A SQLite adatbázis nem elérhető, az adatok nem kerülnek mentésre.",
                        "Indítási hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            services.AddSingleton(provider => new MainWindowViewModel(
                provider.GetRequiredService<INavigationService>(),
                Infrastructure.AppContext.MenuPlugins));
            _services = services.BuildServiceProvider();
            var settingsService = Services.GetRequiredService<ISettingsService>();
            var settings = settingsService.LoadAsync().GetAwaiter().GetResult();
            try
            {
                ApplyTheme(settings.Theme);
            }
            catch
            {
                ApplyTheme("Light");
            }

            try
            {
                ApplyLanguage(settings.Language);
            }
            catch
            {
                ApplyLanguage("hu");
            }


            base.OnStartup(e);

            var mainVm = Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new MainWindow(mainVm);
            MainWindow = mainWindow;
            mainWindow.Show();
            Services.GetRequiredService<IFeedbackService>().Startup();

            if (settings.ShowOnboarding)
            {
                var overlay = new Views.OnboardingOverlay();
                var window = new Window
                {
                    Content = overlay,
                    Owner = mainWindow,
                    WindowStyle = WindowStyle.None,
                    AllowsTransparency = true,
                    Background = System.Windows.Media.Brushes.Transparent,
                    ShowInTaskbar = false,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                window.ShowDialog();
                settings.ShowOnboarding = false;
                _ = settingsService.SaveAsync(settings);
            }
        }

        private void SetupGlobalHandlers()
        {
            var dir = Infrastructure.AppDirectories.GetWritableAppDataDirectory();
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

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("/Themes/"));
            if (existing != null)
                dictionaries.Remove(existing);
            dictionaries.Add(dict);
        }

        public static void ApplyLanguage(string language)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Wrecept;component/Resources/Strings.{language}.xaml", UriKind.Relative)
            };

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("/Resources/Strings."));
            if (existing != null)
                dictionaries.Remove(existing);
            dictionaries.Add(dict);
        }

    }

}
