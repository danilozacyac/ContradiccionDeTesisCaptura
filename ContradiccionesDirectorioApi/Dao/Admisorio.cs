using System;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Admisorio
    {
        private int idAcuerdo;
        private int idContradiccion;
        private DateTime? fechaAcuerdo;
        private string acuerdo;
        
        public int IdAcuerdo
        {
            get
            {
                return this.idAcuerdo;
            }
            set
            {
                this.idAcuerdo = value;
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

        public DateTime? FechaAcuerdo
        {
            get
            {
                return this.fechaAcuerdo;
            }
            set
            {
                this.fechaAcuerdo = value;
            }
        }

        public string Acuerdo
        {
            get
            {
                return this.acuerdo;
            }
            set
            {
                this.acuerdo = value;
            }
        }
    }
}
