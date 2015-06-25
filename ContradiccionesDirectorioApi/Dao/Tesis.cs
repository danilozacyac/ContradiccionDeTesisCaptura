using System;
using System.ComponentModel;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Tesis : INotifyPropertyChanged
    {
        private int idTesis;
        private int idContradiccion;
        private String claveControl;
        private String claveIdentificacion;
        private int ius;
        private String rubro;
        private int tatj;
        private String oficioPublicacion;
        private String oficioPublicacionFilePath;
        private int versionPublica;
        private String versionPublicaFilePath;
        private int copiaCertificada;
        private String copiaCertificadaFilePath;
        private String destinatario;
        private int cambioCriterio;
        private String responsable;
        private String oficioRespuesta;
        private String oficioRespuestaFilePath;

        public int Ius
        {
            get
            {
                return this.ius;
            }
            set
            {
                this.ius = value;
                this.OnPropertyChanged("Ius");
            }
        }

        public int IdTesis
        {
            get
            {
                return this.idTesis;
            }
            set
            {
                this.idTesis = value;
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
            }
        }

        public string ClaveControl
        {
            get
            {
                return this.claveControl;
            }
            set
            {
                this.claveControl = value;
                this.OnPropertyChanged("ClaveControl");
            }
        }

        public string ClaveIdentificacion
        {
            get
            {
                return this.claveIdentificacion;
            }
            set
            {
                this.claveIdentificacion = value;
                this.OnPropertyChanged("ClaveIdentificacion");
            }
        }

        public string Rubro
        {
            get
            {
                return this.rubro;
            }
            set
            {
                this.rubro = value;
                this.OnPropertyChanged("Rubro");
            }
        }

        public int Tatj
        {
            get
            {
                return this.tatj;
            }
            set
            {
                this.tatj = value;
                this.OnPropertyChanged("Tatj");
            }
        }

        public string OficioPublicacion
        {
            get
            {
                return this.oficioPublicacion;
            }
            set
            {
                this.oficioPublicacion = value;
                this.OnPropertyChanged("OficioPublicacion");
            }
        }

        public string OficioPublicacionFilePath
        {
            get
            {
                return this.oficioPublicacionFilePath;
            }
            set
            {
                this.oficioPublicacionFilePath = value;
                this.OnPropertyChanged("OficioPublicacionFilePath");
            }
        }

        public int VersionPublica
        {
            get
            {
                return this.versionPublica;
            }
            set
            {
                this.versionPublica = value;
                this.OnPropertyChanged("VersionPublica");
            }
        }

        public string VersionPublicaFilePath
        {
            get
            {
                return this.versionPublicaFilePath;
            }
            set
            {
                this.versionPublicaFilePath = value;
                this.OnPropertyChanged("VersionPublicaFilePath");
            }
        }

        public int CopiaCertificada
        {
            get
            {
                return this.copiaCertificada;
            }
            set
            {
                this.copiaCertificada = value;
                this.OnPropertyChanged("CopiaCertificada");
            }
        }

        public string CopiaCertificadaFilePath
        {
            get
            {
                return this.copiaCertificadaFilePath;
            }
            set
            {
                this.copiaCertificadaFilePath = value;
                this.OnPropertyChanged("CopiaCertificadaFilePath");
            }
        }

        public string Destinatario
        {
            get
            {
                return this.destinatario;
            }
            set
            {
                this.destinatario = value;
                this.OnPropertyChanged("Destinatario");
            }
        }

        public int CambioCriterio
        {
            get
            {
                return this.cambioCriterio;
            }
            set
            {
                this.cambioCriterio = value;
                this.OnPropertyChanged("CambioCriterio");
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

        public string OficioRespuesta
        {
            get
            {
                return this.oficioRespuesta;
            }
            set
            {
                this.oficioRespuesta = value;
                this.OnPropertyChanged("OficioRespuesta");
            }
        }

        public string OficioRespuestaFilePath
        {
            get
            {
                return this.oficioRespuestaFilePath;
            }
            set
            {
                this.oficioRespuestaFilePath = value;
                this.OnPropertyChanged("OficioRespuestaFilePath");
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
