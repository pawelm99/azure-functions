namespace customer_integration.Application.Models
{
    public class CaseState
    {
        public string Title { get; set; }
        public string Customer { get; set; }
        public string Apartment { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Guid? CaseId { get; set; }
        public Guid? AppointmentId { get; set; }
        public string? Priority { get; set; }
        public Guid? EmailId { get; internal set; }

        public CaseState(string title, string customer, string apartment, string status,
            string description, Guid? caseId = null, Guid? appointmentId = null, string? priority = null, Guid? emailId = null)
        {
            Title = title;
            Customer = customer;
            Apartment = apartment;
            Status = status;
            Description = description;
            CaseId = caseId;
            AppointmentId = appointmentId;
            Priority = priority;
            EmailId = emailId;
        }
    }
}
