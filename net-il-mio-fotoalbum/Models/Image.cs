using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
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
        public string Picture { get; set; }
        public int Visible { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
