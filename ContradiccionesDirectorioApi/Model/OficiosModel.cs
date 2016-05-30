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

            const string SqlQuery = @"SELECT * FROM Oficios WHERE IdContradiccion = @IdContradiccion";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand() { Connection = connection, CommandText = SqlQuery };

            try
            {

                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Oficios oficio = new Oficios()
                    {
                        IdOficio = Convert.ToInt32(reader["IdOficio"]),
                        IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]),
                        Oficio = reader["Oficio"].ToString(),
                        FechaOficio = DateTimeUtilities.GetDateFromReader(reader, "Fecha")
                    };

                    listaOficios.Add(oficio);
                }

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return listaOficios;
        }

        /// <summary>
        /// Agrega un oficio relacionado a la contradicción
        /// </summary>
        /// <param name="admisorio"></param>
        public void SetNewOficio(Oficios oficio, int idContradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                oficio.IdOficio = DataBaseUtilities.GetNextIdForUse("Oficios", "IdOficio", connection);

                const string SqlQuery = "SELECT * FROM Oficios WHERE IdOficio = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);

                dataAdapter.Fill(dataSet, "Oficios");

                dr = dataSet.Tables["Oficios"].NewRow();
                dr["IdOficio"] = oficio.IdOficio;
                dr["IdContradiccion"] = idContradiccion;
                dr["Oficio"] = oficio.Oficio;

                if (oficio.FechaOficio == null)
                {
                    dr["Fecha"] = DBNull.Value;
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

                dataAdapter.InsertCommand.Parameters.Add("@IdOficio", SqlDbType.Int, 0, "IdOficio");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@Oficio", SqlDbType.VarChar, 0, "Oficio");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInt", SqlDbType.Int, 0, "FechaInt");

                dataAdapter.Update(dataSet, "Oficios");

                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
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
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Oficios WHERE IdOficio = " + oficio.IdOficio;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Oficios");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Oficio"] = oficio.Oficio;
                if (oficio.FechaOficio == null)
                {
                    dr["Fecha"] = DBNull.Value;
                    dr["FechaInt"] = 0;
                }
                else
                {
                    dr["Fecha"] = oficio.FechaOficio;
                    dr["FechaInt"] = DateTimeUtilities.DateToInt(oficio.FechaOficio);
                }
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Oficios SET Oficio = @Oficio,Fecha = @Fecha,FechaInt = @FechaInt " +
                                                       " WHERE IdOficio = @IdOficio";

                dataAdapter.UpdateCommand.Parameters.Add("@Oficio", SqlDbType.VarChar, 0, "Oficio");
                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInt", SqlDbType.Int, 0, "FechaInt");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOficio", SqlDbType.Int, 0, "IdOficio");

                dataAdapter.Update(dataSet, "Oficios");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina un oficio relacionado a un contradicción de tesis
        /// </summary>
        /// <param name="oficio">Oficio que será eliminado</param>
        /// <returns></returns>
        public bool DeleteOficio(Oficios oficio)
        {
            bool isDeleteComplete = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Oficios WHERE IdOficio = @IdOficio";
                cmd.Parameters.AddWithValue("@IdOficio", oficio.IdOficio);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OficiosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return isDeleteComplete;
        }


    }
}