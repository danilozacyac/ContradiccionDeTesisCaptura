using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ScjnUtilities;
using System.Data.Sql;

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

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Directorio"].ToString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            String sqlCadena = "SELECT F.*, R.IdOrganismo FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFuncionario = R.IdFuncionario  ORDER BY Apellidos";

            if (tipoOrganismo == 0)
                sqlCadena = "SELECT F.*, R.IdOrganismo FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFuncionario = R.IdFuncionario  ORDER BY Apellidos";
            else if (tipoOrganismo == 1 || tipoOrganismo == 2)
                sqlCadena = "SELECT F.*, R.IdOrganismo FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFuncionario = R.IdFuncionario WHERE Puesto = 'Mgdo.' OR Puesto = 'Mgda.' ORDER BY Nombre";
            else if (tipoOrganismo == 3)
                sqlCadena = "SELECT F.*, R.IdOrganismo FROM Funcionarios F LEFT JOIN Rel_Org_Func R ON F.IdFuncionario = R.IdFuncionario WHERE Puesto = 'Juez' ORDER BY Apellidos";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Funcionarios funcionario = new Funcionarios()
                        {
                            IdFuncionario = Convert.ToInt32(reader["IdFuncionario"]),
                            IdOrganismo = reader["IdOrganismo"] as int? ?? 0,
                            Puesto = reader["Puesto"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            NombreCompleto = String.Format("{0} {1}", reader["Nombre"], reader["Apellidos"]),
                            Texto = reader["Texto"].ToString()
                        };

                        funcionarios.Add(funcionario);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
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