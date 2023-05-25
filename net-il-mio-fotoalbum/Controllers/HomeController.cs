using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace net_il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AlbumContext _context;

        public HomeController(ILogger<HomeController> logger, AlbumContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Image> images;
            images = _context.Images.Include(i => i.Categories).ToList<Image>();

            return View(images);
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

        public IActionResult Details(int id)
        {
            Image imageById = _context.Images.Where(i => i.Id == id).Include(img => img.Categories).FirstOrDefault();

            if (imageById == null)
            {
                return NotFound($"L'immagine con id {id} non è stata trovata.");
            }
            else
            {
                return View("Details", imageById);
            }
        }

        public IActionResult Delete(int id)
        {
            Image imageToDelete = _context.Images.Where(img => img.Id == id).FirstOrDefault();

            if (imageToDelete != null)
            {
                _context.Images.Remove(imageToDelete);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}