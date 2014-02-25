using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContradiccionesDirectorioApi.Utils
{
    public class ConstantMessages
    {
        #region Mensajes Validacion

        public const String SeleccionaEstadoExpediente = "Debe seleccionar el estado actual del expediente";
        public const String SeleccionaTipoDeTesis = "Debe selecciona si se trata de una tesis aislada o jurisprudencia";
        public const String SeleccionaFechaturno = "Antes de continuar selecciona la fecha en que fue turnado el asunto";
        public static String RangoAnual = "Ingrese un año válido, entre 1990 y " + DateTime.Now.Year + 2;

        #endregion

    }
}
