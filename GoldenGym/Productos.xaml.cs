using GoldenGym.Servicios;
using System;
using System.Windows;
using GoldenGym.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DPFP.Verification.Verification;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Window
    {
        public Productos()
        {
            InitializeComponent();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dgProductos_Loaded(object sender, RoutedEventArgs e)
        {
            dgProductos.DataContext = DatoProductos.MuestraProductos();
        }

        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto producto = (Producto)dgProductos.SelectedItem;

            if(producto == null)
            {
                btnEditarProducto.Visibility = Visibility.Collapsed;
                btnEliminarProducto.Visibility = Visibility.Collapsed;
                btnLimpiar.IsEnabled = false;
                return;
            }
            btnEditarProducto.Visibility = Visibility.Visible;
            btnEliminarProducto.Visibility = Visibility.Visible;
            btnLimpiar.IsEnabled = true;

            tbNombreProducto.Text = producto.NombreProducto;
            tbDescripcion.Text = producto.Descripcion;
            tbMarca.Text = producto.Marca;
            tbPrecioCompra.Text = producto.PrecioCompra.ToString();
            tbPrecioVenta.Text = producto.PrecioVenta.ToString();
            tbStock.Text = producto.Stock.ToString();
        }

        private void btnGuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            

            if (tbDescripcion.Text == "")
            {
                tbDescripcion.Text = "Sin Descripcion";
            }

            if (tbNombreProducto.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbPrecioCompra.Text))
            {
                MessageBox.Show("El campo precio de compra es obligatorio", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbPrecioCompra.Text, out _))
                {
                    MessageBox.Show("El campo precio de compra debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(tbPrecioVenta.Text))
            {
                MessageBox.Show("El campo precio de venta debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbPrecioVenta.Text, out _))
                {
                    MessageBox.Show("El campo precio de venta debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(tbStock.Text))
            {
                MessageBox.Show("El campo stock debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!int.TryParse(tbStock.Text, out _))
                {
                    MessageBox.Show("El campo stock debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (tbNombreProducto.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (tbMarca.Text == "")
            {
                MessageBox.Show("El campo marca es obligatorio", "Error");
                return;
            }

            try
            {
                Producto producto = new Producto();
                producto.NombreProducto = tbNombreProducto.Text;
                producto.Descripcion = tbDescripcion.Text;
                producto.Marca = tbMarca.Text;
                producto.PrecioCompra = float.Parse(tbPrecioCompra.Text);
                producto.PrecioVenta = float.Parse(tbPrecioVenta.Text);
                producto.Stock = int.Parse(tbStock.Text);

                int id = DatoProductos.AltaProducto(producto);

                if (id > 0)
                {
                    MessageBox.Show("Producto guardado correctamente", "Guardar");
                    tbNombreProducto.Text = "";
                    tbDescripcion.Text = "";
                    tbMarca.Text = "";
                    tbPrecioCompra.Text = "";
                    tbPrecioVenta.Text = ""; 
                    tbStock.Text = "";
                    dgProductos.DataContext = DatoProductos.MuestraProductos();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("No fue posible guardar el producto: " + ex.Message, "Error en Guardar");
            }
        }

        private void dgProductos_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void tbBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProductos(tbBuscarProducto.Text);
        }

        /*Funcion de busqueda de productos*/

        private void FiltrarProductos(string filtro)
        {
            var productos = DatoProductos.MuestraProductos();
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                var productosFiltrados = productos.Where(p =>
                    p.NombreProducto.ToLower().Contains(filtro) ||
                    p.Marca.ToLower().Contains(filtro)
                    ).ToList();

                dgProductos.DataContext = productosFiltrados;
            }
            else
            {
                dgProductos.DataContext = productos;
            }
        }

        private void btnEditarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (tbDescripcion.Text == "")
            {
                tbDescripcion.Text = "Sin Descripcion";
            }

            if (tbNombreProducto.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbPrecioCompra.Text))
            {
                MessageBox.Show("El campo precio de compra es obligatorio", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbPrecioCompra.Text, out _))
                {
                    MessageBox.Show("El campo precio de compra debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(tbPrecioVenta.Text))
            {
                MessageBox.Show("El campo precio de venta debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbPrecioVenta.Text, out _))
                {
                    MessageBox.Show("El campo precio de venta debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(tbStock.Text))
            {
                MessageBox.Show("El campo stock debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!int.TryParse(tbStock.Text, out _))
                {
                    MessageBox.Show("El campo stock debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }

            if (tbNombreProducto.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (tbMarca.Text == "")
            {
                MessageBox.Show("El campo marca es obligatorio", "Error");
                return;
            }

            try
            {
                //Producto producto = new Producto();
                Producto producto = (Producto)dgProductos.SelectedItem;
                producto.NombreProducto = tbNombreProducto.Text;
                producto.Descripcion = tbDescripcion.Text;
                producto.Marca = tbMarca.Text;
                producto.PrecioCompra = float.Parse(tbPrecioCompra.Text);
                producto.PrecioVenta = float.Parse(tbPrecioVenta.Text);
                producto.Stock = int.Parse(tbStock.Text);

                int resutl = DatoProductos.ModificarProducto(producto);

                if (resutl > 0)
                {
                    MessageBox.Show("Producto modificado correctamente", "Guardar");
                    dgProductos.SelectionChanged -= dgProductos_SelectionChanged;

                    tbNombreProducto.Text = "";
                    tbDescripcion.Text = "";
                    tbMarca.Text = "";
                    tbPrecioCompra.Text = "";
                    tbPrecioVenta.Text = "";
                    tbStock.Text = "";
                    dgProductos.DataContext = DatoProductos.MuestraProductos();
                    dgProductos.SelectionChanged += dgProductos_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible editar el producto: " + ex.Message, "Error en Editar");
            }
        }

        private void btnEliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (dgProductos.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, selecciona un producto para eliminar.", "Eliminar Producto");
                    return;
                }
                Producto producto = (Producto)dgProductos.SelectedItem;
                MessageBoxResult confirmacion = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar el producto, esta acción no se podrá deshacer  '{producto.NombreProducto}'?",
                "Confirmar Eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                if (confirmacion == MessageBoxResult.Yes) 
                {
                    int result = DatoProductos.EliminarProducto(producto);

                    if (result > 0)
                    {
                        
                        MessageBox.Show("Producto eliminado correctamente.", "Eliminar Producto");
                        dgProductos.SelectionChanged -= dgProductos_SelectionChanged;
                        tbNombreProducto.Text = "";
                        tbDescripcion.Text = "";
                        tbMarca.Text = "";
                        tbPrecioCompra.Text = "";
                        tbPrecioVenta.Text = "";
                        tbStock.Text = "";
                        dgProductos.DataContext = DatoProductos.MuestraProductos();
                        dgProductos.SelectionChanged += dgProductos_SelectionChanged;
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible eliminar el producto: " + ex.Message, "Error en Eliminar");
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            tbNombreProducto.Text = "";
            tbDescripcion.Text = "";
            tbMarca.Text = "";
            tbPrecioCompra.Text = "";
            tbPrecioVenta.Text = "";
            tbStock.Text = "";
            btnLimpiar.IsEnabled = false;
            dgProductos.DataContext = DatoProductos.MuestraProductos();
        }
    }
}
