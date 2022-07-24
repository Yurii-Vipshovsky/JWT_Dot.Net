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
    public class FibonachiController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index(int n)
        {
            return Ok(FibonacciCalculator.CalculateFibonacci(n));
        }
    }
}
