using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RFID.Models.ViewModels
{
    public class UsuarioVM
    {
        [Key]
        public int usuarioId { get; set; }
        [Required(ErrorMessage = "El usuario no puede estar vacio")]
        public string userName { get; set; }
    }
}
