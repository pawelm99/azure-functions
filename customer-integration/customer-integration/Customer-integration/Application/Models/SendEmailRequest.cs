namespace customer_integration.Application.Models
{
    public class SendEmailRequest
    {
        public string Customer { get; set; }
        public string Apartment { get; set; }
        public Guid AppointmentId { get; set; }


        public SendEmailRequest(string customerId, string apartmentId, Guid appointmentId)
        {
            Customer = customerId;
            Apartment = apartmentId;
            AppointmentId = appointmentId;
        }
    }
}
