using Microsoft.EntityFrameworkCore;

namespace MiniLoanApplication.Models
{
    public class LoanDbContext:DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
        {
        }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRequest> LoanRequests { get; set; }
        public DbSet<ScheduledRepayment> ScheduledRepayments { get; set; }


        
    }
}
