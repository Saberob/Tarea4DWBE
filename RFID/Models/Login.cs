using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RFID.Models
{
    public class Login
    {
        [Required(ErrorMessage="El usuario el obligatorio")]
        public string userName { get; set; }
        [Required(ErrorMessage = "La contraseña el obligatorio")]
        public string password { get; set; }
    }
}
