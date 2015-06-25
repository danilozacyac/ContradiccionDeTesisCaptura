using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Model;
using ScjnUtilities;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for CriteriosWin.xaml
    /// </summary>
    public partial class CriteriosWin 
    {
        private Contradicciones contradiccion;
        private Criterios criterios;
        private readonly bool isUpdatingCriterio;

        public CriteriosWin(Contradicciones contradiccion, Criterios criterios, bool isUpdatingCriterio)
        {
            InitializeComponent();
            this.contradiccion = contradiccion;
            this.isUpdatingCriterio = isUpdatingCriterio;

            if (criterios != null)
                this.criterios = criterios;
            else
            {
                this.criterios = new Criterios();
                this.criterios.TesisContendientes = new ObservableCollection<int>();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxOrganismos.DataContext = OrganismosSingleton.Colegiados;
            this.DataContext = criterios;

            if (isUpdatingCriterio)
                CbxOrganismos.SelectedValue = criterios.IdOrgano;
        }

        private void TxtTesis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void TxtTesis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAgregar_Click(null, null);
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            NumIusModel numIusModel = new NumIusModel();

            TesisDto tesis = numIusModel.BuscaTesis(Convert.ToInt32(TxtTesis.Text));

            if (tesis != null)
            {
                TxtTesis.Text = String.Empty;
                criterios.TesisContendientes.Add(tesis.Ius);
            }
            else
            {
                MessageBox.Show("Ingrese un número de registro IUS válido");
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            CriteriosModel model = new CriteriosModel();
            criterios.IdOrgano = ((Organismos)CbxOrganismos.SelectedItem).IdOrganismo;
            criterios.Organo = CbxOrganismos.Text;

            if (isUpdatingCriterio)
            {
                model.UpdateCriterios(criterios, contradiccion.IdContradiccion);
            }
            else if (contradiccion.IsUpdating)
            {
                model.SetNewCriterios(criterios, contradiccion.IdContradiccion);
                contradiccion.Criterios.Add(criterios);
            }
            else
            {
                contradiccion.Criterios.Add(criterios);
            }

            this.Close();
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            criterios.TesisContendientes.Remove((Int32)LstTesisContendientes.SelectedItem);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TxtCriterio_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TxtCriterio.Text) && !String.IsNullOrWhiteSpace(TxtCriterio.Text))
                BtnAgregar.IsEnabled = true;

        }
    }
}