namespace Data.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public Product[] Products { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Invoice { get; set; }
    }
}
