using System;
using System.Data.OleDb;
using System.Linq;
using System.Configuration;

namespace ContradiccionDeTesisCaptura.DataAccess
{
    public class DbConnDac
    {
        public static OleDbConnection GetConnection()
        {
             String bdStringSql = ConfigurationManager.ConnectionStrings["CT"].ConnectionString;

            OleDbConnection connection = new OleDbConnection(bdStringSql);

            return connection;
        }


        //public static OleDbConnection GetConnectionDirectorio()
        //{
        //    String bdStringSql = ConfigurationManager.ConnectionStrings["Directorio"].ConnectionString;

        //    OleDbConnection connection = new OleDbConnection(bdStringSql);

        //    return connection;
        //}

    }
}
