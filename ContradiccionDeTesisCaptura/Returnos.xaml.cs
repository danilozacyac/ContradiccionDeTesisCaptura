using System;
using System.Linq;
using System.Windows;
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
        private readonly bool isUpdatingReturno;

        public Returnos(Contradicciones contradiccion)
        {
            InitializeComponent();
            returno = new ReturnosClass();
            this.contradiccion = contradiccion;
            
        }

        public Returnos(Contradicciones contradiccion, ReturnosClass returno, bool isUpdatingReturno)
        {
            InitializeComponent();
            this.contradiccion = contradiccion;
            this.returno = returno;
            this.isUpdatingReturno = isUpdatingReturno;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = returno;

            CbxOrigen.DataContext = OrganismosSingleton.Colegiados;
            CbxDestino.DataContext = OrganismosSingleton.Colegiados;

            if (isUpdatingReturno)
            {
                CbxOrigen.SelectedValue = returno.IdOrganoOrigen;
                CbxDestino.SelectedValue = returno.IdOrganoDestino;
            }


        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            ReturnosModel model = new ReturnosModel();

            returno.IdOrganoOrigen = (Int32)CbxOrigen.SelectedValue;
            returno.IdOrganoDestino = (Int32)CbxDestino.SelectedValue;

            if (isUpdatingReturno)//Actualiza
            {
                model.UpdateReturno(returno);
            }
            else //Agrega nuevo returno
            {
                model.SetNewReturno(contradiccion, returno);
                contradiccion.Returnos.Add(returno);
            }

            
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
