using loan.dataaccess.repositories;
using loan.dto.requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace loan.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanRepository _loanRepo;
        public LoanController(
            ILogger<LoanController> logger
            , ILoanRepository loanRepo)
        {
            _logger = logger;
            _loanRepo = loanRepo;
        }

        [HttpPost, Route("myLoan")]
        public IActionResult A([FromBody] Customer customer)
        {
            var loans = _loanRepo.FindByCustomerId(customer);
            return Ok(loans);
        }
    }
}