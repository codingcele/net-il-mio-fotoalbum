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

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category categoryToEdit = _context.Categories.Where(cat => cat.Id == id).FirstOrDefault();

            if (categoryToEdit == null)
            {
                return NotFound();
            }
            else
            {
                CategoryFormModel model = new CategoryFormModel();

                //model.Id = categoryToEdit.Id;
                model.Name = categoryToEdit.Name;
                model.Description = categoryToEdit.Description;

                return View(model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, CategoryFormModel data)
        {
            Category categoryToEdit = _context.Categories.Where(img => img.Id == id).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return View("Update", data);
            }

            if (categoryToEdit != null)
            {
                categoryToEdit.Name = data.Name;
                categoryToEdit.Description = data.Description;

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
