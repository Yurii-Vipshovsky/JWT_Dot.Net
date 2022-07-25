using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using JWTToken;
using JWTToken.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JWTUnitTest
{
    [TestClass]
    public class UnitTestJWT
    {
        private readonly HttpClient _client;
        public UnitTestJWT()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [TestMethod]
        public void TestStartPage()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<JWTTokenController>();

            var controller = new JWTTokenController(logger);

            var actionResult = controller.Test() as OkObjectResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.Value.ToString(), "Hello");
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }
        [TestMethod]
        public void TestGetToken()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<JWTTokenController>();

            var controller = new JWTTokenController(logger);

            IActionResult actionResult = controller.Token("test1@gmail.com", "12345");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        }
        [TestMethod]
        public void TestGetTokenInvalidPassword ()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<JWTTokenController>();

            var controller = new JWTTokenController(logger);

            IActionResult actionResult = controller.Token("test1@gmail.com", "12341");
            var resValue = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(resValue.Value.ToString(), "Invalid username or password.");
        }
        [TestMethod]
        public void TestGetTokenInvalidUser()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<JWTTokenController>();

            var controller = new JWTTokenController(logger);

            var actionResult = controller.Token("asffewgf", "12345");
            var resValue = actionResult as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
            Assert.AreEqual(resValue.Value.ToString(), "Invalid username or password.");
        }
    }
}
