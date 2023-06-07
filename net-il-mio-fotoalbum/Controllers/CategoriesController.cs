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
    }
}
