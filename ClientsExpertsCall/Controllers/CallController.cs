using ClientsExpertsCall.Business;
using ClientsExpertsCall.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientsExpertsCall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CallController> _logger;

        public CallController(IConfiguration configuration, ILogger<CallController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet(Name = "GetCallPrice")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var clientsUrl = _configuration.GetValue<string>("ClientsUrl");
                var expertsUrl = _configuration.GetValue<string>("ExpertsUrl");
                var ratesUrl = _configuration.GetValue<string>("RatesUrl");

                CallManager callManager = new CallManager();

                var expertCallPriceList = await callManager.CalculateCallPriceAsync(clientsUrl, expertsUrl, ratesUrl);

                return Ok(expertCallPriceList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
