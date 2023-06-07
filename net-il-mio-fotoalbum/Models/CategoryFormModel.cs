using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace net_il_mio_fotoalbum
{
    public class CategoryFormModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}