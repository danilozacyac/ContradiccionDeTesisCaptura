﻿using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class ContradiccionesModel
    {
        public int SetNewContradiccion(Contradicciones contradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                contradiccion.IdContradiccion = DataBaseUtilities.GetNextIdForUse("Contradicciones", "IdContradiccion", connection);

                string sqlCadena = "SELECT * FROM Contradicciones WHERE IdContradiccion = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Contradiccion");

                dr = dataSet.Tables["Contradiccion"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["ExpedienteNumero"] = contradiccion.ExpedienteNumero;
                dr["ExpedienteAnio"] = contradiccion.ExpedienteAnio;
                dr["IdTipoAsunto"] = contradiccion.IdTipoAsunto;
                dr["Tema"] = contradiccion.Tema;
                dr["Status"] = contradiccion.Status;
                //dr["Oficio"] = contradiccion.Oficio;

                if(contradiccion.FechaTurno == null)
                    dr["FechaTurno"] = System.DBNull.Value;
                else
                    dr["FechaTurno"] =  contradiccion.FechaTurno;

                
                dr["Observaciones"] = contradiccion.Observaciones;
                dr["Denunciantes"] = contradiccion.Denunciantes;
                dr["IdPlenoCircuito"] = contradiccion.IdPlenoCircuito;
                dr["IdPresidentePleno"] = contradiccion.IdPresidentePleno;
                dr["IdPonentePleno"] = contradiccion.IdPonentePleno;
                dr["ExpedienteProvisional"] = contradiccion.ExpProvisional;

                dataSet.Tables["Contradiccion"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Contradicciones(IdContradiccion,ExpedienteNumero,ExpedienteAnio,IdTipoAsunto,Tema,Status," +
                                                        "FechaTurno,Observaciones,Denunciantes,IdPlenoCircuito,IdPresidentePleno,IdPonentePleno,ExpedienteProvisional)" +
                                                        " VALUES(@IdContradiccion,@ExpedienteNumero,@ExpedienteAnio,@IdTipoAsunto,@Tema,@Status," +
                                                        "@FechaTurno,@Observaciones,@Denunciantes,@IdPlenoCircuito,@IdPresidentePleno,@IdPonentePleno,@ExpedienteProvisional)";

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteNumero", OleDbType.Numeric, 0, "ExpedienteNumero");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteAnio", OleDbType.Numeric, 0, "ExpedienteAnio");
                dataAdapter.InsertCommand.Parameters.Add("@IdTipoAsunto", OleDbType.Numeric, 0, "IdTipoAsunto");
                dataAdapter.InsertCommand.Parameters.Add("@Tema", OleDbType.VarChar, 0, "Tema");
                dataAdapter.InsertCommand.Parameters.Add("@Status", OleDbType.Numeric, 0, "Status");
                dataAdapter.InsertCommand.Parameters.Add("@FechaTurno", OleDbType.Date, 0, "FechaTurno");
                dataAdapter.InsertCommand.Parameters.Add("@Observaciones", OleDbType.VarChar, 0, "Observaciones");
                dataAdapter.InsertCommand.Parameters.Add("@Denunciantes", OleDbType.VarChar, 0, "Denunciantes");
                dataAdapter.InsertCommand.Parameters.Add("@IdPlenoCircuito", OleDbType.Numeric, 0, "IdPlenoCircuito");
                dataAdapter.InsertCommand.Parameters.Add("@IdPresidentePleno", OleDbType.Numeric, 0, "IdPresidentePleno");
                dataAdapter.InsertCommand.Parameters.Add("@IdPonentePleno", OleDbType.Numeric, 0, "IdPonentePleno");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteProvisional", OleDbType.VarChar, 0, "ExpedienteProvisional");

                dataAdapter.Update(dataSet, "Contradiccion");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return contradiccion.IdContradiccion;
        }

        /// <summary>
        /// Verifica si la contradicción en cuestión ya se encuentra en la base de datos
        /// </summary>
        /// <param name="numero">Número de expediente</param>
        /// <param name="year">Año del expediente</param>
        /// <returns></returns>
        public bool CheckIfExist(int numero, int year,string tema, string denunciante)
        {
            bool doExist = false;

            string sqlCmd = @"SELECT * FROM Contradicciones " +
                            " WHERE ExpedienteNumero = @ExpedienteNumero AND ExpedienteAnio = @ExpedienteAnio AND Tema Like '"+ tema + "%' and Denunciantes LIKE '" + denunciante + "%'";

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connection;
            cmd.CommandText = sqlCmd;

            try
            {
                cmd.Parameters.AddWithValue("@ExpedienteNumero",numero);
                cmd.Parameters.AddWithValue("@ExpedienteAnio", year);

                connection.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return doExist;
        }


        

        /// <summary>
        /// Enlista las Contradicciones que han sido registradas hasta el momento
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Contradicciones> GetContradicciones()
        {
            ObservableCollection<Contradicciones> contradicciones = new ObservableCollection<Contradicciones>();

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            CriteriosModel critModel = new CriteriosModel();
            TesisModel tesModel = new TesisModel();
            EjecutoriasModel ejecModel = new EjecutoriasModel();
            ReturnosModel returnoModel = new ReturnosModel();
            ResolucionModel resolucion = new ResolucionModel();
            OficiosModel oficios = new OficiosModel();
            AdmisorioModel admin = new AdmisorioModel();

            string oleCadena = "SELECT * FROM Contradicciones WHERE IdContradiccion > 0 ORDER By ExpedienteAnio,ExpedienteNumero";

            try
            {
                connection.Open();

                cmd = new OleDbCommand(oleCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contradicciones contra = new Contradicciones();
                    contra.IdContradiccion = (Int32)reader["IdContradiccion"];
                    contra.ExpedienteNumero = Convert.ToInt32(reader["ExpedienteNumero"]);
                    contra.ExpedienteAnio = Convert.ToInt32(reader["ExpedienteAnio"]);
                    contra.IdTipoAsunto = Convert.ToInt32(reader["IdTipoAsunto"]);
                    contra.Tema = reader["Tema"].ToString();
                    contra.Status = Convert.ToInt32(reader["Status"]);
                    //contra.Oficio = reader["Oficio"].ToString();
                    contra.FechaTurno = DateTimeUtilities.GetDateFromReader(reader,"FechaTurno");
                    contra.Observaciones = reader["Observaciones"].ToString();
                    contra.Denunciantes = reader["Denunciantes"].ToString();
                    contra.IdPlenoCircuito = Convert.ToInt32(reader["IdPlenoCircuito"]);
                    contra.IdPresidentePleno = Convert.ToInt32(reader["IdPresidentePleno"]);
                    contra.IdPonentePleno = Convert.ToInt32(reader["IdPonentePleno"]);
                    contra.Criterios = critModel.GetCriterios(contra.IdContradiccion);
                    contra.MiTesis = tesModel.GetTesisPorContradiccion(contra.IdContradiccion);
                    contra.MiEjecutoria = ejecModel.GetEjecutoriasPorContradiccion(contra.IdContradiccion);
                    contra.Returnos = returnoModel.GetReturnos(contra.IdContradiccion);
                    contra.Resolutivo = resolucion.GetResolucion(contra.IdContradiccion);
                    contra.IsComplete = Convert.ToBoolean(reader["Completa"] as Int16? ?? 0);
                    contra.ExpProvisional = reader["ExpedienteProvisional"].ToString();
                    contra.Oficios = oficios.GetOficios(contra.IdContradiccion);
                    contra.AcAdmisorio = admin.GetAcuerdo(contra.IdContradiccion);

                    contradicciones.Add(contra);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return contradicciones;
        }


        /// <summary>
        /// Actualiza la información de una de las contradicciones registradas
        /// </summary>
        /// <param name="contradiccion"></param>
        public void UpdateContradiccion(Contradicciones contradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Contradicciones WHERE IdContradiccion =" + contradiccion.IdContradiccion;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Contradicciones");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["ExpedienteNumero"] = contradiccion.ExpedienteNumero;
                dr["ExpedienteAnio"] = contradiccion.ExpedienteAnio;
                dr["IdTipoAsunto"] = contradiccion.IdTipoAsunto;
                dr["Tema"] = contradiccion.Tema;
                dr["Status"] = contradiccion.Status;

                if (contradiccion.FechaTurno != null)
                    dr["FechaTurno"] = contradiccion.FechaTurno;
                else
                    dr["FechaTurno"] = System.DBNull.Value;

                dr["Observaciones"] = contradiccion.Observaciones;
                dr["Denunciantes"] = contradiccion.Denunciantes;
                dr["IdPlenoCircuito"] = contradiccion.IdPlenoCircuito;
                dr["IdPresidentePleno"] = contradiccion.IdPresidentePleno;
                dr["IdPonentePleno"] = contradiccion.IdPonentePleno;
                dr["ExpedienteProvisional"] = contradiccion.ExpProvisional;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Contradicciones SET ExpedienteNumero = @ExpedienteNumero,ExpedienteAnio = @ExpedienteAnio," +
                                                       "IdTipoAsunto = @IdTipoAsunto,Tema = @Tema,Status = @Status," +
                                                       "FechaTurno = @FechaTurno,Observaciones = @Observaciones,Denunciantes = @Denunciantes," +
                                                       "IdPlenoCircuito = @IdPlenoCircuito,IdPresidentePleno = @IdPresidentePleno,IdPonentePleno = @IdPonentePleno,ExpedienteProvisional = @ExpedienteProvisional " +
                                                       " WHERE IdContradiccion = @Idcontradiccion";

                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteNumero", OleDbType.Numeric, 0, "ExpedienteNumero");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteAnio", OleDbType.Numeric, 0, "ExpedienteAnio");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTipoAsunto", OleDbType.Numeric, 0, "IdTipoAsunto");
                dataAdapter.UpdateCommand.Parameters.Add("@Tema", OleDbType.VarChar, 0, "Tema");
                dataAdapter.UpdateCommand.Parameters.Add("@Status", OleDbType.Numeric, 0, "Status");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaTurno", OleDbType.Date, 0, "FechaTurno");
                dataAdapter.UpdateCommand.Parameters.Add("@Observaciones", OleDbType.VarChar, 0, "Observaciones");
                dataAdapter.UpdateCommand.Parameters.Add("@Denunciantes", OleDbType.VarChar, 0, "Denunciantes");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPlenoCircuito", OleDbType.Numeric, 0, "IdPlenoCircuito");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPresidentePleno", OleDbType.Numeric, 0, "IdPresidentePleno");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPonentePleno", OleDbType.Numeric, 0, "IdPonentePleno");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteProvisional", OleDbType.VarChar, 0, "ExpedienteProvisional");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");

                dataAdapter.Update(dataSet, "Contradicciones");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Actualiza el estado del registro
        /// </summary>
        /// <param name="contradiccion"></param>
        public void UpdateContradiccionStatus(Contradicciones contradiccion)
        {
            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Contradicciones WHERE IdContradiccion = @IdContradiccion";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                dataAdapter.Fill(dataSet, "Contradicciones");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Completa"] = (contradiccion.IsComplete) ? 1 : 0;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE Contradicciones SET Completa = @Completa " +
                                                       " WHERE IdContradiccion = @Idcontradiccion";

                dataAdapter.UpdateCommand.Parameters.Add("@Completa", OleDbType.Numeric, 0, "Completa");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");

                dataAdapter.Update(dataSet, "Contradicciones");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Elimina el registro de la contradiccion seleccionada
        /// </summary>
        /// <param name="contradiccion"></param>
        /// <returns></returns>
        public bool DeleteContradiccion(Contradicciones contradiccion)
        {
            bool isDeleteComplete = false;

            string sqlCmd = @"DELETE FROM Contradicciones WHERE IdContradiccion = @IdContradiccion";

            OleDbConnection connection = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connection;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = "@IdContradiccion";
                parameter.OleDbType = OleDbType.Numeric;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = contradiccion.IdContradiccion;

                cmd.Parameters.Add(parameter);

                connection.Open();
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
                
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ContradiccionesModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return isDeleteComplete;
        }
    }
}