using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using GoldenGym.Modelos;

namespace GoldenGym.Servicios
{
    public class DatoChecking
    {
        public DatoChecking() { }

        public static List<Checking> MuestraChecks()
        { 
            List<Checking> listChecking = new List<Checking>();
            try
            {
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                { 
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "MuestraLosChecks";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                { 
                                    Checking checking = new Checking();

                                    checking.Nombre = dr["Nombre"].ToString();
                                    checking.Fecha = dr["Fecha"] != DBNull.Value ? (DateTime?)dr["Fecha"] : null;
                                    listChecking.Add(checking);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrio un error al consultar los chekings: " + ex.Message, "Error");
            }



            return listChecking; 

        }

        //En este metodo no usamos el objeto cheking porque trairemos la informacion desde usuario
        public static int AltaCheck(int Id_usuario, string nombre)
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
                        command.CommandText = "AltaChecks";

                        command.Parameters.AddWithValue("@Id_usuario", Id_usuario);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        //La fecha no se enviara desde aca si no que se generara automaticamente en la base de datos
                        //command.Parameters.AddWithValue("@Fecha", checking.Fecha);


                        SqlParameter param = new SqlParameter("Id", SqlDbType.Int);

                        param.Value = 0;
                        param.Direction = ParameterDirection.Output;
                        command.Parameters.Add(param);

                        res = command.ExecuteNonQuery();
                        int prueba = 0;
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error desconocido al guardar check en la base de datos, comunicate con el desarrollador y muestrale este mensaje: " + ex.Message, "Error en guardar");
            }
            return res;
        }

        public static bool VerificarCheck(Checking checking)
        {

            try
            {
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "VerificarCheck";

                        command.Parameters.AddWithValue("@Id_usuario", checking.Id_usuario);
                        command.Parameters.AddWithValue("@Fecha", checking.Fecha);


                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            // Si hay filas en el resultado, significa que el usuario y la contraseña coinciden
                            //if (dr.HasRows)
                            if (dr.Read())
                            {
                                return true; // Autenticación exitosa
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al chekear al usuario: " + ex.Message, "Error");
                return false;
            }


        }

        
    }
}
