using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Resolutivos : INotifyPropertyChanged
    {
        private ObservableCollection<String> puntosResolutivos;
        private int regEjecutoria;
        private int regTesis;
        private String rubroTesis;

        public ObservableCollection<string> PuntosResolutivos
        {
            get
            {
                return this.puntosResolutivos;
            }
            set
            {
                this.puntosResolutivos = value;
                this.OnPropertyChanged("PuntosResolutivos");
            }
        }

        public int RegEjecutoria
        {
            get
            {
                return this.regEjecutoria;
            }
            set
            {
                this.regEjecutoria = value;
                this.OnPropertyChanged("RegEjecutoria");
            }
        }

        public int RegTesis
        {
            get
            {
                return this.regTesis;
            }
            set
            {
                this.regTesis = value;
                this.OnPropertyChanged("RegTesis");
            }
        }

        public string RubroTesis
        {
            get
            {
                return this.rubroTesis;
            }
            set
            {
                this.rubroTesis = value;
                this.OnPropertyChanged("RubroTesis");
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
