using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.WebSockets;

namespace TestRedis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController(IDistributedCache cache) : ControllerBase
{
    private readonly string _key = "_TestKey";
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
    private readonly Random _random = new();

    [HttpGet("{number}")]
    public async Task<IActionResult> GetString(int number)
    {
        var key = number + _key;
        var cacheData = cache.GetString(key);

        if (cacheData != null) 
        {
            return Ok(cacheData);
        }

        var result = _random.Next(0, 100);

        cache.SetString(key, result.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _timeout
        });

        return Ok(result);
    }
}
