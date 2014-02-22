﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Ejecutoria : INotifyPropertyChanged
    {
        private List<int> tesisRelacionadas;
        private List<int> votosRelacionados;
        private DateTime? fechaResolucion;
        private DateTime? fechaEngrose;
        private String sise;
        private String responsable;
        private String signatario;
        private String oficioRespuestaEj;
        private String fileEjecPath;

        public List<int> TesisRelacionadas
        {
            get
            {
                return this.tesisRelacionadas;
            }
            set
            {
                this.tesisRelacionadas = value;
                this.OnPropertyChanged("TesisRelacionadas");
            }
        }

        public void Agregatesis(int ius)
        {
            if (tesisRelacionadas == null)
                tesisRelacionadas = new List<int>();

            tesisRelacionadas.Add(ius);
        }

        public List<int> VotosRelacionados
        {
            get
            {
                return this.votosRelacionados;
            }
            set
            {
                this.votosRelacionados = value;
                this.OnPropertyChanged("VotosRelacionados");
            }
        }

        public void AgregaVoto(int ius)
        {
            if (votosRelacionados == null)
                votosRelacionados = new List<int>();
        }    

        public DateTime? FechaResolucion
        {
            get
            {
                return this.fechaResolucion;
            }
            set
            {
                this.fechaResolucion = value;
                this.OnPropertyChanged("FechaResolucion");
            }
        }

        public DateTime? FechaEngrose
        {
            get
            {
                return this.fechaEngrose;
            }
            set
            {
                this.fechaEngrose = value;
                this.OnPropertyChanged("FechaEngrose");
            }
        }

        public string Sise
        {
            get
            {
                return this.sise;
            }
            set
            {
                this.sise = value;
                this.OnPropertyChanged("Sise");
            }
        }

        public string Responsable
        {
            get
            {
                return this.responsable;
            }
            set
            {
                this.responsable = value;
                this.OnPropertyChanged("Responsable");
            }
        }

        public string Signatario
        {
            get
            {
                return this.signatario;
            }
            set
            {
                this.signatario = value;
                this.OnPropertyChanged("Signatario");
            }
        }

        public string OficioRespuestaEj
        {
            get
            {
                return this.oficioRespuestaEj;
            }
            set
            {
                this.oficioRespuestaEj = value;
                this.OnPropertyChanged("OficioRespuestaEj");
            }
        }

        public string FileEjecPath
        {
            get
            {
                return this.fileEjecPath;
            }
            set
            {
                this.fileEjecPath = value;
                this.OnPropertyChanged("FileEjecPath");
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
