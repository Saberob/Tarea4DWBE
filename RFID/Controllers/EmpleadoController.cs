﻿using System;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]    
    public class EmpleadoController : Controller
    {
        private readonly RFIDContext context;

        public EmpleadoController(RFIDContext _context)
        {
            context = _context;
        }

        // GET: api/Empleado
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

        [HttpPost]
        public async Task<IActionResult> Post([Bind("Nombre, ApellidoP, ApellidoM, Rfid")] Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (await context.Empleado.Where(x => x.Rfid == empleado.Rfid).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "el rfid ya existe"));
            }

            context.Empleado.Add(empleado);
            await context.SaveChangesAsync();
            return Created($"/empleado/{empleado.Id}", empleado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await context.Empleado.FindAsync(id);
            if (empleado == null)
            {
                return NotFound(ErrorHelper.Response(404, "No existe ese id"));
            }

            context.Empleado.Remove(empleado);
            await context.SaveChangesAsync();
            return NoContent();
        }

	    [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [Bind("Id, Nombre, ApellidoP, ApellidoM, Rfid")]Empleado empleado)
        {
            if(!await context.Empleado.Where(s => s.Id == empleado.Id).AsNoTracking().AnyAsync())
            {
                return NotFound(ErrorHelper.Response(404, "el empleado a modificar no fue encontrado"));
            }
            if(await context.Empleado.Where(s => s.Rfid == empleado.Rfid && s.Id != empleado.Id).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "el RFID ya existe"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            context.Entry(empleado).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
