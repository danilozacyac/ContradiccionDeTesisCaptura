using System;
using System.Linq;

namespace TableWordToDb
{
    public class TableInfo
    {
        private string plenos;
        private string asunto;
        private string denunciante;
        private string oficio;
        private string acuerdoad;
        private string orgContencientes;
        private string criterios;
        private string tema;
        private string fechaResolucion;
        private string resolutivos;
        private string tesis;
        private string observaciones;

        

        public string Plenos
        {
            get
            {
                return this.plenos;
            }
            set
            {
                this.plenos = value;
            }
        }

        public string Asunto
        {
            get
            {
                return this.asunto;
            }
            set
            {
                this.asunto = value;
            }
        }

        public string Denunciante
        {
            get
            {
                return this.denunciante;
            }
            set
            {
                this.denunciante = value;
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
            }
        }

        public string Acuerdoad
        {
            get
            {
                return this.acuerdoad;
            }
            set
            {
                this.acuerdoad = value;
            }
        }

        public string OrgContencientes
        {
            get
            {
                return this.orgContencientes;
            }
            set
            {
                this.orgContencientes = value;
            }
        }

        public string Criterios
        {
            get
            {
                return this.criterios;
            }
            set
            {
                this.criterios = value;
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
            }
        }

        public string FechaResolucion
        {
            get
            {
                return this.fechaResolucion;
            }
            set
            {
                this.fechaResolucion = value;
            }
        }

        public string Resolutivos
        {
            get
            {
                return this.resolutivos;
            }
            set
            {
                this.resolutivos = value;
            }
        }

        public string Tesis
        {
            get
            {
                return this.tesis;
            }
            set
            {
                this.tesis = value;
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
            }
        }
    }
}
