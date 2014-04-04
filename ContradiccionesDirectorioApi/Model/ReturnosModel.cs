using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ContradiccionesDirectorioApi.Utils;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace ContradiccionesDirectorioApi.Model
{
    public class ReturnosModel
    {

        public ObservableCollection<ReturnosClass> GetReturnos(int idContradiccion)
        {
            ObservableCollection<ReturnosClass> returnos = new ObservableCollection<ReturnosClass>();

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Returnos WHERE idContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReturnosClass returno = new ReturnosClass();
                    returno.IdReturno = Convert.ToInt32(reader["IdReturno"]);
                    returno.IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]);
                    returno.Fecha = Convert.ToDateTime(reader["Fecha"]);
                    returno.IdOrganoOrigen = Convert.ToInt32(reader["IdOrgOrigen"]);
                    returno.IdOrganoDestino = Convert.ToInt32(reader["IdOrgDestino"]);
                    returno.ExpOrigen = reader["ExpOrigen"].ToString();
                    returno.ExpDestino = reader["ExpDestino"].ToString();

                    returnos.Add(returno);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                oleConnection.Close();
            }

            return returnos;
        }

        /// <summary>
        /// Almacena el returno correspondiente
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <param name="returno"></param>
        public void SetNewReturno(Contradicciones contradiccion, ReturnosClass returno)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Returnos WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Returnos");

                dr = dataSet.Tables["Returnos"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["Fecha"] = returno.Fecha;
                dr["FechaInt"] = DateTimeFunctions.ConvertDateToInt(returno.Fecha);
                dr["IdOrgOrigen"] = returno.IdOrganoOrigen;
                dr["IdOrgDestino"] = returno.IdOrganoDestino;
                dr["ExpOrigen"] = returno.ExpOrigen;
                dr["ExpDestino"] = returno.ExpDestino;

                dataSet.Tables["Returnos"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Returnos(IdContradiccion,Fecha,FechaInt,IdOrgOrigen," +
                                                        "IdOrgDestino,ExpOrigen,ExpDestino)" +
                                                        " VALUES(@IdContradiccion,@Fecha,@FechaInt,@IdOrgOrigen," +
                                                        "@IdOrgDestino,@ExpOrigen,@ExpDestino)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgOrigen", OleDbType.Numeric, 0, "IdOrgOrigen");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgDestino", OleDbType.Numeric, 0, "IdOrgDestino");
                dataAdapter.InsertCommand.Parameters.Add("@ExpOrigen", OleDbType.VarChar, 0, "ExpOrigen");
                dataAdapter.InsertCommand.Parameters.Add("@ExpDestino", OleDbType.VarChar, 0, "ExpDestino");

                dataAdapter.Update(dataSet, "Returnos");

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

        public void UpdateReturno(ReturnosClass returno)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Returnos WHERE IdReturno =" + returno.IdReturno;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Returnos");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Fecha"] = returno.Fecha;
                dr["FechaInt"] = DateTimeFunctions.ConvertDateToInt(returno.Fecha);
                dr["IdOrgOrigen"] = returno.IdOrganoOrigen;
                dr["IdOrgDestino"] = returno.IdOrganoDestino;
                dr["ExpOrigen"] = returno.ExpOrigen;
                dr["ExpDestino"] = returno.ExpDestino;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Returnos SET Fecha = @Fecha,FechaInt = @FechaInt,IdOrgOrigen = @IdOrgOrigen," +
                                                       "IdOrgDestino = @IdOrgDestino,ExpOrigen = @ExpOrigen,ExpDestino = @ExpDestino " +
                                                       " WHERE IdReturno = @IdReturno";

                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrgOrigen", OleDbType.Numeric, 0, "IdOrgOrigen");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrgDestino", OleDbType.Numeric, 0, "IdOrgDestino");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpOrigen", OleDbType.VarChar, 0, "ExpOrigen");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpDestino", OleDbType.VarChar, 0, "ExpDestino");
                dataAdapter.UpdateCommand.Parameters.Add("@IdReturno", OleDbType.Numeric, 0, "IdReturno");

                dataAdapter.Update(dataSet, "Returnos");
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
        /// Elimina el returno seleccionado
        /// </summary>
        /// <param name="idReturno">Identificador del returno a eliminar</param>
        public bool DeleteReturno(int idReturno)
        {
            bool isDeleteComplete = true;
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM Returnos WHERE IdReturno = @IdReturno";
                cmd.Parameters.AddWithValue("@IdReturno", idReturno);
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
        /// Elimina todos los returnos asociados a una contradiccion de tesis
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteReturno(Contradicciones contradiccion)
        {
            bool isDeleteComplete = true;

            foreach (ReturnosClass returno in contradiccion.Returnos)
            {
                isDeleteComplete = this.DeleteReturno(returno.IdReturno);

                if (!isDeleteComplete)
                    break;
            }

            return isDeleteComplete;
        }
    }
}