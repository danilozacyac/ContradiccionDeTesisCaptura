using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
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

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;

            try
            {
                
                cmd.Parameters.AddWithValue("@IdContradiccion",idContradiccion);

                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    admisorio.IdAcuerdo = reader["IdAcuerdo"] as int? ?? 0;
                    admisorio.IdContradiccion = reader["IdContradiccion"] as int? ?? 0;
                    admisorio.FechaAcuerdo = DateTimeUtilities.GetDateFromReader(reader, "FechaAcuerdo");
                    admisorio.Acuerdo = reader["Acuerdo"].ToString();
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

            string sqlCmd = @"SELECT * FROM AcAdmisorio " +
                            " WHERE IdAcuerdo = @IdAcuerdo";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;

            try
            {

                cmd.Parameters.AddWithValue("@IdAcuerdo", idAcuerdo);

                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
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

            return doExist;
        }

        /// <summary>
        /// Agrega una resolución para la contradicción señalada
        /// </summary>
        /// <param name="admisorio"></param>
        public void SetNewAdmisorio(Admisorio admisorio)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                admisorio.IdAcuerdo = this.GetNextIdForUse("AcAdmisorio", "IdAcuerdo");

                string sqlCadena = "SELECT * FROM AcAdmisorio WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "AcAdmisorio");

                dr = dataSet.Tables["AcAdmisorio"].NewRow();
                dr["IdAcuerdo"] = admisorio.IdAcuerdo;
                dr["IdContradiccion"] = admisorio.IdContradiccion;
                dr["FechaAcuerdo"] = admisorio.FechaAcuerdo;
                dr["Acuerdo"] = admisorio.Acuerdo;

                dataSet.Tables["AcAdmisorio"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO AcAdmisorio(IdAcuerdo,IdContradiccion,FechaAcuerdo,Acuerdo)" +
                                                        " VALUES(@IdAcuerdo,@IdContradiccion,@FechaAcuerdo,@Acuerdo)";

                dataAdapter.InsertCommand.Parameters.Add("@IdAcuerdo", OleDbType.Numeric, 0, "IdAcuerdo");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaAcuerdo", OleDbType.Date, 0, "FechaAcuerdo");
                dataAdapter.InsertCommand.Parameters.Add("@Acuerdo", OleDbType.VarChar, 0, "Acuerdo");

                dataAdapter.Update(dataSet, "AcAdmisorio");

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
        /// Actualiza la información de la resolución
        /// </summary>
        /// <param name="admisorio"></param>
        public void UpdateAdmisorio(Admisorio admisorio)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM AcAdmisorio WHERE IdAcuerdo = " + admisorio.IdAcuerdo;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "AcAdmisorio");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["FechaAcuerdo"] = admisorio.FechaAcuerdo;
                dr["Acuerdo"] = admisorio.Acuerdo;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE AcAdmisorio SET FechaAcuerdo = @FechaAcuerdo,Acuerdo = @Acuerdo " +
                                                       " WHERE IdAcuerdo = @IdAcuerdo";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaAcuerdo", OleDbType.Numeric, 0, "FechaAcuerdo");
                dataAdapter.UpdateCommand.Parameters.Add("@Acuerdo", OleDbType.VarChar, 0, "Acuerdo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdAcuerdo", OleDbType.Numeric, 0, "IdAcuerdo");

                dataAdapter.Update(dataSet, "AcAdmisorio");
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
        /// Elimina la resolucion asociada a una Contradicción de Tesis
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteAdmisorio(Admisorio admisorio)
        {
            bool isDeleteComplete = true;

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM AcAdmisorio WHERE IdAcuerdo = @IdAcuerdo";
                cmd.Parameters.AddWithValue("@IdAcuerdo", admisorio.IdAcuerdo);
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


        public int GetNextIdForUse(string nombreTabla, string nombreCampo)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader = null;

            int idForUse = 0;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT MAX(" + nombreCampo + ") + 1 AS Id FROM " + nombreTabla;

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    idForUse = reader["Id"] as int? ?? -1;
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Utilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Utilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                reader.Close();
                connection.Close();
            }

            return idForUse;
        }
    }
}
