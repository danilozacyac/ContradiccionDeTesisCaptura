using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ContradiccionesDirectorioApi.Utils;

namespace ContradiccionesDirectorioApi.Model
{
    public class ReturnosModel
    {

        public void SetNewReturno(Contradicciones contradiccion)
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                foreach (ReturnosClass returno in contradiccion.Returnos)
                {

                    string sqlCadena = "SELECT * FROM Returnos WHERE IdContradiccion = 0";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionBitacoraSql);

                    dataAdapter.Fill(dataSet, "Returnos");

                    dr = dataSet.Tables["Returnos"].NewRow();
                    dr["IdContradiccion"] = contradiccion.IdContradiccion;
                    dr["Fecha"] = returno.Fecha;
                    dr["FechaInt"] = DateTimeFunctions.ConvertDateToInt(returno.Fecha);
                    dr["IdOrgOrigen"] = returno.IdOrganoOrigen;
                    dr["IdOrgDestino"] = returno.IdOrganoDestino;
                    dr["ExpOrigen"] = returno.ExpOrigen;
                    dr["ExpDestino"] = returno.ExpDestino;

                    dataSet.Tables["Returnos"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Returnos(IdContradiccion,Fecha,FechaInt,IdOrgOrigen," + 
                                                            "IdOrgDestino,ExpOrigen,ExpDestino)" +
                                                            " VALUES(@IdContradiccion,@Fecha,@FechaInt,@IdOrgOrigen," +
                                                            "@IdOrgDestino,@ExpOrigen,@ExpDestino)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdContradiccion", OleDbType.Numeric, 0, "IdContradiccion");
                    dataAdapter.InsertCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                    dataAdapter.InsertCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                    dataAdapter.InsertCommand.Parameters.Add("@IdOrgOrigen", OleDbType.Numeric, 0, "IdOrgOrigen");
                    dataAdapter.InsertCommand.Parameters.Add("@IdOrgDestino", OleDbType.Numeric, 0, "IdOrgDestino");
                    dataAdapter.InsertCommand.Parameters.Add("@ExpOrigen", OleDbType.VarChar, 0, "ExpOrigen");
                    dataAdapter.InsertCommand.Parameters.Add("@ExpDestino", OleDbType.VarChar, 0, "ExpDestino");

                    dataAdapter.Update(dataSet, "Returnos");

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

    }
}
