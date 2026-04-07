namespace Integration.Application.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? TaxId { get; set; }
        public Address? Address { get; set; }
        public string? AddressRecipient { get; set; }
    }
}
