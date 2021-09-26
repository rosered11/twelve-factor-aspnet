using account.dataaccess.repositories;
using account.dto.requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace account.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepo;
        public AccountController(
            ILogger<AccountController> logger
            , IAccountRepository accountRepo)
        {
            _logger = logger;
            _accountRepo = accountRepo;
        }

        [HttpPost, Route("myAccount")]
        public IActionResult A([FromBody] Customer customer)
        {
            var account = _accountRepo.FindByCustomerId(customer);
            return Ok(account);
        }
    }
}