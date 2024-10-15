using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

using GoldenGym.Modelos;
using System.Data.Common;
using System.Windows.Controls;



namespace GoldenGym.Servicios
{
    public class DatoLogin
    {
        public DatoLogin() { }
        public static bool VerificarUsuario(Logins logins)
        {
            
            try
            {
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "LoginUsuario";

                        command.Parameters.AddWithValue("@Usuario", logins.Usuario);
                        command.Parameters.AddWithValue("@Contrasena", logins.Contrasena);


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
                MessageBox.Show("Ocurrio un error al loguear al usuario: " + ex.Message, "Error");
                return false;
            }
            
           
        }

        public static int ModificarLogin(Logins logins)
        {
            int res = 0;

            try
            {
                // Establecer la conexión con la base de datos
                using (var conn = new SqlConnection("Data Source=localhost; initial catalog=Checador; Integrated Security=True"))
                {
                    conn.Open();

                    // Crear el comando para ejecutar el procedimiento almacenado
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "EditarLogin"; // Nombre del procedimiento almacenado

                        // Pasar los parámetros requeridos para el procedimiento
                        command.Parameters.AddWithValue("@Id", logins.Id);
                        command.Parameters.AddWithValue("@Usuario", logins.Usuario);
                        command.Parameters.AddWithValue("@Id", logins.Contrasena);
                        command.Parameters.AddWithValue("@Id", logins.Rol);


                        // Ejecutar el procedimiento almacenado
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el usuario: " + ex.Message, "Error en Modificación");
            }

            return res;
        }

    }
}
