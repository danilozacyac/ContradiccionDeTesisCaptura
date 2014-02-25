using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ContradiccionesDirectorioApi.Dao
{
    public class ReturnosClass : INotifyPropertyChanged
    {
        private int idContradiccion;
        private DateTime? fecha;
        private int idOrganoOrigen;
        private int idOrganoDestino;
        private String expOrigen;
        private String expDestino;

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

        public DateTime? Fecha
        {
            get
            {
                return this.fecha;
            }
            set
            {
                this.fecha = value;
                this.OnPropertyChanged("Fecha");
            }
        }

        public int IdOrganoOrigen
        {
            get
            {
                return this.idOrganoOrigen;
            }
            set
            {
                this.idOrganoOrigen = value;
                this.OnPropertyChanged("IdOrganoOrigen");
            }
        }

        public int IdOrganoDestino
        {
            get
            {
                return this.idOrganoDestino;
            }
            set
            {
                this.idOrganoDestino = value;
                this.OnPropertyChanged("IdOrganoDestino");
            }
        }

        public string ExpOrigen
        {
            get
            {
                return this.expOrigen;
            }
            set
            {
                this.expOrigen = value;
                this.OnPropertyChanged("ExpOrigen");
            }
        }

        public string ExpDestino
        {
            get
            {
                return this.expDestino;
            }
            set
            {
                this.expDestino = value;
                this.OnPropertyChanged("ExpDestino");
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
