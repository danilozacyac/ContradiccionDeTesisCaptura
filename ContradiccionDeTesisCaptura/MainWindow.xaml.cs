using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Utils;
using ContradiccionDeTesisCaptura.Report;
using TableWordToDb;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private ListadoDeContradicciones contradicciones;
        private Contradicciones selectedContradiction;

        public MainWindow(ListadoDeContradicciones contradicciones)
        {
            InitializeComponent();
            this.contradicciones = contradicciones;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contradicciones = new ListadoDeContradicciones();
            //ContradiccionesModel conModel = new ContradiccionesModel();
            //contradicciones.Listado = conModel.GetContradicciones();

            RGridContradicciones.DataContext = contradicciones.Listado;
        }

        private void CheckCorrectdelete(bool isDeleteComplete)
        {
            if (!isDeleteComplete)
                throw new Exception();
        }

        private void RGridContradicciones_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedContradiction = RGridContradicciones.SelectedItem as Contradicciones;
        }

        private void RBtnAddContra_Click(object sender, RoutedEventArgs e)
        {
            ContraInfoGral contra = new ContraInfoGral(contradicciones);
            contra.ShowDialog();
        }

        private void RBtnEditContra_Click(object sender, RoutedEventArgs e)
        {
            if (RGridContradicciones.SelectedItem != null)
            {
                Contradicciones contradiccion = (Contradicciones)RGridContradicciones.SelectedItem;
                contradiccion.IsUpdating = true;
                ContradiccionesWin contra = new ContradiccionesWin(contradiccion, true);
                contra.ShowDialog();
                contradiccion.IsUpdating = false;
            }
            else
            {
                MessageBox.Show("Seleccione el elemento que desea modificar");
            }
        }

        private void RBtnViewContra_Click(object sender, RoutedEventArgs e)
        {
            Contradicciones contra = (Contradicciones)RGridContradicciones.SelectedItem;
            ContradiccionesWin win = new ContradiccionesWin(contra, false);
            win.ShowDialog();

        }

        private void RBtnDeleteViewContra_Click(object sender, RoutedEventArgs e)
        {
            if (selectedContradiction == null)
            {
                MessageBox.Show("Antes de continuar debes de seleccionar un elemento de la lista", "ATENCIÓN:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show(ConstantMessages.DeseaEliminar, "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (MessageBoxResult.Yes == result)
            {
                try
                {
                    this.CheckCorrectdelete(new TesisModel().DeleteTesis(selectedContradiction.MiTesis));
                    this.CheckCorrectdelete(new ReturnosModel().DeleteReturno(selectedContradiction));
                    this.CheckCorrectdelete(new ResolucionModel().DeleteResolutivo(selectedContradiction));
                    this.CheckCorrectdelete(new ResolucionModel().DeleteResolucion(selectedContradiction));
                    this.CheckCorrectdelete(new EjecutoriasModel().DeleteEjecutoria(selectedContradiction));
                    this.CheckCorrectdelete(new CriteriosModel().DeleteCriterio(selectedContradiction));
                    this.CheckCorrectdelete(new ContradiccionesModel().DeleteContradiccion(selectedContradiction));

                    contradicciones.Listado.Remove(selectedContradiction);
                    selectedContradiction = null;
                }
                catch (Exception)
                {
                    result = MessageBox.Show(ConstantMessages.DeleteNoComplete, "ERROR:", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

                    if (MessageBoxResult.Yes == result)
                    {
                        RBtnDeleteViewContra_Click(null, null);
                    }
                }
            }
        }

        private void RBtnPdf_Click(object sender, RoutedEventArgs e)
        {
            ToPdfReport report = new ToPdfReport();
            report.CtToPdfReport(contradicciones);
        }

        private void RBtnImportarDatos_Click(object sender, RoutedEventArgs e)
        {
            WordInfoProcess merge = new WordInfoProcess();
            merge.GetListaContradiccionesFaltantes();
        }

        private void RBtnAddPleno_Click(object sender, RoutedEventArgs e)
        {
            PlenosCircuito add = new PlenosCircuito();
            add.ShowDialog();
            
        }

        private void RBtnUpdatePleno_Click(object sender, RoutedEventArgs e)
        {
            PlenosLista plenos = new PlenosLista();
            plenos.Owner = this;
            plenos.ShowDialog();
        }


    }

    public class ListadoDeContradicciones : INotifyPropertyChanged
    {
        private ObservableCollection<Contradicciones> listado;


        #region INotifyPropertyChanged Members

        public ObservableCollection<Contradicciones> Listado
        {
            get
            {
                return this.listado;
            }
            set
            {
                this.listado = value;
                this.OnPropertyChanged("Listado");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
