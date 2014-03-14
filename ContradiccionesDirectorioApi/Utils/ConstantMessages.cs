using System;
using System.Linq;

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


        #region Mensajes Error

        public const String DeleteNoComplete = "No se pudo eliminar esta tesis correctamente. ¿Desea volver a intentarlo?";

        #endregion


        #region Mensajes Confirmacion

        public const String DeseaEliminar = "¿Estas seguro de eliminar el elemento seleccionado?";

        #endregion
    }
}
