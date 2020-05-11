using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPISeguridad.Models;

namespace WebAPISeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")] //Todos los endPoint estan protegidos por el sistema de autenticación.

    public class CuentasController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<string>> get()
        {
            return new string[] { "lol", "lol2"};
        }

    

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email }; //Campos de la clase heredada.
            var result = await _userManager.CreateAsync(user, model.Password); //Creacion del usuario
            if (result.Succeeded) //Si es exitoso
            {
                return BuildToken(model, new List<string>()); //Contruye un Token
            }
            else
            {
                return BadRequest("Username or password invalid");
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return BuildToken(userInfo, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }

        private UserToken BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim> //Los Claims son un conjunto de datos confiables las cuales viajan en el Token
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //Jti -> Sirve para identificar de manera unica un TOKEN.
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol)); //Agrego a JWT todos los roles que pertenecen a un usuario, al cual se le esta construyendo su Token.
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"])); //Llave de seguridad.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Creacion de credenciales

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(2); //FEcha de expiracion del Token

            JwtSecurityToken token = new JwtSecurityToken( //Ceacion del Token
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken() //instancia de mi clase
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token), //Retorno del Token como un string
                Expiracion = expiration
            };
        }
    }

}