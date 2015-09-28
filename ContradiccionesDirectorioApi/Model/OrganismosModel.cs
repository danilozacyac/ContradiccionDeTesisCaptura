using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class OrganismosModel
    {


        /// <summary>
        /// Enlista los Tribunales Colegiados, Tribunales Unitarios o Juzgados de Distrito según sea el caso
        /// </summary>
        /// <param name="tipoOrganismo"></param>
        /// <returns></returns>
        public List<Organismos> GetOrganismos(int tipoOrganismo)
        {
            List<Organismos> organismos = new List<Organismos>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Directorio"].ToString());
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT O.*, C.Ciudad, E.Abrev " +
                               "FROM Organismos O INNER JOIN (Ciudades C INNER JOIN Estados E ON C.IdEstado = E.IdEstado) ON O.Ciudad = C.IdCiudad WHERE TpoOrg = " + tipoOrganismo + " ORDER BY OrdenImpr";

            try
            {
                connection.Open();

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //int age = reader["Age"] as int? ?? -1;
                        Organismos organismoAdd = new Organismos();
                        organismoAdd.IdOrganismo = reader["IdOrg"] as int? ?? -1;
                        organismoAdd.TipoOrganismo = reader["TpoOrg"] as int? ?? -1;
                        organismoAdd.Circuito = reader["Circuito"] as int? ?? -1;
                        organismoAdd.Ordinal = reader["Ordinal"] as int? ?? -1;
                        organismoAdd.Materia = reader["Materia"] as int? ?? -1;
                        organismoAdd.Organismo = reader["Organismo"].ToString();
                        organismoAdd.Direccion = reader["Direccion"].ToString();
                        organismoAdd.Telefonos = reader["Tels"].ToString();
                        organismoAdd.Ciudad = reader["O.Ciudad"] as int? ?? -1;
                        organismoAdd.Integrantes = reader["Integrantes"] as int? ?? -1;
                        organismoAdd.OrdenImpresion = reader["OrdenImpr"] as int? ?? -1;
                       // organismoAdd.ListaFuncionarios = new ObservableCollection<Funcionarios>();

                        //foreach (Funcionarios func in new FuncionariosModel().GetFuncionariosPorOrganismo(organismoAdd.IdOrganismo))
                        //    organismoAdd.ListaFuncionarios.Add(func);

                        //organismoAdd.Integrantes = organismoAdd.ListaFuncionarios.Count;

                        organismos.Add(organismoAdd);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return organismos;
        }


        public ObservableCollection<Organismos> GetPlenos()
        {
            ObservableCollection<Organismos> organismos = new ObservableCollection<Organismos>();

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT * FROM PlenoC Order By OrdenImpr";

            try
            {
                connection.Open();

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //int age = reader["Age"] as int? ?? -1;
                        Organismos organismoAdd = new Organismos();
                        organismoAdd.IdOrganismo = reader["IdPleno"] as int? ?? -1;
                        organismoAdd.Organismo = reader["Descripcion"].ToString() + "(" + reader["Especializacion"].ToString() + ")";
                        organismoAdd.OrdenImpresion = reader["OrdenImpr"] as int? ?? -1;

                        organismos.Add(organismoAdd);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return organismos;
        }

        public void SetNewPleno(Organismos organismo)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                organismo.IdOrganismo = DataBaseUtilities.GetNextIdForUse("PlenoC", "IdPleno", connection);

                string sqlCadena = "SELECT * FROM PlenoC WHERE IdPleno = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "PlenoC");

                dr = dataSet.Tables["PlenoC"].NewRow();
                dr["Descripcion"] = organismo.Organismo;
                dr["Especializacion"] = organismo.Especialidad;

                dataSet.Tables["PlenoC"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO PlenoC(Descripcion,Especializacion)" +
                                                        " VALUES(@Descripcion,@Especializacion)";

                dataAdapter.InsertCommand.Parameters.Add("@Descripcion", OleDbType.VarChar, 0, "Descripcion");
                dataAdapter.InsertCommand.Parameters.Add("@Especializacion", OleDbType.VarChar, 0, "Especializacion");

                dataAdapter.Update(dataSet, "PlenoC");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }

        public void UpdatePleno(Organismos organismo)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {

                string sqlCadena = "SELECT * FROM PlenoC WHERE IdPleno = " + organismo.IdOrganismo;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "PlenoC");

                dr = dataSet.Tables["PlenoC"].Rows[0];
                dr.BeginEdit();
                dr["Descripcion"] = organismo.Organismo;
                dr["Especializacion"] = organismo.Especialidad;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE PlenoC SET Descripcion = @Descripcion, Especializacion = @Especializacion WHERE IdPleno = @IdPleno";

                dataAdapter.UpdateCommand.Parameters.Add("@Descripcion", OleDbType.VarChar, 0, "Descripcion");
                dataAdapter.UpdateCommand.Parameters.Add("@Especializacion", OleDbType.VarChar, 0, "Especializacion");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPleno", OleDbType.Numeric, 0, "IdPleno");

                dataAdapter.Update(dataSet, "PlenoC");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
