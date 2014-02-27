using System;
using System.Collections.Generic;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionesDirectorioApi.Singletons
{
    public class TipoAsuntoSingleton
    {
        private static List<Tipos> tipoAsunto;

        private TipoAsuntoSingleton() { }

        public static List<Tipos> TipoAsunto
        {
            get
            {
                if (tipoAsunto == null)
                    tipoAsunto = new TiposModel().GetTiposAsunto();

                return tipoAsunto;
            }
        }
    }
}
