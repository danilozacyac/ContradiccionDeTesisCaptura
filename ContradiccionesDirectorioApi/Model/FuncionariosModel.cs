using System;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;

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

            OleDbConnection oleConne = DbConnDac.GetConnectionDirectorio();
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
                oleConne.Open();

                cmd = new OleDbCommand(sqlCadena, oleConne);
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
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
                oleConne.Close();
            }

            return funcionarios;
        }
    }
}