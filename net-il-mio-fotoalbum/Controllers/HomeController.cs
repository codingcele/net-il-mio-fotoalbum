using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.ComponentModel.Design;

namespace net_il_mio_fotoalbum
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AlbumContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, AlbumContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            List<Image> images;
            images = _context.Images.Include(i => i.Categories).ToList();

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

        [Authorize(Roles = "ADMIN")]
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


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = _context.Categories.ToList();

            ImageFormModel model = new ImageFormModel();

            model.Image = new Image();
            model.Categories = categories;

            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _context.Categories.ToList();

                data.Categories = categories;

                return View("Create", data);
            }

            Image imageToCreate = new Image();
            imageToCreate.Title = data.Image.Title;

            if (data.ImageFile != null)
            {
                var fileName = Guid.NewGuid().ToString(); // Genera un nuovo nome univoco utilizzando un Guid
                var fileExtension = Path.GetExtension(data.ImageFile.FileName); // Ottieni l'estensione del file originale
                var uniqueFileName = fileName + fileExtension; // Unisci il nome univoco e l'estensione del file

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    data.ImageFile.CopyTo(fileStream);
                }

                imageToCreate.Picture = uniqueFileName;
            }


            imageToCreate.Visible = data.Image.Visible;
            imageToCreate.Description = data.Image.Description;


            imageToCreate.Categories = new List<Category>();


            if (data.SelectedCategories != null)
            {
                foreach (int selectedCategoryId in data.SelectedCategories)
                {
                    Category category = _context.Categories.Where(m => m.Id == selectedCategoryId).FirstOrDefault();
                    imageToCreate.Categories.Add(category);
                }
            }

            _context.Images.Add(imageToCreate);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Image imageToEdit = _context.Images.Where(img => img.Id == id).Include(i => i.Categories).FirstOrDefault();
            
            if (imageToEdit == null)
            {
                return NotFound();
            }
            else
            {
                ImageFormUpdateModel model = new ImageFormUpdateModel();

                model.Image = imageToEdit;
                List<Category> categories = _context.Categories.ToList();

                List<int> categoryIds = imageToEdit.Categories.Select(c => c.Id).ToList();
                model.SelectedCategories = categoryIds;

                model.Categories = categories;
                model.SelectedCategories = categoryIds;

                string currentDirectory = Directory.GetCurrentDirectory();
                string wwwrootPath = Path.Combine(currentDirectory, "wwwroot");
                string imagePath = Path.Combine(wwwrootPath, "img", imageToEdit.Picture);
                var fileInfo = new FileInfo(imagePath);

                using (var fileStream = new FileStream(imagePath, FileMode.Open))
                {
                    var formFile = new FormFile(fileStream, 0, fileInfo.Length, null, fileInfo.Name);
                    model.ImageFile = formFile;
                }

                return View(model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, ImageFormUpdateModel data)
        {
            Image imageToEdit = _context.Images.Where(img => img.Id == id).Include(i => i.Categories).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                List<Category> categories = _context.Categories.ToList();

                data.Categories = categories;
                data.Image = imageToEdit;

                return View("Update", data);
            }
            

            if (imageToEdit != null)
            {
                imageToEdit.Title = data.Image.Title;

                if (data.ImageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString(); // Genera un nuovo nome univoco utilizzando un Guid
                    var fileExtension = Path.GetExtension(data.ImageFile.FileName); // Ottieni l'estensione del file originale
                    var uniqueFileName = fileName + fileExtension; // Unisci il nome univoco e l'estensione del file

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                    var filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        data.ImageFile.CopyTo(fileStream);
                    }

                    // Rimuovi il file precedente se esisteva
                    var previousFilePath = Path.Combine(folderPath, imageToEdit.Picture);
                    if (System.IO.File.Exists(previousFilePath))
                    {
                        System.IO.File.Delete(previousFilePath);
                    }

                    imageToEdit.Picture = uniqueFileName;
                }



                imageToEdit.Description = data.Image.Description;
                imageToEdit.Visible = data.Image.Visible;

                imageToEdit.Categories = new List<Category>();

                imageToEdit.Categories.Clear();

                if (data != null && data.SelectedCategories != null)
                {
                    foreach (int selectedCategoryId in data.SelectedCategories)
                    {
                        Category category = _context.Categories.Where(c => c.Id == selectedCategoryId).FirstOrDefault();
                        imageToEdit.Categories.Add(category);
                    }
                }

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