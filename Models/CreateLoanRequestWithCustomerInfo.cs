namespace MiniLoanApplication.Models
{
    public class CreateLoanRequestWithCustomerInfo
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phoneno { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public DateTime Date { get; set; }
    }
}
