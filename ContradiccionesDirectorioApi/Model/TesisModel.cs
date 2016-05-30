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
    public class TesisModel
    {

        /// <summary>
        /// Establece una relación asunto-tesis
        /// </summary>
        /// <param name="tesis"></param>
        /// <returns></returns>
        public int SetNewTesisPorContradiccion(Tesis tesis)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                const string SqlQuery = "SELECT * FROM Tesis WHERE IdTesis = 0";

                tesis.IdTesis = DataBaseUtilities.GetNextIdForUse("Tesis", "IdTesis",connection);

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["IdTesis"] = tesis.IdTesis;
                dr["IdContradiccion"] = tesis.IdContradiccion;
                dr["ClaveControl"] = tesis.ClaveControl;
                dr["ClaveIdentificacion"] = tesis.ClaveIdentificacion;
                dr["Rubro"] = tesis.Rubro;
                dr["tatj"] = tesis.Tatj;
                dr["OficioPublicacion"] = tesis.OficioPublicacion;
                dr["OficioPPath"] = tesis.OficioPublicacionFilePath;
                dr["VersionPublica"] = tesis.VersionPublica;
                dr["VersionPPath"] = tesis.VersionPublicaFilePath;
                dr["CopiaCertificada"] = tesis.CopiaCertificada;
                dr["CopiaCPath"] = tesis.CopiaCertificadaFilePath;
                dr["Destinatario"] = tesis.Destinatario;
                dr["CambioCriterio"] = tesis.CambioCriterio;
                dr["Responsable"] = tesis.Responsable;
                dr["OficioRespuesta"] = tesis.OficioRespuesta;
                dr["OficioRPath"] = tesis.OficioRespuestaFilePath;
                dr["IUS"] = tesis.Ius;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(IdTesis,IdContradiccion,Clavecontrol,ClaveIdentificacion,Rubro,tatj,OficioPublicacion," +
                                                        "OficioPPath,VersionPublica,VersionPPath,CopiaCertificada,CopiaCPath,Destinatario,CambioCriterio," +
                                                        "Responsable,OficioRespuesta,OficioRPath,IUS)" +
                                                        " VALUES(@IdTesis,@IdContradiccion,@Clavecontrol,@ClaveIdentificacion,@Rubro,@tatj,@OficioPublicacion," +
                                                        "@OficioPPath,@VersionPublica,@VersionPPath,@CopiaCertificada,@CopiaCPath,@Destinatario,@CambioCriterio," +
                                                        "@Responsable,@OficioRespuesta,@OficioRPath,@IUS)";

                dataAdapter.InsertCommand.Parameters.Add("@IdTesis", SqlDbType.Int, 0, "IdTesis");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@ClaveControl", SqlDbType.VarChar, 0, "ClaveControl");
                dataAdapter.InsertCommand.Parameters.Add("@ClaveIdentificacion", SqlDbType.VarChar, 0, "ClaveIdentificacion");
                dataAdapter.InsertCommand.Parameters.Add("@Rubro", SqlDbType.VarChar, 0, "Rubro");
                dataAdapter.InsertCommand.Parameters.Add("@tatj", SqlDbType.Int, 0, "tatj");
                dataAdapter.InsertCommand.Parameters.Add("@OficioPublicacion", SqlDbType.VarChar, 0, "OficioPublicacion");
                dataAdapter.InsertCommand.Parameters.Add("@OficioPPath", SqlDbType.VarChar, 0, "OficioPPath");
                dataAdapter.InsertCommand.Parameters.Add("@VersionPublica", SqlDbType.Int, 0, "VersionPublica");
                dataAdapter.InsertCommand.Parameters.Add("@VersionPPath", SqlDbType.VarChar, 0, "VersionPPath");
                dataAdapter.InsertCommand.Parameters.Add("@CopiaCertificada", SqlDbType.Int, 0, "CopiaCertificada");
                dataAdapter.InsertCommand.Parameters.Add("@CopiaCPath", SqlDbType.VarChar, 0, "CopiaCPath");
                dataAdapter.InsertCommand.Parameters.Add("@Destinatario", SqlDbType.VarChar, 0, "Destinatario");
                dataAdapter.InsertCommand.Parameters.Add("@CambioCriterio", SqlDbType.Int, 0, "CambioCriterio");
                dataAdapter.InsertCommand.Parameters.Add("@Responsable", SqlDbType.VarChar, 0, "Responsable");
                dataAdapter.InsertCommand.Parameters.Add("@OficioRespuesta", SqlDbType.VarChar, 0, "OficioRespuesta");
                dataAdapter.InsertCommand.Parameters.Add("@OficioRPath", SqlDbType.VarChar, 0, "OficioRPath");
                dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.Int, 0, "IUS");

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();

                //tesis.IdTesis = this.GetInsertedTesisId(tesis);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return tesis.IdTesis;
        }

        /// <summary>
        /// Devuelve la lista de tesis relacionadas a la Contradiccion 
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public ObservableCollection<Tesis> GetTesisPorContradiccion(int idContradiccion)
        {
            ObservableCollection<Tesis> listaTesis = new ObservableCollection<Tesis>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;

            string oleCadena = "SELECT * FROM Tesis WHERE IdContradiccion = " + idContradiccion;

            try
            {
                connection.Open();

                cmd = new SqlCommand(oleCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Tesis tesis = new Tesis()
                    {
                        IdTesis = Convert.ToInt32(reader["IdTesis"]),
                        IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]),
                        ClaveControl = reader["Clavecontrol"].ToString(),
                        ClaveIdentificacion = reader["ClaveIdentificacion"].ToString(),
                        Rubro = reader["Rubro"].ToString(),
                        Tatj = Convert.ToInt32(reader["tatj"]),
                        OficioPublicacion = reader["OficioPublicacion"].ToString(),
                        OficioPublicacionFilePath = reader["OficioPPath"].ToString(),
                        VersionPublica = Convert.ToInt32(reader["VersionPublica"]),
                        VersionPublicaFilePath = reader["VersionPPath"].ToString(),
                        CopiaCertificada = Convert.ToInt32(reader["CopiaCertificada"]),
                        CopiaCertificadaFilePath = reader["CopiaCPath"].ToString(),
                        Destinatario = reader["Destinatario"].ToString(),
                        CambioCriterio = Convert.ToInt32(reader["CambioCriterio"]),
                        Responsable = reader["Responsable"].ToString(),
                        OficioRespuesta = reader["OficioRespuesta"].ToString(),
                        OficioRespuestaFilePath = reader["OficioRPath"].ToString(),
                        Ius = reader["IUS"] as int? ?? 0
                    };

                    listaTesis.Add(tesis);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return listaTesis;
        }


        /// <summary>
        /// Actualiza la información de la tesis seleccionada
        /// </summary>
        /// <param name="tesis"></param>
        public void UpdateTesis(Tesis tesis)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE IdTesis =" + tesis.IdTesis;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["ClaveControl"] = tesis.ClaveControl;
                dr["ClaveIdentificacion"] = tesis.ClaveIdentificacion;
                dr["Rubro"] = tesis.Rubro;
                dr["tatj"] = tesis.Tatj;
                dr["OficioPublicacion"] = tesis.OficioPublicacion;
                dr["OficioPPath"] = tesis.OficioPublicacionFilePath;
                dr["VersionPublica"] = tesis.VersionPublica;
                dr["VersionPPath"] = tesis.VersionPublicaFilePath;
                dr["CopiaCertificada"] = tesis.CopiaCertificada;
                dr["CopiaCPath"] = tesis.CopiaCertificadaFilePath;
                dr["Destinatario"] = tesis.Destinatario;
                dr["CambioCriterio"] = tesis.CambioCriterio;
                dr["Responsable"] = tesis.Responsable;
                dr["OficioRespuesta"] = tesis.OficioRespuesta;
                dr["OficioRPath"] = tesis.OficioRespuestaFilePath;
                dr["IUS"] = tesis.Ius;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Tesis SET ClaveControl = @ClaveControl,ClaveIdentificacion = @ClaveIdentificacion," +
                                                       "Rubro = @Rubro,tatj = @tatj,OficioPublicacion = @OficioPublicacion,OficioPPath = @OficioPPath," +
                                                       "VersionPublica = @VersionPublica,VersionPPath = @VersionPPath,CopiaCertificada = @CopiaCertificada," +
                                                       "CopiaCPath = @CopiaCPath,Destinatario = @Destinatario,CambioCriterio = @CambioCriterio, " +
                                                       "Responsable = @Responsable,OficioRespuesta = @OficioRespuesta,OficioRPath = @OficioRPath, IUS = @IUS " +
                                                       " WHERE IdTesis = @IdTesis";

                dataAdapter.UpdateCommand.Parameters.Add("@ClaveControl", SqlDbType.VarChar, 0, "ClaveControl");
                dataAdapter.UpdateCommand.Parameters.Add("@ClaveIdentificacion", SqlDbType.VarChar, 0, "ClaveIdentificacion");
                dataAdapter.UpdateCommand.Parameters.Add("@Rubro", SqlDbType.VarChar, 0, "Rubro");
                dataAdapter.UpdateCommand.Parameters.Add("@tatj", SqlDbType.Int, 0, "tatj");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioPublicacion", SqlDbType.VarChar, 0, "OficioPublicacion");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioPPath", SqlDbType.VarChar, 0, "OficioPPath");
                dataAdapter.UpdateCommand.Parameters.Add("@VersionPublica", SqlDbType.Int, 0, "VersionPublica");
                dataAdapter.UpdateCommand.Parameters.Add("@VersionPPath", SqlDbType.VarChar, 0, "VersionPPath");
                dataAdapter.UpdateCommand.Parameters.Add("@CopiaCertificada", SqlDbType.Int, 0, "CopiaCertificada");
                dataAdapter.UpdateCommand.Parameters.Add("@CopiaCPath", SqlDbType.VarChar, 0, "CopiaCPath");
                dataAdapter.UpdateCommand.Parameters.Add("@Destinatario", SqlDbType.VarChar, 0, "Destinatario");
                dataAdapter.UpdateCommand.Parameters.Add("@CambioCriterio", SqlDbType.Int, 0, "CambioCriterio");
                dataAdapter.UpdateCommand.Parameters.Add("@Responsable", SqlDbType.VarChar, 0, "Responsable");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioRespuesta", SqlDbType.VarChar, 0, "OficioRespuesta");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioRPath", SqlDbType.VarChar, 0, "OficioRPath");
                dataAdapter.UpdateCommand.Parameters.Add("@IUS", SqlDbType.Int, 0, "IUS");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTesis", SqlDbType.Int, 0, "IdTesis");

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina la información de la tesis asociada a la contradiccion de Tesis que se esta eliminando
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteTesis(Tesis tesis)
        {
            bool isDeleteComplete = false;
            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Tesis WHERE IdTesis = @IdTesis";
                cmd.Parameters.AddWithValue("@IdTesis", tesis.IdTesis);
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }

        /// <summary>
        /// Elimina una lista completa de tesis
        /// </summary>
        /// <param name="listaTesis"></param>
        /// <returns></returns>
        public bool DeleteTesis(ObservableCollection<Tesis> listaTesis)
        {
            bool deleted = false;
            foreach (Tesis tesis in listaTesis)
            {
                deleted = this.DeleteTesis(tesis);

                if (deleted == false)
                    return deleted;
            }
            return deleted;
        }


    }
}
