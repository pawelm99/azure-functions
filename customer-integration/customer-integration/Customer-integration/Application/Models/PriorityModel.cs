namespace customer_integration.Application.Models
{
    public class PriorityModel
    {
        public Guid CaseId { get; set; }
        public string IssueType { get; set; }


        public PriorityModel(Guid id, string issueType)
        {
            CaseId = id;
            IssueType = issueType;
        }
    }
}
