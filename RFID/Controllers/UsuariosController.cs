using RFID.Helper;
using RFID.Models;
using RFID.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RFID.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        public readonly RFIDContext context;

        public UsuariosController(RFIDContext _context)
        {
            context = _context;
        }

	// POST: /usuarios
	// De una instancia agregada de la peticion, la funcion ingresa esa nueva instancia a la base de datos 
	// Regresa una llamada a la funcion anterior para mostrar el usuario ingresado
        [HttpPost]
        public async Task<IActionResult> Post(Usuarios usuario)
        {
            if (!ModelState.IsValid)  // Verifca que la instancia recibida sea valida segun las reglas establecidas en el modelo
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (await context.Usuarios.Where(x => x.userName == usuario.userName).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El usaurio ya existe, pruebe otro."));
            }

            HashedPassword Password = HashHelper.Hash(usuario.password); // se encripta la contraseña
            usuario.password = Password.Password; // se coloca el encriptado de la constraseña en la instancia recibida
            usuario.sal = Password.Salt; // Para mayor seguridad, tambien se guarda la sal dentro de la base de datos

            context.Usuarios.Add(usuario); // se guarda en la base de datos
            await context.SaveChangesAsync();
            return NoContent(); // se regresa un codigo 204
        }

        
    }
}
