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
            contradiccion.Criterios = new List<Criterios>();
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

            ContradiccionesWin win = new ContradiccionesWin(contradiccion);
            win.Show();

        }
    }
}
