using JWTToken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace JWTToken.Controllers
{
    [Route("[controller]")]
    public class JWTTokenController : ControllerBase
    {
        private List<User> users = new List<User>
        {
            new User {Login="test1@gmail.com", Password="12345", Role="user" },
            new User { Login="test2@gmail.com", Password="12345", Role="user" }
        };

        private readonly ILogger<JWTTokenController> _logger;
        private readonly IConfiguration Configuration;
        public JWTTokenController(ILogger<JWTTokenController> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Hello");
        }

        //uncomment if use apiController

        //public class LoginUser
        //{
        //    public string username { get; set; }
        //    public string password { get; set; }
        //}

        [HttpPost("/auth")]
        //public IActionResult Token([FromForm]LoginUser user)
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            //now time to create expire
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: Configuration["JWT:Issuer"],
                    audience: Configuration["JWT:Audience"],
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(1)),//lifetime 1 min
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Key"])), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

    }
}
