using GoldenGym.Servicios;
using System;
using System.Windows;
using GoldenGym.Modelos;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Invitados.xaml
    /// </summary>
    public partial class Invitados : Window
    {
        public Invitados()
        {
            InitializeComponent();
        }

        private void btnGuardarInvitado_Click(object sender, RoutedEventArgs e)
        {
            if (tbNombreInvitado.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbImporteInvitado.Text))
            {
                tbImporteInvitado.Text = "0.0";
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbImporteInvitado.Text, out _))
                {
                    MessageBox.Show("El campo Importe debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }


            try
            {
                Invitado invitado = new Invitado();
                invitado.Nombre = tbNombreInvitado.Text;

                

                invitado.Fecha = DateTime.Now;

                invitado.Importe = float.Parse(tbImporteInvitado.Text);






                /*Ejecutamos la consulta de la base de datos*/

               
                int id = DatoInvitados.AltaInvitado(invitado);
                if (id > 0)
                {
                    MessageBox.Show("Invitado guardado correctamente", "Guardar");
                    tbNombreInvitado.Text = "";
                    tbImporteInvitado.Text = "";
                    dgInvitados.DataContext = DatoInvitados.MuestraInvitados();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible guardar al invitado: " + ex.Message, "Error en Guardar");

            }
        }

        private void dgInvitados_Loaded(object sender, RoutedEventArgs e)
        {
            dgInvitados.DataContext = DatoInvitados.MuestraInvitados();
        }

        private void dgInvitados_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
