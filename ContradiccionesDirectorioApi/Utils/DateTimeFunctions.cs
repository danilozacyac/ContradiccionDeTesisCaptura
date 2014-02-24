﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContradiccionesDirectorioApi.Utils
{
    public class DateTimeFunctions
    {
        /// <summary>
        /// Devuelve la fecha en el formato yyyyMMdd
        /// </summary>
        /// <param name="myDate"></param>
        /// <returns></returns>
        public static long ConvertDateToInt(DateTime? myDate)
        {
            string day = (myDate.Value.Day.ToString().Length == 1) ? "0" + myDate.Value.Day : myDate.Value.Day.ToString();

            string month = (myDate.Value.Month.ToString().Length == 1) ? "0" + myDate.Value.Month : myDate.Value.Month.ToString();

            int year = myDate.Value.Year;

            return Convert.ToInt64(year + month + day);

        }

    }
}
