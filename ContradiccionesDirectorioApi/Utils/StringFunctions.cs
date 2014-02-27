using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContradiccionesDirectorioApi.Utils
{
    public class StringFunctions
    {

        /// <summary>
        /// Verifica si el texto ingresado es un dígito
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsADigit(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return regex.IsMatch(text);
        }

    }
}
