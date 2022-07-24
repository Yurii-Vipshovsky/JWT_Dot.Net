using JWTToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : Controller
    {
        private readonly ILogger<FibonacciController> _logger;
        public FibonacciController(ILogger<FibonacciController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{length}")]
        public IActionResult Index(int length)
        {
            List<int> res;
            try
            {
                res = FibonacciCalculator.CalculateFibonacci(length);
            }
            catch(ArgumentException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            _logger.LogInformation("Generated "+length+" Fibonacci numbers");
            return Ok(res);
        }
    }
}
