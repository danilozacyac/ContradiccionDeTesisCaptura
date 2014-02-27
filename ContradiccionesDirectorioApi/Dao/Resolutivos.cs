using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Resolutivos : INotifyPropertyChanged
    {
        private ObservableCollection<PResolutivos> puntosResolutivos;
        private int regEjecutoria;
        private int regTesis;
        private String rubroTesis;

        public ObservableCollection<PResolutivos> PuntosResolutivos
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

    public class PResolutivos : INotifyPropertyChanged
    {
        private int idResolutivo;
        private String resolutivo;
        public int IdResolutivo
        {
            get
            {
                return this.idResolutivo;
            }
            set
            {
                this.idResolutivo = value;
                this.OnPropertyChanged("IdResolutivo");
            }
        }

        public string Resolutivo
        {
            get
            {
                return this.resolutivo;
            }
            set
            {
                this.resolutivo = value;
                this.OnPropertyChanged("Resolutivo");
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
