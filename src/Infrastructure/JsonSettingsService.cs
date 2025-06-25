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
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Wrecept");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "settings.json");
    }

    public async Task<Settings> LoadAsync()
    {
        if (!File.Exists(_path))
            return new Settings();
        try
        {
            await using var stream = File.OpenRead(_path);
            var settings = await JsonSerializer.DeserializeAsync<Settings>(stream);
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
        await File.WriteAllTextAsync(_path, json);
    }
}
