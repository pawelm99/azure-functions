namespace Data.Domain
{
    public class CrmReport(Guid id, DateTime reportDate, int totalCases, int highPriority, int closedCases)
    {
        public Guid Id { get; set; } = id;
        public DateTime ReportDate { get; set; } = reportDate;
        public int TotalCases { get; set; } = totalCases;
        public int HighPriority { get; set; } = highPriority;
        public int ClosedCases { get; set; } = closedCases;
    }
}
