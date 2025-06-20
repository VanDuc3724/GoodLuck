using System.Diagnostics;
using GoodLuck.Models;
using GoodLuck.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodLuck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context;
        private readonly IWebHostEnvironment _env;
        private const string UploadFolder = "uploads";

        public HomeController(ILogger<HomeController> logger, DBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }

        private async Task<List<Photo>> LoadPhotos()
        {
            return await _context.Photos
                .OrderBy(p => p.Uploaded)
                .ToListAsync();
        }

        private async Task SavePhotos(List<Photo> photos)
        {
            _context.Photos.RemoveRange(_context.Photos);
            await _context.SaveChangesAsync();
            _context.Photos.AddRange(photos);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> Index()
        {
            // Find the nearest upcoming anniversary (including today)
            var upcoming = await _context.Anniversaries
                                   .Where(a => a.Date >= DateTime.Today)
                                   .OrderBy(a => a.Date)
                                   .FirstOrDefaultAsync();


            if (upcoming != null && !string.IsNullOrWhiteSpace(upcoming.LetterContent))
            {
                ViewBag.EventLetter = upcoming.LetterContent;
            }

            ViewBag.NextAnniversary = upcoming;
            var allPhotos = await LoadPhotos();
            ViewBag.MainPhotos = allPhotos.TakeLast(2).ToList();
            ViewBag.Photos = allPhotos;

            var first = await _context.Anniversaries.OrderBy(a => a.Date).FirstOrDefaultAsync();
            int daysTogether = 0;
            if (first != null)
            {
                daysTogether = (int)(DateTime.Today - first.Date.Date).TotalDays;
                if (daysTogether < 0) daysTogether = 0;
            }
            ViewBag.DaysTogether = daysTogether;
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            ViewBag.Photos = await LoadPhotos();
            var first = await _context.Anniversaries.OrderBy(a => a.Date).FirstOrDefaultAsync();
            ViewBag.StartDate = first?.Date.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSettings(List<IFormFile> files, List<string> titles, DateTime startDate)
        {
            var existing = await LoadPhotos();
            var dir = Path.Combine(_env.WebRootPath, UploadFolder);

            if (Directory.Exists(dir))
            {
                foreach (var p in existing)
                {
                    var path = Path.Combine(dir, p.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var photos = new List<Photo>();
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var title = titles.Count > i ? titles[i] : Path.GetFileNameWithoutExtension(file.FileName);
                    photos.Add(new Photo { FileName = fileName, Title = title, Uploaded = DateTime.UtcNow });
                }
            }

            await SavePhotos(photos);

            var first = await _context.Anniversaries.OrderBy(a => a.Date).FirstOrDefaultAsync();
            if (first != null)
            {
                first.Date = startDate.Date;
                _context.Update(first);
            }
            else
            {
                _context.Anniversaries.Add(new Anniversary { Title = "Ngày Bắt Đầu", Date = startDate.Date });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Calendar(int? month, int? year)
        {
            DateTime displayDate;
            if (month.HasValue && year.HasValue)
            {
                displayDate = new DateTime(year.Value, month.Value, 1);
            }
            else if (month.HasValue)
            {
                displayDate = new DateTime(DateTime.Today.Year, month.Value, 1);
            }
            else if (year.HasValue)
            {
                displayDate = new DateTime(year.Value, DateTime.Today.Month, 1);
            }
            else
            {
                displayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            var events = await _context.Anniversaries
                                  .Where(a => a.Date.Year == displayDate.Year && a.Date.Month == displayDate.Month)
                                  .OrderBy(a => a.Date)
                                  .ToListAsync();

            ViewBag.DisplayDate = displayDate;
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> AddHomePhoto(string fileName)
        {
            var photo = new Photo
            {
                FileName = fileName,
                Title = "trang chủ",
                Uploaded = DateTime.UtcNow
            };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
