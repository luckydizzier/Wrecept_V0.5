namespace Wrecept.Core.Services;

using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;

public interface IPaymentMethodService
{
    Task<List<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(Guid id);
    Task SaveAsync(PaymentMethod entity);
    Task DeleteAsync(Guid id);
}

public class DefaultPaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _repository;

    public DefaultPaymentMethodService(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public Task<List<PaymentMethod>> GetAllAsync() => _repository.GetAllAsync();

    public Task<PaymentMethod?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task SaveAsync(PaymentMethod entity)
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
