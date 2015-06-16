using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class OficiosModel
    {

        /// <summary>
        /// Devuelve los oficios recibidos en relación a la contradicción señalada
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public ObservableCollection<Oficios> GetOficios(int idContradiccion)
        {
            ObservableCollection<Oficios> listaOficios = new ObservableCollection<Oficios>();

            string sqlCmd = @"SELECT * FROM Oficios " +
                            " WHERE IdContradiccion = @IdContradiccion";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;

            try
            {

                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);

                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Oficios oficio = new Oficios();
                    oficio.IdOficio = reader["IdOficio"] as int? ?? 0;
                    oficio.IdContradiccion = reader["IdContradiccion"] as int? ?? 0;
                    oficio.Oficio = reader["Oficio"].ToString();
                    oficio.FechaOficio = DateTimeUtilities.GetDateFromReader(reader, "Fecha");

                    listaOficios.Add(oficio);
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

            return listaOficios;
        }

        /// <summary>
        /// Agrega un oficio relacionado a la contradicción
        /// </summary>
        /// <param name="admisorio"></param>
        public void SetNewOficio(Oficios oficio,int idContradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                oficio.IdOficio = DataBaseUtilities.GetNextIdForUse("Oficios", "IdOficio",connection);

                string sqlCadena = "SELECT * FROM Oficios WHERE IdOficio = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Oficios");

                dr = dataSet.Tables["Oficios"].NewRow();
                dr["IdOficio"] = oficio.IdOficio;
                dr["IdContradiccion"] = idContradiccion;
                dr["Oficio"] = oficio.Oficio;

                if (oficio.FechaOficio == null)
                {
                    dr["Fecha"] = System.DBNull.Value;
                    dr["FechaInt"] = 0;
                }
                else
                {
                    dr["Fecha"] = oficio.FechaOficio;
                    dr["FechaInt"] = DateTimeUtilities.DateToInt(oficio.FechaOficio);
                }
                dataSet.Tables["Oficios"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Oficios(IdOficio,IdContradiccion,Oficio,Fecha,FechaInt)" +
                                                        " VALUES(@IdOficio,@IdContradiccion,@Oficio,@Fecha,@FechaInt)";

                dataAdapter.InsertCommand.Parameters.Add("@IdOficio", OleDbType.Numeric, 0, "IdOficio");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");

                dataAdapter.Update(dataSet, "Oficios");

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
                connection.Close();
            }

        }

        /// <summary>
        /// Actualiza la información de la resolución
        /// </summary>
        /// <param name="oficio"></param>
        public void UpdateOficio(Oficios oficio)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Oficios WHERE IdOficio = " + oficio.IdOficio;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Oficios");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Oficio"] = oficio.Oficio;
                if (oficio.FechaOficio == null)
                {
                    dr["Fecha"] = System.DBNull.Value;
                    dr["FechaInt"] = 0;
                }
                else
                {
                    dr["Fecha"] = oficio.FechaOficio;
                    dr["FechaInt"] = DateTimeUtilities.DateToInt(oficio.FechaOficio);
                }
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Oficios SET Oficio = @Oficio,Fecha = @Fecha,FechaInt = @FechaInt " +
                                                       " WHERE IdOficio = @IdOficio";

                dataAdapter.UpdateCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOficio", OleDbType.Numeric, 0, "IdOficio");

                dataAdapter.Update(dataSet, "Oficios");
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
        /// Elimina un oficio relacionado a un contradicción de tesis
        /// </summary>
        /// <param name="oficio">Oficio que será eliminado</param>
        /// <returns></returns>
        public bool DeleteOficio(Oficios oficio)
        {
            bool isDeleteComplete = true;

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM Oficios WHERE IdOficio = @IdOficio";
                cmd.Parameters.AddWithValue("@IdOficio", oficio.IdOficio);
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


    }
}