using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ContradiccionDeTesisCaptura.Report;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Utils;
using TableWordToDb;
using Telerik.Windows.Controls;
using System.Windows.Media.Imaging;

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
            RGridContradicciones.DataContext = contradicciones.Listado;
            this.ShowInTaskbar(this, "Contradicción de tesis");
        }

        public void ShowInTaskbar(RadWindow control, string title)
        {
            control.Show();
            var window = control.ParentOfType<Window>();
            window.ShowInTaskbar = true;
            window.Title = title;
            var uri = new Uri("pack://application:,,,/ContradiccionDeTesisCaptura;component/Resources/updownbar.ico");
            window.Icon = BitmapFrame.Create(uri);
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
                new ContradiccionesModel().GetContradiccionComplementInfo(ref contradiccion);
                contradiccion.IsUpdating = true;
                ContradiccionesWin contra = new ContradiccionesWin(contradiccion, true);
                contra.Owner = this;
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

            new ContradiccionesModel().GetContradiccionComplementInfo(ref contra);

            ContradiccionesWin win = new ContradiccionesWin(contra, false);
            win.Owner = this;
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

        private void RBtnCerrarContra_Click(object sender, RoutedEventArgs e)
        {
            if (selectedContradiction == null)
            {
                MessageBox.Show("Antes de continuar debes de seleccionar un elemento de la lista", "ATENCIÓN:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            selectedContradiction.IsComplete = true;

            new ContradiccionesModel().UpdateContradiccionStatus(selectedContradiction);
        }

        private void RBtnReabrirContra_Click(object sender, RoutedEventArgs e)
        {
            if (selectedContradiction == null)
            {
                MessageBox.Show("Antes de continuar debes de seleccionar un elemento de la lista", "ATENCIÓN:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            selectedContradiction.IsComplete = false;

            new ContradiccionesModel().UpdateContradiccionStatus(selectedContradiction);
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
