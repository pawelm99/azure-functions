using Data.Domain;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<EGC.dev_Produkt?> RetrieveProductAsync(string description, CancellationToken cancellationToken);
    }
}
