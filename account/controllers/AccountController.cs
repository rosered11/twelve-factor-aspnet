using account.dataaccess.repositories;
using account.dto.requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace account.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepo;
        private readonly IConfiguration _config;
        public AccountController(
            ILogger<AccountController> logger
            , IAccountRepository accountRepo
            , IConfiguration config)
        {
            _logger = logger;
            _accountRepo = accountRepo;
            _config = config;
        }

        [HttpPost, Route("myAccount")]
        public IActionResult A([FromBody] Customer customer)
        {
            var account = _accountRepo.FindByCustomerId(customer);
            account.ConfigValue = _config["accounts:msg"]; //_config["demo:config"];
            return Ok(account);
        }
    }
}