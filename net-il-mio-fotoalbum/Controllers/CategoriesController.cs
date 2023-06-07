using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace net_il_mio_fotoalbum
{
    [Authorize(Roles = "ADMIN")]
    public class CategoriesController : Controller
    {
        private readonly AlbumContext _context;

        public CategoriesController(AlbumContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories;
            categories = _context.Categories.ToList();

            return View(categories);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            Category categoryToDelete = _context.Categories.Where(cat => cat.Id == id).FirstOrDefault();

            if (categoryToDelete != null)
            {
                _context.Categories.Remove(categoryToDelete);
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
