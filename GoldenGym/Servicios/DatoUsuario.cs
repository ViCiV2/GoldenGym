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
    public class DatoUsuario
    {
        public DatoUsuario() { }

        /*Definiremos una clase que nos devuelva una lista de usuarios con la cual vamos a llenar el datagrid*/
        public static List<Usuario> MuestraUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            try
            {
                /*Definimos la conexion a la base de datos aqui nuestra base de datos se va a llamar Checador*/
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "MuestraUsuario";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    
                                    Usuario usuario = new Usuario();
                                    usuario.Id = int.Parse(dr["Id"].ToString());
                                    usuario.Nombre = dr["Nombre"].ToString();
                                    usuario.Apellidos = dr["Apellidos"].ToString();
                                    usuario.Direccion = dr["Direccion"].ToString();
                                    usuario.Numero = dr["Numero"].ToString();
                                    usuario.Fecha_inicio = dr["Fecha_inicio"] != DBNull.Value ? (DateTime?)dr["Fecha_inicio"] : null;
                                    usuario.Fecha_fin = dr["Fecha_fin"] != DBNull.Value ? (DateTime?)dr["Fecha_fin"] : null;
                                    usuario.Promo = dr["Promo"].ToString();
                                    usuario.Importe = float.Parse(dr["Importe"].ToString());
                                    usuario.Adeudo = float.Parse(dr["Adeudo"].ToString());
                                    usuario.Foto = dr["Foto"].ToString();
                                    usuario.Notas = dr["Notas"].ToString();
                                    if (dr["Huella"].ToString() != "")
                                    {
                                        usuario.Huella = (byte[])dr["Huella"];
                                    }
                                    else
                                    {
                                        //usuario.Huella = new byte[0];
                                        usuario.Huella = null;
                                    }

                                    listaUsuarios.Add(usuario);

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
            return listaUsuarios;
        }

        public static int AltaUsuario(Usuario usuario)
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
                        command.CommandText = "AltaUsuario";
                        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                        command.Parameters.AddWithValue("@Numero", usuario.Numero);
                        command.Parameters.AddWithValue("@Direccion", usuario.Direccion);
                        command.Parameters.AddWithValue("@Fecha_inicio", usuario.Fecha_inicio);
                        command.Parameters.AddWithValue("@Fecha_fin", usuario.Fecha_fin);
                        command.Parameters.AddWithValue("@Promo", usuario.Promo);
                        command.Parameters.AddWithValue("@Importe", usuario.Importe);
                        command.Parameters.AddWithValue("@Adeudo", usuario.Adeudo);
                        command.Parameters.AddWithValue("@Foto", usuario.Foto);
                        command.Parameters.AddWithValue("@Notas", usuario.Notas);
                        command.Parameters.AddWithValue("@Huella", usuario.Huella);

                        SqlParameter param = new SqlParameter("id", SqlDbType.Int);

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

        public static int ModificarUsuario(Usuario usuario)
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
                        command.CommandText = "EditarUsuario"; // Nombre del procedimiento almacenado

                        // Pasar los parámetros requeridos para el procedimiento
                        command.Parameters.AddWithValue("@Id", usuario.Id);
                        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                        command.Parameters.AddWithValue("@Numero", usuario.Numero);
                        command.Parameters.AddWithValue("@Direccion", usuario.Direccion);
                        command.Parameters.AddWithValue("@Fecha_inicio", usuario.Fecha_inicio.HasValue ? (object)usuario.Fecha_inicio.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@Fecha_fin", usuario.Fecha_fin.HasValue ? (object)usuario.Fecha_fin.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@Promo", usuario.Promo);
                        command.Parameters.AddWithValue("@Importe", usuario.Importe);
                        command.Parameters.AddWithValue("@Adeudo", usuario.Adeudo);
                        command.Parameters.AddWithValue("@Foto", usuario.Foto);
                        command.Parameters.AddWithValue("@Notas", usuario.Notas);
                        command.Parameters.AddWithValue("@Huella", usuario.Huella);


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

        public static int EliminarUsuario(Usuario usuario)
        {
            int res = 0;

            try
            {
                using (var conn = new SqlConnection("Data Source=localhost; initial catalog=Checador; Integrated Security=True"))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "EliminarUsuario"; // Nombre del procedimiento almacenado.

                        // Usar el Id del producto del objeto.
                        command.Parameters.AddWithValue("@Id", usuario.Id);

                        // Ejecutar el procedimiento almacenado.
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario: " + ex.Message, "Error en Eliminación");
            }

            return res;
        }

    }
}
