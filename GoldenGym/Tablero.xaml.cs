using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Tablero.xaml
    /// </summary>
    public partial class Tablero : Window
    {
        public Tablero()
        {
            InitializeComponent();
        }

        private void btnUsuariosTab_Click(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.Show();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            Productos productos = new Productos();
            productos.Show();
        }

        private void btnBitacora_Click(object sender, RoutedEventArgs e)
        {
            Checkeados checkeados = new Checkeados();
            checkeados.Show();
        }
    }
}
