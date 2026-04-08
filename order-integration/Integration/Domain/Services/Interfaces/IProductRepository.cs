using Data.Domain;
using Integration.Application.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> RetrieveProduct(Item item, CancellationToken cancellationToken = default);
    }
}
