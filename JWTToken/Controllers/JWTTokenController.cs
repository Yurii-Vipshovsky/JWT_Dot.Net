using JWTToken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWTToken.Controllers
{
    /// <summary>
    /// Controller for creating Tokens
    /// </summary>
    [Route("api/[controller]")]
    public class JWTTokenController : ControllerBase
    {
        /// <summary>
        /// List of users insted of DB
        /// </summary>
        private List<User> users = new List<User>
        {
            new User {Login="test1@gmail.com", Password="12345"},
            new User {Login="test2@gmail.com", Password="12345"}
        };

        private readonly ILogger<JWTTokenController> _logger;
        public JWTTokenController(ILogger<JWTTokenController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Start Page
        /// </summary>
        /// <returns>String "Hello"</returns>
        [HttpGet]
        public IActionResult Test()
        {
            _logger.LogInformation("Opened Hello Page");
            return Ok("Hello");
        }

        /// <summary>
        /// Creates Token for user
        /// </summary>
        /// <param name="username">User Login</param>
        /// <param name="password">User Password</param>
        /// <returns>String access_token</returns>
        [HttpPost("/api/auth")]
        public IActionResult Token(string username, string password)
        {
            _logger.LogInformation("Generating Token for User " + username);
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                _logger.LogInformation("Can't Find User" + username);
                return BadRequest("Invalid username or password.");
            }
            //now time to create expire
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: JWTTokenOptions.Issuer,
                    audience: JWTTokenOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(1)),//lifetime 1 min
                    signingCredentials: new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTTokenOptions.Key)),
                                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };
            _logger.LogInformation("Generated Token for " + username + " "+ DateTime.UtcNow);
            return Ok(response);
        }

        /// <summary>
        /// Search for user in DB
        /// </summary>
        /// <param name="username">User Login</param>
        /// <param name="password">User Password</param>
        /// <returns>User identity or null if user doesn't exist</returns>
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }
            return null;
        }

    }
}
