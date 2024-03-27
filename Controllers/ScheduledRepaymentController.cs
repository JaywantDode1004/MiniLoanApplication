using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniLoanApplication.Models;
using MiniLoanApplication.Service;

namespace MiniLoanApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledRepaymentController : ControllerBase
    {
        private readonly IScheduledRepaymentService _scheduledRepaymentService;

        public ScheduledRepaymentController(IScheduledRepaymentService scheduledRepaymentService)
        {
            _scheduledRepaymentService = scheduledRepaymentService;
        }

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetScheduledRepaymentsByLoanId(int loanId)
        {
            try
            {
                var scheduledRepayments = await _scheduledRepaymentService.GetScheduledRepaymentsByLoanIdAsync(loanId);
                if (scheduledRepayments == null)
                {
                    return NotFound();
                }
                return Ok(scheduledRepayments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
