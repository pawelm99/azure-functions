using Integration.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<Guid> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default);
    }
}
