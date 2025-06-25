namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task SaveAsync(Product entity);
    Task DeleteAsync(Guid id);
}

public class DefaultProductService : IProductService
{
    private readonly IProductRepository _repository;

    public DefaultProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Product>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load products.");

    public Task<Product?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load product.");

    public async Task SaveAsync(Product entity)
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
        }, "Failed to save product.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete product.");
}
