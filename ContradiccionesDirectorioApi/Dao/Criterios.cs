﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Criterios : INotifyPropertyChanged
    {
        private bool isEnable;
        private int idCriterio;
        private int idContradiccion;
        private int orden;
        private String criterio;
        private int idOrgano;
        private String organo;
        private ObservableCollection<int> tesisContendientes;
        private String tesisContendientesStr;
        private string observaciones;

       
        public bool IsEnable
        {
            get
            {
                return this.isEnable;
            }
            set
            {
                this.isEnable = value;
            }
        }

        public int IdCriterio
        {
            get
            {
                return this.idCriterio;
            }
            set
            {
                this.idCriterio = value;
                this.OnPropertyChanged("IdCriterio");
            }
        }

        public int IdContradiccion
        {
            get
            {
                return this.idContradiccion;
            }
            set
            {
                this.idContradiccion = value;
                this.OnPropertyChanged("IdContradiccion");
            }
        }

        public int Orden
        {
            get
            {
                return this.orden;
            }
            set
            {
                this.orden = value;
                this.OnPropertyChanged("Orden");
            }
        }

        public string Criterio
        {
            get
            {
                return this.criterio;
            }
            set
            {
                this.criterio = value;
                this.OnPropertyChanged("Criterio");
            }
        }

        public int IdOrgano
        {
            get
            {
                return this.idOrgano;
            }
            set
            {
                this.idOrgano = value;
                this.OnPropertyChanged("IdOrgano");
            }
        }

        public string Organo
        {
            get
            {
                return this.organo;
            }
            set
            {
                this.organo = value;
                this.OnPropertyChanged("Organo");
            }
        }

        public ObservableCollection<int> TesisContendientes
        {
            get
            {
                return this.tesisContendientes;
            }
            set
            {
                this.tesisContendientes = value;
                this.OnPropertyChanged("TesisContendientes");
            }
        }

        public string TesisContendientesStr
        {
            get
            {
                return this.tesisContendientesStr;
            }
            set
            {
                this.tesisContendientesStr = value;
                this.OnPropertyChanged("TesisContendientesStr");
            }
        }

        public string Observaciones
        {
            get
            {
                return this.observaciones;
            }
            set
            {
                this.observaciones = value;
                this.OnPropertyChanged("Observaciones");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
