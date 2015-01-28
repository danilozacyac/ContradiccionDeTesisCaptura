using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;
using ContradiccionesDirectorioApi.Singletons;
using ScjnUtilities;

namespace TableWordToDb
{
    public class WordInfoProcess
    {

        public void CopyWordToDb()
        {
            ContradiccionesModel cModel = new ContradiccionesModel();

            ReadTable readInfo = new ReadTable();
            List<TableInfo> listaCt = readInfo.GetTableInfo();
            
            foreach (TableInfo info in listaCt)
            {
                try
                {
                    info.Asunto = this.ReplaceWeirdChars(info.Asunto.Replace("CT", " "));

                    string[] asunto = info.Asunto.Split('/');

                    int expNumero = Convert.ToInt32(asunto[0]);
                    int expYear = Convert.ToInt32(asunto[1].Substring(0, 4));

                    bool cExist = cModel.CheckIfExist(expNumero, expYear);

                    if (!cExist)
                    {
                        Contradicciones contradiccion = new Contradicciones();
                        contradiccion.ExpedienteNumero = expNumero;
                        contradiccion.ExpedienteAnio = expYear;
                        contradiccion.Denunciantes = this.ReplaceWeirdChars(info.Denunciante);
                        contradiccion.AcAdmisorio = this.GetAcAdmisorio(info.Acuerdoad);

                        contradiccion.Criterios = this.GetCriteriosContendientes(info.OrgContencientes,info.Criterios);

                    }
                }
                catch (FormatException)
                {

                }

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
                        admin.Acuerdo = text;
                    }
                    else
                    {
                        admin.Acuerdo = text.Substring(index + 1);
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

            string[] org = organismos.Trim().Split('&');
            string[] cri = criterios.Trim().Split('&');

            if (org.Count() == cri.Count())
            {
                int index = 0;

                while (index < org.Count())
                {
                    Criterios criterio = new Criterios();
                    criterio.Criterio = cri[index];
                    criterio.IdOrgano = (from n in OrganismosSingleton.Colegiados
                                         where n.Organismo == org[index]
                                         select n.IdOrganismo).ToList()[0];

                    contienden.Add(criterio);
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
    }
}
