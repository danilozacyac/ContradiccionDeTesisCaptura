using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class ReturnosModel
    {
        /// <summary>
        /// Enlista los returnos que ha tenido el asunto desde su primer registro en el sistema
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public ObservableCollection<ReturnosClass> GetReturnos(int idContradiccion)
        {
            ObservableCollection<ReturnosClass> returnos = new ObservableCollection<ReturnosClass>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            string oleCadena = "SELECT * FROM Returnos WHERE idContradiccion = " + idContradiccion;

            try
            {
                connection.Open();

                cmd = new SqlCommand(oleCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReturnosClass returno = new ReturnosClass()
                    {
                        IdReturno = Convert.ToInt32(reader["IdReturno"]),
                        IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        IdOrganoOrigen = Convert.ToInt32(reader["IdOrgOrigen"]),
                        IdOrganoDestino = Convert.ToInt32(reader["IdOrgDestino"]),
                        ExpOrigen = reader["ExpOrigen"].ToString(),
                        ExpDestino = reader["ExpDestino"].ToString()
                    };

                    returnos.Add(returno);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
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
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Returnos WHERE IdContradiccion = 0", connection);

                dataAdapter.Fill(dataSet, "Returnos");

                dr = dataSet.Tables["Returnos"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["Fecha"] = returno.Fecha;
                dr["FechaInt"] = DateTimeUtilities.DateToInt(returno.Fecha);
                dr["IdOrgOrigen"] = returno.IdOrganoOrigen;
                dr["IdOrgDestino"] = returno.IdOrganoDestino;
                dr["ExpOrigen"] = returno.ExpOrigen;
                dr["ExpDestino"] = returno.ExpDestino;

                dataSet.Tables["Returnos"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Returnos(IdContradiccion,Fecha,FechaInt,IdOrgOrigen," +
                                                        "IdOrgDestino,ExpOrigen,ExpDestino)" +
                                                        " VALUES(@IdContradiccion,@Fecha,@FechaInt,@IdOrgOrigen," +
                                                        "@IdOrgDestino,@ExpOrigen,@ExpDestino)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInt", SqlDbType.Int, 0, "FechaInt");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgOrigen", SqlDbType.Int, 0, "IdOrgOrigen");
                dataAdapter.InsertCommand.Parameters.Add("@IdOrgDestino", SqlDbType.Int, 0, "IdOrgDestino");
                dataAdapter.InsertCommand.Parameters.Add("@ExpOrigen", SqlDbType.VarChar, 0, "ExpOrigen");
                dataAdapter.InsertCommand.Parameters.Add("@ExpDestino", SqlDbType.VarChar, 0, "ExpDestino");

                dataAdapter.Update(dataSet, "Returnos");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }


        /// <summary>
        /// Actualiza la información existente de un returno
        /// </summary>
        /// <param name="returno"></param>
        public void UpdateReturno(ReturnosClass returno)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Returnos WHERE IdReturno =" + returno.IdReturno;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Returnos");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Fecha"] = returno.Fecha;
                dr["FechaInt"] = DateTimeUtilities.DateToInt(returno.Fecha);
                dr["IdOrgOrigen"] = returno.IdOrganoOrigen;
                dr["IdOrgDestino"] = returno.IdOrganoDestino;
                dr["ExpOrigen"] = returno.ExpOrigen;
                dr["ExpDestino"] = returno.ExpDestino;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Returnos SET Fecha = @Fecha,FechaInt = @FechaInt,IdOrgOrigen = @IdOrgOrigen," +
                                                       "IdOrgDestino = @IdOrgDestino,ExpOrigen = @ExpOrigen,ExpDestino = @ExpDestino " +
                                                       " WHERE IdReturno = @IdReturno";

                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInt", SqlDbType.Int, 0, "FechaInt");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrgOrigen", SqlDbType.Int, 0, "IdOrgOrigen");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrgDestino", SqlDbType.Int, 0, "IdOrgDestino");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpOrigen", SqlDbType.VarChar, 0, "ExpOrigen");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpDestino", SqlDbType.VarChar, 0, "ExpDestino");
                dataAdapter.UpdateCommand.Parameters.Add("@IdReturno", SqlDbType.Int, 0, "IdReturno");

                dataAdapter.Update(dataSet, "Returnos");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina el returno seleccionado
        /// </summary>
        /// <param name="idReturno">Identificador del returno a eliminar</param>
        public bool DeleteReturno(int idReturno)
        {
            bool isDeleteComplete = false;
            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Returnos WHERE IdReturno = @IdReturno";
                cmd.Parameters.AddWithValue("@IdReturno", idReturno);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReturnosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
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