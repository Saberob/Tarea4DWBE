using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RFID.Models
{
    public class Usuarios
    {
        [Key]
        public int usuarioId { get; set; }
        [Required(ErrorMessage ="El usuario no puede estar vacio")]
        public string userName { get; set; }
        [Required(ErrorMessage ="La constraseña no puede estar vacia")]
        public string password { get; set; }
        [Compare("password",ErrorMessage ="Las constraseñas no coinciden")]
        [NotMapped]
        public string confirmPassword { get; set; }
        public string sal { get; set; }
    }
}
