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
    public class ContradiccionesModel
    {
        public int SetNewContradiccion(Contradicciones contradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                contradiccion.IdContradiccion = DataBaseUtilities.GetNextIdForUse("Contradicciones", "IdContradiccion", connection);

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Contradicciones WHERE IdContradiccion = 0", connection);

                dataAdapter.Fill(dataSet, "Contradiccion");

                dr = dataSet.Tables["Contradiccion"].NewRow();
                dr["IdContradiccion"] = contradiccion.IdContradiccion;
                dr["ExpedienteNumero"] = contradiccion.ExpedienteNumero;
                dr["ExpedienteAnio"] = contradiccion.ExpedienteAnio;
                dr["IdTipoAsunto"] = contradiccion.IdTipoAsunto;
                dr["Tema"] = contradiccion.Tema;
                dr["Status"] = contradiccion.Status;
                //dr["Oficio"] = contradiccion.Oficio;

                if (contradiccion.FechaTurno == null)
                    dr["FechaTurno"] = DBNull.Value;
                else
                    dr["FechaTurno"] = contradiccion.FechaTurno;


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

                dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteNumero", SqlDbType.Int, 0, "ExpedienteNumero");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteAnio", SqlDbType.Int, 0, "ExpedienteAnio");
                dataAdapter.InsertCommand.Parameters.Add("@IdTipoAsunto", SqlDbType.Int, 0, "IdTipoAsunto");
                dataAdapter.InsertCommand.Parameters.Add("@Tema", SqlDbType.VarChar, 0, "Tema");
                dataAdapter.InsertCommand.Parameters.Add("@Status", SqlDbType.Int, 0, "Status");
                dataAdapter.InsertCommand.Parameters.Add("@FechaTurno", SqlDbType.Date, 0, "FechaTurno");
                dataAdapter.InsertCommand.Parameters.Add("@Observaciones", SqlDbType.VarChar, 0, "Observaciones");
                dataAdapter.InsertCommand.Parameters.Add("@Denunciantes", SqlDbType.VarChar, 0, "Denunciantes");
                dataAdapter.InsertCommand.Parameters.Add("@IdPlenoCircuito", SqlDbType.Int, 0, "IdPlenoCircuito");
                dataAdapter.InsertCommand.Parameters.Add("@IdPresidentePleno", SqlDbType.Int, 0, "IdPresidentePleno");
                dataAdapter.InsertCommand.Parameters.Add("@IdPonentePleno", SqlDbType.Int, 0, "IdPonentePleno");
                dataAdapter.InsertCommand.Parameters.Add("@ExpedienteProvisional", SqlDbType.VarChar, 0, "ExpedienteProvisional");

                dataAdapter.Update(dataSet, "Contradiccion");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
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

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand() { Connection = connection, CommandText = sqlCmd };

            try
            {
                cmd.Parameters.AddWithValue("@ExpedienteNumero",numero);
                cmd.Parameters.AddWithValue("@ExpedienteAnio", year);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doExist = true;
                }

            }
            catch (SqlException ex)
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

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;


            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM Contradicciones WHERE IdContradiccion > 0 ORDER By ExpedienteAnio,ExpedienteNumero", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contradicciones contra = new Contradicciones()
                    {
                        IdContradiccion = (Int32)reader["IdContradiccion"],
                        ExpedienteNumero = Convert.ToInt32(reader["ExpedienteNumero"]),
                        ExpedienteAnio = Convert.ToInt32(reader["ExpedienteAnio"]),
                        IdTipoAsunto = Convert.ToInt32(reader["IdTipoAsunto"]),
                        Tema = reader["Tema"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        FechaTurno = DateTimeUtilities.GetDateFromReader(reader, "FechaTurno"),
                        Observaciones = reader["Observaciones"].ToString(),
                        Denunciantes = reader["Denunciantes"].ToString(),
                        IdPlenoCircuito = Convert.ToInt32(reader["IdPlenoCircuito"]),
                        IdPresidentePleno = Convert.ToInt32(reader["IdPresidentePleno"]),
                        IdPonentePleno = Convert.ToInt32(reader["IdPonentePleno"]),
                        IsComplete = Convert.ToBoolean(reader["Completa"] as Int16? ?? 0),
                        ExpProvisional = reader["ExpedienteProvisional"].ToString()
                    };
                    

                    contradicciones.Add(contra);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
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

        public Contradicciones GetContradicciones(int idContradiccion)
        {
            Contradicciones contradiccion = null;

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd;
            SqlDataReader reader;


            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM Contradicciones WHERE IdContradiccion = @IdContradiccion ORDER By ExpedienteAnio,ExpedienteNumero", connection);
                cmd.Parameters.AddWithValue("@IdContradiccion", idContradiccion);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    contradiccion = new Contradicciones()
                    {
                        IdContradiccion = (Int32)reader["IdContradiccion"],
                        ExpedienteNumero = Convert.ToInt32(reader["ExpedienteNumero"]),
                        ExpedienteAnio = Convert.ToInt32(reader["ExpedienteAnio"]),
                        IdTipoAsunto = Convert.ToInt32(reader["IdTipoAsunto"]),
                        Tema = reader["Tema"].ToString(),
                        Status = Convert.ToInt32(reader["Status"]),
                        FechaTurno = DateTimeUtilities.GetDateFromReader(reader, "FechaTurno"),
                        Observaciones = reader["Observaciones"].ToString(),
                        Denunciantes = reader["Denunciantes"].ToString(),
                        IdPlenoCircuito = Convert.ToInt32(reader["IdPlenoCircuito"]),
                        IdPresidentePleno = Convert.ToInt32(reader["IdPresidentePleno"]),
                        IdPonentePleno = Convert.ToInt32(reader["IdPonentePleno"]),
                        IsComplete = Convert.ToBoolean(reader["Completa"] as Int16? ?? 0),
                        ExpProvisional = reader["ExpedienteProvisional"].ToString()
                    };


                }

                reader.Close();
                cmd.Dispose();

                this.GetContradiccionComplementInfo(ref contradiccion);
            }
            catch (SqlException ex)
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

            return contradiccion;
        }

        public void GetContradiccionComplementInfo(ref Contradicciones contradiccion)
        {
            contradiccion.Criterios = new CriteriosModel().GetCriterios(contradiccion.IdContradiccion);
            contradiccion.MiTesis = new TesisModel().GetTesisPorContradiccion(contradiccion.IdContradiccion);
            contradiccion.MiEjecutoria = new EjecutoriasModel().GetEjecutoriasPorContradiccion(contradiccion.IdContradiccion);
            contradiccion.Returnos = new ReturnosModel().GetReturnos(contradiccion.IdContradiccion);
            contradiccion.Resolutivo = new ResolucionModel().GetResolucion(contradiccion.IdContradiccion);
            contradiccion.Oficios = new OficiosModel().GetOficios(contradiccion.IdContradiccion);
            contradiccion.AcAdmisorio = new AdmisorioModel().GetAcuerdo(contradiccion.IdContradiccion);
        }

        /// <summary>
        /// Actualiza la información de una de las contradicciones registradas
        /// </summary>
        /// <param name="contradiccion"></param>
        public void UpdateContradiccion(Contradicciones contradiccion)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Contradicciones WHERE IdContradiccion =" + contradiccion.IdContradiccion;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

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
                    dr["FechaTurno"] = DBNull.Value;

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

                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteNumero", SqlDbType.Int, 0, "ExpedienteNumero");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteAnio", SqlDbType.Int, 0, "ExpedienteAnio");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTipoAsunto", SqlDbType.Int, 0, "IdTipoAsunto");
                dataAdapter.UpdateCommand.Parameters.Add("@Tema", SqlDbType.VarChar, 0, "Tema");
                dataAdapter.UpdateCommand.Parameters.Add("@Status", SqlDbType.Int, 0, "Status");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaTurno", SqlDbType.Date, 0, "FechaTurno");
                dataAdapter.UpdateCommand.Parameters.Add("@Observaciones", SqlDbType.VarChar, 0, "Observaciones");
                dataAdapter.UpdateCommand.Parameters.Add("@Denunciantes", SqlDbType.VarChar, 0, "Denunciantes");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPlenoCircuito", SqlDbType.Int, 0, "IdPlenoCircuito");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPresidentePleno", SqlDbType.Int, 0, "IdPresidentePleno");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPonentePleno", SqlDbType.Int, 0, "IdPonentePleno");
                dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteProvisional", SqlDbType.VarChar, 0, "ExpedienteProvisional");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");

                dataAdapter.Update(dataSet, "Contradicciones");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
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
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Contradicciones WHERE IdContradiccion = @IdContradiccion", connection);
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

                dataAdapter.UpdateCommand.Parameters.Add("@Completa", SqlDbType.Int, 0, "Completa");
                dataAdapter.UpdateCommand.Parameters.Add("@IdContradiccion", SqlDbType.Int, 0, "IdContradiccion");

                dataAdapter.Update(dataSet, "Contradicciones");
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
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

            const string SqlQuery = @"DELETE FROM Contradicciones WHERE IdContradiccion = @IdContradiccion";

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = new SqlCommand() { Connection = connection, CommandText = SqlQuery, CommandType = CommandType.Text };

            try
            {
                cmd.Parameters.AddWithValue("@IdContradiccion", contradiccion.IdContradiccion);
                connection.Open();
                cmd.ExecuteNonQuery();
                isDeleteComplete = true;
                
            }
            catch (SqlException ex)
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