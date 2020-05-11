using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPISeguridad.Entities;
using WebAPISeguridad.Models;

namespace WebAPISeguridad.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {//IdentityDbContext -> Es un contexto de datos especial configurado para trabajr con sistema de login

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Empresa> Empresas { get; set; }


        //ApiFluente
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Crear instacia de Rol
            var rolAdmin = new IdentityRole()
            {
                Id = "1212121",
                Name = "contador",
                NormalizedName = "contador"
            };

            builder.Entity<IdentityRole>().HasData(rolAdmin); //Crea migracion
            base.OnModelCreating(builder);
        }

    }
}
