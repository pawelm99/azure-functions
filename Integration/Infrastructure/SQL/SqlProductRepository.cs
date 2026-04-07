using Data.Context;
using Data.Domain;
using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Integration.Infrastructure.SQL
{
    public class SqlProductRepository : IProductRepository
    {
        public readonly AppDbContext _dbContext;


        public SqlProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Product?> RetrieveProduct(Item item, CancellationToken cancellationToken = default)
            => await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Name == item.Description, cancellationToken);
    }
}
