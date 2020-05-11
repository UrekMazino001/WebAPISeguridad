using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISeguridad.Entities
{
    public class Empresa
    {
        public int ID{ get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Ingenio { get; set; }
        public string Siglas { get; set; }

    }
}
