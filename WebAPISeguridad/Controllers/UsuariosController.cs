using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPISeguridad.Context;
using WebAPISeguridad.Models;

namespace WebAPISeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        [HttpPost("AsignarUsuarioRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserID);
            if(usuario == null) { return NotFound(); }
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RolName)); //Agregar un Claim al usuario para decir que pertenece a un Rol.
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RolName); //jWT

            return Ok();
        }

        [HttpPost("RemoverUsuarioRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserID);
            if (usuario == null) { return NotFound(); }
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RolName)); //Agregar un Claim al usuario para decir que pertenece a un Rol.
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RolName); //jWT

            return Ok();
        }

    }
}