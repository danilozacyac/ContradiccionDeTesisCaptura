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

            contradiccion.MiTesis = (contradiccion.MiTesis == null) ? new Tesis() : contradiccion.MiTesis;
            contradiccion.MiEjecutoria = (contradiccion.MiEjecutoria == null) ? new Ejecutoria() : contradiccion.MiEjecutoria;
            contradiccion.Returnos = (contradiccion.Returnos == null) ? new ObservableCollection<ReturnosClass>() : contradiccion.Returnos;
            contradiccion.Resolutivo = (contradiccion.Resolutivo == null) ? new Resolutivos() : contradiccion.Resolutivo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxTiposAsuntos.DataContext = TipoAsuntoSingleton.TipoAsunto;

            this.DataContext = contradiccion;

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
            if (RadJuris.IsChecked == false && RadAislada.IsChecked == false)
            {
                MessageBox.Show(ConstantMessages.SeleccionaTipoDeTesis);
                return;
            }


            ///Valores ComboBox y RadioButtons

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

            //Actualiza Info Tesis
            TesisModel tes = new TesisModel();
            Tesis tesis = tes.GetTesisPorContradiccion(contradiccion.IdContradiccion);

            if (tesis.IdContradiccion == 0)
            {
                tes.SetNewTesisPorContradiccion(contradiccion);
            }
            else
            {
                tes.UpdateTesis(contradiccion);
            }
            tesis = null;

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

        private void BtnArchivoOficioPub_Click(object sender, RoutedEventArgs e)
        {
            TxtOPPath.Text = this.OpenDialogForPath();
        }

        private void BtnArchivoVP_Click(object sender, RoutedEventArgs e)
        {
            TxtFileVpPath.Text = this.OpenDialogForPath();
        }

        private void BtnArchivoCC_Click(object sender, RoutedEventArgs e)
        {
            TxtFileCopiaPath.Text = this.OpenDialogForPath();
        }

        private void BtnFileEjecPath_Click(object sender, RoutedEventArgs e)
        {
            TxtFileEjecPath.Text = this.OpenDialogForPath();
        }

        private void RadSiPublica_Checked(object sender, RoutedEventArgs e)
        {
            LblVersionPublica.Visibility = Visibility.Visible;
            TxtFileVpPath.Visibility = Visibility.Visible;
            BtnArchivoVP.Visibility = Visibility.Visible;
        }

        private void RadNoPublica_Checked(object sender, RoutedEventArgs e)
        {
            LblVersionPublica.Visibility = Visibility.Hidden;
            TxtFileVpPath.Visibility = Visibility.Hidden;
            BtnArchivoVP.Visibility = Visibility.Hidden;

            TxtFileVpPath.Text = String.Empty;
        }

        private void RadSiCopia_Checked(object sender, RoutedEventArgs e)
        {
            LblCopiaCertif.Visibility = Visibility.Visible;
            TxtFileCopiaPath.Visibility = Visibility.Visible;
            BtnArchivoCC.Visibility = Visibility.Visible;
        }

        private void RadNoCopia_Checked(object sender, RoutedEventArgs e)
        {
            LblCopiaCertif.Visibility = Visibility.Hidden;
            TxtFileCopiaPath.Visibility = Visibility.Hidden;
            BtnArchivoCC.Visibility = Visibility.Hidden;

            TxtFileCopiaPath.Text = String.Empty;
        }

        private void SetInitialSettingsAfterLoad()
        {
            RadNoPublica.IsChecked = true;
            RadNoCopia.IsChecked = true;
            RadNoCambio.IsChecked = true;
        }

        private void TxtExpNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringFunctions.IsADigit(e.Text);
        }

        private void TxtRelTesis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringFunctions.IsADigit(e.Text);
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

            if (contradiccion.MiTesis.Tatj == 1)
                RadJuris.IsChecked = true;
            else
                RadAislada.IsChecked = true;

            if (contradiccion.MiTesis.VersionPublica == 1)
                RadSiPublica.IsChecked = true;
            else
                RadNoPublica.IsChecked = true;

            if (contradiccion.MiTesis.CopiaCertificada == 1)
                RadSiCopia.IsChecked = true;
            else
                RadNoCopia.IsChecked = true;

            if (contradiccion.MiTesis.CambioCriterio == 1)
                RadSiCambio.IsChecked = true;
            else
                RadNoCambio.IsChecked = true;
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
    }
}