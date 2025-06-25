using System.Text.Json;

namespace Wrecept.Infrastructure;

public class Settings
{
    public string Theme { get; set; } = "Light";
}

public static class SettingsService
{
    private static readonly string _path;

    static SettingsService()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Wrecept");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "settings.json");
    }

    public static Settings Load()
    {
        if (!File.Exists(_path))
            return new Settings();
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
    }

    public static void Save(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(_path, json);
    }
}
