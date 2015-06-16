using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Word;

namespace TableWordToDb
{
    public class ReadTable
    {
        public List<TableInfo> GetTableInfo()
        {
            List<TableInfo> listaContras = new List<TableInfo>();

            // Open a doc file.
            Application application = new Application();
            Document document = application.Documents.Open(@"C:\Users\lavega\Documents\Visual Studio 2010\Projects\ContradiccionDeTesisCaptura\ContradiccionDeTesisCaptura\bin\Debug\CT.docx");

            document.ActiveWindow.View.ShowFormatChanges = false;
            document.ActiveWindow.View.ShowRevisionsAndComments = false;

            Tables ttt = document.Tables;

            foreach (Table tab in ttt)
                foreach (Row aRow in tab.Rows)
                {
                    TableInfo info = new TableInfo();

                    info.Plenos = aRow.Cells[2].Range.Text;
                    info.Asunto = aRow.Cells[3].Range.Text.ToString();
                    info.Denunciante = aRow.Cells[4].Range.Text;
                    info.Oficio = aRow.Cells[5].Range.Text;
                    info.Acuerdoad = aRow.Cells[6].Range.Text;
                    info.OrgContencientes = aRow.Cells[7].Range.Text;
                    info.Criterios = aRow.Cells[8].Range.Text;
                    info.Tema = aRow.Cells[9].Range.Text;
                    info.FechaResolucion = aRow.Cells[10].Range.Text;
                    info.Resolutivos = aRow.Cells[11].Range.Text;
                    info.Tesis = aRow.Cells[12].Range.Text;

                    listaContras.Add(info);
                }

            // Close word.
            application.Quit();

            return listaContras;
        }
    }
}
