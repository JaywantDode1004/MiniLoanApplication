using System.ComponentModel.DataAnnotations;

namespace MiniLoanApplication.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        public LoanRequest Request { get; set; }
        public List<ScheduledRepayment> Repayments { get; set; }
        public string State { get; set; } = "PENDING";
        public string CustomerName { get; set; } 
        public string CustomerEmail { get; set; }

        public string Password { get; set; }
        public string CustomerPhone { get; set; }
    }
}

