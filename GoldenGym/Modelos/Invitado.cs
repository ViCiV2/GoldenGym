using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenGym.Modelos
{
    public class Invitado
    {
        public Invitado() { }
        /*Definimos los camps*/
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public float Importe { get; set; }
    }
}
