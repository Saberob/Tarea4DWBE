using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            Ingresos = new HashSet<Ingresos>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre del empleado es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Apellido del empleado es obligatorio")]
        public string ApellidoP { get; set; }
        [Required(ErrorMessage = "El Apellido del empleado es obligatorio")]
        public string ApellidoM { get; set; }
        [Required(ErrorMessage = "El Rfid del empleado es obligatorio")]
        [MinLength(10, ErrorMessage ="El Rfid del empleado debe ser mayor a 10 caracteres")]
        [MaxLength(50, ErrorMessage = "El Rfid del empleado debe ser menor a 50 caracteres")]
        public string Rfid { get; set; }

        public virtual ICollection<Ingresos> Ingresos { get; set; }
    }
}
