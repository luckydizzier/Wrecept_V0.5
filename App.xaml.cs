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

            ApplyFontScale(settings.FontScale);

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

        public static void ApplyFontScale(int scale)
        {
            var baseSize = 14 + scale;
            if (baseSize < 8) baseSize = 8;
            Application.Current.Resources["BaseFontSize"] = baseSize;
            var small = Math.Max(2, 4 + scale);
            var medium = Math.Max(4, 8 + scale);
            var large = Math.Max(6, 12 + scale);
            Application.Current.Resources["SpacingSmall"] = new Thickness(small);
            Application.Current.Resources["SpacingMedium"] = new Thickness(medium);
            Application.Current.Resources["SpacingLarge"] = new Thickness(large);
            Application.Current.Resources["MarginBottomSmall"] = new Thickness(0, 0, 0, small);
            Application.Current.Resources["MarginBottomMedium"] = new Thickness(0, 0, 0, medium);
            Application.Current.Resources["MarginBottomLarge"] = new Thickness(0, 0, 0, large);
            Application.Current.Resources["MarginTopMedium"] = new Thickness(0, medium, 0, 0);
            Application.Current.Resources["MarginTopLarge"] = new Thickness(0, large, 0, 0);
            Application.Current.Resources["MarginVerticalSmall"] = new Thickness(0, small, 0, small);
            Application.Current.Resources["MarginRightMediumBottomSmall"] = new Thickness(0, 0, medium, small);
            Application.Current.Resources["MarginRightSmallBottomSmall"] = new Thickness(0, 0, small, small);
            Application.Current.Resources["MarginTopSmallRightSmallBottomSmall"] = new Thickness(0, small, small, small);
            Application.Current.Resources["MarginLeftLarge"] = new Thickness(large, 0, 0, 0);
            Application.Current.Resources["MarginLeftMedium"] = new Thickness(medium, 0, 0, 0);
            Application.Current.Resources["MarginLeftMediumTopMediumBottomSmall"] = new Thickness(medium, medium, 0, small);
            Application.Current.Resources["IconSize"] = Math.Max(12, 16 + scale);
            Application.Current.Resources["RowHeight"] = Math.Max(18, 26 + scale);
        }
    }

}
