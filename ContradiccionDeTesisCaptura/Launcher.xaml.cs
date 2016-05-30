using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Model;
using Telerik.Windows.Controls;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Lógica de interacción para Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        private ListadoDeContradicciones contradicciones;

        public Launcher()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StyleManager.ApplicationTheme = new Windows8Theme();

            this.LaunchBusyIndicator();

            string path = ConfigurationManager.AppSettings["ErrorPath"];

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }



        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            contradicciones = new ListadoDeContradicciones();
            ContradiccionesModel conModel = new ContradiccionesModel();
            contradicciones.Listado = conModel.GetContradicciones();
            
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Dispatcher.BeginInvoke(new Action<ObservableCollection<Organismos>>(this.UpdateGridDataSource), e.Result);
            this.BusyIndicator.IsBusy = false;
            MainWindow diccionario = new MainWindow(contradicciones);
            diccionario.Show();
            this.Close();
        }

        private void LaunchBusyIndicator()
        {
            if (!worker.IsBusy)
            {
                this.BusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();

            }
        }

        #endregion
    }
}
