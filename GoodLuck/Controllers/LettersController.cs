using GoodLuck.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodLuck.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodLuck.Controllers
{
    public class LettersController : Controller
    {
        private readonly DBContext _context;

        public LettersController(DBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var letters = await _context.Letters
                .Include(l => l.Anniversary)
                .OrderByDescending(l => l.Created)
                .ToListAsync();
            return View(letters);
        }

        public IActionResult Create()
        {
            ViewBag.Anniversaries = new SelectList(_context.Anniversaries.OrderBy(a => a.Date), "Id", "Title");
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
            ViewBag.Anniversaries = new SelectList(_context.Anniversaries.OrderBy(a => a.Date), "Id", "Title", letter.AnniversaryId);
            return View(letter);
        }
    }
}
