﻿using System;
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
using ContradiccionesDirectorioApi.Utils;
using ContradiccionesDirectorioApi.Dao;
using System.Collections.ObjectModel;
using ContradiccionesDirectorioApi.Singletons;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionDeTesisCaptura
{
    /// <summary>
    /// Interaction logic for ContraInfoGral.xaml
    /// </summary>
    public partial class ContraInfoGral : Window
    {
        private Contradicciones contradiccion;
        private ListadoDeContradicciones listado;

        public ContraInfoGral(ListadoDeContradicciones listado)
        {
            InitializeComponent();
            this.contradiccion = new Contradicciones();
            this.listado = listado;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contradiccion.Criterios = new ObservableCollection<Criterios>();
            this.DataContext = contradiccion;
            CbxTiposAsuntos.DataContext = TipoAsuntoSingleton.TipoAsunto;
        }

        private void BtnAddCriterio_Click(object sender, RoutedEventArgs e)
        {
            CriteriosWin criterios = new CriteriosWin(contradiccion, null, false);
            criterios.ShowDialog();
        }

        private void BtnDelCriterio_Click(object sender, RoutedEventArgs e)
        {
            Criterios criterio = (Criterios)RGridCriterios.SelectedItem;
            contradiccion.Criterios.Remove(criterio);
            
        }

        private void TxtExpNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringFunctions.IsADigit(e.Text);
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (RadTramite.IsChecked == false && RadResuelto.IsChecked == false)
            {
                MessageBox.Show(ConstantMessages.SeleccionaEstadoExpediente);
                return;
            }

            if (contradiccion.ExpedienteAnio < 1990 && contradiccion.ExpedienteAnio > DateTime.Now.Year + 2)
            {
                MessageBox.Show(ConstantMessages.RangoAnual);
                return;
            }
            contradiccion.IdTipoAsunto = (Int32)CbxTiposAsuntos.SelectedValue;
            //contradiccion.IdPlenoCircuito = (Int32)CbxPlenos.SelectedValue;
            //contradiccion.IdPresidentePleno = (Int32)CbxPresidente.SelectedValue;
            //contradiccion.IdPonentePleno = (Int32)CbxPonente.SelectedValue;

            ContradiccionesModel contra = new ContradiccionesModel();
            CriteriosModel crit = new CriteriosModel();

            contradiccion.IdContradiccion = contra.SetNewContradiccion(contradiccion);
            crit.SetNewCriterios(contradiccion);

            listado.Listado.Add(contradiccion);
            this.Close();
        }
    }
}
