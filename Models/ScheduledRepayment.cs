using System.ComponentModel.DataAnnotations;

namespace MiniLoanApplication.Models
{
    public class ScheduledRepayment
    {
        [Key]
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; } = "PENDING";
        
    }
}
