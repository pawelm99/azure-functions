using Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<IssuePriorityRule> IssuePriorityRules { get; set; }
        public DbSet<CrmReport> CrmReports { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
