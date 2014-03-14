using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for PuntoResolutivo.xaml
    /// </summary>
    public partial class PuntoResolutivo : Window
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
