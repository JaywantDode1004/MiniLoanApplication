using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoanApplication.Models;
using System.Globalization;

namespace MiniLoanApplication.Service
{
    public class LoanService : IloanService
    {
        private readonly LoanDbContext _context;

        public LoanService(LoanDbContext context)
        {
            _context = context;
        }

        public Loan CreateLoan(string customerName, string email, string password, string phoneNo, decimal amount, int term, DateTime date)
        {
            var loan = new Loan
            {
                Request = new LoanRequest
                {
                    Amount = amount,
                    Term = term,
                    Date = date
                },
                Repayments = GenerateRepayments(amount, term, date),
                CustomerName = customerName,
                CustomerEmail = email,
                Password = password,
                CustomerPhone = phoneNo
            };
            _context.Add(loan);
            _context.SaveChanges();
            return loan;
        }


        public void ApproveLoan(int loanId)
        {
            var loan = _context.Loans.FirstOrDefault(l => l.Id == loanId);
            if (loan != null)
            {
                loan.State = "APPROVED";
                _context.SaveChanges(); // Save changes to the database
            }
        }




        public IEnumerable<CustomerLoanDetails> GetLoansByCustomer(string email, string password)
        {
            try
            {
                var loans = _context.Loans
                                    .Where(l => l.CustomerEmail == email && l.Password == password)
                                    .Select(l => new CustomerLoanDetails
                                    {
                                        Id = l.Id,
                                        Amount = l.Request.Amount, 
                                        State = l.State,
                                        CustomerName = l.CustomerName
                                    })
                                    .ToList();

                return loans;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while fetching loans: {e.Message}");
                throw;
            }
        }


        public void AddRepayment(int loanId, decimal amount)
        {
            var loan = _context.Loans.Include(l => l.Repayments)
                                     .FirstOrDefault(l => l.Id == loanId);

            if (loan == null)
            {
                throw new ArgumentException($"Loan with ID {loanId} not found.");
            }

            if (loan.State != "APPROVED")
            {
                throw new InvalidOperationException($"Loan with ID {loanId} is not approved.");
            }

            var scheduledRepayment = loan.Repayments.FirstOrDefault(r => r.State == "PENDING" && r.Amount <= amount);

            if (scheduledRepayment == null)
            {
                throw new InvalidOperationException($"No pending repayment found for loan with ID {loanId} for the specified amount.");
            }
            if (scheduledRepayment.Amount != amount)
            {
                throw new InvalidOperationException($"The specified amount does not match the pending repayment amount for loan with ID {loanId}.");
            }

            scheduledRepayment.State = "PAID";

            if (loan.Repayments.All(r => r.State == "PAID"))
            {
                loan.State = "PAID";
            }

            _context.SaveChanges();
        }

        

        private List<ScheduledRepayment> GenerateRepayments(decimal amount, int term, DateTime startDate)
        {
            var repayments = new List<ScheduledRepayment>();
            decimal repaymentAmount = amount / term;

            for (int i = 0; i < term - 1; i++) // Loop to generate repayments except the last one
            {
                repayments.Add(new ScheduledRepayment
                {
                    DueDate = startDate.AddYears(1).AddDays(i * 7),
                    Amount = Math.Round(repaymentAmount, 2)
                });
            }

            // Calculate the last repayment amount by subtracting the sum of previous repayments from the total amount
            decimal totalPaid = repayments.Sum(r => r.Amount);
            decimal lastRepaymentAmount = amount - totalPaid;

            repayments.Add(new ScheduledRepayment
            {
                DueDate = startDate.AddYears(1).AddDays((term - 1) * 7),
                Amount = Math.Round(lastRepaymentAmount, 2)
            });

            // Save repayments to the database using the context
            foreach (var repayment in repayments)
            {
                _context.ScheduledRepayments.Add(repayment);
            }

            _context.SaveChanges();

            return repayments;
        }



        public ApproveLoanRequestModel GetLoanDetails(int loanId)
        {
            var loan = _context.Loans.Include(l => l.Request)
                                     .FirstOrDefault(l => l.Id == loanId);

            if (loan == null) throw new KeyNotFoundException($"Loan with ID {loanId} not found.");

            return new ApproveLoanRequestModel
            {
                CustomerName = loan.CustomerName,
                Amount = loan.Request.Amount,
                State=loan.State
            };
        }



        public bool ValidateUser(string Email, string Password)
        {
            
            return _context.Loans.Any(u => u.CustomerEmail == Email && u.Password == Password);
        }



        public IEnumerable<Loan> GetAll()
        {
            return _context.Loans;
        }
    }
}
