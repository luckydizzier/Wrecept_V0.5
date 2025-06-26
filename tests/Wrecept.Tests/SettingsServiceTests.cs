using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class SettingsServiceTests : IDisposable
{
    private readonly string _tempDir;

    public SettingsServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(_tempDir, "Wrecept"));
        Environment.SetEnvironmentVariable("LOCALAPPDATA", _tempDir);
    }

    public void Dispose()
    {
        Directory.Delete(_tempDir, true);
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnDefaults_WhenFileMissing()
    {
        var service = new JsonSettingsService();

        var settings = await service.LoadAsync();

        Assert.NotNull(settings);
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnDefaults_WhenFileCorrupt()
    {
        var path = Path.Combine(_tempDir, "Wrecept", "settings.json");
        File.WriteAllText(path, "{invalid}");
        var service = new JsonSettingsService();

        var settings = await service.LoadAsync();

        Assert.NotNull(settings);
    }
}

