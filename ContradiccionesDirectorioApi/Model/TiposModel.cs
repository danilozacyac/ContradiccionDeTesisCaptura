using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class TiposModel
    {

        /// <summary>
        /// Enlista los diferentes tipo de Asunto por lo cuales se genera un registro
        /// </summary>
        /// <returns></returns>
        public List<Tipos> GetTiposAsunto()
        {
            SqlConnection connection = DbConnDac.GetConnection();
            List<Tipos> tipoAuntos = new List<Tipos>();

            SqlCommand cmd;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM cTipoAsunto", connection);
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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TiposModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TiposModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return tipoAuntos;
        }

    }
}
