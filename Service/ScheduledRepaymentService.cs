using Microsoft.EntityFrameworkCore;
using MiniLoanApplication.Models;

namespace MiniLoanApplication.Service
{
    public class ScheduledRepaymentService : IScheduledRepaymentService
    {
       

        private readonly LoanDbContext _dbContext;

        public ScheduledRepaymentService(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ScheduledRepayment>> GetScheduledRepaymentsByLoanIdAsync(int loanId)
        {
            var loan = await _dbContext.Loans.Include(l => l.Repayments).FirstOrDefaultAsync(l => l.Id == loanId);
            return loan?.Repayments.Where(r => r.State == "PENDING").ToList();
        }


    }
}
