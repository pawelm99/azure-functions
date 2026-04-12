namespace Data.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
