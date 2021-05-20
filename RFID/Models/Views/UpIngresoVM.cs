using System;
using System.ComponentModel.DataAnnotations;

namespace RFID.Models.Views
{
    public class UpIngresoVM
    {
        [Key]
        public int RegistroId { get; set; }
        [Required(ErrorMessage ="El id del empledo es obligatorio")]
        public int EmpleadoId { get; set; }
        [Required(ErrorMessage = "El dia de ingreso es obligatorio")]
        [Range(1,31, ErrorMessage = "El dia debe ser mayor o igual a 1 ")]
        public int day { get; set; }
        [Required(ErrorMessage = "El mes de ingreso es obligatorio")]
        [Range(1, 12, ErrorMessage = "El mes debe ser mayor o igual a 1  y menor a 12")]
        public int month { get; set; }
        [Required(ErrorMessage = "El año de ingreso es obligatorio")]
        [Range(2000,2030, ErrorMessage = "El año debe ser mayor o igual a 1")]
        public int year { get; set; }
        [Range(0, 24, ErrorMessage = "La hora de ingreso debe ser mayor o igual a 0 y menor a 24")]
        public int hours { get; set; }
        [Range(0, 60, ErrorMessage = "Los minutos de ingreso debe ser mayor o igual a 0 y menor a 60")]
        public int minutes { get; set; }
        [Range(0, 60, ErrorMessage = "Los segundos de ingreso debe ser mayor o igual a 0 y menor a 60")]
        public int seconds { get; set; }
    }
}