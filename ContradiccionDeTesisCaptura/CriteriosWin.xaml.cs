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
using ContradiccionesDirectorioApi.Singletons;
using System.Text.RegularExpressions;
using MantesisVerIusCommonObjects.Model;
using MantesisVerIusCommonObjects.Dto;
using ContradiccionesDirectorioApi.Dao;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for CriteriosWin.xaml
    /// </summary>
    public partial class CriteriosWin : Window
    {
        private Contradicciones contradiccion;
        private Criterios criterios;


        public CriteriosWin(Contradicciones contradiccion,Criterios criterios)
        {

            InitializeComponent();
            this.contradiccion = contradiccion;
            if (criterios != null)
                this.criterios = criterios;
            else
            {
                this.criterios = new Criterios();
                this.criterios.TesisContendientes = new List<int>();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxOrganismos.DataContext = OrganismosSingleton.Colegiados;
            this.DataContext = criterios;
        }

        private void TxtTesis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return regex.IsMatch(text);
        }

        private void TxtNumIus_KeyDown(object sender, KeyEventArgs e)
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
                LstTesisContendientes.Items.Add(tesis.Ius);
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
            criterios.IdOrgano = (Int32)CbxOrganismos.SelectedValue;

            contradiccion.Criterios.Add(criterios);

            this.Close();
        }
    }
}
