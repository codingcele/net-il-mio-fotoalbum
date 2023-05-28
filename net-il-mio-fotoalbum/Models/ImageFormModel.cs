using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace net_il_mio_fotoalbum
{
    public class ImageFormModel
    {
        public Image Image { get; set; }
        public List<Category>? Categories { get; set; }

        public List<int>? SelectedCategories { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}