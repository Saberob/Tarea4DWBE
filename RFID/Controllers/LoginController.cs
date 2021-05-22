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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(Login value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            Usuarios usuario = await context.Usuarios.Where(x => x.userName == value.userName).FirstOrDefaultAsync();
            if(usuario == null)
            {
                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado"));
            }

            if (HashHelper.CheckHash(value.password, usuario.password, usuario.sal))
            {
                var secretKey = configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, value.userName));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var createdToken = tokenHandler.CreateToken(tokenDescriptor);
                string bearer_Token = tokenHandler.WriteToken(createdToken);
                return Ok(new tokenVM{
                    token = bearer_Token
                });
            }
            else
            {
                return Forbid();
            }
        }

      
    }
}
