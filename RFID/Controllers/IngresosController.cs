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

        // GET: /Ingresos
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

        // POST: /Ingresos
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

        // DELETE: /Ingresos/1
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

	// PUT: /Ingresos
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
            
            var fechaIngreso = new DateTime();
            var newIngreso = new Ingresos();

            if (ingreso.EmpleadoId == 0 || ingreso.day == 32 || ingreso.hours == 25)
            {
                var ingresoAnterior = await context.Ingresos.Where(w => w.RegistroId == ingreso.RegistroId).AsNoTracking().FirstOrDefaultAsync();
                if (ingreso.EmpleadoId == 0) { ingreso.EmpleadoId = ingresoAnterior.EmpleadoId; }
                if ((ingreso.day == 32) && (ingreso.hours == 25))
                {
                    fechaIngreso = ingresoAnterior.Fecha;
                }
                else
                {
                    if (ingreso.day == 32)
                    {
                        fechaIngreso = new DateTime(ingresoAnterior.Fecha.Year, ingresoAnterior.Fecha.Month, ingresoAnterior.Fecha.Day, ingreso.hours, ingreso.minutes, ingreso.seconds);
                    }
                    else
                    {
                        if (ingreso.hours == 25)
                        {
                            fechaIngreso = new DateTime(ingreso.year, ingreso.month, ingreso.day, ingresoAnterior.Fecha.Hour, ingresoAnterior.Fecha.Minute, ingresoAnterior.Fecha.Second);
                        }
                    }
                }
                
                newIngreso = new Ingresos(ingreso.RegistroId, ingreso.EmpleadoId, fechaIngreso);
            }
            else
            {
               try
                {
                    fechaIngreso = new DateTime(ingreso.year, ingreso.month, ingreso.day, ingreso.hours, ingreso.minutes, ingreso.seconds);
                }
                catch (Exception)
                {
                    return BadRequest(ErrorHelper.Response(400, "La fecha u hora de ingreso no es valida"));
                }

                newIngreso = new Ingresos(ingreso.RegistroId, ingreso.EmpleadoId, fechaIngreso);
                
            }

            context.Entry(newIngreso).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
