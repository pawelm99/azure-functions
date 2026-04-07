using Data.Domain;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Product;
using EGC = CrmEarlyBound;

namespace Integration.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductCrmClient _productClient;


        public ProductService(IProductCrmClient productClient)
        {
            _productClient = productClient;
        }


        public async Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
            => await _productClient.CreateProductAsync(product, cancellationToken);


        public async Task<EGC.dev_Produkt?> RetrieveProductAsync(string description, CancellationToken cancellationToken)
            => await _productClient.RetrieveProductAsync(description, cancellationToken);
    }
}
