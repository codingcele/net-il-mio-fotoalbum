using Microsoft.Build.Framework;

namespace net_il_mio_fotoalbum.Models
{
    public class MessageModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }

}
