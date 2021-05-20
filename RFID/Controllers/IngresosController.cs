using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RFID.Helper;
using RFID.Models;
using RFID.Models.Views;

namespace RFID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : Controller
    {
        private readonly RFIDContext context;

        public IngresosController(RFIDContext _context)
        {
            context = _context;
        }

        // GET: Ingresos
        [HttpGet]
        public async Task<IEnumerable<IngresoWNameVM>> GetIngresos()
        {
            var ingresos = await context.Ingresos.Join(context.Empleado, ing => ing.EmpleadoId, emp => emp.Id, (ing, emp) => new IngresoWNameVM
            {
                RegistroId = ing.RegistroId,
                Nombre = emp.Nombre + " "+ emp.ApellidoP + " " + emp.ApellidoM,
                Fecha = ing.Fecha.Month.ToString() + "/" + ing.Fecha.Day.ToString() + "/" + ing.Fecha.Year.ToString(),
                Hora = ing.Fecha.ToShortTimeString()
            }).ToListAsync();

            return ingresos;
        }


        [HttpGet("byEmp/{id}")]
        public async Task<IActionResult> GetIngresobyEmpleado(int id)
        {
            var empleado = await context.Empleado.FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound(ErrorHelper.Response(404, "No existe ese id de empleado"));
            }

            var request = await context.Ingresos.Where(x => x.EmpleadoId == empleado.Id).ToListAsync();
            var ingresos = new List<IngresoVM>();

            foreach(Ingresos item in request)
            {
                ingresos.Add(new IngresoVM
                {
                    RegistroId = item.RegistroId,
                    Fecha = item.Fecha.Month.ToString() + "/" + item.Fecha.Day.ToString() + "/" + item.Fecha.Year.ToString(),
                    Hora = item.Fecha.ToShortTimeString()
                });
            }

            return Ok(new IngresoEmpleadoVM
            {
                EmpleadoId = id,
                Nombre = empleado.Nombre + " "+ empleado.ApellidoP + " " + empleado.ApellidoM,
                ingresos = ingresos
            });
        }

        // POST: Ingresos
        [HttpPost]
        public async Task<IActionResult> RegistrarIngreso([Bind("EmpleadoId")] Ingresos ingreso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            ingreso.Fecha = DateTime.Now;
            context.Ingresos.Add(ingreso);
            await context.SaveChangesAsync();
            return Created($"/ingresos/{ingreso.RegistroId}", ingreso);
        }

        // POST: Ingresos
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ingreso = await context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound(ErrorHelper.Response(404, "No existe ese id"));
            }

            context.Ingresos.Remove(ingreso);
            await context.SaveChangesAsync();
            return NoContent();
        }

	    [HttpPut]
        public async Task<IActionResult> Put([Bind("RegistroId, EmpleadoId, day, month, year, hours, minutes, seconds")]UpIngresoVM ingreso)
        {
            if(!await context.Ingresos.Where(s => s.RegistroId == ingreso.RegistroId).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "el ingreso a modificar no fue encontrado"));
            }
            if(!await context.Empleado.Where(s => s.Id == ingreso.EmpleadoId).AsNoTracking().AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "el empleado a modificar no fue encontrado"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var fechaIngreso = new DateTime();

            try
            {
                fechaIngreso = new DateTime(ingreso.year, ingreso.month, ingreso.day, ingreso.hours, ingreso.minutes, ingreso.seconds);
            }
            catch(Exception)
            {
                return BadRequest(ErrorHelper.Response(400, "La fecha u hora de ingreso no es valida"));
            }

            var newIngreso = new Ingresos(ingreso.RegistroId, ingreso.EmpleadoId, fechaIngreso);

            context.Entry(newIngreso).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
