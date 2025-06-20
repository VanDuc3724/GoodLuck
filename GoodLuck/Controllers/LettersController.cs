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


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var letter = await _context.Letters.FindAsync(id);
            if (letter == null)
            {
                return NotFound();
            }

            ViewBag.Anniversaries = new SelectList(_context.Anniversaries.OrderBy(a => a.Date), "Id", "Title", letter.AnniversaryId);
            return View(letter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Letter letter)
        {
            if (id != letter.Id)
            {
                return NotFound();
            }

            if (!_context.Anniversaries.Any(a => a.Id == letter.AnniversaryId))
            {
                ModelState.AddModelError("AnniversaryId", "Anniversary does not exist.");
            }

            if (ModelState.IsValid)
            {
                var existing = await _context.Letters.FindAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }
                existing.Title = letter.Title;
                existing.Content = letter.Content;
                existing.AnniversaryId = letter.AnniversaryId;

                _context.Update(existing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Anniversaries = new SelectList(_context.Anniversaries.OrderBy(a => a.Date), "Id", "Title", letter.AnniversaryId);
            return View(letter);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var letter = await _context.Letters
                .Include(l => l.Anniversary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (letter == null)
            {
                return NotFound();
            }

            return View(letter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var letter = await _context.Letters.FindAsync(id);
            if (letter != null)
            {
                _context.Letters.Remove(letter);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
