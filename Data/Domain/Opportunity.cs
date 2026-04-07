namespace Data.Domain
{
    public class Opportunity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
