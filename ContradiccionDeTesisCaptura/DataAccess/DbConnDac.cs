using System;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;

namespace ContradiccionDeTesisCaptura.DataAccess
{
    public class DbConnDac
    {
        public static SqlConnection GetConnection()
        {
             String bdStringSql = ConfigurationManager.ConnectionStrings["CT"].ConnectionString;

            SqlConnection connection = new SqlConnection(bdStringSql);

            return connection;
        }


        //public static SqlConnection GetConnectionDirectorio()
        //{
        //    String bdStringSql = ConfigurationManager.ConnectionStrings["Directorio"].ConnectionString;

        //    SqlConnection connection = new SqlConnection(bdStringSql);

        //    return connection;
        //}

    }
}
