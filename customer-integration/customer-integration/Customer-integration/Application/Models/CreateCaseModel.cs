namespace customer_integration.Application.Models
{
    public class CreateCaseModel
    {
        public string Title { get; set; }
        public string Customer { get; set; }
        public string Apartment { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }


        public CreateCaseModel(string title, string customer, string apartment, string status, string description)
        {
            Title = title;
            Customer = customer;
            Status = status;
            Description = description;
            Apartment = apartment;
        }
    }
}
