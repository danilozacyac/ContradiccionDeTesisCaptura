using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;
using ContradiccionesDirectorioApi.Utils;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Model;
using ScjnUtilities;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for ContradiccionesWin.xaml
    /// </summary>
    public partial class ContradiccionesWin : Window
    {
        private Contradicciones contradiccion;
        private ListadoDeContradicciones listado;
        private readonly bool isUpdating = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contradiccion">Expediente de contradiccion a ser visualizado o modificado</param>
        /// <param name="isUpdating">false indica que solo se visualiza y true que se podrá actualizar</param>
        public ContradiccionesWin(Contradicciones contradiccion, bool isUpdating)
        {
            InitializeComponent();
            this.contradiccion = contradiccion;
            this.isUpdating = isUpdating;

            contradiccion.MiEjecutoria = (contradiccion.MiEjecutoria == null) ? new Ejecutoria() : contradiccion.MiEjecutoria;
            contradiccion.Returnos = (contradiccion.Returnos == null) ? new ObservableCollection<ReturnosClass>() : contradiccion.Returnos;
            contradiccion.Resolutivo = (contradiccion.Resolutivo == null) ? new Resolutivos() : contradiccion.Resolutivo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxTiposAsuntos.DataContext = TipoAsuntoSingleton.TipoAsunto;

            this.DataContext = contradiccion;
            //this.RGridResolutivos.DataContext = contradiccion.Resolutivo.PuntosResolutivos;

            CbxPresidente.DataContext = FuncionariosSingleton.FuncionariosCollection;
            CbxPonente.DataContext = FuncionariosSingleton.FuncionariosCollection;
            CbxPlenos.DataContext = OrganismosSingleton.Plenos;

            CbxPresidente.SelectedValue = contradiccion.IdPresidentePleno;
            CbxPonente.SelectedValue = contradiccion.IdPonentePleno;

            this.LoadNoBindings();
        }

        private void BtnAddCriterio_Click(object sender, RoutedEventArgs e)
        {
            CriteriosWin criterios = new CriteriosWin(contradiccion,null,false);
            criterios.ShowDialog();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ///Validaciones
            if (RadJuris.IsChecked == false && RadAislada.IsChecked == false && RadImprocedente.IsChecked == false)
            {
                MessageBox.Show(ConstantMessages.SeleccionaTipoDeTesis);
                return;
            }

            ///Valores ComboBox y RadioButtons
            contradiccion.IdPresidentePleno = (CbxPresidente.SelectedValue != null) ? (Int32)CbxPresidente.SelectedValue : 0;
            contradiccion.IdPonentePleno = (CbxPonente.SelectedValue != null) ? (Int32)CbxPonente.SelectedValue : 0;
            contradiccion.Status = (RadResuelto.IsChecked == true) ? 1 : 0;
            contradiccion.IdTipoAsunto = (Int32)CbxTiposAsuntos.SelectedValue;
            

            ///Actualiza Info General de Contradiccion
            ContradiccionesModel contra = new ContradiccionesModel();
            contra.UpdateContradiccion(contradiccion);
            
            
            ///Actualiza info Resolucion
            ResolucionModel resol = new ResolucionModel();
            if (resol.CheckIfExist(contradiccion.IdContradiccion))
            {
                resol.UpdateResolucion(contradiccion.Resolutivo, contradiccion.IdContradiccion);
            }
            else
            {
                resol.SetNewResolucion(contradiccion);
            }

            

            //Actualiza Info ejecutoria
            EjecutoriasModel eje = new EjecutoriasModel();

            if (eje.CheckIsExist(contradiccion.IdContradiccion))
            {
                eje.UpdateEjecutoria(contradiccion);
            }
            else
            {
                eje.SetNewEjecutoriaPorContradiccion(contradiccion);
            }

            AdmisorioModel admisorio = new AdmisorioModel();
            if (admisorio.CheckIfExist(contradiccion.AcAdmisorio.IdAcuerdo))
            {
                admisorio.UpdateAdmisorio(contradiccion.AcAdmisorio);
            }
            else
            {
                admisorio.SetNewAdmisorio(contradiccion.AcAdmisorio);
            }

            this.Close();
        }

        private String OpenDialogForPath()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            //dlg.DefaultExt = ".txt";
            //dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                return dlg.FileName;
            }
            else
            {
                return "";
            }
        }

        

        

        

        private void BtnFileEjecPath_Click(object sender, RoutedEventArgs e)
        {
            TxtFileEjecPath.Text = this.OpenDialogForPath();
        }

        

        

        

        private void TxtExpNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void TxtRelTesis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void BtnAgregarTesis_Click(object sender, RoutedEventArgs e)
        {
            if (contradiccion.MiEjecutoria.TesisRelacionadas == null)
                contradiccion.MiEjecutoria.TesisRelacionadas = new ObservableCollection<int>();

            NumIusModel numIusModel = new NumIusModel();

            TesisDto tesis = numIusModel.BuscaTesis(Convert.ToInt32(TxtRelTesis.Text));

            if (tesis != null)
            {
                EjecutoriasModel model = new EjecutoriasModel();
                model.SetRelacionesEjecutorias(tesis.Ius, contradiccion.IdContradiccion, 1);
                TxtRelTesis.Text = String.Empty;
                contradiccion.MiEjecutoria.TesisRelacionadas.Add(tesis.Ius);
            }
            else
            {
                MessageBox.Show("Ingrese un número de registro IUS válido");
            }
        }

        private void BtnAgregarVoto_Click(object sender, RoutedEventArgs e)
        {
            if (contradiccion.MiEjecutoria.VotosRelacionados == null)
                contradiccion.MiEjecutoria.VotosRelacionados = new ObservableCollection<int>();

            NumIusModel numIusModel = new NumIusModel();

            TesisDto tesis = numIusModel.BuscaVoto(Convert.ToInt32(TxtRelVotos.Text));

            if (tesis != null)
            {
                EjecutoriasModel model = new EjecutoriasModel();
                model.SetRelacionesEjecutorias(tesis.Ius, contradiccion.IdContradiccion, 3);
                TxtRelVotos.Text = String.Empty;
                contradiccion.MiEjecutoria.VotosRelacionados.Add(tesis.Ius);
            }
            else
            {
                MessageBox.Show("Ingrese un número de registro IUS válido");
            }
        }

        private void BtnAgregaReturno_Click(object sender, RoutedEventArgs e)
        {
            Returnos returno = new Returnos(contradiccion);
            returno.Show();
        }

        private void LoadNoBindings()
        {
            if (contradiccion.Status == 1)
                RadResuelto.IsChecked = true;
            else
                RadTramite.IsChecked = true;

            CbxTiposAsuntos.SelectedValue = contradiccion.IdTipoAsunto;

            

            
        }

        private void BtnEditCriterios_Click(object sender, RoutedEventArgs e)
        {
            Criterios editCriterio = (Criterios)RGridCriterios.SelectedItem;

            CriteriosWin criterios = new CriteriosWin(contradiccion, editCriterio, true);
            criterios.ShowDialog();
        }

        private void BtnDelCriterio_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Deseas eliminar el criterio seleccionado?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Criterios criterio = (Criterios)RGridCriterios.SelectedItem;
                CriteriosModel model = new CriteriosModel();
                model.DeleteCriterio(criterio);

                contradiccion.Criterios.Remove(criterio);
            }
        }

        private void BtnAddResolutivo_Click(object sender, RoutedEventArgs e)
        {
            PuntoResolutivo punto = new PuntoResolutivo(contradiccion);
            punto.ShowDialog();
        }

        private void BtnEditResolutivo_Click(object sender, RoutedEventArgs e)
        {
            PResolutivos resolutivo = (PResolutivos)RGridResolutivos.SelectedItem;

            PuntoResolutivo punto = new PuntoResolutivo(contradiccion,resolutivo);
            punto.ShowDialog();
        }

        private void BtnDltResolutivo_Click(object sender, RoutedEventArgs e)
        {
            PResolutivos resolutivo = (PResolutivos)RGridResolutivos.SelectedItem;

            ResolucionModel model = new ResolucionModel();
            model.DeleteResolutivo(resolutivo.IdResolutivo);

            contradiccion.Resolutivo.PuntosResolutivos.Remove(resolutivo);
        }

        private void BtnEditaReturno_Click(object sender, RoutedEventArgs e)
        {
            ReturnosClass returno = (ReturnosClass)RGridReturnos.SelectedItem;

            Returnos retWin = new Returnos(contradiccion, returno, true);
            retWin.ShowDialog();
        }

        private void BtnEliminaReturno_Click(object sender, RoutedEventArgs e)
        {
            ReturnosClass returno = (ReturnosClass)RGridReturnos.SelectedItem;
            ReturnosModel model = new ReturnosModel();
            model.DeleteReturno(returno.IdReturno);

            contradiccion.Returnos.Remove(returno);
        }

        private void BtnObservaciones_Click(object sender, RoutedEventArgs e)
        {
            Observaciones observ = new Observaciones(contradiccion);
            observ.ShowDialog();
        }

        private void BtnAddTesis_Click(object sender, RoutedEventArgs e)
        {
            CapturaTesis captura = new CapturaTesis(contradiccion.MiTesis,contradiccion.IdContradiccion);
            captura.Show();
        }

        private Tesis selectedTesis;
        private void GTesisContra_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTesis = GTesisContra.SelectedItem as Tesis;

            TxtCControl.Text = selectedTesis.ClaveControl;
            TxtClaveIdent.Text = selectedTesis.ClaveIdentificacion;
            TxtRubro.Text = selectedTesis.Rubro;

            if (selectedTesis.Tatj == 1)
                RadJuris.IsChecked = true;
            else if (selectedTesis.Tatj == 0)
                RadAislada.IsChecked = true;
            else
                RadImprocedente.IsChecked = true;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTesis != null)
            {
                CapturaTesis captura = new CapturaTesis(selectedTesis);
                captura.ShowDialog();
            }
            else
            {
                MessageBox.Show("Debes de seleccionar la tesis que quieres seleccionar");
            }
        }

        
    }
}