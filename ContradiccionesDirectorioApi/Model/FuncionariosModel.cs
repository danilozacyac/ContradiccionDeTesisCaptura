using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class FuncionariosModel
    {

        /// <summary>
        /// Enlista los funcionarios de acuerdo al tipo de Organismo
        /// </summary>
        /// <param name="tipoOrganismo"></param>
        /// <returns></returns>
        public ObservableCollection<Funcionarios> GetFuncionarios(int tipoOrganismo)
        {
            ObservableCollection<Funcionarios> funcionarios = new ObservableCollection<Funcionarios>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Directorio"].ToString());
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT F.*, R.IdOrg FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFunc = R.IdFunc  ORDER BY Apellidos";

            if (tipoOrganismo == 0)
                sqlCadena = "SELECT F.*, R.IdOrg FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFunc = R.IdFunc  ORDER BY Apellidos";
            else if (tipoOrganismo == 1 || tipoOrganismo == 2)
                sqlCadena = "SELECT F.*, R.IdOrg FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFunc = R.IdFunc WHERE Puesto = 'Mgdo.' OR Puesto = 'Mgda.' ORDER BY Nombre";
            else if (tipoOrganismo == 3)
                sqlCadena = "SELECT F.*, R.IdOrg FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFunc = R.IdFunc WHERE Puesto = 'Juez' ORDER BY Apellidos";

            try
            {
                connection.Open();

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Funcionarios funcionario = new Funcionarios();
                        funcionario.IdFuncionario = Convert.ToInt32(reader["idFunc"]);
                        funcionario.IdOrganismo = reader["IdOrg"] as int? ?? 0;
                        funcionario.Puesto = reader["Puesto"].ToString();
                        funcionario.Apellidos = reader["Apellidos"].ToString();
                        funcionario.Nombre = reader["Nombre"].ToString();
                        funcionario.NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellidos"].ToString();
                        funcionario.Texto = reader["Texto"].ToString();

                        funcionarios.Add(funcionario);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,FuncionariosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,FuncionariosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return funcionarios;
        }
    }
}