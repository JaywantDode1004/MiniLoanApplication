using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniLoanApplication.Models;
using MiniLoanApplication.Service;
// my loan controller
namespace MiniLoanApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly IloanService _loanService;

        public LoanController(IloanService loanService)
        {
            _loanService = loanService;
        }
        [HttpPost("create")]
        public IActionResult CreateLoan([FromBody] CreateLoanRequestWithCustomerInfo requestModel)
        {
            if (requestModel == null)
            {
                return BadRequest("Invalid loan request.");
            }

            var loan = _loanService.CreateLoan(
                requestModel.CustomerName,
                requestModel.Email,
                requestModel.Password,
                requestModel.Phoneno,
                requestModel.Amount,
                requestModel.Term,
                requestModel.Date
            );
            return Ok(loan);
        }


        [HttpPost("approve/{loanId}")]
        public IActionResult ApproveLoan(int loanId)
        {
            try
            {
                _loanService.ApproveLoan(loanId);
                return Ok("Approved");
            }
            catch (Exception)
            {
                return BadRequest("Not found");
                throw;
            }

        }



        [HttpGet("customer")]
        public IActionResult GetLoansByCustomer(string Email, string Password)
        {
            var loans = _loanService.GetLoansByCustomer(Email, Password);

            if (loans == null || !loans.Any())
            {
                return NotFound("Customer not found or no loans available.");
            }

            return Ok(loans);
        }


        

        [HttpPost("repayment")]
        public IActionResult AddRepayment(int loanId, decimal amount)
        {
            try
            {
                _loanService.AddRepayment(loanId, amount);
                return Ok("Repayment Successful");
            }
            catch (ArgumentException ex)
            {
                // This can be caught if a specific argument-related exception is thrown, like "Loan not found" or "Invalid amount"
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Catch if the loan state is not approved or no pending repayment found
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        


        [HttpGet("details/{loanId}")]
        public IActionResult GetLoanDetails(int loanId)
        {
            try
            {
                var loanDetails = _loanService.GetLoanDetails(loanId);
                return Ok(loanDetails);
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(knfe.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet]
        public ActionResult<IEnumerable<Loan>> GetAll()
        {
            var loans = _loanService.GetAll();
            return Ok(loans);
        }
    }
}
