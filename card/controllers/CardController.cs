using System;
using card.dataaccess.repositories;
using card.dto.requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace card.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardRepository _repo;
        private readonly IConfiguration _config;
        public CardController(
            ILogger<CardController> logger
            , ICardRepository repo
            , IConfiguration config)
        {
            _logger = logger;
            _repo = repo;
            _config = config;
        }

        [HttpPost, Route("myCard")]
        public IActionResult A([FromBody] Customer customer)
        {
            var loans = _repo.FindByCustomerId(customer);
            loans.ConfigServer = _config["env"];
            return Ok(loans);
        }
    }
}