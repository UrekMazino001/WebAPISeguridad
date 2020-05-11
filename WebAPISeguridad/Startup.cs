using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebAPISeguridad.Context;
using WebAPISeguridad.Models;
using WebAPISeguridad.Services;

namespace WebAPISeguridad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HashService>(); //Usado para usar el servicio de hash.
            services.AddDataProtection(); //Usado para usar encriptacion

            services.AddCors(options => 
            {
                options.AddPolicy("PermitirConexiones",
                 builder => builder.WithOrigins("https://www.apirequest.io").WithMethods("GET", "POST").AllowAnyHeader());
             }); //Permitir conexiones desde otro origenes, CORS Por atributo

            services.AddDbContext<ApplicationDbContext>(options => //Implementacion del DbContext
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>() //Estamos configurando el sistema de autenticacion
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders(); //Configuración básica de autenticación.


            //Configuracion para que acepte la configuracion de Barear
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                        ClockSkew = TimeSpan.Zero
                    });
                



            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); //Sirve para redirigir peticiones HTTP a HTTPS

            app.UseRouting();
            app.UseCors(); //CORS por atributo
            //app.UseCors(bulder => bulder.WithOrigins("https://www.apirequest.io/").WithMethods("*"));
            app.UseAuthentication(); //Middelware para que acepte la autenticación.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
