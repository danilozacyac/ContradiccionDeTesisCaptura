using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;

namespace ContradiccionDeTesisCaptura.ConsultaViewa
{
    /// <summary>
    /// Interaction logic for VistaConsulta.xaml
    /// </summary>
    public partial class VistaConsulta
    {
        private ObservableCollection<Consulta> contradicciones;
        private Consulta selectedConsulta;
        private Contradicciones selectedContradiccion;

        public VistaConsulta(ObservableCollection<Consulta> contradicciones)
        {
            InitializeComponent();
            this.contradicciones = contradicciones;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RGridContradicciones.DataContext = contradicciones;
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


        private void RGridContradicciones_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedConsulta = RGridContradicciones.SelectedItem as Consulta;

            selectedContradiccion = new ContradiccionesModel().GetContradicciones(selectedConsulta.Id);
        }

        private void RBtnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            ContradiccionesWin win = new ContradiccionesWin(selectedContradiccion, false);
            win.Owner = this;
            win.ShowDialog();
        }

        private void RbtnLimpiarF_Click(object sender, RoutedEventArgs e)
        {
            this.RGridContradicciones.FilterDescriptors.SuspendNotifications();
            foreach (Telerik.Windows.Controls.GridViewColumn column in this.RGridContradicciones.Columns)
            {
                column.ClearFilters();
            }
            this.RGridContradicciones.FilterDescriptors.ResumeNotifications();
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                //tempString = StringUtilities.PrepareToAlphabeticalOrder(tempString);

                var temporal = (from n in contradicciones
                                where n.Expediente.Contains(tempString) || n.Pleno.Contains(tempString) || n.Tema.Contains(tempString)
                                select n).ToList();
                RGridContradicciones.DataContext = temporal;
            }
            else
            {
                RGridContradicciones.DataContext = contradicciones;
            }
        }

       
    }
}
