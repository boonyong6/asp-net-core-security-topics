using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using AspNetCoreIdentitySandbox.CustomStorageProviders.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CreateUser([FromQuery] string userName)
        {
            string userId = Guid.NewGuid().ToString();
            ApplicationUser user = new(userId, userName);

            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Errors.Any())
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }

            return View();
        }
    }
}
