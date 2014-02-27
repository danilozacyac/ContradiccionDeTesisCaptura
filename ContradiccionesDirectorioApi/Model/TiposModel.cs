using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using System.Windows.Forms;
using ContradiccionesDirectorioApi.DataAccess;

namespace ContradiccionesDirectorioApi.Model
{
    public class TiposModel
    {


        public List<Tipos> GetTiposAsunto()
        {
            OleDbConnection connectionBitacoraSql = DbConnDac.GetConnection();
            List<Tipos> tipoAuntos = new List<Tipos>();

            OleDbCommand cmd;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT * FROM TipoAsunto";

            try
            {
                connectionBitacoraSql.Open();

                cmd = new OleDbCommand(sqlCadena, connectionBitacoraSql);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tipoAuntos.Add(new Tipos(Convert.ToInt16(reader["IdTipo"]),
                                                reader["TipoAsunto"].ToString()
                                                )
                                      );
                    }
                }
                reader.Close();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                reader.Close();
                connectionBitacoraSql.Close();
            }

            return tipoAuntos;
        }

    }
}
