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

            OleDbConnection connection = new OleDbConnection(bdStringSql);

            return connection;
        }


        
    }
}
