﻿using System;
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
    public class EjecutoriasModel
    {

        public void SetNewEjecutoriaPorContradiccion(Contradicciones contradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Ejecutorias");

                dr = dataSet.Tables["Ejecutorias"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["FechaResolucion"] = contradiccion.MiEjecutoria.FechaResolucion;
                dr["FechaResolucionInt"] = DateTimeFunctions.ConvertDateToInt(contradiccion.MiEjecutoria.FechaResolucion);
                dr["FechaEngrose"] = contradiccion.MiEjecutoria.FechaEngrose;
                dr["FechaEngroseInt"] = DateTimeFunctions.ConvertDateToInt(contradiccion.MiEjecutoria.FechaEngrose); 
                dr["SISE"] = contradiccion.MiEjecutoria.Sise;
                dr["Responsable"] = contradiccion.MiEjecutoria.Responsable;
                dr["Signatario"] = contradiccion.MiEjecutoria.Signatario;
                dr["Oficio"] = contradiccion.MiEjecutoria.OficioRespuestaEj;
                dr["FileEjecPath"] = contradiccion.MiEjecutoria.FileEjecPath;

                dataSet.Tables["Ejecutorias"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutorias(IdContradiccion,FechaResolucion,FechaResolucionInt,FechaEngrose,FechaEngroseInt," +
                                                        "SISE,Responsable,Signatario,Oficio,FileEjecPath)" +
                                                        " VALUES(@IdContradiccion,@FechaResolucion,@FechaResolucionInt,@FechaEngrose,@FechaEngroseInt," +
                                                        "@SISE,@Responsable,@Signatario,@Oficio,@FileEjecPath)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaResolucion", OleDbType.Date, 0, "FechaResolucion");
                dataAdapter.InsertCommand.Parameters.Add("@FechaResolucionInt", OleDbType.Numeric, 0, "FechaResolucionInt");
                dataAdapter.InsertCommand.Parameters.Add("@FechaEngrose", OleDbType.Date, 0, "FechaEngrose");
                dataAdapter.InsertCommand.Parameters.Add("@FechaEngroseInt", OleDbType.Numeric, 0, "FechaEngroseInt");
                dataAdapter.InsertCommand.Parameters.Add("@SISE", OleDbType.VarChar, 0, "SISE");
                dataAdapter.InsertCommand.Parameters.Add("@Responsable", OleDbType.VarChar, 0, "Responsable");
                dataAdapter.InsertCommand.Parameters.Add("@Signatario", OleDbType.VarChar, 0, "Signatario");
                dataAdapter.InsertCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.InsertCommand.Parameters.Add("@FileEjecPath", OleDbType.VarChar, 0, "FileEjecPath");


                dataAdapter.Update(dataSet, "Ejecutorias");

                dataSet.Dispose();
                dataAdapter.Dispose();

               
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            finally
            {
                connectionBitacoraSql.Close();
            }


        }

        public void SetRelacionesEjecutorias(int ius,int idContradiccion, int tipoRelacion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                    string sqlCadena = "SELECT * FROM RelacionesEjecutoria WHERE IdContradiccion = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                    dataAdapter.Fill(dataSet, "Ejecutorias");

                    dr = dataSet.Tables["Ejecutorias"].NewRow();
                    dr["IdContradiccion"] = idContradiccion;
                    dr["TipoRelacion"] = tipoRelacion;
                    dr["ius"] = ius;

                    dataSet.Tables["Ejecutorias"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO RelacionesEjecutoria(IdContradiccion,TipoRelacion,ius)" +
                                                            " VALUES(@IdContradiccion,@TipoRelacion,@ius)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@TipoRelacion", OleDbType.Numeric, 0, "TipoRelacion");
                    dataAdapter.InsertCommand.Parameters.Add("@ius", OleDbType.Numeric, 0, "ius");

                    dataAdapter.Update(dataSet, "Ejecutorias");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            finally
            {
                connectionBitacoraSql.Close();
            }


        }

        public void UpdateEjecutoria(Contradicciones contradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion =" + contradiccion.IdContradiccion;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Ejecutorias");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["FechaResolucion"] = contradiccion.MiEjecutoria.FechaResolucion;
                dr["FechaResolucionInt"] = DateTimeFunctions.ConvertDateToInt(contradiccion.MiEjecutoria.FechaResolucion);
                dr["FechaEngrose"] = contradiccion.MiEjecutoria.FechaEngrose;
                dr["FechaEngroseInt"] = DateTimeFunctions.ConvertDateToInt(contradiccion.MiEjecutoria.FechaEngrose);
                dr["SISE"] = contradiccion.MiEjecutoria.Sise;
                dr["Responsable"] = contradiccion.MiEjecutoria.Responsable;
                dr["Signatario"] = contradiccion.MiEjecutoria.Signatario;
                dr["Oficio"] = contradiccion.MiEjecutoria.OficioRespuestaEj;
                dr["FileEjecPath"] = contradiccion.MiEjecutoria.FileEjecPath;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Ejecutorias SET FechaResolucion = @FechaResolucion,FechaResolucionInt = @FechaResolucionInt," +
                                                       "FechaEngrose = @FechaEngrose,FechaEngroseInt = @FechaEngroseInt,SISE = @SISE,Responsable = @Responsable," +
                                                       "Signatario = @Signatario,Oficio = @Oficio,FileEjecPath = @FileEjecPath" +
                                                       " WHERE IdContradiccion = @IdContradiccion";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaResolucion", OleDbType.Date, 0, "FechaResolucion");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaResolucionInt", OleDbType.Numeric, 0, "FechaResolucionInt");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaEngrose", OleDbType.Date, 0, "FechaEngrose");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaEngroseInt", OleDbType.Numeric, 0, "FechaEngroseInt");
                dataAdapter.UpdateCommand.Parameters.Add("@SISE", OleDbType.VarChar, 0, "SISE");
                dataAdapter.UpdateCommand.Parameters.Add("@Responsable", OleDbType.VarChar, 0, "Responsable");
                dataAdapter.UpdateCommand.Parameters.Add("@Signatario", OleDbType.VarChar, 0, "Signatario");
                dataAdapter.UpdateCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.UpdateCommand.Parameters.Add("@FileEjecPath", OleDbType.VarChar, 0, "FileEjecPath");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");


                dataAdapter.Update(dataSet, "Ejecutorias");
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
        /// Obtiene la información de la ejecutoria relacionada a una contradicción o asunto en particular
        /// </summary>
        /// <param name="idContradiccion"></param>
        /// <returns></returns>
        public Ejecutoria GetEjecutoriasPorContradiccion(int idContradiccion)
        {
            Ejecutoria ejecutoria = new Ejecutoria();

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    if (reader["FechaResolucion"] == DBNull.Value)
                        ejecutoria.FechaResolucion = null;
                    else
                        ejecutoria.FechaResolucion = Convert.ToDateTime(reader["FechaResolucion"]);

                    if (reader["FechaEngrose"] == DBNull.Value)
                        ejecutoria.FechaEngrose = null;
                    else
                        ejecutoria.FechaEngrose = Convert.ToDateTime(reader["FechaEngrose"]);

                    ejecutoria.Sise = reader["SISE"].ToString();
                    ejecutoria.Responsable = reader["Responsable"].ToString();
                    ejecutoria.Signatario = reader["Signatario"].ToString();
                    ejecutoria.OficioRespuestaEj = reader["Oficio"].ToString();
                    ejecutoria.FileEjecPath = reader["FileEjecPath"].ToString();
                    ejecutoria.TesisRelacionadas = this.GetRelacionesEjecutoria(idContradiccion, 1);
                    ejecutoria.VotosRelacionados = this.GetRelacionesEjecutoria(idContradiccion, 3);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            finally
            {
                oleConnection.Close();
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

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Ejecutorias WHERE IdContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            finally
            {
                oleConnection.Close();
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

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM RelacionesEjecutoria WHERE IdContradiccion = " + idContradiccion + " AND TipoRelacion = " + tipo;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    regIus.Add(Convert.ToInt32(reader["ius"]));
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno --- SalvarRegistroMantesisSql");
            }
            finally
            {
                oleConnection.Close();
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
            bool isDeleteComplete = true;

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd;

            cmd = connectionBitacoraSql.CreateCommand();
            cmd.Connection = connectionBitacoraSql;

            try
            {
                connectionBitacoraSql.Open();

                cmd.CommandText = "DELETE FROM Ejecutorias WHERE IdContradiccion = @IdContradiccion";
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM RelacionesEjecutoria WHERE IdContradiccion = @IdContradiccion";
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
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
