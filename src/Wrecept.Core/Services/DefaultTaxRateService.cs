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

    public Task<List<TaxRate>> GetAllAsync() => _repository.GetAllAsync();

    public Task<TaxRate?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(TaxRate entity)
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
