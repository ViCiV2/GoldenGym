using Microsoft.Win32;
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
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Window
    {
        public Usuarios()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnFoto_Click(object sender, RoutedEventArgs e)
        {
            
            if (tbNumero.Text == "")
            {
                MessageBox.Show("El numero del usuario debe de ser definido", "Error");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen (.jpg)|*.jpg|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            /*Verificamos si todo fue correcto*/
            if (ofd.ShowDialog() == true)
            {
                tbUrlFoto.Text = "";
                try
                {
                    /*Mostramos la foto en el lugar que pusimos para la imagen*/
                    BitmapImage foto = new BitmapImage();
                    foto.BeginInit();
                    foto.UriSource = new Uri(ofd.FileName);
                    foto.EndInit();
                    foto.Freeze();

                    imgFoto.Source = foto;
                    tbUrlFoto.Text = "foto_" + tbNumero.Text + ".jpg";
                    string destino = @"C:\GoldenGym\";
                    string url = destino + tbUrlFoto.Text;
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error");
                }
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (tbNombre.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }
            if (tbApellidos.Text == "")
            {
                MessageBox.Show("El campo Apellido es obligatorio", "Error");
                return;
            }
            if (tbDireccion.Text == "")
            {
                tbDireccion.Text = "Sin direccion especificada";
            }
            if (tbNumero.Text == "")
            {
                tbNumero.Text = "Sin numero especificado";
            }
            if (string.IsNullOrWhiteSpace(tbImporte.Text))
            {
                MessageBox.Show("El campo Importe es obligatorio", "Error");
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbImporte.Text, out _))
                {
                    MessageBox.Show("El campo Importe debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }
            if (tbPromo.Text == "")
            {
                tbPromo.Text = "Sin especificar";
            }
            if (string.IsNullOrWhiteSpace(tbAdeudo.Text))
            {
                tbAdeudo.Text = "0.0";
                return;
            }
            else
            {
                // Intentar convertir el texto a un float
                if (!float.TryParse(tbAdeudo.Text, out _))
                {
                    MessageBox.Show("El campo Adeudo debe ser un número válido, verifica que no se hayan ingresado espacios", "Error");
                    return;
                }
            }
            if (tbNotas.Text == "")
            {
                tbNotas.Text = "Sin notas";
            }
            if (dateInicio.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de inicio.", "Error");
                return;
            }
            // Obtener el valor seleccionado
            DateTime fechaSeleccionada = dateInicio.SelectedDate.Value;
            DateTime horaActual = DateTime.Now;
            DateTime fechaConHora = new DateTime(
                fechaSeleccionada.Year,
                fechaSeleccionada.Month,
                fechaSeleccionada.Day,
                horaActual.Hour,   // Hora actual
                horaActual.Minute, // Minutos actuales
                horaActual.Second  // Segundos actuales
            );
            //string fechaFormateada = fechaSeleccionada.ToString("dd/MM/yyyy");
            //MessageBox.Show($"Fecha seleccionada: {fechaConHora}", "Información");
            if (dateFin.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de fin.", "Error");
                return;
            }


            DateTime fechaSeleccionadaFin = dateFin.SelectedDate.Value;
            DateTime horaActualFin = DateTime.Now;
            DateTime fechaConHoraFin = new DateTime(
                fechaSeleccionadaFin.Year,
                fechaSeleccionadaFin.Month,
                fechaSeleccionadaFin.Day,
                horaActualFin.Hour,   // Hora actual
                horaActualFin.Minute, // Minutos actuales
                horaActualFin.Second  // Segundos actuales
            );
            //string fechaFormateadaFin = fechaSeleccionadaFin.ToString("dd/MM/yyyy");
            //MessageBox.Show($"Fecha seleccionada: {fechaConHoraFin}", "Información");

            if (tbUrlFoto.Text == "")
            {
                MessageBox.Show("No ha seleccionado una foto, puede guardar el usuario.", "Advertencia");
                tbUrlFoto.Text = "";
            }

            if(Template == null)
            {
                var result = MessageBox.Show("No ha capturado la huella, ¿desea continuar sin huella?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return; // Sale si el usuario decide no continuar sin la huella.
                }
            }

            try
            {
                Usuario usuario = new Usuario();
                usuario.Nombre = tbNombre.Text;
                usuario.Apellidos = tbApellidos.Text;
                usuario.Numero = tbNumero.Text;
                usuario.Direccion = tbDireccion.Text;
                usuario.Fecha_inicio = fechaConHora;
                usuario.Fecha_fin = fechaConHoraFin;
                usuario.Promo = tbPromo.Text;
                usuario.Importe = float.Parse(tbImporte.Text);
                usuario.Adeudo = float.Parse(tbAdeudo.Text);
                usuario.Foto = tbUrlFoto.Text;
                usuario.Notas = tbNotas.Text;
                usuario.Huella = Template != null ? Template.Bytes : null;

                if (tbUrlFoto.Text != "")
                {
                    string destino = @"C:\GoldenGym\";
                    string recurso = imgFoto.Source.ToString().Replace("file:///", "");
                    File.Copy(recurso, destino + tbUrlFoto.Text, true);
                }



                /*Ejecutamos la consulta de la base de datos*/

                int id = DatoUsuario.AltaUsuario(usuario);
                if (id > 0)
                {
                    MessageBox.Show("Usuario guardado correctamente", "Guardar");
                    tbNombre.Text = "";
                    tbApellidos.Text = "";
                    tbNumero.Text = "";
                    tbDireccion.Text = "";
                    dateInicio.SelectedDate = null;
                    dateFin.SelectedDate = null;
                    tbPromo.Text = "";
                    tbImporte.Text = "";
                    tbAdeudo.Text = "";
                    tbUrlFoto.Text = "";
                    imgFoto.Source = null;
                    tbNotas.Text = "";
                    imgVerHuella.Visibility = Visibility.Hidden;
                    Template = null;
                    //dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();
                    ActualizarTablaYConteos();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible guardar el usuario: " + ex.Message, "Error en Guardar");

            }


        }



        private void dgUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();

            // Obtenemos la colección de usuarios
            var usuarios = DatoUsuario.MuestraUsuarios();

            if (usuarios != null)
            {
                // Calculamos los conteos
                int total = usuarios.Count();
                int activos = usuarios.Count(u => u.Estatus == "Activo");
                int porVencer = usuarios.Count(u => u.Estatus == "Por vencer");
                int vencidos = usuarios.Count(u => u.Estatus == "Vencido");

                // Actualizamos las etiquetas
                txtUsuariosActivos.Text = $"{activos}";
                txtUsuariosPorVencer.Text = $"{porVencer}";
                txtUsuariosVencidos.Text = $"{vencidos}";
            }
        }


        private void ActualizarTablaYConteos()
        {
            // Cargar los datos en la tabla
            dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();

            // Obtenemos la colección de usuarios
            var usuarios = DatoUsuario.MuestraUsuarios();

            if (usuarios != null)
            {
                // Calculamos los conteos
                int total = usuarios.Count();
                int activos = usuarios.Count(u => u.Estatus == "Activo");
                int porVencer = usuarios.Count(u => u.Estatus == "Por vencer");
                int vencidos = usuarios.Count(u => u.Estatus == "Vencido");

                // Actualizamos las etiquetas
                txtUsuariosActivos.Text = $"{activos}";
                txtUsuariosPorVencer.Text = $"{porVencer}";
                txtUsuariosVencidos.Text = $"{vencidos}";
            }
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Usuario usuario = (Usuario)dgUsuarios.SelectedItem;

            if (usuario == null)
            {
                btnEditar.Visibility = Visibility.Collapsed;
                btnLimpiar.IsEnabled = false;
                btnEliminar.Visibility = Visibility.Collapsed;
                return;
            }
            btnEditar.Visibility = Visibility.Visible;
            btnEliminar.Visibility = Visibility.Visible;
            btnLimpiar.IsEnabled = true;

            tbNombre.Text = usuario.Nombre;
            tbApellidos.Text = usuario.Apellidos;
            tbNumero.Text = usuario.Numero;
            tbDireccion.Text = usuario.Direccion;
            dateInicio.SelectedDate = usuario.Fecha_inicio;
            dateFin.SelectedDate = usuario.Fecha_fin;
            tbPromo.Text = usuario.Promo;
            tbImporte.Text = usuario.Importe.ToString();
            tbAdeudo.Text = usuario.Adeudo.ToString();
            tbNotas.Text = usuario.Notas;

            if(usuario.Huella != null)
                imgVerHuella.Visibility = Visibility.Visible;
            else
                imgVerHuella.Visibility= Visibility.Hidden;

            if (usuario.Foto != "" && usuario.Foto != null)
            {
                BitmapImage foto = new BitmapImage();
                foto.BeginInit();
                foto.UriSource = new Uri(@"C:\GoldenGym\" + usuario.Foto);
                foto.EndInit();
                imgFoto.Source = foto;
                tbUrlFoto.Text = usuario.Foto;

            }
            else
            {
                imgFoto.Source = null;
                tbUrlFoto.Text = "";
            }
        }

        private void btnCaptura_Click(object sender, RoutedEventArgs e)
        {
            EnrollmentForm Enroller = new EnrollmentForm();
            Enroller.OnTemplate += this.OnTemplate;
            Enroller.ShowDialog();
        }

        private void OnTemplate(DPFP.Template template)
        {
            this.Dispatcher.Invoke(new Function(delegate ()
            {
                Template = template;
                //VerifyButton.Enabled = SaveButton.Enabled = (Template != null);
                if (Template != null)
                {
                    MessageBox.Show("La huella ha sido capturada correctamente.", "Capturar huella");
                    imgVerHuella.Visibility = Visibility.Visible;
                }    
                else
                    MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
            }));
        }

        private DPFP.Template Template;

        private void FiltrarUsuarios(string filtro)
        {
            var usuarios = DatoUsuario.MuestraUsuarios(); // Cargamos todos los usuarios
            if (!string.IsNullOrEmpty(filtro))
            {
                // Convertimos el filtro a minúsculas para hacer la búsqueda case-insensitive
                filtro = filtro.ToLower();

                // Filtramos la lista en función del nombre, apellidos o número
                var usuariosFiltrados = usuarios.Where(u =>
                    u.Nombre.ToLower().Contains(filtro) ||
                    u.Apellidos.ToLower().Contains(filtro) ||
                    u.Numero.ToLower().Contains(filtro)).ToList();

                // Actualizamos el DataContext con los usuarios filtrados
                dgUsuarios.DataContext = usuariosFiltrados;
            }
            else
            {
                // Si no hay filtro, mostramos todos los usuarios
                dgUsuarios.DataContext = usuarios;
            }
        }


        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarUsuarios(tbBuscar.Text);
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            tbNombre.Text = "";
            tbApellidos.Text = "";
            tbNumero.Text = "";
            tbDireccion.Text = "";
            dateInicio.SelectedDate = null;
            dateFin.SelectedDate = null;
            tbPromo.Text = "";
            tbImporte.Text = "";
            tbAdeudo.Text = "";
            tbUrlFoto.Text = "";
            imgFoto.Source = null;
            tbNotas.Text = "";
            imgVerHuella.Visibility = Visibility.Hidden;
            Template = null;
            dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();

        }

        public static string GenerarNombreAleatorio(int longitud)
        {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(caracteresPermitidos, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (tbNombre.Text == "")
            {
                MessageBox.Show("El campo Nombre es obligatorio", "Error");
                return;
            }

            if (tbApellidos.Text == "")
            {
                MessageBox.Show("El campo Apellido es obligatorio", "Error");
                return;
            }

            if (tbDireccion.Text == "")
            {
                tbDireccion.Text = "Sin direccion especificada";
            }

            if (tbNumero.Text == "")
            {
                tbNumero.Text = "Sin numero especificado";
            }

            if (string.IsNullOrWhiteSpace(tbImporte.Text))
            {
                MessageBox.Show("El campo Importe es obligatorio", "Error");
                return;
            }
            else
            {
                if (!float.TryParse(tbImporte.Text, out _))
                {
                    MessageBox.Show("El campo Importe debe ser un número válido", "Error");
                    return;
                }
            }

            if (tbPromo.Text == "")
            {
                tbPromo.Text = "Sin especificar";
            }

            if (string.IsNullOrWhiteSpace(tbAdeudo.Text))
            {
                tbAdeudo.Text = "0.0";
            }
            else
            {
                if (!float.TryParse(tbAdeudo.Text, out _))
                {
                    MessageBox.Show("El campo Adeudo debe ser un número válido", "Error");
                    return;
                }
            }

            if (tbNotas.Text == "")
            {
                tbNotas.Text = "Sin notas";
            }

            if (dateInicio.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de inicio.", "Error");
                return;
            }

            DateTime fechaSeleccionada = dateInicio.SelectedDate.Value;
            DateTime horaActual = DateTime.Now;
            DateTime fechaConHora = new DateTime(
                fechaSeleccionada.Year,
                fechaSeleccionada.Month,
                fechaSeleccionada.Day,
                horaActual.Hour,   // Hora actual
                horaActual.Minute, // Minutos actuales
                horaActual.Second  // Segundos actuales
            );

            if (dateFin.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de fin.", "Error");
                return;
            }

            DateTime fechaSeleccionadaFin = dateFin.SelectedDate.Value;
            DateTime horaActualFin = DateTime.Now;
            DateTime fechaConHoraFin = new DateTime(
                fechaSeleccionadaFin.Year,
                fechaSeleccionadaFin.Month,
                fechaSeleccionadaFin.Day,
                horaActualFin.Hour,   // Hora actual
                horaActualFin.Minute, // Minutos actuales
                horaActualFin.Second  // Segundos actuales
            );


            Usuario usuarioHuella = (Usuario)dgUsuarios.SelectedItem;

            if (usuarioHuella.Huella == null && Template == null)
            {
                var result = MessageBox.Show("No ha capturado la huella, ¿desea continuar sin huella?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return; // Si el usuario decide no continuar, salimos del método
                }
            }

            try
            {
                // Obtenemos el usuario seleccionado en la tabla
                Usuario usuario = (Usuario)dgUsuarios.SelectedItem;

                // Aquí editamos los valores del usuario seleccionado
                usuario.Nombre = tbNombre.Text;
                usuario.Apellidos = tbApellidos.Text;
                usuario.Numero = tbNumero.Text;
                usuario.Direccion = tbDireccion.Text;
                usuario.Fecha_inicio = fechaConHora;
                usuario.Fecha_fin = fechaConHoraFin;
                usuario.Promo = tbPromo.Text;
                usuario.Importe = float.Parse(tbImporte.Text);
                usuario.Adeudo = float.Parse(tbAdeudo.Text);
                usuario.Foto = tbUrlFoto.Text;
                usuario.Notas = tbNotas.Text;
                if (usuarioHuella.Huella == null && Template == null)
                {
                    usuario.Huella = Template != null ? Template.Bytes : null;
                }
                else if (Template != null)
                {
                    usuario.Huella = Template != null ? Template.Bytes : null;
                }
                
                
                
                //MessageBox.Show(string.Format("Foto URL: {0}", usuario.Foto), "Detalles de Foto");
                string respaldo = ".jpg";
                string new_edit = GenerarNombreAleatorio(10);
                string generaImagen = new_edit + respaldo;
                



                if (tbUrlFoto.Text != "")
                {
                    // imgFoto.Source = null;
                    /*Aqui simplemente no hacemos nada si ya hay foto y no la quieren cambiar*/
                    string destino = @"C:\GoldenGym\";
                    string recurso = imgFoto.Source.ToString().Replace("file:///", "");
                    File.Copy(recurso, destino + generaImagen, true);
                    usuario.Foto = generaImagen;

                }
                // Ejecutamos la consulta de actualización en la base de datos usando el ID del usuario
                int result = DatoUsuario.ModificarUsuario(usuario);
                if (result > 0)
                {

                    MessageBox.Show("Usuario editado correctamente", "Editar");

                    // Limpiar los campos después de editar
                    tbNombre.Text = "";
                    tbApellidos.Text = "";
                    tbNumero.Text = "";
                    tbDireccion.Text = "";
                    dateInicio.SelectedDate = null;
                    dateFin.SelectedDate = null;
                    tbPromo.Text = "";
                    tbImporte.Text = "";
                    tbAdeudo.Text = "";
                    tbUrlFoto.Text = "";
                    imgFoto.Source = null;
                    tbNotas.Text = "";
                    imgVerHuella.Visibility = Visibility.Hidden;
                    Template = null;



                    // Actualizar el DataGrid
                    //dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();
                    ActualizarTablaYConteos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible editar el usuario: " + ex.Message, "Error en Editar");
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dgUsuarios.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, selecciona un producto para eliminar.", "Eliminar Producto");
                    return;
                }
                Usuario usuario = (Usuario)dgUsuarios.SelectedItem;
                MessageBoxResult confirmacion = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar el usuario, esta acción no se podrá deshacer  '{usuario.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    //int result = DatoProductos.EliminarProducto(producto);
                    int result = DatoUsuario.EliminarUsuario(usuario);

                    if (result > 0)
                    {

                        MessageBox.Show("Usuario eliminado correctamente.", "Eliminar Usuario");
                        
                        // Limpiar los campos después de editar
                        tbNombre.Text = "";
                        tbApellidos.Text = "";
                        tbNumero.Text = "";
                        tbDireccion.Text = "";
                        dateInicio.SelectedDate = null;
                        dateFin.SelectedDate = null;
                        tbPromo.Text = "";
                        tbImporte.Text = "";
                        tbAdeudo.Text = "";
                        tbUrlFoto.Text = "";
                        imgFoto.Source = null;
                        tbNotas.Text = "";
                        imgVerHuella.Visibility = Visibility.Hidden;
                        Template = null;
                        //dgUsuarios.DataContext = DatoUsuario.MuestraUsuarios();
                        ActualizarTablaYConteos();

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible eliminar el producto: " + ex.Message, "Error en Eliminar");
            }
        }

        private void btnInvitados_Click(object sender, RoutedEventArgs e)
        {
            Invitados invitados = new Invitados();
            invitados.Show();
        }

        

        private void dgUsuarios_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
