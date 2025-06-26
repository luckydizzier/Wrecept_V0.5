namespace Wrecept.Infrastructure;

public static class AppDirectories
{
    public static string GetWritableAppDataDirectory()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(path) || !TryCreate(path))
            path = AppContext.BaseDirectory;
        var dir = System.IO.Path.Combine(path, "Wrecept");
        System.IO.Directory.CreateDirectory(dir);
        return dir;
    }

    private static bool TryCreate(string path)
    {
        try
        {
            System.IO.Directory.CreateDirectory(path);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
