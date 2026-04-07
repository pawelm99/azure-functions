using Integration.Application.Models;

namespace Integration.Application.Request
{
    public class CreateInvoiceRequest
    {
        public List<Guid> SellersIds { get; set; }
        public List<Guid> CustomersIds { get; set; }
        public List<Invoice> Invoices { get; set; }


        public CreateInvoiceRequest(List<Guid> sellersIds, List<Guid> customersIds, List<Invoice> invoices)
        {
            SellersIds = sellersIds;
            CustomersIds = customersIds;
            Invoices = invoices;
        }
    }
}
