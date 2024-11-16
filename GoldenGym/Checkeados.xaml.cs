using GoldenGym.Servicios;
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

using GoldenGym.Modelos;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Checkeados.xaml
    /// </summary>
    public partial class Checkeados : Window
    {
        public Checkeados()
        {
            InitializeComponent();
        }
        private void dgChekeados_Loaded(object sender, RoutedEventArgs e)
        {
            //dgInvitados.DataContext = DatoInvitados.MuestraInvitados();
            dgChekeados.DataContext = DatoChecking.MuestraChecks();
        }

        private void dgChekeados_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
