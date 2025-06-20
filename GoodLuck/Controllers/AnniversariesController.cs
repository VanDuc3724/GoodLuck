using GoodLuck.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodLuck.Repositories;

namespace GoodLuck.Controllers
{
    public class AnniversariesController : Controller
    {
        private readonly DBContext _context;

        public AnniversariesController(DBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var anniversaries = await _context.Anniversaries.OrderBy(a => a.Date).ToListAsync();
            return View(anniversaries);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anniversary = await _context.Anniversaries.FindAsync(id);
            if (anniversary == null)
            {
                return NotFound();
            }
            return View(anniversary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Anniversary anniversary)
        {
            if (id != anniversary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(anniversary.LetterTitle) && !string.IsNullOrWhiteSpace(anniversary.LetterContent) && !anniversary.LetterCreated.HasValue)
                {
                    anniversary.LetterCreated = DateTime.UtcNow;
                }
                _context.Update(anniversary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anniversary);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anniversary = await _context.Anniversaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anniversary == null)
            {
                return NotFound();
            }

            return View(anniversary);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anniversary = await _context.Anniversaries.FindAsync(id);
            if (anniversary != null)
            {
                _context.Anniversaries.Remove(anniversary);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            // Pass a new instance so default values like the Icon emoji
            // are populated in the form. Without this the input fields
            // would be empty and validation for the Icon property would fail
            // even though a default value is specified in the model.
            return View(new Anniversary());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Anniversary anniversary)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(anniversary.LetterTitle) && !string.IsNullOrWhiteSpace(anniversary.LetterContent))
                {
                    anniversary.LetterCreated = DateTime.UtcNow;
                }
                _context.Add(anniversary);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(anniversary);
        }
    }
}
