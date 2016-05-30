using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class AdmisorioModel
    {
        /// <summary>
        /// Devuelve el acuerdo admisorio de la contradicción señalada
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public Admisorio GetAcuerdo(int idContradiccion)
        {
            Admisorio admisorio = new Admisorio();

            string sqlCmd = @"SELECT * FROM AcAdmisorio " +
                            " WHERE IdContradiccion = @IdContradiccion";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = sqlCmd;

            try
            {
                
                cmd.Parameters.AddWithValue("@IdContradiccion",idContradiccion);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    admisorio.IdAcuerdo = reader["IdAcuerdo"] as int? ?? 0;
                    admisorio.IdContradiccion = reader["IdContradiccion"] as int? ?? 0;
                    admisorio.FechaAcuerdo = DateTimeUtilities.GetDateFromReader(reader, "FechaAcuerdo");
                    admisorio.Acuerdo = reader["Acuerdo"].ToString();
                }

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return admisorio;
        }

        /// <summary>
        /// Verifica si existe un acuerdo admisorio para la contradicción señalada
        /// </summary>
        /// <param name="idAcuerdo"></param>
        /// <returns></returns>
        public bool CheckIfExist(int idAcuerdo)
        {
            bool doExist = false;

            string sqlCmd = @"SELECT * FROM AcAdmisorio WHERE IdAcuerdo = @IdAcuerdo";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = sqlCmd;

            try
            {

                cmd.Parameters.AddWithValue("@IdAcuerdo", idAcuerdo);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
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
        /// <param name="admisorio"></param>
        public void SetNewAdmisorio(Admisorio admisorio,int idContradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                admisorio.IdAcuerdo = DataBaseUtilities.GetNextIdForUse("AcAdmisorio", "IdAcuerdo",connection);

                string sqlCadena = "SELECT * FROM AcAdmisorio WHERE IdContradiccion = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "AcAdmisorio");

                dr = dataSet.Tables["AcAdmisorio"].NewRow();
                dr["IdAcuerdo"] = admisorio.IdAcuerdo;
                dr["IdContradiccion"] = idContradiccion;

                if (admisorio.FechaAcuerdo != null)
                    dr["FechaAcuerdo"] = admisorio.FechaAcuerdo;
                else
                    dr["FechaAcuerdo"] = System.DBNull.Value;

                dr["Acuerdo"] = admisorio.Acuerdo;

                dataSet.Tables["AcAdmisorio"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO AcAdmisorio(IdAcuerdo,IdContradiccion,FechaAcuerdo,Acuerdo)" +
                                                        " VALUES(@IdAcuerdo,@IdContradiccion,@FechaAcuerdo,@Acuerdo)";

                dataAdapter.InsertCommand.Parameters.Add("@IdAcuerdo", SqlDbType.Int, 0, "IdAcuerdo");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaAcuerdo", SqlDbType.Date, 0, "FechaAcuerdo");
                dataAdapter.InsertCommand.Parameters.Add("@Acuerdo", SqlDbType.VarChar, 0, "Acuerdo");

                dataAdapter.Update(dataSet, "AcAdmisorio");

                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }

        /// <summary>
        /// Actualiza la información de la resolución
        /// </summary>
        /// <param name="admisorio"></param>
        public void UpdateAdmisorio(Admisorio admisorio)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM AcAdmisorio WHERE IdAcuerdo = " + admisorio.IdAcuerdo;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "AcAdmisorio");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                if (admisorio.FechaAcuerdo != null)
                    dr["FechaAcuerdo"] = admisorio.FechaAcuerdo;
                else
                    dr["FechaAcuerdo"] = System.DBNull.Value;

                dr["Acuerdo"] = admisorio.Acuerdo;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE AcAdmisorio SET FechaAcuerdo = @FechaAcuerdo,Acuerdo = @Acuerdo " +
                                                       " WHERE IdAcuerdo = @IdAcuerdo";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaAcuerdo", SqlDbType.Date, 0, "FechaAcuerdo");
                dataAdapter.UpdateCommand.Parameters.Add("@Acuerdo", SqlDbType.VarChar, 0, "Acuerdo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdAcuerdo", SqlDbType.Int, 0, "IdAcuerdo");

                dataAdapter.Update(dataSet, "AcAdmisorio");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
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
        public bool DeleteAdmisorio(Admisorio admisorio)
        {
            bool isDeleteComplete = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM AcAdmisorio WHERE IdAcuerdo = @IdAcuerdo";
                cmd.Parameters.AddWithValue("@IdAcuerdo", admisorio.IdAcuerdo);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AdmisorioModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }


    }
}
