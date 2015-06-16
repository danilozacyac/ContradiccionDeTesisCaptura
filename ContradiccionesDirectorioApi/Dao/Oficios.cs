using System;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Oficios
    {
        private int idOficio;
        private int idContradiccion;
        private string oficio;
        private DateTime? fechaOficio;

        public int IdOficio
        {
            get
            {
                return this.idOficio;
            }
            set
            {
                this.idOficio = value;
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

        public DateTime? FechaOficio
        {
            get
            {
                return this.fechaOficio;
            }
            set
            {
                this.fechaOficio = value;
            }
        }
    }
}
