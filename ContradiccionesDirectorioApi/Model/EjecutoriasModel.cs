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

                this.SetRelacionesEjecutorias(contradiccion.MiEjecutoria.TesisRelacionadas, contradiccion.IdContradiccion, 1);
                this.SetRelacionesEjecutorias(contradiccion.MiEjecutoria.VotosRelacionados, contradiccion.IdContradiccion, 3);
            }
            catch (OleDbException ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }


        }

        private void SetRelacionesEjecutorias(ObservableCollection<int> regIus,int idContradiccion, int tipoRelacion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                foreach (int ius in regIus)
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
            }
            catch (OleDbException ex)
            {
                Console.Write(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }


        }


        public Ejecutoria GetEjecutoriasPorContradiccion(int idContradiccion)
        {
            Ejecutoria ejecutoria = null;

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
                    ejecutoria = new Ejecutoria();
                    ejecutoria.FechaResolucion = Convert.ToDateTime(reader["FechaResolucion"]);
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

            return ejecutoria;
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

            return regIus;
        }
    }
}
