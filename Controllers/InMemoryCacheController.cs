using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json.Serialization;

namespace CacheTutorial.Controllers
{
    [ApiController]
    [Route("InMemoryCache")]
    public class InMemoryCacheController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<InMemoryCacheController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public InMemoryCacheController(ILogger<InMemoryCacheController> logger, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            string cacheKey = "GetWeatherForecast";
          //  List<WeatherForecast> forecastList = new List<WeatherForecast>();
            if (!_memoryCache.TryGetValue(cacheKey, out List<WeatherForecast> cacheValue))
            {
                cacheValue = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _memoryCache.Set(cacheKey, cacheValue, cacheEntryOptions);
            }
            return cacheValue;
        }

        //[HttpGet(Name = "GetDistributedCacheData")]
        //public string GetDistributedCacheData()
        //{
        //    string cacheKey = "sampleKey";
        //    var cachedValue = _distributedCache.GetAsync(cacheKey);
        //    string returnValue = "";

        //    if (cachedValue == null)
        //    {
        //        var data = Encoding.UTF8.GetBytes("This is cached data.");
        //        var options = new DistributedCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Sets expiration after 5 minutes of inactivity

        //        // Set the cache

        //        _distributedCache.SetAsync(cacheKey, data, options);
        //        return "data without cache";
        //    }
        //    else
        //    {
        //        return "data from cache:"+cachedValue;
        //    }
        //}
    }
}
