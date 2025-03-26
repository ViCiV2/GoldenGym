using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenGym.Modelos
{
    public class Usuario
    {
        public Usuario() { }
        /*Definimos los camps*/
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Numero { get; set; }
        public string Direccion { get; set; }
        public DateTime? Fecha_inicio { get; set; }
        public DateTime? Fecha_fin { get; set; }
        public string Promo { get; set; }
        public float Importe { get; set; }
        public float Adeudo { get; set; }
        public string Foto { get; set; }
        public string Notas { get; set; }
        public byte[] Huella { get; set; }
        /*Aqui hay que checar si debemos de definir un seter y getter de los dias faltantes y el estatus*/
        public int DiasFaltantes
        {
            get
            {
                if (Fecha_fin.HasValue)
                {
                    // Calcula la diferencia de tiempo total, incluyendo la fecha y la hora.
                    TimeSpan diferencia = Fecha_fin.Value - DateTime.Now;

                    // Devuelve la diferencia en días. La propiedad .Days redondea al día completo.
                    return (int)Math.Ceiling(diferencia.TotalDays);
                }
                return 0;
            }
        }

        public string Estatus
        {
            get
            {
                int diasFaltantes = DiasFaltantes;

                if (diasFaltantes > 3)
                {
                    return "Activo";
                }
                else if (diasFaltantes > 0 && diasFaltantes <= 3)
                {
                    return "Por vencer";
                }
                else
                {
                    return "Vencido";
                }
            }
        }

    }
}
