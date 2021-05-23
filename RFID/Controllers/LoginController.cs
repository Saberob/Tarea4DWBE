using RFID.Helper;
using RFID.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RFID.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamenDWBE.Controllers
{
    //Se establece la ruta para acceder a las funciones relacionadas al incio de sesion
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly RFIDContext context;
        private readonly IConfiguration configuration;

        public LoginController(RFIDContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }

        // POST /login
	// De la instancia enviada del request, la funcion verifica la existencia de ese nombre de usuario y contraseña
	// Si se encuentra en la base de datos, la funcion regresa un token jwt para permitir la autenticacion en las llamadas a los demas controladores
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(Login value)
        {
            if (!ModelState.IsValid)		// Verifica que la instancia recibida sea valida de acuerdo a las reglas del modelo
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            Usuarios usuario = await context.Usuarios.Where(x => x.userName == value.userName).FirstOrDefaultAsync(); 
	    // Se busca y en dado caso se manda a llamar la instancia del usuario relacionado a el nombre de usuario recibido
            if(usuario == null)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado"));
            }

            if (HashHelper.CheckHash(value.password, usuario.password, usuario.sal))
            {
                var secretKey = configuration.GetValue<string>("SecretKey"); // se recupera la llave secreta establecida dentro de el archivo llamda appsettings.json
                var key = Encoding.ASCII.GetBytes(secretKey); // Se codifica a bytes la llave secreta

                var claims = new ClaimsIdentity();  		// Se establece la autorizacion del usuario 
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, value.userName));

                var tokenDescriptor = new SecurityTokenDescriptor  // se crea la cofiguracion que va a tener el jwt
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var createdToken = tokenHandler.CreateToken(tokenDescriptor); // se crea el token a partir de la configuracion establecida
                string bearer_Token = tokenHandler.WriteToken(createdToken);  // Se guarda en una variable el token final
                return Ok(new tokenVM{
                    token = bearer_Token
                }); // se le da formato al token
            }
            else
            {
                return Forbid();
            }
        }

      
    }
}
