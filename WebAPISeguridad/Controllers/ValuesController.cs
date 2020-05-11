using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPISeguridad.Services;

namespace WebAPISeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("PermitirConexiones")] //Permite la politica en todos los endPoints
    public class ValuesController : ControllerBase
    {                                               //IDataProtectionProvider -> encargador de crear el DataProtector
        private readonly IDataProtector _protector; //Encargado de encriptar y desencriptar las mensajes.
        private readonly HashService _hashService;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("valor_unico_y_quizas_secreto"); //va a encriptar y desencriptar la data.  String de proposito -> usado para desencriptar
            _hashService = hashService;
        }


        // GET api/values
        [HttpGet]
        [EnableCors("PermitirConexiones")] //Habilitar el CORS, solo por atributo.
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("hash")]
        public ActionResult GetHash()
        {
            string textoPlano = "Ariel Cabrera";
            var hashResult1 = _hashService.Hash(textoPlano).Hash;
            var hashResult2 = _hashService.Hash(textoPlano).Hash;
            return Ok(new { textoPlano, hashResult1, hashResult2 });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [EnableCors("PermitirConexiones")]
        public ActionResult<string> Get(int id)
        {
            string textoPlano = "Ariel Cabbrera texto plano";
            string textoCifrado = _protector.Protect(textoPlano);
            string textoDesencriptado = _protector.Unprotect(textoCifrado);
            return Ok(new { textoPlano, textoCifrado, textoDesencriptado });
        }

        // GET api/values/5
        [HttpGet("list/{id}")]
        [EnableCors("PermitirConexiones")]
        public ActionResult<IEnumerable<string>> EjemploDeEncriptacionLimitadaPorTiempo(int id)
        {
            var protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();
            string textoPlano = "Felipe Gavilán";
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            string textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);

            return Ok(new { textoPlano, textoCifrado, textoDesencriptado, id });
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}