namespace customer_integration.Application.Models
{
    public class CreateEmailModel
    {
        public Guid CustomerId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }


        public CreateEmailModel(Guid customerId, Guid ownerId, string title, string body)
        {
            CustomerId = customerId;
            OwnerId = ownerId;
            Title = title;
            Body = body;
        }
    }
}
