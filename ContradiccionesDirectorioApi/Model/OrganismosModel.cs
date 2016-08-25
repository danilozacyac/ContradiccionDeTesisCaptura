using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.DataAccess;
using ScjnUtilities;
using System.Data.OleDb;

namespace ContradiccionesDirectorioApi.Model
{
    public class OrganismosModel
    {


        /// <summary>
        /// Enlista los Tribunales Colegiados, Tribunales Unitarios o Juzgados de Distrito según sea el caso
        /// </summary>
        /// <param name="tipoOrganismo"></param>
        /// <returns></returns>
        public List<Organismos> GetOrganismos(int tipoOrganismo)
        {
            List<Organismos> organismos = new List<Organismos>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Directorio"].ToString());
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            String sqlCadena = "SELECT O.*, C.Ciudad, E.Abrev " +
                               "FROM Organismos O INNER JOIN (Ciudades C INNER JOIN Estados E ON C.IdEstado = E.IdEstado) " + 
                               "ON O.IdCiudad = C.IdCiudad WHERE IdTpoOrg = @TipoOrg ORDER BY OrdenImpr";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@TipoOrg", tipoOrganismo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Organismos organismoAdd = new Organismos()
                        {
                            IdOrganismo = reader["IdOrganismo"] as int? ?? -1,
                            TipoOrganismo = reader["IdTpoOrg"] as int? ?? -1,
                            Circuito = reader["IdCircuito"] as int? ?? -1,
                            Ordinal = reader["IdOrdinal"] as int? ?? -1,
                            Materia = reader["IdMateria"] as int? ?? -1,
                            Organismo = reader["Organismo"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Telefonos = reader["Tels"].ToString(),
                            Ciudad = reader["IdCiudad"] as int? ?? -1,
                            Integrantes = reader["Integrantes"] as int? ?? -1,
                            OrdenImpresion = reader["OrdenImpr"] as int? ?? -1
                        };

                        organismos.Add(organismoAdd);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return organismos;
        }


        public ObservableCollection<Organismos> GetPlenos()
        {
            ObservableCollection<Organismos> organismos = new ObservableCollection<Organismos>();

            SqlConnection connection = DbConnDac.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM cPlenoC Order By OrdenImpr", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //int age = reader["Age"] as int? ?? -1;
                        Organismos organismoAdd = new Organismos() {
                            IdOrganismo = reader["IdPleno"] as int? ?? -1,
                            Organismo = String.Format("{0}({1})", reader["Descripcion"], reader["Especializacion"]), 
                            OrdenImpresion = reader["OrdenImpr"] as int? ?? -1 };

                        organismos.Add(organismoAdd);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

            return organismos;
        }

        public void SetNewPleno(Organismos organismo)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                organismo.IdOrganismo = DataBaseUtilities.GetNextIdForUse("cPlenoC", "IdPleno", connection);

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM cPlenoC WHERE IdPleno = 0", connection);

                dataAdapter.Fill(dataSet, "cPlenoC");

                dr = dataSet.Tables["cPlenoC"].NewRow();
                dr["Descripcion"] = organismo.Organismo;
                dr["Especializacion"] = organismo.Especialidad;

                dataSet.Tables["cPlenoC"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO cPlenoC(Descripcion,Especializacion)" +
                                                        " VALUES(@Descripcion,@Especializacion)";

                dataAdapter.InsertCommand.Parameters.Add("@Descripcion", SqlDbType.VarChar, 0, "Descripcion");
                dataAdapter.InsertCommand.Parameters.Add("@Especializacion", SqlDbType.VarChar, 0, "Especializacion");

                dataAdapter.Update(dataSet, "cPlenoC");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }

        public void UpdatePleno(Organismos organismo)
        {
            SqlConnection connection = DbConnDac.GetConnection();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {

                string sqlCadena = "SELECT * FROM cPlenoC WHERE IdPleno = " + organismo.IdOrganismo;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "cPlenoC");

                dr = dataSet.Tables["cPlenoC"].Rows[0];
                dr.BeginEdit();
                dr["Descripcion"] = organismo.Organismo;
                dr["Especializacion"] = organismo.Especialidad;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE cPlenoC SET Descripcion = @Descripcion, Especializacion = @Especializacion WHERE IdPleno = @IdPleno";

                dataAdapter.UpdateCommand.Parameters.Add("@Descripcion", SqlDbType.VarChar, 0, "Descripcion");
                dataAdapter.UpdateCommand.Parameters.Add("@Especializacion", SqlDbType.VarChar, 0, "Especializacion");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPleno", SqlDbType.Int, 0, "IdPleno");

                dataAdapter.Update(dataSet, "cPlenoC");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
