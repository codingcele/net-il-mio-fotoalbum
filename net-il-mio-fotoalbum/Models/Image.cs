using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace net_il_mio_fotoalbum
{
    [Table("Images")]
    public class Image
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required]
        [DisplayName("Upload immagine")]
        public string Picture { get; set; }
        public bool Visible { get; set; }
        public List<Category>? Categories { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
