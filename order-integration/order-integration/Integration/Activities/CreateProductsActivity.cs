using SQL = Data.Domain;
using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Activities
{
    public class CreateProductsActivity
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;


        public CreateProductsActivity(IProductService accountService, IProductRepository productRepository, ILoggerFactory loggerFactory)
        {
            _productRepository = productRepository;
            _productService = accountService;
            _logger = loggerFactory.CreateLogger<CreateProductsActivity>();
        }


        [Function(nameof(CreateProductsActivity))]
        public async Task<List<Invoice>> Run([ActivityTrigger] List<Invoice> invoices, CancellationToken cancellationToken = default)
        {
            foreach (Invoice invoice in invoices)
            {
                foreach (Item item in invoice.Items)
                {
                    EGC.dev_Produkt? product = string.IsNullOrWhiteSpace(item.Description)
                        ? null : await _productService.RetrieveProductAsync(item.Description, cancellationToken);

                    if (product != null)
                    {
                        _logger.LogInformation($"Exists customer with id: {product.Id}");
                        item.Id = product.Id;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.Description)) continue;

                    SQL.Product? productSQL = await _productRepository.RetrieveProduct(item, cancellationToken);
                    if (productSQL == null)
                    {
                        _logger.LogWarning($"Could not find product named {item.Description} in SQL. Product could not be created in CRM.");
                        continue;
                    }

                    Guid productId = await _productService.CreateProductAsync(productSQL!, cancellationToken);
                    _logger.LogInformation($"Created product with id: {productId}");
                    item.Id = productId;
                }
            }
            return invoices;
        }
    }
}
