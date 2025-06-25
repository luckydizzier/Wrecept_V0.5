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

    public Task<List<Supplier>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load suppliers.");

    public Task<Supplier?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load supplier.");

    public async Task SaveAsync(Supplier entity)
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
        }, "Failed to save supplier.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete supplier.");
}
