using System;
using System.Data.OleDb;
using System.Linq;
using System.Configuration;

namespace ContradiccionesDirectorioApi.DataAccess
{
    public class DbConnDac
    {

        public static OleDbConnection GetConnection()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["CT"].ConnectionString;

            OleDbConnection oleConne = new OleDbConnection(bdStringSql);

            return oleConne;
        }


        public static OleDbConnection GetConnectionDirectorio()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["Directorio"].ConnectionString;

            OleDbConnection oleConne = new OleDbConnection(bdStringSql);

            return oleConne;
        }
    }
}
