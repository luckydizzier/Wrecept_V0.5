using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;
using Wrecept.Infrastructure;
using Wrecept.Core.Domain;
using Wrecept.Views.Dialogs;

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
                    var dialog = new KeyboardConfirmDialog(
                        "Adatbázis sérült. Újra legyen létrehozva? (I: Igen, N: Nem)");
                    if (dialog.ShowDialog() == true && Infrastructure.AppContext.TryRecoverDatabase())
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

            var settings = Infrastructure.AppContext.SettingsService.LoadAsync().GetAwaiter().GetResult();
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

            var invoices = Infrastructure.AppContext.InvoiceService.GetAllAsync().GetAwaiter().GetResult();
            var list = new ObservableCollection<Invoice>(invoices);
            var current = list.FirstOrDefault() ?? new Invoice();
            var editorVm = new ViewModels.InvoiceEditorViewModel(current, false, Infrastructure.AppContext.InvoiceService, list);
            var mainWindow = new Views.InvoiceEditorWindow { DataContext = editorVm };
            MainWindow = mainWindow;
            mainWindow.Show();

            if (settings.ShowOnboarding)
            {
                var overlay = new Views.OnboardingOverlay { Owner = mainWindow };
                overlay.ShowDialog();
                settings.ShowOnboarding = false;
                _ = Infrastructure.AppContext.SettingsService.SaveAsync(settings);
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
