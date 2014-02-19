using System;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Funcionarios
    {
        private int idFuncionario;
        private int idCargo;
        private int idOrgano;
        private String apellidos;
        private String nombre;
        private String abrevCargo;

        public int IdFuncionario
        {
            get
            {
                return this.idFuncionario;
            }
            set
            {
                this.idFuncionario = value;
            }
        }

        public int IdCargo
        {
            get
            {
                return this.idCargo;
            }
            set
            {
                this.idCargo = value;
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

        public string Apellidos
        {
            get
            {
                return this.apellidos;
            }
            set
            {
                this.apellidos = value;
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public string AbrevCargo
        {
            get
            {
                return this.abrevCargo;
            }
            set
            {
                this.abrevCargo = value;
            }
        }
    }
}
