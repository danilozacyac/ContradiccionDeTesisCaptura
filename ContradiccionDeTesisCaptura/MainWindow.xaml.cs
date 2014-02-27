using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListadoDeContradicciones contradicciones;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contradicciones = new ListadoDeContradicciones();
            ContradiccionesModel conModel = new ContradiccionesModel();
            contradicciones.Listado = conModel.GetContradicciones();

            RGridContradicciones.DataContext = contradicciones.Listado;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            ContraInfoGral contra = new ContraInfoGral(contradicciones);
            contra.ShowDialog();
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
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

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            Contradicciones contra = (Contradicciones)RGridContradicciones.SelectedItem;
            ContradiccionesWin win = new ContradiccionesWin(contra,false);
            win.ShowDialog();
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
