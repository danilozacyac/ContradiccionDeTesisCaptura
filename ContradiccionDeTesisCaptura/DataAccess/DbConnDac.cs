using System;
using System.Data.OleDb;
using System.Linq;

namespace ContradiccionDeTesisCaptura.DataAccess
{
    public class DbConnDac
    {
        public static OleDbConnection GetConnection()
        {
            OleDbConnection oleConne = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=CT.accdb;Persist Security Info=False;");

            return oleConne;
        }

    }
}
