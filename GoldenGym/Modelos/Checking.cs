using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenGym.Modelos
{
    public class Checking
    {
        public Checking() { }
        /*Definimos los camps*/
        public int Id { get; set; }
        public int Id_usuario { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
