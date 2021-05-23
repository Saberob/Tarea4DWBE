using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RFID.Helper;
using RFID.Models;
using RFID.Models.Views;

namespace RFID.Controllers
{
    [Authorize] // Se establece que para hacer peticiones a este endpoint se necesite autorizacion
    [Route("api/[controller]")] // el url debera contener /api/empleado para poder hacer peticiones a este controlador
    [ApiController]    
    public class EmpleadoController : Controller
    {
        private readonly RFIDContext context;

        public EmpleadoController(RFIDContext _context)
        {
            context = _context;
        }

        // GET: api/empleado
	// Regresa una lista con todos los empleados registrados en la base de datos esceptuadno el dato de RFID puesto que se trata de un valor delicado, puesto que con el se permitiria la entrada a el lugar donde se aplique el sistema
        [HttpGet]
        public async Task<IEnumerable<EmpleadoVM>> GetEmpleadosWORfid()
        {
            return await context.Empleado.Select(s => new EmpleadoVM
            {
                Id = s.Id,
                Nombre = s.Nombre,
                ApellidoP = s.ApellidoP,
                ApellidoM = s.ApellidoM
            }).ToListAsync();
        }

        // POST: api/empleado
	// De una instancia de empleado recibida en la peticion, esta se ingresara como nuevo empleado en la base de datos 
        [HttpPost]
        public async Task<IActionResult> Post([Bind("Nombre, ApellidoP, ApellidoM, Rfid")] Empleado empleado)
        {
            if (!ModelState.IsValid) // verifica que la instancia sea valida de acuerdo a las reglas establecidas para el modelo
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (await context.Empleado.Where(x => x.Rfid == empleado.Rfid).AnyAsync()) // si el dato RFID (que es unico para cada empleado) existe dentro de la base de datos, esta instancia no podra ingresarse
            {
                return BadRequest(ErrorHelper.Response(400, "el rfid ya existe"));
            }

            context.Empleado.Add(empleado);   // se realiza la peticion a base de datos para agregar la nueva instancia
            await context.SaveChangesAsync(); // se guarda la instancia nueva en la base de datos
            return Ok(); // se regresa un codigo 200 para confirmar la operacion
        }

        // DELETE: api/empleado/1
	// De un id tipo int correspondiente a un empleado, se eliminara la instancia correspondiente de la base de datos
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await context.Empleado.FindAsync(id);  // se busca la instancia con el id recibido
            if (empleado == null) // verifica que exista
            {
                return NotFound(ErrorHelper.Response(404, "No existe ese id"));
            }

            context.Empleado.Remove(empleado); // se pide eliminar de la base de datos
            await context.SaveChangesAsync();  // se guardan los cambios
            return NoContent();  // se regresa un codigo 204 para confirmar la eliminacion del empleado
        }

        // PUT: api/empleado/1
	// De la instancia recibida en la peticion, se modificara los datos correspondientes a una instancia ya existente en la base de datos
	[HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [Bind("Id, Nombre, ApellidoP, ApellidoM, Rfid")]Empleado empleado)
        {
            if(!await context.Empleado.Where(s => s.Id == empleado.Id).AsNoTracking().AnyAsync())  // Verifica que el empleado exista en la base de datos
            {
                return NotFound(ErrorHelper.Response(404, "el empleado a modificar no fue encontrado"));
            }
            if(await context.Empleado.Where(s => s.Rfid == empleado.Rfid && s.Id != empleado.Id).AnyAsync())  // verifica que el rfid de la instancia recibida no sea compartida de otra instancia dentro de la base de datos a escepcion de la correspondiente al empleado que se va a modificar 
            {
                return BadRequest(ErrorHelper.Response(400, "el RFID ya existe"));
            }

            if(empleado.Nombre == "" || empleado.ApellidoM == "" || empleado.ApellidoP == "" || empleado.Rfid == "") // Verifica en la instancia recibida los valores de nombre, apellidos y rfid por si estan vacios
            {
                var empleadoAnterior = await context.Empleado.Where(w => w.Id == empleado.Id).AsNoTracking().FirstOrDefaultAsync();  // en caso de que sea verdadero, se manda a traer la instancia la cual se va a modificar
                if(empleado.Nombre == "") { empleado.Nombre = empleadoAnterior.Nombre; }        	// en caso de que cualquier valor este vacio, se reemplazará con el valor que ya estaba registrado
                if(empleado.ApellidoP == "") { empleado.ApellidoP = empleadoAnterior.ApellidoP; }
                if(empleado.ApellidoM == "") { empleado.ApellidoM = empleadoAnterior.ApellidoM; }
                if(empleado.Rfid == "") { empleado.Rfid = empleadoAnterior.Rfid; }
            }

            context.Entry(empleado).State = EntityState.Modified;  // se estable la peticion de modificar la instancia
            await context.SaveChangesAsync();  // se guardan los cambios
            return Ok();   // regresa un codigo 200 para confirmar su cambio
        }
    }
}
