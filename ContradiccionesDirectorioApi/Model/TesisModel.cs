using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using System.Windows.Forms;

namespace ContradiccionesDirectorioApi.Model
{
    public class TesisModel
    {

        public void SetNewTesisPorContradiccion(Contradicciones contradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["ClaveControl"] = contradiccion.MiTesis.ClaveControl;
                dr["ClaveIdentificacion"] = contradiccion.MiTesis.ClaveIdentificacion;
                dr["Rubro"] = contradiccion.MiTesis.Rubro;
                dr["tatj"] = contradiccion.MiTesis.Tatj;
                dr["OficioPublicacion"] = contradiccion.MiTesis.OficioPublicacion;
                dr["OficioPPath"] = contradiccion.MiTesis.OficioPublicacionFilePath;
                dr["VersionPublica"] = contradiccion.MiTesis.VersionPublica;
                dr["VersionPPath"] = contradiccion.MiTesis.VersionPublicaFilePath;
                dr["CopiaCertificada"] = contradiccion.MiTesis.CopiaCertificada;
                dr["CopiaCPath"] = contradiccion.MiTesis.CopiaCertificadaFilePath;
                dr["Destinatario"] = contradiccion.MiTesis.Destinatario;
                dr["CambioCriterio"] = contradiccion.MiTesis.CambioCriterio;
                dr["Responsable"] = contradiccion.MiTesis.Responsable;
                dr["OficioRespuesta"] = contradiccion.MiTesis.OficioRespuesta;
                dr["OficioRPath"] = contradiccion.MiTesis.OficioRespuestaFilePath;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(IdContradiccion,Clavecontrol,ClaveIdentificacion,Rubro,tatj,OficioPublicacion," +
                                                        "OficioPPath,VersionPublica,VersionPPath,CopiaCertificada,CopiaCPath,Destinatario,CambioCriterio," +
                                                        "Responsable,OficioRespuesta,OficioRPath)" +
                                                        " VALUES(@IdContradiccion,@Clavecontrol,@ClaveIdentificacion,@Rubro,@tatj,@OficioPublicacion," +
                                                        "@OficioPPath,@VersionPublica,@VersionPPath,@CopiaCertificada,@CopiaCPath,@Destinatario,@CambioCriterio," +
                                                        "@Responsable,@OficioRespuesta,@OficioRPath)";

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
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }

            
        }

        public Tesis GetTesisPorContradiccion(int idContradiccion)
        {
            Tesis tesis = null;

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
                    tesis = new Tesis();
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

            return tesis;
        }

        //public Tesis GetTesisPorContradiccion(int idContradiccion)
        //{


        //}

    }
}
