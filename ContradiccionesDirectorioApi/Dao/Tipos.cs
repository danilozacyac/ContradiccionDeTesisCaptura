using System;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Tipos
    {
        private int idTipo;
        private String descripcion;

        
        public Tipos(int idTipo, String descripcion)
        {
            this.idTipo = idTipo;
            this.descripcion = descripcion;
        }

        public int IdTipo
        {
            get
            {
                return this.idTipo;
            }
            set
            {
                this.idTipo = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }
    }
}
