using GoodLuck.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodLuck.Controllers
{
    public class AnniversariesController : Controller
    {
        private readonly DbContext _context;

        public AnniversariesController(DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var anniversaries = await _context.Anniversaries.OrderBy(a => a.Date).ToListAsync();
            return View(anniversaries);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Anniversary anniversary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anniversary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anniversary);
        }
    }
}
