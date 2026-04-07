using Data.Domain;
using EGC = CrmEarlyBound;

namespace Integration.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<EGC.dev_Produkt?> RetrieveProductAsync(string description, CancellationToken cancellationToken);
    }
}
