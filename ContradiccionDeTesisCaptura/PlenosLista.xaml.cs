using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for PlenosLista.xaml
    /// </summary>
    public partial class PlenosLista
    {
        private Organismos selectedOrganismo;

        public PlenosLista()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RLstPlenos.DataContext = OrganismosSingleton.Plenos;
        }

        private void RBtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrganismo == null)
            {
                MessageBox.Show("Primero debes de seleccionar el Pleno que deseas actualizar");
                return;
            }
            else
            {
                PlenosCircuito pleno = new PlenosCircuito(selectedOrganismo);
                pleno.ShowDialog();
            }

        }

        private void RBtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            PlenosCircuito pleno = new PlenosCircuito();
            pleno.ShowDialog();
        }

        private void RBtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RLstPlenos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrganismo = RLstPlenos.SelectedItem as Organismos;
        }
    }
}
