namespace Integration.Application.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public Currency? Amount { get; set; }
    }
}
