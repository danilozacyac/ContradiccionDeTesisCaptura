using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Consulta
    {
        private int id;
        private string expediente;
        private string pleno;
        private string tema;
        private int estado;
        private DateTime? fechaResolucion;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Expediente
        {
            get
            {
                return this.expediente;
            }
            set
            {
                this.expediente = value;
            }
        }

        public string Pleno
        {
            get
            {
                return this.pleno;
            }
            set
            {
                this.pleno = value;
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

        public int Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                this.estado = value;
            }
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
            }
        }
    }
}
