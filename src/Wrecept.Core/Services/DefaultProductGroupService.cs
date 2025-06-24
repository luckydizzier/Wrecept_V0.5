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

    public Task<List<ProductGroup>> GetAllAsync() => _repository.GetAllAsync();

    public Task<ProductGroup?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(ProductGroup entity)
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
