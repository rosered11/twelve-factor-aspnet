using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace config_server.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IConfiguration _config;
        public ConfigController(ILogger<ConfigController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config; 
        }

        [HttpGet, Route("myConfig")]
        public IActionResult A()
        {
            return Ok(new { config = _config["demo:config"] });
        }
    }
}