using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using System.Windows.Forms;
using System.Collections.ObjectModel;
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
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE IdTesis = 0";

                tesis.IdTesis = DataBaseUtilities.GetNextIdForUse("Tesis", "IdTesis",connection);

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

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

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(IdTesis,IdContradiccion,Clavecontrol,ClaveIdentificacion,Rubro,tatj,OficioPublicacion," +
                                                        "OficioPPath,VersionPublica,VersionPPath,CopiaCertificada,CopiaCPath,Destinatario,CambioCriterio," +
                                                        "Responsable,OficioRespuesta,OficioRPath)" +
                                                        " VALUES(@IdTesis,@IdContradiccion,@Clavecontrol,@ClaveIdentificacion,@Rubro,@tatj,@OficioPublicacion," +
                                                        "@OficioPPath,@VersionPublica,@VersionPPath,@CopiaCertificada,@CopiaCPath,@Destinatario,@CambioCriterio," +
                                                        "@Responsable,@OficioRespuesta,@OficioRPath)";

                dataAdapter.InsertCommand.Parameters.Add("@IdTesis", OleDbType.Numeric, 0, "IdTesis");
                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@ClaveControl", OleDbType.VarChar, 0, "ClaveControl");
                dataAdapter.InsertCommand.Parameters.Add("@ClaveIdentificacion", OleDbType.VarChar, 0, "ClaveIdentificacion");
                dataAdapter.InsertCommand.Parameters.Add("@Rubro", OleDbType.VarChar, 0, "Rubro");
                dataAdapter.InsertCommand.Parameters.Add("@tatj", OleDbType.Numeric, 0, "tatj");
                dataAdapter.InsertCommand.Parameters.Add("@OficioPublicacion", OleDbType.VarChar, 0, "OficioPublicacion");
                dataAdapter.InsertCommand.Parameters.Add("@OficioPPath", OleDbType.VarChar, 0, "OficioPPath");
                dataAdapter.InsertCommand.Parameters.Add("@VersionPublica", OleDbType.Numeric, 0, "VersionPublica");
                dataAdapter.InsertCommand.Parameters.Add("@VersionPPath", OleDbType.VarChar, 0, "VersionPPath");
                dataAdapter.InsertCommand.Parameters.Add("@CopiaCertificada", OleDbType.Numeric, 0, "CopiaCertificada");
                dataAdapter.InsertCommand.Parameters.Add("@CopiaCPath", OleDbType.VarChar, 0, "CopiaCPath");
                dataAdapter.InsertCommand.Parameters.Add("@Destinatario", OleDbType.VarChar, 0, "Destinatario");
                dataAdapter.InsertCommand.Parameters.Add("@CambioCriterio", OleDbType.Numeric, 0, "CambioCriterio");
                dataAdapter.InsertCommand.Parameters.Add("@Responsable", OleDbType.VarChar, 0, "Responsable");
                dataAdapter.InsertCommand.Parameters.Add("@OficioRespuesta", OleDbType.VarChar, 0, "OficioRespuesta");
                dataAdapter.InsertCommand.Parameters.Add("@OficioRPath", OleDbType.VarChar, 0, "OficioRPath");


                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();

                tesis.IdTesis = this.GetInsertedTesisId(tesis);
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
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

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Tesis WHERE IdContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Tesis tesis = new Tesis();
                    tesis.IdTesis = Convert.ToInt32(reader["IdTesis"]);
                    tesis.IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]);
                    tesis.ClaveControl = reader["Clavecontrol"].ToString();
                    tesis.ClaveIdentificacion = reader["ClaveIdentificacion"].ToString();
                    tesis.Rubro = reader["Rubro"].ToString();
                    tesis.Tatj = Convert.ToInt32(reader["tatj"]);
                    tesis.OficioPublicacion = reader["OficioPublicacion"].ToString();
                    tesis.OficioPublicacionFilePath = reader["OficioPPath"].ToString();
                    tesis.VersionPublica = Convert.ToInt32(reader["VersionPublica"].ToString());
                    tesis.VersionPublicaFilePath = reader["VersionPPath"].ToString();
                    tesis.CopiaCertificada = Convert.ToInt32(reader["CopiaCertificada"].ToString());
                    tesis.CopiaCertificadaFilePath = reader["CopiaCPath"].ToString();
                    tesis.Destinatario = reader["Destinatario"].ToString();
                    tesis.CambioCriterio = Convert.ToInt32(reader["CambioCriterio"]);
                    tesis.Responsable = reader["Responsable"].ToString();
                    tesis.OficioRespuesta = reader["OficioRespuesta"].ToString();
                    tesis.OficioRespuestaFilePath = reader["OficioRPath"].ToString();

                    listaTesis.Add(tesis);
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

            return listaTesis;
        }


        /// <summary>
        /// Actualiza la información de la tesis seleccionada
        /// </summary>
        /// <param name="tesis"></param>
        public void UpdateTesis(Tesis tesis)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE IdTesis =" + tesis.IdTesis;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

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
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Tesis SET ClaveControl = @ClaveControl,ClaveIdentificacion = @ClaveIdentificacion," +
                                                       "Rubro = @Rubro,tatj = @tatj,OficioPublicacion = @OficioPublicacion,OficioPPath = @OficioPPath," +
                                                       "VersionPublica = @VersionPublica,VersionPPath = @VersionPPath,CopiaCertificada = @CopiaCertificada," +
                                                       "CopiaCPath = @CopiaCPath,Destinatario = @Destinatario,CambioCriterio = @CambioCriterio, " +
                                                       "Responsable = @Responsable,OficioRespuesta = @OficioRespuesta,OficioRPath = @OficioRPath " +
                                                       " WHERE IdTesis = @IdTesis";

                dataAdapter.UpdateCommand.Parameters.Add("@ClaveControl", OleDbType.VarChar, 0, "ClaveControl");
                dataAdapter.UpdateCommand.Parameters.Add("@ClaveIdentificacion", OleDbType.VarChar, 0, "ClaveIdentificacion");
                dataAdapter.UpdateCommand.Parameters.Add("@Rubro", OleDbType.VarChar, 0, "Rubro");
                dataAdapter.UpdateCommand.Parameters.Add("@tatj", OleDbType.Numeric, 0, "tatj");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioPublicacion", OleDbType.VarChar, 0, "OficioPublicacion");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioPPath", OleDbType.VarChar, 0, "OficioPPath");
                dataAdapter.UpdateCommand.Parameters.Add("@VersionPublica", OleDbType.Numeric, 0, "VersionPublica");
                dataAdapter.UpdateCommand.Parameters.Add("@VersionPPath", OleDbType.VarChar, 0, "VersionPPath");
                dataAdapter.UpdateCommand.Parameters.Add("@CopiaCertificada", OleDbType.Numeric, 0, "CopiaCertificada");
                dataAdapter.UpdateCommand.Parameters.Add("@CopiaCPath", OleDbType.VarChar, 0, "CopiaCPath");
                dataAdapter.UpdateCommand.Parameters.Add("@Destinatario", OleDbType.VarChar, 0, "Destinatario");
                dataAdapter.UpdateCommand.Parameters.Add("@CambioCriterio", OleDbType.Numeric, 0, "CambioCriterio");
                dataAdapter.UpdateCommand.Parameters.Add("@Responsable", OleDbType.VarChar, 0, "Responsable");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioRespuesta", OleDbType.VarChar, 0, "OficioRespuesta");
                dataAdapter.UpdateCommand.Parameters.Add("@OficioRPath", OleDbType.VarChar, 0, "OficioRPath");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTesis", OleDbType.Numeric, 0, "IdTesis");

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }

        /// <summary>
        /// Elimina la información de la tesis asociada a la contradiccion de Tesis que se esta eliminando
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteTesis(Tesis tesis)
        {
            bool isDeleteComplete = true;
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM Tesis WHERE IdTesis = @IdTesis";
                cmd.Parameters.AddWithValue("@IdTesis", tesis.IdTesis);
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

        /// <summary>
        /// Devuelve el identificador de la tesis que se inserto
        /// </summary>
        /// <param name="tesis"></param>
        /// <returns></returns>
        public int GetInsertedTesisId(Tesis tesis)
        {
            int idTesis = 0;

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT IdTesis FROM Tesis WHERE IdContradiccion = " + tesis.IdContradiccion + " AND ClaveControl = '" + tesis.ClaveControl + "'";

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idTesis = Convert.ToInt32(reader["IdTesis"]);
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

            return idTesis;
        }
    }
}
