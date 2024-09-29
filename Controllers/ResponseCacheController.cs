using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CacheTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseCacheController : ControllerBase
    {
        private readonly ILogger<InMemoryCacheController> _logger;

        public ResponseCacheController(ILogger<InMemoryCacheController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ResponseCache")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]

        public string ResponseCache()
        {
                return "Data from response cache:";
        }
    }
}
