using System;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Lógica de interacción para PlenosCircuito.xaml
    /// </summary>
    public partial class PlenosCircuito : Window
    {
        private Organismos organismo;
        private readonly bool isUpdatable;

        public PlenosCircuito()
        {
            InitializeComponent();
            organismo = new Organismos();
            isUpdatable = false;
        }

        public PlenosCircuito(Organismos organismo)
        {
            InitializeComponent();
            this.organismo = organismo;
            isUpdatable = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = organismo;
        }

        private void ChkEspecializado_Checked(object sender, RoutedEventArgs e)
        {
            TxtEspecializacion.IsEnabled = true;
        }

        private void ChkEspecializado_Unchecked(object sender, RoutedEventArgs e)
        {
            TxtEspecializacion.IsEnabled = false;
            TxtEspecializacion.Text = String.Empty;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            OrganismosModel model = new OrganismosModel();

            if (isUpdatable)
            {
                model.UpdatePleno(organismo);
            }
            else
            {
                model.SetNewPleno(organismo);
                OrganismosSingleton.Plenos.Add(organismo);
            }

            this.Close();
        }
    }
}
