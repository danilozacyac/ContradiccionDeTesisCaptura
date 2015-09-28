using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using System.Collections.ObjectModel;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class ResolucionModel
    {
        #region Resoluciones

        /// <summary>
        /// Devuelve la resolución de una contradicción
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public Resolutivos GetResolucion(int idContradiccion)
        {
            Resolutivos resolutivos = new Resolutivos();

            string sqlCmd = @"SELECT * FROM Resolucion " +
                            " WHERE IdContradiccion = @IdContradiccion";

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connection;
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

                connection.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resolutivos.RegEjecutoria = Convert.ToInt32(reader["RegEjecutoria"]);
                    resolutivos.RegTesis = Convert.ToInt32(reader["RegTesis"]);
                    resolutivos.RubroTesis = reader["RubroTesis"].ToString();
                }

                resolutivos.PuntosResolutivos = this.GetResolutivos(idContradiccion);
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return resolutivos;
        }

        /// <summary>
        /// Verifica si existe una resolución para la contradicción señalada
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public bool CheckIfExist(int idContradiccion)
        {
            bool doExist = false;

            string sqlCmd = @"SELECT * FROM Resolucion " +
                            " WHERE IdContradiccion = @IdContradiccion";

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connection;
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

                connection.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return doExist;
        }

        /// <summary>
        /// Agrega una resolución para la contradicción señalada
        /// </summary>
        /// <param name="contradiccion"></param>
        public void SetNewResolucion(Contradicciones contradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Resolucion WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Resolucion");

                dr = dataSet.Tables["Resolucion"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["RegEjecutoria"] = contradiccion.Resolutivo.RegEjecutoria;
                dr["RegTesis"] = contradiccion.Resolutivo.RegTesis;
                dr["RubroTesis"] = contradiccion.Resolutivo.RubroTesis;
                

                dataSet.Tables["Resolucion"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Resolucion(IdContradiccion,RegEjecutoria,RegTesis,RubroTesis)" +
                                                        " VALUES(@IdContradiccion,@RegEjecutoria,@RegTesis,@RubroTesis)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@RegEjecutoria", OleDbType.Numeric, 0, "RegEjecutoria");
                dataAdapter.InsertCommand.Parameters.Add("@RegTesis", OleDbType.Numeric, 0, "RegTesis");
                dataAdapter.InsertCommand.Parameters.Add("@RubroTesis", OleDbType.VarChar, 0, "RubroTesis");

                dataAdapter.Update(dataSet, "Resolucion");

                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }

        /// <summary>
        /// Actualiza la información de la resolución
        /// </summary>
        /// <param name="resolutivo"></param>
        /// <param name="idContradiccion"></param>
        public void UpdateResolucion(Resolutivos resolutivo, int idContradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Resolucion WHERE IdContradiccion =" + idContradiccion;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Resolucion");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["RegEjecutoria"] = resolutivo.RegEjecutoria;
                dr["RegTesis"] = resolutivo.RegTesis;
                dr["RubroTesis"] = resolutivo.RubroTesis;
                dr["IdContradiccion"] = idContradiccion;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Resolucion SET RegEjecutoria = @RegEjecutoria,RegTesis = @RegTesis,RubroTesis = @RubroTesis" +
                                                       " WHERE IdContradiccion = @IdContradiccion";

                dataAdapter.UpdateCommand.Parameters.Add("@RegEjecutoria", OleDbType.Numeric, 0, "RegEjecutoria");
                dataAdapter.UpdateCommand.Parameters.Add("@RegTesis", OleDbType.Numeric, 0, "RegTesis");
                dataAdapter.UpdateCommand.Parameters.Add("@RubroTesis", OleDbType.VarChar, 0, "RubroTesis");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");

                dataAdapter.Update(dataSet, "Resolucion");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina la resolucion asociada a una Contradicción de Tesis
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteResolucion(Contradicciones contradiccion)
        {
            bool isDeleteComplete = false;

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Resolucion WHERE IdContradiccion = @IdContradiccion";
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }


        #endregion


        #region Resolutivos

        public void SetNewResolutivo(PResolutivos resolutivos, int idContradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                resolutivos.IdResolutivo = DataBaseUtilities.GetNextIdForUse("Resolutivos", "IdResolutivo", connection);

                string sqlCadena = "SELECT * FROM Resolutivos WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Resolutivos");

                dr = dataSet.Tables["Resolutivos"].NewRow();
                dr["IdContradiccion"] = idContradiccion;
                dr["Resolutivo"] = resolutivos.Resolutivo;
                dr["IdResolutivo"] = resolutivos.IdResolutivo;

                dataSet.Tables["Resolutivos"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Resolutivos(IdContradiccion,Resolutivo,IdResolutivo)" +
                                                        " VALUES(@IdContradiccion,@Resolutivo,@IdResolutivo)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Resolutivo", OleDbType.VarChar, 0, "Resolutivo");
                dataAdapter.InsertCommand.Parameters.Add("@IdResolutivo", OleDbType.Numeric, 0, "IdResolutivo");

                dataAdapter.Update(dataSet, "Resolutivos");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }
        
        private ObservableCollection<PResolutivos> GetResolutivos(int idContradiccion)
        {
            ObservableCollection<PResolutivos> resolutivos = new ObservableCollection<PResolutivos>();

            string sqlCmd = @"SELECT * FROM Resolutivos " +
                            " WHERE IdContradiccion = @IdContradiccion";

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connection;
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

                connection.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PResolutivos resolutivo = new PResolutivos();
                    resolutivo.IdResolutivo = Convert.ToInt32(reader["IdResolutivo"]);
                    resolutivo.Resolutivo = reader["Resolutivo"].ToString();

                    resolutivos.Add(resolutivo);
                }


            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return resolutivos;
        }

        public void UpdateResolutivo(PResolutivos resolutivo)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Resolutivos WHERE IdResolutivo =" + resolutivo.IdResolutivo;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Resolutivos");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Resolutivo"] = resolutivo.Resolutivo;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Resolutivos SET Resolutivo = @Resolutivo " +
                                                       " WHERE IdResolutivo = @IdResolutivo";

                dataAdapter.UpdateCommand.Parameters.Add("@Resolutivo", OleDbType.VarChar, 0, "Resolutivo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdResolutivo", OleDbType.Numeric, 0, "IdResolutivo");

                dataAdapter.Update(dataSet, "Resolutivos");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina el punto resolutivo seleccionado
        /// </summary>
        /// <param name="idResolutivo"></param>
        /// <returns></returns>
        public bool DeleteResolutivo(int idResolutivo)
        {
            bool isDeleteComplete = false;
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Resolutivos WHERE IdResolutivo = @IdResolutivo";
                cmd.Parameters.AddWithValue("@IdResolutivo", idResolutivo);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ResolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }

        /// <summary>
        /// Elimina todos los puntos resolutivos asociados a una contradicción
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteResolutivo(Contradicciones contradiccion)
        {
            bool isDeleteComplete = true;

            foreach (PResolutivos resolutivo in contradiccion.Resolutivo.PuntosResolutivos)
            {
                isDeleteComplete = this.DeleteResolutivo(resolutivo.IdResolutivo);

                if (!isDeleteComplete)
                    break;
            }

            return isDeleteComplete;
        }

        #endregion
    }
}
