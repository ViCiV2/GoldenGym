using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenGym.Modelos
{
    public class Producto
    {
        public Producto() { }
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public string Marca {  get; set; }
        public float PrecioCompra {  get; set; }
        public float PrecioVenta { get; set; }
        public int Stock {  get; set; }
        

    }
}
