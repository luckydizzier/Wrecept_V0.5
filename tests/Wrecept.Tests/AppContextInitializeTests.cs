using System;
using System.IO;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class AppContextInitializeTests : IDisposable
{
    private readonly string? _origLocalAppData;

    public AppContextInitializeTests()
    {
        _origLocalAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
        Reset();
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("LOCALAPPDATA", _origLocalAppData);
        Reset();
    }

    private static void Reset()
    {
        var type = typeof(AppContext);
        var initField = type.GetField("_initialized", BindingFlags.Static | BindingFlags.NonPublic)!;
        initField.SetValue(null, false);
        var servicesField = type.GetField("_services", BindingFlags.Static | BindingFlags.NonPublic)!;
        var dict = (System.Collections.IDictionary)servicesField.GetValue(null)!;
        dict.Clear();
        var errField = type.GetProperty("LastError")!;
        errField.SetValue(null, null);
    }

    [Fact]
    public void Initialize_ShouldReturnTrue_WithWritablePath()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("LOCALAPPDATA", temp);

        var ok = AppContext.Initialize();

        Assert.True(ok);
        Assert.Null(AppContext.LastError);
    }

    [Fact]
    public void Initialize_ShouldReturnFalse_WhenDirectoryUnavailable()
    {
        Environment.SetEnvironmentVariable("LOCALAPPDATA", "/sys");

        var ok = AppContext.Initialize();

        Assert.False(ok);
        Assert.NotNull(AppContext.LastError);
    }

    [Fact]
    public void TryRecoverDatabase_ShouldRecreate_WhenCorrupt()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var dbDir = Path.Combine(dir, "Wrecept");
        Directory.CreateDirectory(dbDir);
        File.WriteAllText(Path.Combine(dbDir, "wrecept.db"), "bad");
        Environment.SetEnvironmentVariable("LOCALAPPDATA", dir);

        var ok = AppContext.Initialize();
        Assert.False(ok);

        var recovered = AppContext.TryRecoverDatabase();
        Assert.True(recovered);
        Assert.True(File.Exists(Path.Combine(dbDir, "wrecept.db")));
    }

    [Fact]
    public void IsDatabaseCorrupt_ShouldDetectErrorCodes()
    {
        var corrupt = new SqliteException("c", 11);
        var notadb = new SqliteException("c", 26);
        var other = new SqliteException("c", 5);

        Assert.True(AppContext.IsDatabaseCorrupt(corrupt));
        Assert.True(AppContext.IsDatabaseCorrupt(notadb));
        Assert.False(AppContext.IsDatabaseCorrupt(other));
    }

    [Fact]
    public void IsDatabaseLocked_ShouldDetectErrorCodes()
    {
        var locked1 = new SqliteException("locked", 5);
        var locked2 = new SqliteException("busy", 6);
        var other = new SqliteException("ok", 1);

        Assert.True(AppContext.IsDatabaseLocked(locked1));
        Assert.True(AppContext.IsDatabaseLocked(locked2));
        Assert.False(AppContext.IsDatabaseLocked(other));
    }

    [Fact]
    public void Initialize_ShouldCreateDatabase_WhenMissing()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("LOCALAPPDATA", dir);

        var ok = AppContext.Initialize();

        Assert.True(ok);
        Assert.True(File.Exists(Path.Combine(dir, "Wrecept", "wrecept.db")));
    }

    [Fact]
    public void Initialize_ShouldReturnFalse_WhenFileLocked()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("LOCALAPPDATA", dir);
        var dbDir = Path.Combine(dir, "Wrecept");
        Directory.CreateDirectory(dbDir);
        var dbPath = Path.Combine(dbDir, "wrecept.db");
        using var fs = File.Open(dbPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        var ok = AppContext.Initialize();

        Assert.False(ok);
        Assert.NotNull(AppContext.LastError);
    }
}
