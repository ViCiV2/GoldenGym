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

using System.IO;
using GoldenGym.Modelos;
using GoldenGym.Servicios;
using System.Security.Cryptography.X509Certificates;

namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void tbApellidos_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logins logins = new Logins();
                logins.Usuario = tbUser.Text;
                logins.Contrasena = tbContra.Password;

                bool res = DatoLogin.VerificarUsuario(logins);
                if (res)
                {
                    MessageBox.Show("Inicio de sesión exitoso", "Éxito");
                    Usuarios usuarios = new Usuarios();
                    usuarios.Show();
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Algo salio mal al iniciar sesion: " + ex.Message, "Error al iniciar");

            }
        }
    }
}
