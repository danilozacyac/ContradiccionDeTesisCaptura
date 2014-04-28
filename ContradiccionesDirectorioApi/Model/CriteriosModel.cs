using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;

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
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            int currentOrder = 1;

            try
            {
                foreach (Criterios criterio in contradiccion.Criterios)
                {
                    string sqlCadena = "SELECT * FROM Criterios WHERE IdCriterio = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);
                    criterio.Orden = currentOrder;

                    dataAdapter.Fill(dataSet, "Criterios");

                    dr = dataSet.Tables["Criterios"].NewRow();
                    dr["IdContradiccion"] = contradiccion.IdContradiccion;
                    dr["Orden"] = currentOrder;
                    dr["Criterio"] = criterio.Criterio;
                    dr["IdOrgano"] = criterio.IdOrgano;
                    dr["Observaciones"] = criterio.Observaciones;

                    dataSet.Tables["Criterios"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Criterios(IdContradiccion,Orden,Criterio,IdOrgano,Observaciones)" +
                                                            " VALUES(@IdContradiccion,@Orden,@Criterio,@IdOrgano,@Observaciones)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@Orden", OleDbType.Numeric, 0, "Orden");
                    dataAdapter.InsertCommand.Parameters.Add("@Criterio", OleDbType.VarChar, 0, "Criterio");
                    dataAdapter.InsertCommand.Parameters.Add("@IdOrgano", OleDbType.Numeric, 0, "IdOrgano");
                    dataAdapter.InsertCommand.Parameters.Add("@Observaciones", OleDbType.VarChar, 0, "Observaciones");

                    dataAdapter.Update(dataSet, "Criterios");

                    dataSet.Dispose();
                    dataAdapter.Dispose();

                    criterio.IdCriterio = this.GetLastCriterioId(contradiccion.IdContradiccion, currentOrder);

                    this.SetNewCriteriosTesis(criterio);
                    currentOrder++;
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterio"></param>
        /// <param name="idContradiccion"></param>
        public void SetNewCriterios(Criterios criterio, int idContradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                criterio.Orden = this.GetMaxOrderCriterio(idContradiccion);
                string sqlCadena = "SELECT * FROM Criterios WHERE IdCriterio = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Criterios");

                dr = dataSet.Tables["Criterios"].NewRow();
                dr["IdContradiccion"] = idContradiccion;
                dr["Orden"] = criterio.Orden;
                dr["Criterio"] = criterio.Criterio;
                dr["IdOrgano"] = criterio.IdOrgano;
                dr["Observaciones"] = criterio.Observaciones;

                dataSet.Tables["Criterios"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Criterios(IdContradiccion,Orden,Criterio,IdOrgano,Observaciones)" +
                                                        " VALUES(@IdContradiccion,@Orden,@Criterio,@IdOrgano,@Observaciones)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Orden", OleDbType.Numeric, 0, "Orden");
                dataAdapter.InsertCommand.Parameters.Add("@Criterio", OleDbType.VarChar, 0, "Criterio");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgano", OleDbType.Numeric, 0, "IdOrgano");
                dataAdapter.InsertCommand.Parameters.Add("@Observaciones", OleDbType.VarChar, 0, "Observaciones");

                dataAdapter.Update(dataSet, "Criterios");

                dataSet.Dispose();
                dataAdapter.Dispose();

                criterio.IdCriterio = this.GetLastCriterioId(idContradiccion, criterio.Orden);

                this.SetNewCriteriosTesis(criterio);
                //currentOrder++;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }

        public void UpdateCriterios(Criterios criterio, int idContradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Criterios WHERE IdCriterio =" + criterio.IdCriterio;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Criterios");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Criterio"] = criterio.Criterio;
                dr["IdOrgano"] = criterio.IdOrgano;
                dr["Observaciones"] = criterio.Observaciones;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();

                string sSql = "UPDATE Criterios SET Criterio = @Criterio, IdOrgano = @IdOrgano, Observaciones = @Observaciones " +
                              " WHERE IdCriterio = @IdCriterio";

                dataAdapter.UpdateCommand.CommandText = sSql;

                AddParms(dataAdapter.UpdateCommand, "Criterio", "IdOrgano", "Observaciones", "IdCriterio");

                dataAdapter.Update(dataSet, "Criterios");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }

        /// <summary>
        /// Elimina el criterio seleccionado
        /// </summary>
        /// <param name="criterio"></param>
        public bool DeleteCriterio(Criterios criterio)
        {
            bool isDeleteComplete = true;

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd ;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM Criterios WHERE IdCriterio = @IdCriterio";
                cmd.Parameters.AddWithValue("@IdCriterio", criterio.IdCriterio);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM CriteriosTesis WHERE IdCriterio = @IdCriterio";
                cmd.Parameters.AddWithValue("@IdCriterio", criterio.IdCriterio);
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isDeleteComplete = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isDeleteComplete = false;
            }
            finally
            {
                connectionBitacoraSql.Close();
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
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                foreach (int tesis in criterio.TesisContendientes)
                {
                    string sqlCadena = "SELECT * FROM CriteriosTesis WHERE IdCriterio = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                    dataAdapter.Fill(dataSet, "Criterios");

                    dr = dataSet.Tables["Criterios"].NewRow();
                    dr["IdCriterio"] = criterio.IdCriterio;
                    dr["IUS"] = tesis;

                    dataSet.Tables["Criterios"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO CriteriosTesis(IdCriterio,IUS)" +
                                                            " VALUES(@IdCriterio,@IUS)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdCriterio", OleDbType.Numeric, 0, "IdCriterio");
                    dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Numeric, 0, "IUS");

                    dataAdapter.Update(dataSet, "Criterios");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }

        private int GetMaxOrderCriterio(int idContradiccion)
        {
            int maxOrden = 0;

            string sqlCmd = @"SELECT Max(Orden) AS Orden FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion ";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = "@IdContradiccion";
                parameter.OleDbType = OleDbType.Numeric;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = idContradiccion;

                cmd.Parameters.Add(parameter);
                
                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    maxOrden = Convert.ToInt32(reader["Orden"]);
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }

            return maxOrden + 1;
        }

        private int GetLastCriterioId(int idContradiccion, int maxOrden)
        {
            int lastId = 0;

            string sqlCmd = @"SELECT IdCriterio FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion AND Orden = @Orden";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = "@IdContradiccion";
                parameter.OleDbType = OleDbType.Numeric;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = idContradiccion;

                cmd.Parameters.Add(parameter);

                OleDbParameter parameter2 = new OleDbParameter();
                parameter2.ParameterName = "@Orden";
                parameter2.OleDbType = OleDbType.Numeric;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = maxOrden;

                cmd.Parameters.Add(parameter2);

                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lastId = Convert.ToInt32(reader["IdCriterio"]);
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }

            return lastId;
        }

        public ObservableCollection<Criterios> GetCriterios(int idContradiccion)
        {
            ObservableCollection<Criterios> criterios = new ObservableCollection<Criterios>();

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Criterios WHERE idContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Criterios criterio = new Criterios();
                    criterio.IdContradiccion = Convert.ToInt32(reader["idContradiccion"]);
                    criterio.IdCriterio = Convert.ToInt32(reader["IdCriterio"]);
                    criterio.Orden = Convert.ToInt32(reader["Orden"]);
                    criterio.Criterio = reader["Criterio"].ToString();
                    criterio.IdOrgano = Convert.ToInt32(reader["IdOrgano"]);
                    criterio.Observaciones = reader["Observaciones"].ToString();
                    criterio.Organo = (from n in Singletons.OrganismosSingleton.Colegiados
                                       where n.IdOrganismo == criterio.IdOrgano
                                       select n.Organismo).ToList()[0];

                    criterios.Add(criterio);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                oleConnection.Close();
            }

            return criterios;
        }

        private void AddParms(OleDbCommand cmd, params string[] cols)
        {
            // Add each parameter. Note that each colum in
            // table "Customers" is of type VARCHAR !
            foreach (String column in cols)
            {
                cmd.Parameters.Add("@" + column, OleDbType.Char, 0, column);
            }
        }
    }
}