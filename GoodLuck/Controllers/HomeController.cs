using System.Diagnostics;
using GoodLuck.Models;
using GoodLuck.Repositories;
using System.Text.Json;
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
        private const string DataFile = "photos.json";

        public HomeController(ILogger<HomeController> logger, DBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }

        private List<Photo> LoadPhotos()
        {
            var dir = Path.Combine(_env.WebRootPath, UploadFolder);
            var file = Path.Combine(dir, DataFile);
            if (!System.IO.File.Exists(file)) return new List<Photo>();
            try
            {
                var json = System.IO.File.ReadAllText(file);
                return JsonSerializer.Deserialize<List<Photo>>(json) ?? new List<Photo>();
            }
            catch
            {
                return new List<Photo>();
            }
        }

        private void SavePhotos(List<Photo> photos)
        {
            var dir = Path.Combine(_env.WebRootPath, UploadFolder);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, DataFile);
            var json = JsonSerializer.Serialize(photos);
            System.IO.File.WriteAllText(file, json);
        }

        public IActionResult Index()
        {
            // Find the nearest upcoming anniversary (including today)
            var upcoming = _context.Anniversaries
                                   .Where(a => a.Date >= DateTime.Today)
                                   .OrderBy(a => a.Date)
                                   .FirstOrDefault();

            if (upcoming != null)
            {
                var letter = _context.Letters
                                   .Include(l => l.Anniversary)
                                   .FirstOrDefault(l => l.AnniversaryId == upcoming.Id);
                ViewBag.NextLetter = letter;
            }

            ViewBag.NextAnniversary = upcoming;
            ViewBag.Photos = LoadPhotos().TakeLast(2).ToList();
            return View();
        }

        public IActionResult Edit()
        {
            ViewBag.Photos = LoadPhotos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhotos(List<IFormFile> files, List<string> titles)
        {
            var photos = LoadPhotos();
            var dir = Path.Combine(_env.WebRootPath, UploadFolder);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

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

            SavePhotos(photos);
            return RedirectToAction(nameof(Edit));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Calendar(int? month, int? year)
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

            var events = _context.Anniversaries
                                  .Where(a => a.Date.Year == displayDate.Year && a.Date.Month == displayDate.Month)
                                  .OrderBy(a => a.Date)
                                  .ToList();

            ViewBag.DisplayDate = displayDate;
            return View(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
