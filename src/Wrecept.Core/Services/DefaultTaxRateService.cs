namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface ITaxRateService
{
    Task<List<TaxRate>> GetAllAsync();
    Task<TaxRate?> GetByIdAsync(Guid id);
    Task SaveAsync(TaxRate entity);
    Task DeleteAsync(Guid id);
}

public class DefaultTaxRateService : ITaxRateService
{
    private readonly ITaxRateRepository _repository;

    public DefaultTaxRateService(ITaxRateRepository repository)
    {
        _repository = repository;
    }

    public Task<List<TaxRate>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load tax rates.");

    public Task<TaxRate?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load tax rate.");

    public async Task SaveAsync(TaxRate entity)
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
        }, "Failed to save tax rate.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete tax rate.");
}
