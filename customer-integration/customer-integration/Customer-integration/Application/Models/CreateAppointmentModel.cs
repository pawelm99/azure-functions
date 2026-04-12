namespace customer_integration.Application.Models
{
    public class CreateAppointmentModel
    {
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public Guid Apartment { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }


        public CreateAppointmentModel(string name, Guid customerId, Guid apartment, DateTime date, string notes)
        {
            Name = name;
            CustomerId = customerId;
            Apartment = apartment;
            Date = date;
            Notes = notes;
        }
    }
}
