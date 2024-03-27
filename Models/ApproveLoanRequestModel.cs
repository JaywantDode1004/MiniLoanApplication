namespace MiniLoanApplication.Models
{
    public class ApproveLoanRequestModel
    {
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; }
    }
}
