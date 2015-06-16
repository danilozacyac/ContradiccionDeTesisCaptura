using System;
using System.Linq;

namespace ContradiccionDeTesisCaptura.Report
{
    public class ToWordReport
    {


        //public static Dictionary<long, string> DicDescripciones = new Dictionary<long, string>();
        //public static Dictionary<long, int> DicNiveles = new Dictionary<long, int>();
        //public static Dictionary<long, int> DicPadres = new Dictionary<long, int>();

        //readonly string filepath = Path.GetTempFileName() + ".docx";

        //int fila = 1;

        //private readonly ObservableCollection<Contradicciones> listaContradicciones;
        //Microsoft.Office.Interop.Word.Application oWord;
        //Microsoft.Office.Interop.Word.Document oDoc;
        //object oMissing = System.Reflection.Missing.Value;
        //object oEndOfDoc = "\\endofdoc";

        //Microsoft.Office.Interop.Word.Table oTable;


        //public ToWordReport(ObservableCollection<Contradicciones> listaContradicciones)
        //{
        //    this.listaContradicciones = listaContradicciones;
        //}


        //public void GeneraWord()
        //{
        //    oWord = new Microsoft.Office.Interop.Word.Application();
        //    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //    oDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

        //    //Insert a paragraph at the beginning of the document.
        //    Microsoft.Office.Interop.Word.Paragraph oPara1;
        //    oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
        //    oPara1.Range.Text = "SISTEMATIZACIÓN DE TESIS JURISPRUDENCIALES Y AISLADAS PUBLICADAS EN EL SEMANARIO JUDICIAL DE LA FEDERACIÓN Y SU GACETA (" + DateTimeUtilities.ToMonthName(DateTime.Now.Month - 1) + " " + DateTime.Now.Year + ")";
        //    oPara1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //    oPara1.Range.Font.Bold = 1;
        //    oPara1.Range.Font.Name = "Times New Roman";
        //    oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
        //    oPara1.Range.InsertParagraphAfter();

        //    Microsoft.Office.Interop.Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

        //    oTable = oDoc.Tables.Add(wrdRng, (listaContradicciones.Count + 1), 12, ref oMissing, ref oMissing);
        //    oTable.Range.ParagraphFormat.SpaceAfter = 6;
        //    oTable.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
        //    oTable.Range.Font.Size = 10;
        //    oTable.Range.Font.Bold = 1;
        //    oTable.Borders.Enable = 1;

        //    oTable.Columns[1].SetWidth(40, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[2].SetWidth(70, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[3].SetWidth(90, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[4].SetWidth(86, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[5].SetWidth(80, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[6].SetWidth(43, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[7].SetWidth(210, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[8].SetWidth(60, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[9].SetWidth(80, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[10].SetWidth(43, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[11].SetWidth(210, WdRulerStyle.wdAdjustSameWidth);
        //    oTable.Columns[12].SetWidth(60, WdRulerStyle.wdAdjustSameWidth);

        //    oTable.Cell(fila, 1).Range.Text = "#";
        //    oTable.Cell(fila, 2).Range.Text = "Pleno de Circuito";
        //    oTable.Cell(fila, 3).Range.Text = "Asunto";
        //    oTable.Cell(fila, 4).Range.Text = "Denunciante";
        //    oTable.Cell(fila, 5).Range.Text = "Oficio";
        //    oTable.Cell(fila, 6).Range.Text = "Acuerdo admisorio";
        //    oTable.Cell(fila, 7).Range.Text = "Órganos contendientes";
        //    oTable.Cell(fila, 8).Range.Text = "Criterios o tesis contendientes";
        //    oTable.Cell(fila, 9).Range.Text = "Tema o posible tema de la contradicción";
        //    oTable.Cell(fila, 10).Range.Text = "Fecha de resolución";
        //    oTable.Cell(fila, 11).Range.Text = "Resolutivos";
        //    oTable.Cell(fila, 12).Range.Text = "Rubro/Título y subtítulo de tesis prevalenciente(s)";

        //    for (int x = 1; x < 12; x++)
        //    {
        //        oTable.Cell(fila, x).Range.Font.Size = 10;
        //        oTable.Cell(fila, x).Range.Font.Bold = 1;
        //        oTable.Cell(fila, x).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //    }



        //    fila++;

        //    try
        //    {
        //        int consecutivo = 1;
        //        foreach (Contradicciones contra in listaContradicciones)
        //        {
        //            string asunto = (from n in TipoAsuntoSingleton.TipoAsunto
        //                             where n.IdTipo == contra.IdTipoAsunto
        //                             select n.Descripcion).ToList()[0];

        //            if (asunto.Equals("Contradicción de Tesis"))
        //                asunto = "CT";

        //            oTable.Cell(fila, 1).Range.Text = consecutivo.ToString();

        //            //Aqui va el nombre del pleno del Circuito del cual actualmente no existe un catálogo
        //            oTable.Cell(fila, 2).Range.Text = contra.IdPlenoCircuito.ToString();

        //            oTable.Cell(fila, 3).Range.Text = asunto + " " + contra.ExpedienteNumero + "/" + contra.ExpedienteAnio;

        //            cell = this.InitializeCell(contra.Denunciantes + ".   ", Fuentes.ContenidoCelda, 0);
        //            table.AddCell(cell);

        //            cell = this.InitializeCell(contra.Oficios, Fuentes.ContenidoCelda, 1);
        //            table.AddCell(cell);

        //            if (contra.AcAdmisorio != null)
        //                cell = this.InitializeCell(contra.AcAdmisorio.Acuerdo, Fuentes.ContenidoCelda, 0);
        //            else
        //                cell = this.InitializeCell("", Fuentes.ContenidoCelda, 1);
        //            table.AddCell(cell);

        //            cell = this.InitializeCell(contra.Criterios, Fuentes.ContenidoCelda, 3);
        //            table.AddCell(cell);

        //            cell = new PdfPCell();
        //            foreach (Criterios criterio in contra.Criterios)
        //            {
        //                Paragraph parat = new Paragraph(criterio.Criterio, Fuentes.ContenidoCelda);
        //                parat.Alignment = 3;
        //                cell.AddElement(parat);
        //            }
        //            cell.Border = 1;
        //            cell.HorizontalAlignment = 3; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell);

        //            cell = this.InitializeCell(contra.Tema, Fuentes.ContenidoCelda, 3);
        //            table.AddCell(cell);

        //            if (contra.MiEjecutoria != null && contra.MiEjecutoria.FechaResolucion != null)
        //            {

        //                cell = this.InitializeCell(contra.MiEjecutoria.FechaResolucion.Value.ToShortDateString(), Fuentes.ContenidoCelda, 1);
        //                //cell = new PdfPCell(new Phrase(contra.MiEjecutoria.FechaResolucion.Value.ToString(format), Fuentes.ContenidoCelda));
        //                table.AddCell(cell);
        //            }
        //            else
        //            {
        //                cell = this.InitializeCell("  ", Fuentes.ContenidoCelda, 1);
        //                //cell = new PdfPCell(new Phrase(contra.MiEjecutoria.FechaResolucion.Value.ToString(format), Fuentes.ContenidoCelda));
        //                table.AddCell(cell);
        //            }

        //            cell = this.InitializeCell(contra.Resolutivo.PuntosResolutivos, Fuentes.ContenidoCelda, 3);
        //            table.AddCell(cell);

        //            cell = this.InitializeCell(contra.MiTesis, Fuentes.ContenidoCelda, 3);
        //            table.AddCell(cell);

        //            consecutivo++;
        //        }

        //        foreach (Section wordSection in oDoc.Sections)
        //        {
        //            object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
        //            object firstpage = true;
        //            wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
        //        }

        //        oWord.ActiveDocument.SaveAs(filepath);
        //        oWord.ActiveDocument.Saved = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
        //    }
        //    finally
        //    {
        //        oWord.Visible = true;
        //        DicNiveles.Clear();
        //        DicDescripciones.Clear();
        //        DicPadres.Clear();
        //        oDoc.Close();

        //        Process.Start(filepath);
        //    }
        //}

        //private void ImprimeDocumento()
        //{
        //    List<long> idsMatSga = new ReporteModel().GetidMatSgaMes();

        //    int numTesis = 0;

        //    tesisImprime = new ReporteModel().GetTesis();

        //    foreach (long idMat in idsMatSga)
        //    {
        //        if (!DicDescripciones.ContainsKey(idMat))
        //        {
        //            new EstructuraModel().FillDictionary(Convert.ToChar(idMat.ToString().Substring(0, 1)));
        //        }

        //        List<TesisReg> listaImprimir = (from n in tesisImprime
        //                                        where n.IdMateriaSga == idMat
        //                                        orderby n.txtGenealogia
        //                                        select n).ToList();

        //        foreach (TesisReg tesis in listaImprimir)
        //        {
        //            oWord.Visible = true;
        //            int nivelImprime = DicNiveles[idMat];
        //            int nivelPadre = DicPadres[idMat];

        //            if (nivelImprime == 4)
        //            {
        //                oTable.Cell(fila, 5).Range.Text = DicDescripciones[idMat]; //Subsección
        //                oTable.Cell(fila, 4).Range.Text = DicDescripciones[nivelPadre];//Sección

        //                int nivelAbuelo = DicPadres[nivelPadre];

        //                nivelImprime = DicNiveles[nivelAbuelo];

        //                if (nivelImprime != 2)
        //                {
        //                    oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelAbuelo];
        //                }//SubMateria}
        //                else
        //                {
        //                    int nivelBisAbuelo = DicPadres[nivelAbuelo];
        //                    if (DicNiveles[nivelBisAbuelo] != 0)
        //                        oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelBisAbuelo];
        //                    else
        //                        oTable.Cell(fila, 3).Range.Text = "";
        //                }
        //                oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
        //            }
        //            else if (nivelImprime == 3)
        //            {
        //                oTable.Cell(fila, 5).Range.Text = ""; //Subsección
        //                oTable.Cell(fila, 4).Range.Text = DicDescripciones[idMat];//Sección

        //                nivelImprime = DicNiveles[nivelPadre];

        //                if (nivelImprime != 2)
        //                {
        //                    oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelPadre];
        //                }//SubMateria}
        //                else
        //                {
        //                    int nivelAbuelo = DicPadres[nivelPadre];
        //                    if (DicNiveles[nivelAbuelo] != 0)
        //                        oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelAbuelo];
        //                    else
        //                        oTable.Cell(fila, 3).Range.Text = "";
        //                }
        //                oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
        //            }
        //            else if (nivelImprime == 2)
        //            {
        //                oTable.Cell(fila, 5).Range.Text = ""; //Subsección
        //                oTable.Cell(fila, 4).Range.Text = "";//Sección
        //                oTable.Cell(fila, 3).Range.Text = (DicNiveles[nivelPadre] == 1) ? DicDescripciones[nivelPadre] : "";// new EstructuraModel().getSubMateriaNivelDos(idTema);//SubMateria
        //                oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
        //            }

        //            oTable.Cell(fila, 1).Range.Text = tesis.ius4.ToString();
        //            oTable.Cell(fila, 6).Range.Text = tesis.epoca;
        //            oTable.Cell(fila, 7).Range.Text = tesis.RUBRO;
        //            oTable.Cell(fila, 8).Range.Text = tesis.tesis;

        //            for (int x = 1; x < 9; x++)
        //            {
        //                oTable.Cell(fila, x).Borders.Enable = 1;
        //                oTable.Cell(fila, x).Range.Font.Size = 9;
        //                oTable.Cell(fila, x).Range.Font.Bold = 0;

        //                oTable.Cell(fila, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                oTable.Cell(fila, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                oTable.Cell(fila, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //            }

        //            fila++;
        //            numTesis++;
        //        }
        //    }
        //    MessageBox.Show("Total de Tesis impresas " + numTesis);
        //}


        //private Cell InitializeCell(object enumer, int textAlign)
        //{
        //    Cell cell = new Cell();

        //    if (enumer is ObservableCollection<Criterios>)
        //    {

        //        foreach (Criterios criterio in enumer as ObservableCollection<Criterios>)
        //        {
        //            string organoC = (from n in OrganismosSingleton.Colegiados
        //                              where n.IdOrganismo == criterio.IdOrgano
        //                              select n.Organismo).ToList()[0];

        //            Paragraph parart = new Paragraph(organoC, cellFont);
        //            cell.Range.Text = 
        //            parart.Range.InsertParagraphAfter();
        //            parart.Alignment = textAlign;
        //            cell.AddElement(parart);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
        //        }
        //    }
        //    else if (enumer is ObservableCollection<PResolutivos>)
        //    {
        //        foreach (PResolutivos res in enumer as ObservableCollection<PResolutivos>)
        //        {
        //            Paragraph parat = new Paragraph(res.Resolutivo, Fuentes.ContenidoCelda);
        //            parat.Alignment = textAlign;
        //            cell.AddElement(parat);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
        //        }
        //    }
        //    else if (enumer is ObservableCollection<Tesis>)
        //    {
        //        foreach (Tesis tes in enumer as ObservableCollection<Tesis>)
        //        {
        //            Paragraph parargr = new Paragraph(tes.ClaveIdentificacion, Fuentes.ContenidoCelda);
        //            parargr.Alignment = textAlign;
        //            cell.AddElement(parargr);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));

        //            parargr = new Paragraph(tes.Rubro, Fuentes.ContenidoCelda);
        //            parargr.Alignment = textAlign;
        //            cell.AddElement(parargr);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));

        //            parargr = new Paragraph("REG. IUS  ", Fuentes.ContenidoCelda);
        //            parargr.Alignment = textAlign;
        //            cell.AddElement(parargr);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
        //        }
        //    }
        //    else if (enumer is ObservableCollection<Oficios>)
        //    {
        //        Paragraph parat;
        //        foreach (Oficios oficio in enumer as ObservableCollection<Oficios>)
        //        {
        //            if (oficio.Oficio.Contains("CORREO") || oficio.FechaOficio == null)
        //                parat = new Paragraph(oficio.Oficio, Fuentes.ContenidoCelda);
        //            else
        //                parat = new Paragraph(oficio.Oficio + ", DE " + DateTimeUtilities.ToLongDateFormat(oficio.FechaOficio).ToUpper(), Fuentes.ContenidoCelda);

        //            parat.Alignment = textAlign;
        //            cell.AddElement(parat);
        //            cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
        //        }
        //    }

        //    return cell;
        //}
    }
}
