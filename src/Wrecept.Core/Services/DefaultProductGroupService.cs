namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IProductGroupService
{
    Task<List<ProductGroup>> GetAllAsync();
    Task<ProductGroup?> GetByIdAsync(Guid id);
    Task SaveAsync(ProductGroup entity);
    Task DeleteAsync(Guid id);
}

public class DefaultProductGroupService : IProductGroupService
{
    private readonly IProductGroupRepository _repository;

    public DefaultProductGroupService(IProductGroupRepository repository)
    {
        _repository = repository;
    }

    public Task<List<ProductGroup>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load product groups.");

    public Task<ProductGroup?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load product group.");

    public async Task SaveAsync(ProductGroup entity)
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
        }, "Failed to save product group.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete product group.");
}
