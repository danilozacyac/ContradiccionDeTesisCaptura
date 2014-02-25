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
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Singletons;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for Returnos.xaml
    /// </summary>
    public partial class Returnos : Window
    {
        private Contradicciones contradiccion;
        private ReturnosClass returno;

        public Returnos(Contradicciones contradiccion)
        {
            InitializeComponent();
            returno = new ReturnosClass();
            this.contradiccion = contradiccion;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = returno;

            CbxOrigen.DataContext = OrganismosSingleton.Colegiados;
            CbxDestino.DataContext = OrganismosSingleton.Colegiados;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            returno.IdOrganoOrigen = (Int32)CbxOrigen.SelectedValue;
            returno.IdOrganoDestino = (Int32)CbxDestino.SelectedValue;

            contradiccion.Returnos.Add(returno);
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
