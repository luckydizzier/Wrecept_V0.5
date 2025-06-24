namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface ISupplierService
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(Guid id);
    Task SaveAsync(Supplier entity);
    Task DeleteAsync(Guid id);
}

public class DefaultSupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public DefaultSupplierService(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Supplier>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Supplier?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(Supplier entity)
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
