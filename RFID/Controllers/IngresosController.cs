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
    [Authorize] 	// Se establece que para hacer peticiones a este endpoint se necesite autorizacion
    [Route("api/[controller]")]	  // el url debera contener /api/ingresos para poder hacer peticiones a este controlador
    [ApiController]
    public class IngresosController : Controller
    {
        private readonly RFIDContext context;

        public IngresosController(RFIDContext _context)
        {
            context = _context;
        }

        // GET: api/ingresos
	// Regresa una lista con todos los ingresos registrados
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

        // POST: api/ingresos
	// De una instancia recibida de la peticion, esta se agregara a la base de datos
	// Regresa al emisor la instancia recibida si se ha ingresado correctamente
        [HttpPost]
        public async Task<IActionResult> RegistrarIngreso([Bind("EmpleadoId")] Ingresos ingreso)
        {
            if (!ModelState.IsValid)			// Se verfica que la instancia sea valida en relacion a las reglas establecidas para el modelo
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            ingreso.Fecha = DateTime.Now; 	// En nuestra solucion, cada que ingrese un empleado se anotara junto a fecha actual generada
            context.Ingresos.Add(ingreso);   	// se hace el query para agregar el ingreso
            await context.SaveChangesAsync();
            return Ok(); // se regresa un codigo 200 debido a que el ingreso fue agregado correctamente
        }

        // DELETE: api/ingresos/1
	// De un id tipo int enviado en la peticion, se eliminara de la base de datos a la instancia con el id correspondiente
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ingreso = await context.Ingresos.FindAsync(id); // se hace la busqueda de la instancia con el id recibido
            if (ingreso == null) // en caso de que no se encuentre
            {
                return NotFound(ErrorHelper.Response(404, "No existe ese id"));
            }

            context.Ingresos.Remove(ingreso); 	// se realiza la peticion de borrado de la instancia en la base de datos
            await context.SaveChangesAsync();
            return NoContent(); // se regresa el codigo 204 para mostrar la eliminacion satisfactoria
        }

	// PUT: api/ingresos
	// De la instancia que es recibida de la peticion, se modificara a la correspondiente guardada en la base de datos
	[HttpPut]
        public async Task<IActionResult> Put([Bind("RegistroId, EmpleadoId, day, month, year, hours, minutes, seconds")]UpIngresoVM ingreso)
        {
            if(!await context.Ingresos.Where(s => s.RegistroId == ingreso.RegistroId).AsNoTracking().AnyAsync())  // verifica que se encuentra el id del registro en la base de datos
            {
                return NotFound(ErrorHelper.Response(404, "el ingreso a modificar no fue encontrado"));
            }

            if(!await context.Empleado.Where(s => s.Id == ingreso.EmpleadoId).AsNoTracking().AnyAsync())   // Verifica que el empleado exista en la base de datos
            {
                return BadRequest(ErrorHelper.Response(400, "el empleado a modificar no fue encontrado"));
            }
            
            var fechaIngreso = new DateTime();
            var newIngreso = new Ingresos();

            if (ingreso.EmpleadoId == 0 || ingreso.day == 32 || ingreso.hours == 25) // Verifica que se haya recibido un empleado, una fecha y hora validas
            {
                var ingresoAnterior = await context.Ingresos.Where(w => w.RegistroId == ingreso.RegistroId).AsNoTracking().FirstOrDefaultAsync();  // se trae a la instancia correspondiente al id de registro de la nueva instancia
                if (ingreso.EmpleadoId == 0) { ingreso.EmpleadoId = ingresoAnterior.EmpleadoId; }  // En caso de que no se haya recibido un empleado, se deja al mismo empleado que ya estaba registrado con ese ingreso
                if ((ingreso.day == 32) && (ingreso.hours == 25))
                {
                    fechaIngreso = ingresoAnterior.Fecha; // si ninguno de los valores de dia o de hora son validos, se deja la misma fecha y hora de la instancia anterior
                }
                else
                {
                    if (ingreso.day == 32)
                    {
                        fechaIngreso = new DateTime(ingresoAnterior.Fecha.Year, ingresoAnterior.Fecha.Month, ingresoAnterior.Fecha.Day, ingreso.hours, ingreso.minutes, ingreso.seconds); // si el dia no es valido, se deja la fecha anterior pero se cambia la hora de ingreso segun lo datos recibidos
                    }
                    else
                    {
                        if (ingreso.hours == 25)
                        {
                            fechaIngreso = new DateTime(ingreso.year, ingreso.month, ingreso.day, ingresoAnterior.Fecha.Hour, ingresoAnterior.Fecha.Minute, ingresoAnterior.Fecha.Second);
			    // En caso de que la fecha recibida no sea valida, se deja la hora de ingreso anterior y solo se modifica la fecha
                        }
                    }
                }
                
                newIngreso = new Ingresos(ingreso.RegistroId, ingreso.EmpleadoId, fechaIngreso); // se genera el nuevo ingreso que corresponde a el model Ingresos que es compatible con la base de datos
            }
            else // en caso de que los valores condicionados sean validos se genera la fecha y hora  que sera guardada en la base de datos
            {
               try
                {
                    fechaIngreso = new DateTime(ingreso.year, ingreso.month, ingreso.day, ingreso.hours, ingreso.minutes, ingreso.seconds);
                }
                catch (Exception)
                {
                    return BadRequest(ErrorHelper.Response(400, "La fecha u hora de ingreso no es valida"));
                }

                newIngreso = new Ingresos(ingreso.RegistroId, ingreso.EmpleadoId, fechaIngreso); // se genera el nuevo ingreso que corresponde a el model Ingresos que es compatible con la base de datos
            }

            context.Entry(newIngreso).State = EntityState.Modified; // se establece la modificacion de la instancia con el id de registro recibido
            await context.SaveChangesAsync();
            return Ok(); // regresa un codigo 200 para confirmar su modificacion
        }

    }
}
