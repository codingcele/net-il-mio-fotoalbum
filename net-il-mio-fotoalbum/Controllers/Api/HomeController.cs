using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Signing;
using System.Net;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Authorize]
    [Route("/Home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AlbumContext _context;

        public HomeController(AlbumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetImages(string? str)
        {
            List<Image> images;
            images = _context.Images.Include(p => p.Categories).ToList<Image>();

            if (str != null)
            {
                images = images.Where(img => img.Title.ToLower().Contains(str.ToLower())).ToList();
            }

            return Ok(images);
        }

        [HttpPost]
        public IActionResult SaveMessage([FromBody] MessageModel model)
        {
            if (ModelState.IsValid)
            {
                var message = new Message
                {
                    Email = model.Email,
                    MessageText = model.Message
                };

                _context.Messages.Add(message);
                _context.SaveChanges();

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
