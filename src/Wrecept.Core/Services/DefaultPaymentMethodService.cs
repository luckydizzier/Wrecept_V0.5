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

    public Task<List<PaymentMethod>> GetAllAsync() =>
        ServiceUtil.WrapAsync(_repository.GetAllAsync, "Failed to load payment methods.");

    public Task<PaymentMethod?> GetByIdAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.GetByIdAsync(id), "Failed to load payment method.");

    public async Task SaveAsync(PaymentMethod entity)
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
        }, "Failed to save payment method.");
    }

    public Task DeleteAsync(Guid id) =>
        ServiceUtil.WrapAsync(() => _repository.DeleteAsync(id), "Failed to delete payment method.");
}
