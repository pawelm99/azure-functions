
namespace Integration.Application.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string? InvoiceId { get; set; }
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
        public DateTimeOffset? InvoiceDate { get; set; }
        public Currency? InvoiceTotal { get; set; }
        public string? PaymentTerm { get; set; }
        public string? VendorTaxId { get; set; }
        public Currency? SubTotal { get; set; }
        public Currency? TotalTax { get; set; }
        public List<PaymentDetail>? PaymentDetails { get; set; }
        public List<Item> Items { get; set; }
    }
}
