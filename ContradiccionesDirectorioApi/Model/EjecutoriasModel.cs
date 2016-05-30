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
    public class EjecutoriasModel
    {

        public void SetNewEjecutoriaPorContradiccion(Contradicciones contradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Ejecutorias WHERE IdContradiccion = 0", connection);

                dataAdapter.Fill(dataSet, "Ejecutorias");

                dr = dataSet.Tables["Ejecutorias"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;

                if (contradiccion.MiEjecutoria.FechaResolucion != null)
                {
                    dr["FechaResolucion"] = contradiccion.MiEjecutoria.FechaResolucion;
                    dr["FechaResolucionInt"] = DateTimeUtilities.DateToInt(contradiccion.MiEjecutoria.FechaResolucion);
                }
                else
                {
                    dr["FechaResolucion"] = DBNull.Value;
                    dr["FechaResolucionInt"] = 0;
                }

                if (contradiccion.MiEjecutoria.FechaEngrose != null)
                {
                    dr["FechaEngrose"] = contradiccion.MiEjecutoria.FechaEngrose;
                    dr["FechaEngroseInt"] = DateTimeUtilities.DateToInt(contradiccion.MiEjecutoria.FechaEngrose);
                }
                else
                {
                    dr["FechaEngrose"] = DBNull.Value;
                    dr["FechaEngroseInt"] = 0;
                }
                dr["SISE"] = contradiccion.MiEjecutoria.Sise;
                dr["Responsable"] = contradiccion.MiEjecutoria.Responsable;
                dr["Signatario"] = contradiccion.MiEjecutoria.Signatario;
                dr["Oficio"] = contradiccion.MiEjecutoria.OficioRespuestaEj;
                dr["FileEjecPath"] = contradiccion.MiEjecutoria.FileEjecPath;
                dr["Razones"] = contradiccion.MiEjecutoria.Razones;

                dataSet.Tables["Ejecutorias"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutorias(IdContradiccion,FechaResolucion,FechaResolucionInt,FechaEngrose,FechaEngroseInt," +
                                                        "SISE,Responsable,Signatario,Oficio,FileEjecPath,Razones)" +
                                                        " VALUES(@IdContradiccion,@FechaResolucion,@FechaResolucionInt,@FechaEngrose,@FechaEngroseInt," +
                                                        "@SISE,@Responsable,@Signatario,@Oficio,@FileEjecPath,@Razones)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaResolucion", SqlDbType.Date, 0, "FechaResolucion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaResolucionInt", SqlDbType.Int, 0, "FechaResolucionInt");
                dataAdapter.InsertCommand.Parameters.Add("@FechaEngrose", SqlDbType.Date, 0, "FechaEngrose");
                dataAdapter.InsertCommand.Parameters.Add("@FechaEngroseInt", SqlDbType.Int, 0, "FechaEngroseInt");
                dataAdapter.InsertCommand.Parameters.Add("@SISE", SqlDbType.VarChar, 0, "SISE");
                dataAdapter.InsertCommand.Parameters.Add("@Responsable", SqlDbType.VarChar, 0, "Responsable");
                dataAdapter.InsertCommand.Parameters.Add("@Signatario", SqlDbType.VarChar, 0, "Signatario");
                dataAdapter.InsertCommand.Parameters.Add("@Oficio", SqlDbType.VarChar, 0, "Oficio");
                dataAdapter.InsertCommand.Parameters.Add("@FileEjecPath", SqlDbType.VarChar, 0, "FileEjecPath");
                dataAdapter.InsertCommand.Parameters.Add("@Razones", SqlDbType.VarChar, 0, "Razones");

                dataAdapter.Update(dataSet, "Ejecutorias");

                dataSet.Dispose();
                dataAdapter.Dispose();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }


        }

        public void SetRelacionesEjecutorias(int ius,int idContradiccion, int tipoRelacion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                    const string SqlQuery = "SELECT * FROM RelacionesEjecutoria WHERE IdContradiccion = 0";

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);

                    dataAdapter.Fill(dataSet, "Ejecutorias");

                    dr = dataSet.Tables["Ejecutorias"].NewRow();
                    dr["IdContradiccion"] = idContradiccion;
                    dr["TipoRelacion"] = tipoRelacion;
                    dr["ius"] = ius;

                    dataSet.Tables["Ejecutorias"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO RelacionesEjecutoria(IdContradiccion,TipoRelacion,ius)" +
                                                            " VALUES(@IdContradiccion,@TipoRelacion,@ius)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@TipoRelacion", SqlDbType.Int, 0, "TipoRelacion");
                    dataAdapter.InsertCommand.Parameters.Add("@ius", SqlDbType.Int, 0, "ius");

                    dataAdapter.Update(dataSet, "Ejecutorias");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }


        }

        public void DeleteRelacionesEjecutorias(int ius, int idContradiccion, int tipoRelacion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM RelacionesEjecutoria WHERE IdContradiccion = @IdContradiccion AND TipoRelacion = @TipoRelacion AND IUS = @IUS";
                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                cmd.Parameters.AddWithValue("@TipoRelacion", tipoRelacion);
                cmd.Parameters.AddWithValue("@IUS", ius);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }


        }


        public void UpdateEjecutoria(Contradicciones contradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion =" + contradiccion.IdContradiccion;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Ejecutorias");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();

                if (contradiccion.MiEjecutoria.FechaResolucion != null)
                {
                    dr["FechaResolucion"] = contradiccion.MiEjecutoria.FechaResolucion;
                    dr["FechaResolucionInt"] = DateTimeUtilities.DateToInt(contradiccion.MiEjecutoria.FechaResolucion);
                }
                else
                {
                    dr["FechaResolucion"] = DBNull.Value;
                    dr["FechaResolucionInt"] = DBNull.Value;
                }

                if (contradiccion.MiEjecutoria.FechaEngrose != null)
                {
                    dr["FechaEngrose"] = contradiccion.MiEjecutoria.FechaEngrose;
                    dr["FechaEngroseInt"] = DateTimeUtilities.DateToInt(contradiccion.MiEjecutoria.FechaEngrose);
                }
                else
                {
                    dr["FechaEngrose"] = DBNull.Value;
                    dr["FechaEngroseInt"] = DBNull.Value;
                }
                dr["SISE"] = contradiccion.MiEjecutoria.Sise;
                dr["Responsable"] = contradiccion.MiEjecutoria.Responsable;
                dr["Signatario"] = contradiccion.MiEjecutoria.Signatario;
                dr["Oficio"] = contradiccion.MiEjecutoria.OficioRespuestaEj;
                dr["FileEjecPath"] = contradiccion.MiEjecutoria.FileEjecPath;
                dr["Razones"] = contradiccion.MiEjecutoria.Razones;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Ejecutorias SET FechaResolucion = @FechaResolucion,FechaResolucionInt = @FechaResolucionInt," +
                                                       "FechaEngrose = @FechaEngrose,FechaEngroseInt = @FechaEngroseInt,SISE = @SISE,Responsable = @Responsable," +
                                                       "Signatario = @Signatario,Oficio = @Oficio,FileEjecPath = @FileEjecPath, Razones = @Razones" +
                                                       " WHERE IdContradiccion = @IdContradiccion";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaResolucion", SqlDbType.Date, 0, "FechaResolucion");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaResolucionInt", SqlDbType.Int, 0, "FechaResolucionInt");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaEngrose", SqlDbType.Date, 0, "FechaEngrose");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaEngroseInt", SqlDbType.Int, 0, "FechaEngroseInt");
                dataAdapter.UpdateCommand.Parameters.Add("@SISE", SqlDbType.VarChar, 0, "SISE");
                dataAdapter.UpdateCommand.Parameters.Add("@Responsable", SqlDbType.VarChar, 0, "Responsable");
                dataAdapter.UpdateCommand.Parameters.Add("@Signatario", SqlDbType.VarChar, 0, "Signatario");
                dataAdapter.UpdateCommand.Parameters.Add("@Oficio", SqlDbType.VarChar, 0, "Oficio");
                dataAdapter.UpdateCommand.Parameters.Add("@FileEjecPath", SqlDbType.VarChar, 0, "FileEjecPath");
                dataAdapter.UpdateCommand.Parameters.Add("@Razones", SqlDbType.VarChar, 0, "Razones");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");


                dataAdapter.Update(dataSet, "Ejecutorias");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Obtiene la información de la ejecutoria relacionada a una contradicción o asunto en particular
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public Ejecutoria GetEjecutoriasPorContradiccion(int idContradiccion)
        {
            Ejecutoria ejecutoria = null;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            string oleCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion = " + idContradiccion;

            try
            {
                connection.Open();

                cmd = new SqlCommand(oleCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ejecutoria = new Ejecutoria();
                    ejecutoria.FechaResolucion = DateTimeUtilities.GetDateFromReader(reader, "FechaResolucion");
                    ejecutoria.FechaEngrose = DateTimeUtilities.GetDateFromReader(reader, "FechaEngrose");
                    ejecutoria.Sise = reader["SISE"].ToString();
                    ejecutoria.Responsable = reader["Responsable"].ToString();
                    ejecutoria.Signatario = reader["Signatario"].ToString();
                    ejecutoria.OficioRespuestaEj = reader["Oficio"].ToString();
                    ejecutoria.FileEjecPath = reader["FileEjecPath"].ToString();
                    ejecutoria.Razones = reader["Razones"].ToString();
                    ejecutoria.TesisRelacionadas = this.GetRelacionesEjecutoria(idContradiccion, 1);
                    ejecutoria.VotosRelacionados = this.GetRelacionesEjecutoria(idContradiccion, 3);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return ejecutoria;
        }

        /// <summary>
        /// Verifica si la contradicción señalada ya tiene asociada una ejecutoria
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public bool CheckIsExist(int idContradiccion)
        {
            bool doExist = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            string oleCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion = " + idContradiccion;

            try
            {
                connection.Open();

                cmd = new SqlCommand(oleCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return doExist;
        }

        /// <summary>
        /// Lista de tesis o Votos relacionados con la ejecutoria seleccionada
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <param name="tipo">1. Ejec-Tesis   2. Ejec-Ejec   3. Ejec-Votos</param>
        /// <returns></returns>
        private ObservableCollection<int> GetRelacionesEjecutoria(int idContradiccion,int tipo)
        {
            ObservableCollection<int> regIus = new ObservableCollection<int>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            const string SqlQuery = "SELECT * FROM RelacionesEjecutoria WHERE IdContradiccion = @IdContradiccion AND TipoRelacion = @Tipo";

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    regIus.Add(Convert.ToInt32(reader["ius"]));
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return regIus;
        }

        /// <summary>
        /// Elimina la ejecutoria asociada a una Contradicción de Tesis
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteEjecutoria(Contradicciones contradiccion)
        {
            bool isDeleteComplete = false;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Ejecutorias WHERE IdContradiccion = @IdContradiccion";
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM RelacionesEjecutoria WHERE IdContradiccion = @IdContradiccion";
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,EjecutoriasModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }
    }
}
