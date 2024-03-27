using MiniLoanApplication.Models;

namespace MiniLoanApplication.Service
{
    public interface IScheduledRepaymentService
    {


        Task<List<ScheduledRepayment>> GetScheduledRepaymentsByLoanIdAsync(int loanId);
    }
}
