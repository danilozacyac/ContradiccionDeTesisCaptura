using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;
using ScjnUtilities;

namespace TableWordToDb
{
    public class WordInfoProcess
    {
        public void GetListaContradiccionesFaltantes()
        {
            ContradiccionesModel cModel = new ContradiccionesModel();

            List<Contradicciones> listaContradicciones = new List<Contradicciones>();

            ReadTable readInfo = new ReadTable();
            List<TableInfo> listaCt = readInfo.GetTableInfo();
            
            foreach (TableInfo info in listaCt)
            {
                string asuntoTemp = this.ReplaceWeirdChars(info.Asunto.Replace("CT", " "));

                string[] asunto = asuntoTemp.Split('/');

                int expNumero = 0;
                int expYear = 0;
                string observaciones = "";

                Int32.TryParse(asunto[0], out expNumero);

                if (expNumero != 0) 
                    expYear = Convert.ToInt32(asunto[1].Substring(0, 4));

                Contradicciones contradiccion = new Contradicciones();

                if (expNumero != 0 && expYear != 0)
                {
                    contradiccion.ExpedienteNumero = expNumero;
                    contradiccion.ExpedienteAnio = expYear;

                    if (asunto[1].Contains("("))
                    {
                        int index = asunto[1].IndexOf("(");
                        observaciones = asunto[1].Substring(index) + "/" + asunto[2];
                    }
                }
                else
                {
                    contradiccion.ExpProvisional = this.ReplaceWeirdChars(info.Asunto);
                }

                contradiccion.Status = 0;
                contradiccion.Denunciantes = this.ReplaceWeirdChars(info.Denunciante);
                contradiccion.AcAdmisorio = this.GetAcAdmisorio(info.Acuerdoad);
                contradiccion.Oficios = this.GetOficios(info.Oficio);
                contradiccion.IdTipoAsunto = 1;
                contradiccion.Observaciones = observaciones;
                contradiccion.Criterios = this.GetCriteriosContendientes(info.OrgContencientes, info.Criterios);
                contradiccion.Tema = this.ReplaceWeirdChars(info.Tema);
                contradiccion.MiEjecutoria = new Ejecutoria();
                contradiccion.MiEjecutoria.FechaResolucion = DateTimeUtilities.ToShortDateFormat(info.FechaResolucion, info.FechaResolucion);
                contradiccion.Resolutivo = new Resolutivos();
                contradiccion.Resolutivo = this.GetPuntosResolutivos(info.Resolutivos);

                if ((!String.IsNullOrWhiteSpace(info.Tesis) || !String.IsNullOrEmpty(info.Tesis)) && !info.Tesis.Contains("PENDIENTE DE PUBLICACIÓN"))
                {
                    contradiccion.MiTesis = this.GetTesis(info.Tesis);
                }

                listaContradicciones.Add(contradiccion);
            }

            this.InsertContradiccionesFaltantes(listaContradicciones);
        }

        private void InsertContradiccionesFaltantes(List<Contradicciones> listaContradicciones)
        {
            foreach (Contradicciones contradiccion in listaContradicciones)
            {
                ContradiccionesModel contra = new ContradiccionesModel();
                contradiccion.IdContradiccion = contra.SetNewContradiccion(contradiccion);

                //Actualiza Info ejecutoria
                EjecutoriasModel eje = new EjecutoriasModel();
                eje.SetNewEjecutoriaPorContradiccion(contradiccion);

                AdmisorioModel admisorio = new AdmisorioModel();
                admisorio.SetNewAdmisorio(contradiccion.AcAdmisorio, contradiccion.IdContradiccion);

                OficiosModel oficios = new OficiosModel();
                foreach (Oficios oficio in contradiccion.Oficios)
                {
                    oficios.SetNewOficio(oficio, contradiccion.IdContradiccion);
                }

                TesisModel tesisModel = new TesisModel();

                if (contradiccion.MiTesis != null)
                    foreach (Tesis tesis in contradiccion.MiTesis)
                    {
                        tesis.IdContradiccion = contradiccion.IdContradiccion;
                        tesisModel.SetNewTesisPorContradiccion(tesis);
                    }

                ///Actualiza info Resolucion
                ResolucionModel resol = new ResolucionModel();
                resol.SetNewResolucion(contradiccion);
                foreach (PResolutivos resuelve in contradiccion.Resolutivo.PuntosResolutivos)
                    resol.SetNewResolutivo(resuelve, contradiccion.IdContradiccion);

                CriteriosModel criterio = new CriteriosModel();
                criterio.SetNewCriterios(contradiccion);
            }
        }

        private string ReplaceWeirdChars(string text)
        {
            text = text.Replace('\r', ' ');
            //text = text.Replace('\l',' ');
            text = text.Replace('\a', ' ');

            return text.Trim();
        }

        private Admisorio GetAcAdmisorio(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                Admisorio admin = new Admisorio();

                int index = text.IndexOf('.');
                if (index > -1)
                {
                    admin.FechaAcuerdo = DateTimeUtilities.ToShortDateFormat(text.Substring(0, index), text.Substring(0, index));

                    if (admin.FechaAcuerdo == null)
                    {
                        admin.Acuerdo = this.ReplaceWeirdChars(text);
                    }
                    else
                    {
                        admin.Acuerdo = this.ReplaceWeirdChars(text.Substring(index + 1));
                    }
                }
                else
                {
                    admin.FechaAcuerdo = null;
                    admin.Acuerdo = String.Empty;
                }

                return admin;
            }
        }

        private ObservableCollection<Criterios> GetCriteriosContendientes(string organismos, string criterios)
        {
            ObservableCollection<Criterios> contienden = new ObservableCollection<Criterios>();

            organismos = this.ReplaceWeirdCharsForSplit(organismos);
            criterios = this.ReplaceWeirdCharsForSplit(criterios);

            string[] org = organismos.Trim().Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            string[] cri = criterios.Trim().Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            if (org.Count() == cri.Count())
            {
                int index = 0;

                while (index < org.Count())
                {
                    Criterios criterio = new Criterios();
                    criterio.Criterio = cri[index];

                    try
                    {
                        criterio.IdOrgano = (from n in OrganismosSingleton.Colegiados
                                             where n.Organismo.ToUpper() == org[index].Replace('.', ' ').Trim()
                                             select n.IdOrganismo).ToList()[0];
                        contienden.Add(criterio);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        criterio.IdOrgano = -1000;
                    }
                    
                    index++;
                }
            }

            return contienden;
        }

        private string ReplaceWeirdCharsForSplit(string text)
        {
            text = text.Replace('\r', '&');
            //text = text.Replace('\l',' ');
            text = text.Replace('\a', '&');

            return text.Trim();
        }

        private ObservableCollection<Oficios> GetOficios(string oficio)
        {
            ObservableCollection<Oficios> listaOficios = new ObservableCollection<Oficios>();
            oficio = this.ReplaceWeirdCharsForSplit(oficio);

            string[] difOficios = oficio.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string ver in difOficios)
            {
                if (!String.IsNullOrEmpty(ver) || !String.IsNullOrWhiteSpace(ver))
                {
                    Oficios newOficio = new Oficios();

                    if (DateTimeUtilities.StringContainsCompleteDate(ver))
                    {
                        Match date = DateTimeUtilities.GetCompleteDate(ver);
                        newOficio.FechaOficio = DateTimeUtilities.ToShortDateFormat(date.Value.ToString(), date.Value.ToString());
                    }

                    if (ver.Contains("CORREO ELECTRÓNICO"))
                    {
                        newOficio.Oficio = ver;

                        listaOficios.Add(newOficio);
                    }
                    else
                    {
                        if (ver.StartsWith("OF. NÚM."))
                        {
                            string splitString = ver.Replace("OF. NÚM.", "").Trim();

                            int index = splitString.IndexOf("DE");

                            if (index != -1)
                            {
                                newOficio.Oficio = splitString.Substring(0, index - 2);
                            }
                            else
                            {
                                newOficio.Oficio = splitString;
                            }

                            listaOficios.Add(newOficio);
                        }
                        else
                        {
                            listaOficios.Add(newOficio);
                        }
                    }
                }
            }
            return listaOficios;
        }

        private Resolutivos GetPuntosResolutivos(string texto)
        {
            Resolutivos resuelven = new Resolutivos();
            ObservableCollection<PResolutivos> puntos = new ObservableCollection<PResolutivos>();

            texto = this.ReplaceWeirdCharsForSplit(texto);

            string[] resolutivos = texto.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string resolutivo in resolutivos)
            {
                PResolutivos res = new PResolutivos();
                res.Resolutivo = resolutivo;

                puntos.Add(res);
            }

            resuelven.PuntosResolutivos = puntos;

            return resuelven;
        }

        private ObservableCollection<Tesis> GetTesis(string texto)
        {
            ObservableCollection<Tesis> misTesis = new ObservableCollection<Tesis>();

            try
            {
                texto = this.ReplaceWeirdCharsForSplit(texto);
                texto = texto.Replace("TESIS AISLADA", "").Trim();
                string[] separados = texto.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                int indexer = 0;

                while (indexer < separados.Count())
                {
                    Tesis tesis = new Tesis();
                    tesis.ClaveControl = separados[indexer++];
                    tesis.Rubro = separados[indexer++];
                    tesis.Ius = Convert.ToInt32(separados[indexer].Replace("REG. IUS", "").Trim());
                    indexer = indexer + 1;

                    tesis.Tatj = tesis.ClaveControl.Contains("J") ? 1 : 0;

                    misTesis.Add(tesis);
                }
            }
            catch (Exception)
            {
            }
            return misTesis;
        }
    }
}