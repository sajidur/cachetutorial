using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace CacheTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributedCacheController : ControllerBase
    {

        private readonly ILogger<DistributedCacheController> _logger;
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheController(ILogger<DistributedCacheController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }


        [HttpGet(Name = "GetDistributedCacheData")]
        public string GetDistributedCacheData()
        {
            string cacheKey = "sampleKey";
            var cachedValue = _distributedCache.GetString(cacheKey);
            if (cachedValue == null)
            {
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Sets expiration after 5 minutes of inactivity

                // Set the cache

                _distributedCache.SetString(cacheKey, "This is cached data.", options);
                return "data without cache";
            }
            else
            {
                return "data from cache:" + cachedValue;
            }
        }
    }
}
