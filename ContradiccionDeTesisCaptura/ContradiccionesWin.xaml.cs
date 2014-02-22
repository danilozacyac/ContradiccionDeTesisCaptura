using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContradiccionesDirectorioApi.Model;
using ContradiccionDeTesisCaptura.DataAccess;
using ContradiccionesDirectorioApi.Dao;
using System.Collections.ObjectModel;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for ContradiccionesWin.xaml
    /// </summary>
    public partial class ContradiccionesWin : Window
    {
        private Contradicciones contradiccion;

        public ContradiccionesWin()
        {
            InitializeComponent();
            contradiccion = new Contradicciones();
            contradiccion.Criterios = new ObservableCollection<Criterios>();
        }

        public ContradiccionesWin(Contradicciones contradiccion)
        {
            InitializeComponent();
            this.contradiccion = contradiccion;

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxTiposAsuntos.DataContext = new TiposModel().GetTiposAsunto(DbConnDac.GetConnection());

            this.DataContext = contradiccion;
        }

        private void BtnAddCriterio_Click(object sender, RoutedEventArgs e)
        {
            CriteriosWin criterios = new CriteriosWin(contradiccion,null);
            criterios.ShowDialog();

            this.DataContext = contradiccion;


        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ContradiccionesModel contra = new ContradiccionesModel();
            contradiccion.IdContradiccion = contra.SetNewContradiccion(contradiccion);
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

        

       
    }
}
