using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenGym.Modelos
{
    public class Logins
    {
        public Logins() { }
        /*Definimos los camps*/
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }

    }
}
