using System.ComponentModel.DataAnnotations;

namespace MiniLoanApplication.Models
{
    public class LoanRequest
    {
        [Key]
        public int id { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public DateTime Date { get; set; }
    }
}
