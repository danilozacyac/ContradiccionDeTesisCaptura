using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ContradiccionesDirectorioApi.Model
{
    public class CriteriosModel
    {

        public void SetNewCriterios(Contradicciones contradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                foreach (Criterios criterio in contradiccion.Criterios)
                {
                    int maxOrden = this.GetMaxOrderCriterio(contradiccion.IdContradiccion);

                    string sqlCadena = "SELECT * FROM Criterios WHERE IdCriterio = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                    dataAdapter.Fill(dataSet, "Criterios");

                    dr = dataSet.Tables["Criterios"].NewRow();
                    dr["IdContradiccion"] = contradiccion.IdContradiccion;
                    dr["Orden"] = maxOrden;
                    dr["Criterio"] = criterio.Criterio;
                    dr["IdOrgano"] = criterio.IdOrgano;

                    dataSet.Tables["Criterios"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Criterios(IdContradiccion,Orden,Criterio,IdOrgano)" +
                                                            " VALUES(@IdContradiccion,@Orden,@Criterio,@IdOrgano)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@Orden", OleDbType.Numeric, 0, "Orden");
                    dataAdapter.InsertCommand.Parameters.Add("@Criterio", OleDbType.VarChar, 0, "Criterio");
                    dataAdapter.InsertCommand.Parameters.Add("@IdOrgano", OleDbType.Numeric, 0, "IdOrgano");

                    dataAdapter.Update(dataSet, "Criterios");

                    dataSet.Dispose();
                    dataAdapter.Dispose();

                    criterio.IdCriterio = this.GetLastCriterioId(contradiccion.IdContradiccion, maxOrden);

                    this.SetNewCriteriosTesis(criterio);
                }
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


        private void SetNewCriteriosTesis(Criterios criterio)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                foreach (int tesis in criterio.TesisContendientes)
                {
                    string sqlCadena = "SELECT * FROM CriteriosTesis WHERE IdCriterio = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                    dataAdapter.Fill(dataSet, "Criterios");

                    dr = dataSet.Tables["Criterios"].NewRow();
                    dr["IdCriterio"] = criterio.IdCriterio;
                    dr["IUS"] = tesis;

                    dataSet.Tables["Criterios"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO CriteriosTesis(IdCriterio,IUS)" +
                                                            " VALUES(@IdCriterio,@IUS)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdCriterio", OleDbType.Numeric, 0, "IdCriterio");
                    dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Numeric, 0, "IUS");

                    dataAdapter.Update(dataSet, "Criterios");

                    dataSet.Dispose();
                    dataAdapter.Dispose();

                }
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


        private int GetMaxOrderCriterio(int idContradiccion)
        {
            int maxOrden = 0;

            string sqlCmd = @"SELECT Max(Orden) Orden FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion ";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = "@IdContradiccion";
                parameter.OleDbType = OleDbType.Numeric;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = idContradiccion;

                cmd.Parameters.Add(parameter);

                
                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    maxOrden = Convert.ToInt32(reader["Orden"]);
                }
            }
            catch (OleDbException ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }

            return maxOrden + 1;
        }

        private int GetLastCriterioId(int idContradiccion,int maxOrden)
        {
            int lastId = 0;

            string sqlCmd = @"SELECT IdCriterio FROM Criterios " +
                            " WHERE IdContradiccion = @IdContradiccion AND Orden = @Orden";

            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connectionBitacoraSql;
            cmd.CommandText = sqlCmd;
            cmd.CommandType = CommandType.Text;

            try
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = "@IdContradiccion";
                parameter.OleDbType = OleDbType.Numeric;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = idContradiccion;

                cmd.Parameters.Add(parameter);

                OleDbParameter parameter2 = new OleDbParameter();
                parameter2.ParameterName = "@Orden";
                parameter2.OleDbType = OleDbType.Numeric;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = maxOrden;

                cmd.Parameters.Add(parameter2);

                connectionBitacoraSql.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lastId = Convert.ToInt32(reader["IdCriterio"]);
                }
            }
            catch (OleDbException ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }

            return lastId;
        }


        public ObservableCollection<Criterios> GetCriterios(int idContradiccion)
        {
            ObservableCollection<Criterios> criterios = new ObservableCollection<Criterios>();

            OleDbConnection oleConnection = DbConnDac.GetConnection();
            OleDbCommand cmd;
            OleDbDataReader reader;

            string oleCadena = "SELECT * FROM Criterios WHERE idContradiccion = " + idContradiccion;

            try
            {
                oleConnection.Open();

                cmd = new OleDbCommand(oleCadena, oleConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Criterios criterio = new Criterios();
                    criterio.IdContradiccion = Convert.ToInt32(reader["IdContradiccion"]);
                    criterio.IdCriterio = Convert.ToInt32(reader["IdCriterio"]);
                    criterio.Orden = Convert.ToInt32(reader["Orden"]);
                    criterio.Criterio = reader["Criterio"].ToString();
                    criterio.IdOrgano = Convert.ToInt32(reader["IdOrgano"]);
                    criterio.Organo = (from n in Singletons.OrganismosSingleton.Colegiados
                                       where n.IdOrganismo == criterio.IdOrgano
                                       select n.Organismo).ToList()[0];
                    

                    criterios.Add(criterio);
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

            return criterios;
        }
    }
}
