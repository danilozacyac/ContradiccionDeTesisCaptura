using System;
using System.Collections.Generic;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Criterios
    {
        private int idCriterio;
        private int idContradiccion;
        private int orden;
        private String criterio;
        private int idOrgano;
        private String organo;
        private List<int> tesisContendientes;
        private String tesisContendientesStr;
       
        public int IdCriterio
        {
            get
            {
                return this.idCriterio;
            }
            set
            {
                this.idCriterio = value;
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

        public int Orden
        {
            get
            {
                return this.orden;
            }
            set
            {
                this.orden = value;
            }
        }

        public string Criterio
        {
            get
            {
                return this.criterio;
            }
            set
            {
                this.criterio = value;
            }
        }

        public int IdOrgano
        {
            get
            {
                return this.idOrgano;
            }
            set
            {
                this.idOrgano = value;
            }
        }

        public string Organo
        {
            get
            {
                return this.organo;
            }
            set
            {
                this.organo = value;
            }
        }

        public List<int> TesisContendientes
        {
            get
            {
                return this.tesisContendientes;
            }
            set
            {
                this.tesisContendientes = value;
            }
        }

        public string TesisContendientesStr
        {
            get
            {
                return this.tesisContendientesStr;
            }
            set
            {
                this.tesisContendientesStr = value;
            }
        }
    }
}
