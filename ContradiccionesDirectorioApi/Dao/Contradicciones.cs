using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Contradicciones : INotifyPropertyChanged
    {
        private int idContradiccion;
        private int expedienteNumero;
        private int expedienteAnio;
        private int idTipoAsunto;
        private String tema;
        private int status;
        private String oficio;
        private DateTime? fechaTurno;
        private ObservableCollection<Criterios> criterios;
        private String observaciones;
        private String denunciantes;
        private int idPlenoCircuito;
        private int idPresidentePleno;
        private int idPonentePleno;
        private Tesis miTesis;
        private Ejecutoria miEjecutoria;

        
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

        public int ExpedienteNumero
        {
            get
            {
                return this.expedienteNumero;
            }
            set
            {
                this.expedienteNumero = value;
            }
        }

        public int ExpedienteAnio
        {
            get
            {
                return this.expedienteAnio;
            }
            set
            {
                this.expedienteAnio = value;
            }
        }

        public int IdTipoAsunto
        {
            get
            {
                return this.idTipoAsunto;
            }
            set
            {
                this.idTipoAsunto = value;
                this.OnPropertyChanged("IdTipoAsunto");
            }
        }

        public string Tema
        {
            get
            {
                return this.tema;
            }
            set
            {
                this.tema = value;
                this.OnPropertyChanged("Tema");
            }
        }

        public int Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public string Oficio
        {
            get
            {
                return this.oficio;
            }
            set
            {
                this.oficio = value;
                this.OnPropertyChanged("Oficio");
            }
        }

        public DateTime? FechaTurno
        {
            get
            {
                return this.fechaTurno;
            }
            set
            {
                this.fechaTurno = value;
                this.OnPropertyChanged("FechaTurno");
            }
        }

        public ObservableCollection<Criterios> Criterios
        {
            get
            {
                return this.criterios;
            }
            set
            {
                this.criterios = value;
                this.OnPropertyChanged("Criterios");
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

        public string Denunciantes
        {
            get
            {
                return this.denunciantes;
            }
            set
            {
                this.denunciantes = value;
                this.OnPropertyChanged("Denunciantes");
            }
        }

        public int IdPlenoCircuito
        {
            get
            {
                return this.idPlenoCircuito;
            }
            set
            {
                this.idPlenoCircuito = value;
                this.OnPropertyChanged("IdPlenoCircuito");
            }
        }

        public int IdPresidentePleno
        {
            get
            {
                return this.idPresidentePleno;
            }
            set
            {
                this.idPresidentePleno = value;
                this.OnPropertyChanged("IdPresidentePleno");
            }
        }

        public int IdPonentePleno
        {
            get
            {
                return this.idPonentePleno;
            }
            set
            {
                this.idPonentePleno = value;
                this.OnPropertyChanged("IdPonentePleno");
            }
        }


        public Tesis MiTesis
        {
            get
            {
                return this.miTesis;
            }
            set
            {
                this.miTesis = value;
                this.OnPropertyChanged("MiTesis");
            }
        }

        public Ejecutoria MiEjecutoria
        {
            get
            {
                return this.miEjecutoria;
            }
            set
            {
                this.miEjecutoria = value;
                this.OnPropertyChanged("MiEjecutoria");
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
