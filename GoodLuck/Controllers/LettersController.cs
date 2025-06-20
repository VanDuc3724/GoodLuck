using GoodLuck.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodLuck.Controllers
{
    public class LettersController : Controller
    {
        private readonly DbContext _context;

        public LettersController(DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var letters = await _context.Letters.OrderByDescending(l => l.Created).ToListAsync();
            return View(letters);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Letter letter)
        {
            if (ModelState.IsValid)
            {
                letter.Created = DateTime.UtcNow;
                _context.Add(letter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(letter);
        }
    }
}
