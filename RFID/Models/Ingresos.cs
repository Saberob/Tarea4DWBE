using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID.Models
{
    public partial class Ingresos
    {
        public Ingresos()
        {
        }
        public Ingresos(int _registroId, int _empleadoId, DateTime _fecha)
        {
            this.RegistroId = _registroId;
            this.EmpleadoId = _empleadoId;
            this.Fecha = _fecha;
        }

        [Key]
        public int RegistroId { get; set; }
        [Required(ErrorMessage="El Id del empleado es obligatorio")]
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Empleado Empleado { get; set; }
    }
}
