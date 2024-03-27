using MiniLoanApplication.Models;

namespace MiniLoanApplication.Service
{
    public interface IloanService
    {
        Loan CreateLoan(string customerName, string email, string password, string phoneNo, decimal amount, int term, DateTime date);
        void ApproveLoan(int loanId);
        ApproveLoanRequestModel GetLoanDetails(int loanId);
        IEnumerable<CustomerLoanDetails> GetLoansByCustomer(string Email, string Password);
        void AddRepayment(int loanid, decimal amount);

        public bool ValidateUser(string Email, string Password);
        IEnumerable<Loan> GetAll();
    }
}
