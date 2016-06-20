using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ScjnUtilities;

namespace ContradiccionesDirectorioApi.Model
{
    public class ConsultaModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CT"].ConnectionString;



        public ObservableCollection<Consulta> GetContradicciones()
        {
            ObservableCollection<Consulta> contradicciones = new ObservableCollection<Consulta>();

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand cmd;
            SqlDataReader reader;


            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM vConsulta ORDER BY ExpedienteAnio desc, ExpedienteNumero, Descripcion, Tema", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Consulta contra = new Consulta()
                    {
                        Id = (Int32)reader["IdContradiccion"],
                        Expediente = String.Format("{0}/{1}", reader["ExpedienteNumero"],reader["ExpedienteAnio"]),
                        Tema = reader["Tema"].ToString(),
                        Pleno = reader["Descripcion"].ToString(),
                        Estado = Convert.ToInt16(reader["Status"]),
                        FechaResolucion = DateTimeUtilities.GetDateFromReader(reader,"FechaResolucion")
                       
                    };


                    contradicciones.Add(contra);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ConsultaModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ConsultaModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return contradicciones;
        }


    }
}
