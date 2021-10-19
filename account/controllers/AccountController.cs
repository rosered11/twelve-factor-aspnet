using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(
            ILogger<AccountController> logger
            , IAccountRepository accountRepo
            , IConfiguration config
            , IHttpClientFactory httpClentFactory)
        {
            _logger = logger;
            _accountRepo = accountRepo;
            _config = config;
            _httpClientFactory = httpClentFactory;
        }

        [HttpPost, Route("myAccount")]
        public async Task<IActionResult> A([FromBody] Customer customer)
        {
            var account = _accountRepo.FindByCustomerId(customer);
            account.ConfigValue = _config["accounts:msg"];
            var client = _httpClientFactory.CreateClient("card");
            var response = await client.PostAsync("http://cardapi/card/myCard", new StringContent("{}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return Ok(new { account = account, result });
            
        }
    }
}