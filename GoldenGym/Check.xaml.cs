﻿using DPFP;
using DPFP.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.Concurrent;
using GoldenGym.Modelos;
using GoldenGym.Servicios;
using System.Windows.Threading;
using static DPFP.Verification.Verification;
using System.Runtime.InteropServices.ComTypes;




namespace GoldenGym
{
    /// <summary>
    /// Lógica de interacción para Check.xaml
    /// </summary>
    public partial class Check : Window, DPFP.Capture.EventHandler
    {
        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;
        private DPFP.Capture.Capture Capturer;
        public Check()
        {
            InitializeComponent();

            
        }


        protected virtual void Init()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();              // Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;                   // Subscribe for capturing events.
                else
                    lblReport.Content = "Can't initiate capture operation!";
            }
            catch
            {

                MessageBox.Show("Can't initiate capture operation!", "Error",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            Verificator = new DPFP.Verification.Verification();
        }

        protected void Process(DPFP.Sample Sample)
        {

            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                // Compare the feature set with our template
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                
                DPFP.Template template = new DPFP.Template();

                Stream stream;
                List<Usuario> usuarios = DatoUsuario.MuestraUsuarios();

                foreach( var usuario in usuarios)
                {
                    if (usuario.Huella != null)
                    {
                        stream = new MemoryStream(usuario.Huella);
                        template = new DPFP.Template(stream);
                        Verificator.Verify(features, template, ref result);
                        if (result.Verified)
                        {
                            this.Dispatcher.Invoke(new Function(delegate ()
                            {
                                ColapsaImagen();
                                int id_usuario;
                                string nombre;
                                id_usuario = usuario.Id;
                                nombre = usuario.Nombre;
                                //Checkear(id_usuario,nombre);
                                CheckGuardar(id_usuario, nombre);
                                Desplegar(usuario);
                                
                                StartTimer();
                            }));
                            break;
                        }
                        else {
                            DesplegarNoRegistrado();
                            StartTimer();
                        }

                    }
                    
                       
                    
                }
            }
        }


        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    lblReport.Content = "Using the fingerprint reader, scan your fingerprint.";
                }
                catch
                {
                    lblReport.Content="Can't initiate capture!";
                }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    lblReport.Content = ("Can't terminate capture!");
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Stop();
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        public void Desplegar(Usuario usuario)
        {
            imgPerfil.Visibility = Visibility.Visible;
            string url = "";

            lblNombre.Content = $"Nombre: {usuario.Nombre}";
            lblApellidos.Content = $"Apellidos: {usuario.Apellidos}";
            lblNumero.Content = $"Numero: {usuario.Numero}";
            lblEstatus.Content = $"Estatus: {usuario.Estatus}";
            lblDias.Content = $"Días faltantes: {usuario.DiasFaltantes} días";
            lblAdeudo.Content = $"Adeudo: ${usuario.Adeudo:F2}";

            if (usuario.Estatus == "Activo")
            {
                lblEstatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF65FF00")); // Verde
            }
            else if (usuario.Estatus == "Por vencer")
            {
                lblEstatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFDFF00")); // Amarillo
            }
            else if (usuario.Estatus == "Vencido")
            {
                lblEstatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEF1D57")); // Rojo
            }

            if (usuario.Adeudo > 0)
            {
                lblAdeudo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEF1D57")); // Color personalizado
            }
            else
            {
                lblAdeudo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF2D64B")); // Amarilllo normal
            }

            if (usuario.Foto != "" && usuario.Foto != null)
            {
                url = "C:/GoldenGym/" + usuario.Foto;
            }
            else
                url = "C:/GoldenGym/sin-imagen.jpeg";

            BitmapImage foto = new BitmapImage();
            foto.BeginInit();
            foto.UriSource = new Uri(url);  
            foto.EndInit();
            foto.Freeze();

            imgPerfil.Source = foto;
        }
        public void CheckGuardar(int id_usuario, string nombre)
        {
            DateTime date = DateTime.Now;
            try 
            { 
                Checking checking = new Checking();
                checking.Id_usuario = id_usuario;
                checking.Fecha = date;
                bool res = DatoChecking.VerificarCheck(checking);
                if (res)
                {
                    //MessageBox.Show("Ya existe el usuario checkeado", "Éxito");
                    ;
                    
                }
                else
                {

                    //MessageBox.Show("No existe y se checkea", "Éxito");
                    Checkear(id_usuario, nombre);
                }


            }
            catch (Exception ex) 
            {
                MessageBox.Show("Algo sucedio: " + ex.Message, "Error en verificar");
            }

        }
        public void Checkear(int id_usuario, string nombre)
        {
            try
            {
                //Usuario usuario = new Usuario();
                
                /*Ejecutamos la consulta de la base de datos*/

                //int id = DatoUsuario.AltaUsuario(usuario)\;
                int id = DatoChecking.AltaCheck(id_usuario, nombre);
                if (id > 0)
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible guardar el usuario: " + ex.Message, "Error en Guardar");

            }
        }
        public void Checkear2(int id_usuario, string nombre)
        {
            try
            {
                //Usuario usuario = new Usuario();
                /*Ejecutamos la consulta de la base de datos*/

                //int id = DatoUsuario.AltaUsuario(usuario)\;
                int id = DatoChecking.AltaCheck(id_usuario, nombre);
                if (id > 0)
                {
                    MessageBox.Show("Registro exitoso", "Guardar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible guardar el usuario: " + ex.Message, "Error en Guardar");

            }
        }
        public void DesplegarNoRegistrado()
        {
            imgPerfil.Visibility = Visibility.Visible;
            ColapsaImagen();
            string url = "";

            lblNombre.Content = " Que esperas";
            lblApellidos.Content = "       Para";
            lblNumero.Content = "Formar parte";
            lblEstatus.Content = "        De";
            lblDias.Content = "Golden Gym";
            lblAdeudo.Content = "";


            url = "C:/GoldenGym/sin-registro.jpg";

            BitmapImage foto = new BitmapImage();
            foto.BeginInit();
            foto.UriSource = new Uri(url);
            foto.EndInit();
            foto.Freeze();

            imgPerfil.Source = foto;
        }
        private void StartTimer()
        {
            // Crea un temporizador de 5 segundos
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);

            // Suscribirse al evento Tick que se dispara cuando el tiempo ha pasado
            timer.Tick += (sender, args) =>
            {
                // Parar el temporizador
                ((DispatcherTimer)sender).Stop();

                // Llamar la función limpiar
                Limpiar();
                Visible();
            };

            // Iniciar el temporizador
            timer.Start();
        }
        public void Limpiar()
        {
            

            lblNombre.Content = "";
            lblApellidos.Content = "";
            lblNumero.Content = "";
            lblEstatus.Content = "";
            lblDias.Content = "";
            lblAdeudo.Content = "";


            

            imgPerfil.Visibility = Visibility.Collapsed;
        }
        public void ColapsaImagen()
        {
            imgEspera.Visibility = Visibility.Collapsed;
        }
        public void Visible()
        {
            imgEspera.Visibility = Visibility.Visible;
        }


        #region EventHandler Members:

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
           

            this.Dispatcher.Invoke(new Function(delegate () {
                lblReport.Content = "The fingerprint sample was captured.";
                lblStatus.Content = "Scan the same fingerprint again.";
                Process(Sample);
            }));
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            this.Dispatcher.Invoke(new Function(delegate () {
                lblReport.Content = "The finger was removed from the fingerprint reader.";
            }));
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            this.Dispatcher.Invoke(new Function(delegate () {
                lblReport.Content = "The fingerprint reader was touched.";
            }));
            
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            this.Dispatcher.Invoke(new Function(delegate () {
                lblReport.Content = "The fingerprint reader was connected.";
            }));
            
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            this.Dispatcher.Invoke(new Function(delegate () {
                lblReport.Content = "The fingerprint reader was disconnected.";
            }));
            
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {   
            /*if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                lblReport.Content = "The quality of the fingerprint sample is good.";
            else
                lblReport.Content = "The quality of the fingerprint sample is poor.";*/
        }


        #endregion

        
    }

}
