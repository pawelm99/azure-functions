using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Product
{
    public interface IProductCrmClient
    {
        Task<Guid> CreateProductAsync(Data.Domain.Product product, CancellationToken cancellationToken = default);
        Task<EGC.dev_Produkt?> RetrieveProductAsync(string description, CancellationToken cancellationToken = default);
    }
}
