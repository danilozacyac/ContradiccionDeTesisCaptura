using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;
using ContradiccionesDirectorioApi.Utils;
using ScjnUtilities;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for ContraInfoGral.xaml
    /// </summary>
    public partial class ContraInfoGral 
    {
        private Contradicciones contradiccion;
        private ListadoDeContradicciones listado;

        public ContraInfoGral(ListadoDeContradicciones listado)
        {
            InitializeComponent();
            this.contradiccion = new Contradicciones();
            this.listado = listado;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contradiccion.Criterios = new ObservableCollection<Criterios>();
            this.DataContext = contradiccion;
            CbxTiposAsuntos.DataContext = TipoAsuntoSingleton.TipoAsunto;

            CbxPlenos.DataContext = OrganismosSingleton.Plenos;
            CbxPresidente.DataContext = FuncionariosSingleton.FuncionariosCollection;
            CbxPonente.DataContext = FuncionariosSingleton.FuncionariosCollection;
        }

        private void BtnAddCriterio_Click(object sender, RoutedEventArgs e)
        {
            CriteriosWin criterios = new CriteriosWin(contradiccion, null, false);
            criterios.ShowDialog();
        }

        private void BtnDelCriterio_Click(object sender, RoutedEventArgs e)
        {
            Criterios criterio = (Criterios)RGridCriterios.SelectedItem;
            contradiccion.Criterios.Remove(criterio);
            
        }

        private void TxtExpNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (RadTramite.IsChecked == false && RadResuelto.IsChecked == false)
            {
                MessageBox.Show(ConstantMessages.SeleccionaEstadoExpediente);
                return;
            }

            if (contradiccion.ExpedienteAnio < 1990 && contradiccion.ExpedienteAnio > DateTime.Now.Year + 2)
            {
                MessageBox.Show(ConstantMessages.RangoAnual);
                return;
            }

            if (contradiccion.FechaTurno == null)
            {
                MessageBox.Show(ConstantMessages.SeleccionaFechaturno);
                return;
            }

            contradiccion.Status = (RadTramite.IsChecked == true) ? 0 : 1;
            contradiccion.IdTipoAsunto = (Int32)CbxTiposAsuntos.SelectedValue;
            //contradiccion.IdPlenoCircuito = (Int32)CbxPlenos.SelectedValue;


            contradiccion.IdPresidentePleno = (CbxPresidente.SelectedValue != null) ? (Int32)CbxPresidente.SelectedValue : 0;
            contradiccion.IdPonentePleno = (CbxPonente.SelectedValue != null) ? (Int32)CbxPonente.SelectedValue : 0;

            ContradiccionesModel contra = new ContradiccionesModel();
            CriteriosModel crit = new CriteriosModel();
            OficiosModel ofi = new OficiosModel();

            Oficios oficio = new Oficios() { Oficio = TxtOficio.Text, FechaOficio = DateFOficio.SelectedDate };

            contradiccion.IdContradiccion = contra.SetNewContradiccion(contradiccion);
            crit.SetNewCriterios(contradiccion);
            ofi.SetNewOficio(oficio, contradiccion.IdContradiccion);

            contradiccion.Oficios = new ObservableCollection<Oficios>();
            contradiccion.Oficios.Add(oficio);

            listado.Listado.Add(contradiccion);
            this.Close();
        }

        private void BtnObservaciones_Click(object sender, RoutedEventArgs e)
        {
            Observaciones obsr = new Observaciones(contradiccion);
            obsr.ShowDialog();
        }

        

        
    }
}
