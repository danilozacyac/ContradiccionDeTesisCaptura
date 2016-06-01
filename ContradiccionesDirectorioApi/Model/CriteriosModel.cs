using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;
using ContradiccionesDirectorioApi.Singletons;

namespace ContradiccionesDirectorioApi.Model
{
    public class CriteriosModel
    {
        /// <summary>
        /// Guarda los criterios de una contradiccion cuando se esta capturando la misma 
        /// por primera vez
        /// </summary>
        /// <param name="contradiccion"></param>
        public void SetNewCriterios(Contradicciones contradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            int currentOrder = 1;

            try
            {
                foreach (Criterios criterio in contradiccion.Criterios)
                {
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Criterios WHERE IdCriterio = 0", connection);
                    criterio.Orden = currentOrder;

                    dataAdapter.Fill(dataSet, "Criterios");

                    dr = dataSet.Tables["Criterios"].NewRow();
                    dr["IdContradiccion"] = contradiccion.IdContradiccion;
                    dr["Orden"] = currentOrder;
                    dr["Criterio"] = criterio.Criterio;
                    dr["IdOrgano"] = criterio.IdOrgano;
                    dr["Observaciones"] = criterio.Observaciones;

                    dataSet.Tables["Criterios"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Criterios(IdContradiccion,Orden,Criterio,IdOrgano,Observaciones)" +
                                                            " VALUES(@IdContradiccion,@Orden,@Criterio,@IdOrgano,@Observaciones)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                    dataAdapter.InsertCommand.Parameters.Add("@Criterio", SqlDbType.VarChar, 0, "Criterio");
                    dataAdapter.InsertCommand.Parameters.Add("@IdOrgano", SqlDbType.Int, 0, "IdOrgano");
                    dataAdapter.InsertCommand.Parameters.Add("@Observaciones", SqlDbType.VarChar, 0, "Observaciones");

                    dataAdapter.Update(dataSet, "Criterios");

                    dataSet.Dispose();
                    dataAdapter.Dispose();

                    criterio.IdCriterio = this.GetLastCriterioId(contradiccion.IdContradiccion, currentOrder);

                    this.SetNewCriteriosTesis(criterio);
                    currentOrder++;
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterio"></param>
        /// <param name="idContradiccion"></param>
        public void SetNewCriterios(Criterios criterio, int idContradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                criterio.Orden = this.GetMaxOrderCriterio(idContradiccion);

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Criterios WHERE IdCriterio = 0", connection);

                dataAdapter.Fill(dataSet, "Criterios");

                dr = dataSet.Tables["Criterios"].NewRow();
                dr["IdContradiccion"] = idContradiccion;
                dr["Orden"] = criterio.Orden;
                dr["Criterio"] = criterio.Criterio;
                dr["IdOrgano"] = criterio.IdOrgano;
                dr["Observaciones"] = criterio.Observaciones;

                dataSet.Tables["Criterios"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Criterios(IdContradiccion,Orden,Criterio,IdOrgano,Observaciones)" +
                                                        " VALUES(@IdContradiccion,@Orden,@Criterio,@IdOrgano,@Observaciones)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                dataAdapter.InsertCommand.Parameters.Add("@Criterio", SqlDbType.VarChar, 0, "Criterio");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgano", SqlDbType.Int, 0, "IdOrgano");
                dataAdapter.InsertCommand.Parameters.Add("@Observaciones", SqlDbType.VarChar, 0, "Observaciones");

                dataAdapter.Update(dataSet, "Criterios");

                dataSet.Dispose();
                dataAdapter.Dispose();

                criterio.IdCriterio = this.GetLastCriterioId(idContradiccion, criterio.Orden);

                this.SetNewCriteriosTesis(criterio);
                //currentOrder++;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateCriterios(Criterios criterio)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Criterios WHERE IdCriterio =" + criterio.IdCriterio;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Criterios");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Criterio"] = criterio.Criterio;
                dr["IdOrgano"] = criterio.IdOrgano;
                dr["Observaciones"] = criterio.Observaciones;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                string sSql = "UPDATE Criterios SET Criterio = @Criterio, IdOrgano = @IdOrgano, Observaciones = @Observaciones " +
                              " WHERE IdCriterio = @IdCriterio";

                dataAdapter.UpdateCommand.CommandText = sSql;

                AddParms(dataAdapter.UpdateCommand, "Criterio", "IdOrgano", "Observaciones", "IdCriterio");

                dataAdapter.Update(dataSet, "Criterios");
                dataSet.Dispose();
                dataAdapter.Dispose();

                this.DeleteCriterioTesis(criterio);
                this.SetNewCriteriosTesis(criterio);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina el criterio seleccionado
        /// </summary>
        /// <param name="criterio"></param>
        public bool DeleteCriterio(Criterios criterio)
        {
            bool isDeleteComplete = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Criterios WHERE IdCriterio = @IdCriterio";
                cmd.Parameters.AddWithValue("@IdCriterio", criterio.IdCriterio);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM CriteriosTesis WHERE IdCriterio = @IdCriterio";
                cmd.Parameters.AddWithValue("@IdCriterio", criterio.IdCriterio);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }

        /// <summary>
        /// Elimina todos los criterios asociados a una contradicción
        /// </summary>
        /// <param name="contradiccion"></param>
        public bool DeleteCriterio(Contradicciones contradiccion)
        {
            bool isDeleteComplete = true;

            foreach (Criterios criterio in contradiccion.Criterios)
            {
                isDeleteComplete = this.DeleteCriterio(criterio);
                if (!isDeleteComplete)
                    break;
            }

            return isDeleteComplete;
        }

        
        private void SetNewCriteriosTesis(Criterios criterio)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                if (criterio.TesisContendientes != null)
                {
                    foreach (int tesis in criterio.TesisContendientes)
                    {
                        dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM CriteriosTesis WHERE IdCriterio = 0", connection);

                        dataAdapter.Fill(dataSet, "Criterios");

                        dr = dataSet.Tables["Criterios"].NewRow();
                        dr["IdCriterio"] = criterio.IdCriterio;
                        dr["IUS"] = tesis;

                        dataSet.Tables["Criterios"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connection.CreateCommand();
                        dataAdapter.InsertCommand.CommandText = "INSERT INTO CriteriosTesis(IdCriterio,IUS)" +
                                                                " VALUES(@IdCriterio,@IUS)";

                        dataAdapter.InsertCommand.Parameters.Add("@IdCriterio", SqlDbType.Int, 0, "IdCriterio");
                        dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.Int, 0, "IUS");

                        dataAdapter.Update(dataSet, "Criterios");

                        dataSet.Dispose();
                        dataAdapter.Dispose();
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }


        public bool DeleteCriterioTesis(Criterios criterio)
        {
            bool isDeleteComplete = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM CriteriosTesis WHERE IdCriterio = @IdCriterio";
                cmd.Parameters.AddWithValue("@IdCriterio", criterio.IdCriterio);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }

        private int GetMaxOrderCriterio(int idContradiccion)
        {
            int maxOrden = 0;

            string sqlCmd = @"SELECT Max(Orden) AS Orden FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion ";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.Connection = connection;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                connection.Open();

                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    if (reader["Orden"] != DBNull.Value)
                        maxOrden = Convert.ToInt32(reader["Orden"]);
                    else
                        maxOrden = 0;
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return maxOrden + 1;
        }

        private int GetLastCriterioId(int idContradiccion, int maxOrden)
        {
            int lastId = 0;

            string sqlCmd = @"SELECT IdCriterio FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion AND Orden = @Orden";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand() { Connection = connection, CommandText = sqlCmd, CommandType = CommandType.Text };

            try
            {
                connection.Open();

                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                cmd.Parameters.AddWithValue("@Orden", maxOrden);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lastId = Convert.ToInt32(reader["IdCriterio"]);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return lastId;
        }

        public ObservableCollection<Criterios> GetCriterios(int idContradiccion)
        {
            ObservableCollection<Criterios> criterios = new ObservableCollection<Criterios>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            const string SqlQuery = "SELECT * FROM Criterios WHERE idContradiccion = @IdContradiccion";

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Criterios criterio = new Criterios()
                    {
                        IdContradiccion = Convert.ToInt32(reader["idContradiccion"]),
                        IdCriterio = Convert.ToInt32(reader["IdCriterio"]),
                        Orden =
                            Convert.ToInt32(reader["Orden"]),
                        Criterio = reader["Criterio"].ToString(),
                        IdOrgano = Convert.ToInt32(reader["IdOrgano"]),
                        Observaciones = reader["Observaciones"].ToString()
                    };

                    if (criterio.IdOrgano != -1000)
                        criterio.Organo = (from n in OrganismosSingleton.Colegiados
                                           where n.IdOrganismo == criterio.IdOrgano
                                           select n.Organismo).ToList()[0];

                    criterio.TesisContendientes = this.GetCriteriosTesis(criterio.IdCriterio);

                    criterios.Add(criterio);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return criterios;
        }


        public ObservableCollection<int> GetCriteriosTesis(int idCriterio)
        {
            ObservableCollection<int> tesisRelacionadas = new ObservableCollection<int>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM CriteriosTesis WHERE IdCriterio = @IdCriterio", connection);
                cmd.Parameters.AddWithValue("@IdCriterio", idCriterio);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tesisRelacionadas.Add(Convert.ToInt32(reader["IUS"]));
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,CriteriosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return tesisRelacionadas;
        }


        private void AddParms(SqlCommand cmd, params string[] cols)
        {
            // Add each parameter. Note that each colum in
            // table "Customers" is of type VARCHAR !
            foreach (String column in cols)
            {
                cmd.Parameters.Add("@" + column, SqlDbType.Char, 0, column);
            }
        }
    }
}