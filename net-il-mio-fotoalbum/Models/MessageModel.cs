using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class MessageModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Il campo Email non è un indirizzo email valido.")]
        public string Email { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Il campo Messaggio è obbligatorio.")]
        public string Message { get; set; }
    }

}
