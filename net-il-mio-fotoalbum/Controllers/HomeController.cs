﻿using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace net_il_mio_fotoalbum
{
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

        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = _context.Categories.ToList();

            ImageFormModel model = new ImageFormModel();

            model.Image = new Image();
            model.Categories = categories;

            return View("Create", model);
        }

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
                var fileName = Path.GetFileName(data.ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                var filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    data.ImageFile.CopyTo(fileStream);
                }

                imageToCreate.Picture = fileName;
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

    }
}