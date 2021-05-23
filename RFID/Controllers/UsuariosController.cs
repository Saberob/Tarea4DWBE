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

	// GET: /usuarios/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            UsuarioVM usuarios = await context.Usuarios.Where(x => x.usuarioId == id).Select(x => new UsuarioVM()
            {
                usuarioId = x.usuarioId,
                userName = x.userName
            }).SingleOrDefaultAsync();

            return Ok(usuarios);
        }

	// POST: /usuarios
        [HttpPost]
        public async Task<IActionResult> Post(Usuarios usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (await context.Usuarios.Where(x => x.userName == usuario.userName).AnyAsync())
            {
                return BadRequest(ErrorHelper.Response(400, "El usaurio ya existe, pruebe otro."));
            }

            HashedPassword Password = HashHelper.Hash(usuario.password);
            usuario.password = Password.Password;
            usuario.sal = Password.Salt;

            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
            return NoContent();
        }

        
    }
}
