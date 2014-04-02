using System;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for Observaciones.xaml
    /// </summary>
    public partial class Observaciones : Window
    {
        private Contradicciones contradiccion;

        public Observaciones(Contradicciones contradiccion)
        {
            InitializeComponent();
            this.contradiccion = contradiccion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtObservaciones.Text = contradiccion.Observaciones;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            contradiccion.Observaciones = TxtObservaciones.Text;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
