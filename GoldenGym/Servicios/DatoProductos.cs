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
    public class DatoProductos
    {
        public DatoProductos() { } 

        /*Definimos la lista de productos para mostrar al datagrid*/

        public static List<Producto> MuestraProductos() 
        { 
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                using (var conn = new SqlConnection("Data Source = localhost; initial catalog = Checador; Integrated Security = True"))
                { 
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "MuestraProducto";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Producto producto = new Producto();
                                    producto.Id = int.Parse(dr["Id"].ToString());
                                    producto.NombreProducto = dr["NombreProducto"].ToString();
                                    producto.Descripcion = dr["Descripcion"].ToString();
                                    producto.Marca = dr["Marca"].ToString();
                                    producto.PrecioCompra = float.Parse(dr["PrecioCompra"].ToString());
                                    producto.PrecioVenta = float.Parse(dr["PrecioVenta"].ToString());
                                    producto.Stock = int.Parse(dr["Stock"].ToString());

                                    listaProductos.Add(producto);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al consultar los productos: " + ex.Message, "Error");
            }
            return listaProductos; 
        }

        public static int AltaProducto(Producto producto)
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
                        command.CommandText = "AltaProducto";
                        command.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        command.Parameters.AddWithValue("@Marca", producto.Marca);
                        command.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);
                        command.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        command.Parameters.AddWithValue("@Stock", producto.Stock);

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
                MessageBox.Show("Error al dar de alta un producto: " + ex.Message, "Error en Alta");
            }



            return res;
        }

        public static int ModificarProducto(Producto producto)
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
                        command.CommandText = "EditarProducto";


                        command.Parameters.AddWithValue("@Id", producto.Id);
                        command.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        command.Parameters.AddWithValue("@Marca", producto.Marca);
                        command.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);
                        command.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        command.Parameters.AddWithValue("@Stock", producto.Stock);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar un producto: " + ex.Message, "Error en Editar");
            }

            return res;
        }

        public static int EliminarProducto(Producto producto)
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
                        command.CommandText = "EliminarProducto"; // Nombre del procedimiento almacenado.

                        // Usar el Id del producto del objeto.
                        command.Parameters.AddWithValue("@Id", producto.Id);

                        // Ejecutar el procedimiento almacenado.
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message, "Error en Eliminación");
            }

            return res;
        }



    }




}
