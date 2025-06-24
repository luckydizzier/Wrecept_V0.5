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

    public Task<List<Unit>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Unit?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(Unit entity)
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
    }

    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
}
