using System;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for PuntoResolutivo.xaml
    /// </summary>
    public partial class PuntoResolutivo 
    {
        private PResolutivos resolutivo;
        private Contradicciones contradiccion;

        public PuntoResolutivo(Contradicciones contradiccion)
        {
            InitializeComponent();
            resolutivo = new PResolutivos();
            this.contradiccion = contradiccion;
        }

        public PuntoResolutivo(Contradicciones contradiccion, PResolutivos resolutivo)
        {
            InitializeComponent();
            this.resolutivo = resolutivo;
            this.contradiccion = contradiccion;
        }

        public PuntoResolutivo(PResolutivos resolutivo)
        {
            InitializeComponent();
            this.resolutivo = resolutivo;
            
            BtnAceptar.Visibility = Visibility.Collapsed;
            BtnCancelar.Content = "Salir";
            TxtResolutivo.IsReadOnly = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = resolutivo;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            ResolucionModel model = new ResolucionModel();

            if (TxtResolutivo.Text.Length > 0)
            {
                if (resolutivo.IdResolutivo != 0)
                {
                    model.UpdateResolutivo(resolutivo);
                }
                else
                {
                    model.SetNewResolutivo(resolutivo, contradiccion.IdContradiccion);

                    if (contradiccion.Resolutivo.PuntosResolutivos == null)
                        contradiccion.Resolutivo.PuntosResolutivos = new System.Collections.ObjectModel.ObservableCollection<PResolutivos>();
                    
                    contradiccion.Resolutivo.PuntosResolutivos.Add(resolutivo);
                }
            }
            

            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
