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
using System.Collections.ObjectModel;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para PuntoVenta.xaml
    /// </summary>
    public partial class PuntoVenta : Window
    {
        public ObservableCollection<Producto> ProductosVenta { get; set; }
        public ObservableCollection<Producto> ListaVenta { get; set; }

        public int Cantidad { get; private set; }

        public PuntoVenta()
        {
            InitializeComponent();

            // Inicializar las colecciones
            ProductosVenta = new ObservableCollection<Producto>(DatoProductos.MuestraProductos());
            ListaVenta = new ObservableCollection<Producto>();

            // Enlazar las colecciones a los DataGrid
            dgProductosVenta.ItemsSource = ProductosVenta;
            dgListaVenta.ItemsSource = ListaVenta;
        }

        private void dgProductosVenta_Loaded(object sender, RoutedEventArgs e)
        {
            dgProductosVenta.DataContext = DatoProductos.MuestraProductos();
        }

        private void dgProductosVenta_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgProductosVenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductosVenta.SelectedItem is Producto productoSeleccionado)
            {
                // Mostrar la ventana emergente para ingresar la cantidad
                var cantidadWindow = new Cantidad();
                cantidadWindow.Owner = this; // Establecer la ventana principal como propietaria
                if (cantidadWindow.ShowDialog() == true)
                {
                    int cantidad = cantidadWindow.CantidadArticulo;
                    if (productoSeleccionado.Stock == 0)
                    {
                        MessageBox.Show("Ya no hay articulos de este producto", "Error");
                        return;
                    }
                    else if(productoSeleccionado.Stock < cantidad)
                    {
                        MessageBox.Show("La cantidad ingresada de articulos es mayor a la que actualmente existe", "Error");
                        return;
                    }

                    var nuevoProducto = new Producto
                    {
                        NombreProducto = productoSeleccionado.NombreProducto,
                        Descripcion = productoSeleccionado.Descripcion,
                        Marca = productoSeleccionado.Marca,
                        PrecioCompra = productoSeleccionado.PrecioCompra,
                        PrecioVenta = productoSeleccionado.PrecioVenta * cantidad,
                        Stock = cantidad // Aquí el stock representa la cantidad seleccionada
                    };
                   
                    ListaVenta.Add(nuevoProducto);
                    ActualizarTotal();

                }

                // Deseleccionar el producto en dgProductosVenta
                dgProductosVenta.SelectedItem = null;
            }
        }
        private void ActualizarTotal()
        {
            float total = (float)ListaVenta.Sum(producto => producto.PrecioVenta);
            lblTotal.Content = $"Total: {total:C}";
        }
        private void tbBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProductos(tbBuscarProducto.Text);
        }
        private void FiltrarProductos(string filtro)
        {
            var productos = DatoProductos.MuestraProductos();

            ProductosVenta.Clear(); // Limpiar la colección actual

            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                var productosFiltrados = productos.Where(p =>
                    p.NombreProducto.ToLower().Contains(filtro) ||
                    p.Marca.ToLower().Contains(filtro)
                );

                foreach (var producto in productosFiltrados)
                {
                    ProductosVenta.Add(producto);
                }
            }
            else
            {
                foreach (var producto in productos)
                {
                    ProductosVenta.Add(producto);
                }
            }
        }

        private void dgListaVenta_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void btnCobrar_Click(object sender, RoutedEventArgs e)
        {
            // Sumar el total de la venta
            float lblVenta = (float)ListaVenta.Sum(producto => producto.PrecioVenta);
            if(lblVenta == 0.0)
            {
                MessageBox.Show("Venta en 0.0", "Advertencia");
                return;
            }
            else
            {
                // Mostrar un mensaje confirmando el total (opcional)
                MessageBox.Show($"Venta realizada con éxito. Total: {lblVenta:C}", "Cobro exitoso");

                // Limpiar la lista de venta
                ListaVenta.Clear();

                // Restablecer el total en el Label
                lblTotal.Content = "Total: $0.00";
            }

            


            // Aquí puedes guardar `lblVenta` y los detalles de `ListaVenta` en tu base de datos en el futuro
            // Ejemplo:
            // GuardarVentaEnBaseDeDatos(lblVenta, ListaVenta);
        }
    }
}
