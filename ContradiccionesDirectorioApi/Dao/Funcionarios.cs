using System;
using System.Linq;

namespace ContradiccionesDirectorioApi.Dao
{
    public class Funcionarios
    {
        private bool isSelected;
        private int idFuncionario;
        private int idOrganismo;
        private String puesto;
        private String apellidos;
        private String nombre;
        private String nombreCompleto;
        private int activo;
        /// <summary>
        /// Fecha a partir de la cual entra en funciones
        /// </summary>
        private String texto;



        public string NombreCompleto
        {
            get
            {
                return this.nombreCompleto;
            }
            set
            {
                this.nombreCompleto = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }

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

        public int IdOrganismo
        {
            get
            {
                return this.idOrganismo;
            }
            set
            {
                this.idOrganismo = value;
            }
        }

        public String Puesto
        {
            get
            {
                return this.puesto;
            }
            set
            {
                this.puesto = value;
            }
        }

        public String Apellidos
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

        public String Nombre
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

        public int Activo
        {
            get
            {
                return this.activo;
            }
            set
            {
                this.activo = value;
            }
        }

        public String Texto
        {
            get
            {
                return this.texto;
            }
            set
            {
                this.texto = value;
            }
        }

    }
}
