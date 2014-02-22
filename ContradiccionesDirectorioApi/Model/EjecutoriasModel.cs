using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;

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
                dr["FechaResolucion"] = contradiccion.MiTesis.ClaveControl;
                dr["FechaResolucionInt"] = contradiccion.MiTesis.ClaveIdentificacion;
                dr["FechaEngrose"] = contradiccion.MiTesis.Rubro;
                dr["FechaEngroseInt"] = contradiccion.MiTesis.Tatj;
                dr["SISE"] = contradiccion.MiTesis.OficioPublicacion;
                dr["Responsable"] = contradiccion.MiTesis.OficioPublicacionFilePath;
                dr["Signatario"] = contradiccion.MiTesis.VersionPublica;
                dr["Oficio"] = contradiccion.MiTesis.VersionPublicaFilePath;
                dr["FileEjecPath"] = contradiccion.MiTesis.CopiaCertificada;

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
