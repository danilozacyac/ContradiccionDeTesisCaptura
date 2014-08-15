﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for CapturaTesis.xaml
    /// </summary>
    public partial class CapturaTesis : Window
    {
        private Tesis tesis = null;
        private ObservableCollection<Tesis> listaTesis = null;
        private readonly int idContradiccion;
        private readonly bool isUpdating;

        public CapturaTesis()
        {
            InitializeComponent();
        }

        public CapturaTesis(Tesis tesis)
        {
            InitializeComponent();
            this.tesis = tesis;
            isUpdating = true;
        }

        public CapturaTesis(ObservableCollection<Tesis> listaTesis,int idContradiccion)
        {
            InitializeComponent();
            this.listaTesis = listaTesis;
            isUpdating = false;
            this.idContradiccion = idContradiccion;
        }

        private void WinTesis_Loaded(object sender, RoutedEventArgs e)
        {
            if (isUpdating) //Se va a actualizar
            {
                this.DataContext = tesis;
            }
            else if (!isUpdating)  //Se agregará una nueva tesis
            {
                tesis = new Tesis();
                tesis.IdContradiccion = idContradiccion;
                this.DataContext = tesis;
            }
            LoadNoBindings();
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


        private String OpenDialogForPath()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

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

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            tesis.VersionPublica = (RadSiPublica.IsChecked == true) ? 1 : 0;
            tesis.CopiaCertificada = (RadSiCopia.IsChecked == true) ? 1 : 0;
            tesis.CambioCriterio = (RadSiCambio.IsChecked == true) ? 1 : 0;
            tesis.Tatj = (RadJuris.IsChecked == true) ? 1 : 0;

            if (isUpdating)
            {
                new TesisModel().UpdateTesis(tesis);
            }
            else
            {
                new TesisModel().SetNewTesisPorContradiccion(tesis);
                listaTesis.Add(tesis);
            }

            this.Close();
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

        private void LoadNoBindings()
        {
            

            if (tesis.Tatj == 1)
                RadJuris.IsChecked = true;
            else
                RadAislada.IsChecked = true;

            if (tesis.VersionPublica == 1)
                RadSiPublica.IsChecked = true;
            else
                RadNoPublica.IsChecked = true;

            if (tesis.CopiaCertificada == 1)
                RadSiCopia.IsChecked = true;
            else
                RadNoCopia.IsChecked = true;

            if (tesis.CambioCriterio == 1)
                RadSiCambio.IsChecked = true;
            else
                RadNoCambio.IsChecked = true;
        }
    }
}