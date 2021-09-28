using card.dataaccess.repositories;
using card.dto.requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace card.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardRepository _repo;
        public CardController(
            ILogger<CardController> logger
            , ICardRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost, Route("myCard")]
        public IActionResult A([FromBody] Customer customer)
        {
            var loans = _repo.FindByCustomerId(customer);
            return Ok(loans);
        }
    }
}