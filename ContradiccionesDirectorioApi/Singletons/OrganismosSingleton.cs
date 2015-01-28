using System;
using System.Collections.Generic;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionesDirectorioApi.Singletons
{
    public class OrganismosSingleton
    {
        private static List<Organismos> colegiados;
        private static List<Organismos> unitarios;
        private static List<Organismos> juzgados;
        private static List<Organismos> plenos;

        private OrganismosSingleton()
        {
        }

        public static List<Organismos> Colegiados
        {
            get
            {
                if (colegiados == null)
                    colegiados = new OrganismosModel().GetOrganismos(1);

                return colegiados;
            }
        }

        public static List<Organismos> Unitarios
        {
            get
            {
                if (unitarios == null)
                    unitarios = new OrganismosModel().GetOrganismos(2);

                return unitarios;
            }
        }

        public static List<Organismos> Juzgados
        {
            get
            {
                if (juzgados == null)
                    juzgados = new OrganismosModel().GetOrganismos(3);

                return juzgados;
            }
        }

        public static List<Organismos> Plenos
        {
            get
            {
                if (plenos == null)
                    plenos = new OrganismosModel().GetPlenos();

                return plenos;
            }
        }
    }
}
