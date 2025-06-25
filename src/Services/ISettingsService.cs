using Wrecept.Infrastructure;

namespace Wrecept.Services;

public interface ISettingsService
{
    Task<Settings> LoadAsync();
    Task SaveAsync(Settings settings);
}
