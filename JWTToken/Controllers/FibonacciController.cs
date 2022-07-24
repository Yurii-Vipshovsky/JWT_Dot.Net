using JWTToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(e.Message);
            }
            return Ok(res);
        }
    }
}
