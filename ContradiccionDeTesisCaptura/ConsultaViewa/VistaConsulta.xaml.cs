using ContradiccionesDirectorioApi.Dao;
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

        public VistaConsulta(ObservableCollection<Consulta> contradicciones)
        {
            InitializeComponent();
            this.contradicciones = contradicciones;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RGridContradicciones.DataContext = contradicciones;
        }
    }
}
