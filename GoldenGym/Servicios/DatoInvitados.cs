
using GoldenGym.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoldenGym.Servicios
{
    public class DatoInvitados
    {
        public DatoInvitados() { }
        public static List<Invitado> MuestraInvitados()
        {
            List<Invitado> listaInvitados = new List<Invitado>();
            try
            {
                /*Definimos la conexion a la base de datos aqui nuestra base de datos se va a llamar Checador*/
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "MuestraInvitados";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                  
                                    Invitado invitado = new Invitado();

                                    
                                    
                                    invitado.Id = int.Parse(dr["Id"].ToString());
                                    invitado.Nombre = dr["Nombre"].ToString();
                                    invitado.Fecha = dr["Fecha"] != DBNull.Value ? (DateTime?)dr["Fecha"] : null;
                                    invitado.Importe = float.Parse(dr["Importe"].ToString());



                                    listaInvitados.Add(invitado);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al consultar los usuarios: " + ex.Message, "Error");
            }
            return listaInvitados;
        }

        public static int AltaInvitado(Invitado invitado)
        {
            int res = 0;

            try
            {
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "AltaInvitados";
                        command.Parameters.AddWithValue("@Nombre", invitado.Nombre);
                        command.Parameters.AddWithValue("@Fecha", invitado.Fecha);
                        command.Parameters.AddWithValue("@Importe", invitado.Importe);

                        SqlParameter param = new SqlParameter("Id", SqlDbType.Int);

                        param.Value = 0;
                        param.Direction = ParameterDirection.Output;
                        command.Parameters.Add(param);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar de alta un empleado: " + ex.Message, "Error en Alta");
            }
            return res;
        }


    }
}
