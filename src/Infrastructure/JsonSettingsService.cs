using System.Text.Json;
using System;
using System.IO;

namespace Wrecept.Infrastructure;

using Wrecept.Services;

public class JsonSettingsService : ISettingsService
{
    private readonly string _path;

    public JsonSettingsService()
    {
        var dir = AppDirectories.GetWritableAppDataDirectory();
        _path = Path.Combine(dir, "settings.json");
    }

    public async Task<Settings> LoadAsync()
    {
        if (!File.Exists(_path))
            return new Settings();
        try
        {
            await using var stream = File.OpenRead(_path);
            var settings = await JsonSerializer.DeserializeAsync<Settings>(stream).ConfigureAwait(false);
            return settings ?? new Settings();
        }
        catch
        {
            return new Settings();
        }
    }

    public async Task SaveAsync(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        try
        {
            await File.WriteAllTextAsync(_path, json).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogError(ex);
            System.Windows.MessageBox.Show(
                "A beállítások mentése nem sikerült. Részletek az errors.log-ban.",
                "Mentési hiba", System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    private static void LogError(Exception ex)
    {
        var dir = AppDirectories.GetWritableAppDataDirectory();
        var logPath = Path.Combine(dir, "errors.log");
        var line = $"{DateTime.UtcNow:o} | {ex}\n";
        File.AppendAllText(logPath, line);
    }
}
