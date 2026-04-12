namespace Integration.Application.Models
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Address? Address { get; set; }
        public string? AddressRecipient { get; set; }
        public string? TaxId { get; set; }
    }
}
