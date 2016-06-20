using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class AccesoModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CT"].ConnectionString;

        public int ObtenerTipoUsuario()
        {
            int tipoUsuario = 2;

            string sSql;

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand cmd;
            SqlDataReader reader;


            try
            {
                connection.Open();

                sSql = "SELECT * FROM Usuarios WHERE usuario = @usuario";// AND Pass = @Pass";
                cmd = new SqlCommand(sSql, connection);
                cmd.Parameters.AddWithValue("@usuario", Environment.UserName);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tipoUsuario = Convert.ToInt16(reader["Grupo"]);
                }
                
            }
            catch (DbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }

            return tipoUsuario;
        }

    }
}
