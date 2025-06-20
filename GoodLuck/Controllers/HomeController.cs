using System.Diagnostics;
using GoodLuck.Models;
using GoodLuck.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoodLuck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context;

        public HomeController(ILogger<HomeController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Find the nearest upcoming anniversary (including today)
            var upcoming = _context.Anniversaries
                                   .Where(a => a.Date >= DateTime.Today)
                                   .OrderBy(a => a.Date)
                                   .FirstOrDefault();

            ViewBag.NextAnniversary = upcoming;
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
    }
}
