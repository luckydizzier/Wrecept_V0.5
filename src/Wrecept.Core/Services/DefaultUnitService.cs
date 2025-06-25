namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IUnitService
{
    Task<List<Unit>> GetAllAsync();
    Task<Unit?> GetByIdAsync(Guid id);
    Task SaveAsync(Unit entity);
    Task DeleteAsync(Guid id);
}

public class DefaultUnitService : IUnitService
{
    private readonly IUnitRepository _repository;

    public DefaultUnitService(IUnitRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Unit>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load units.");

    public Task<Unit?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load unit.");

    public async Task SaveAsync(Unit entity)
    {
        await ServiceUtil.WrapAsync(async () =>
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                await _repository.AddAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }
        }, "Failed to save unit.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete unit.");
}
