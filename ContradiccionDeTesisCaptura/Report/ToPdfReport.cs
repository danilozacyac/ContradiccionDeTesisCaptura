using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Singletons;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ScjnUtilities;

namespace ContradiccionDeTesisCaptura.Report
{
    public class ToPdfReport
    {
        private iTextSharp.text.Document myDocument;

        public void CtToPdfReport(ListadoDeContradicciones listadoContradicciones)
        {
            myDocument = new iTextSharp.text.Document(new Rectangle(288f, 144f), 15, 15, 30, 30);
            myDocument.SetPageSize(PageSize.LEGAL_LANDSCAPE.Rotate());

            string filePath = Path.GetTempFileName() + ".pdf";

            //try
            //{
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filePath, FileMode.Create));
                HeaderFooter pdfPage = new HeaderFooter();
                writer.PageEvent = pdfPage;
                writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                //Paragraph para;

                myDocument.Open();

                int consec = 1;

                Paragraph para = new Paragraph("CONTRADICCIONES DE TESIS PENDIENTES DE RESOLVER Y RESUELTAS POR LOS PLENOS DE CIRCUITO",Fuentes.Encabezado);
                para.Alignment = 1;
                myDocument.Add(para);

                PdfPTable table = new PdfPTable(12);
                //table.TotalWidth = 400;
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
            

                table.SplitLate = false;
                table.SplitRows = true;

                float[] widths = new float[] { .3f, 1f, .5f, 1f, 1f, 1f, 1f, 2f, 1.5f, 1f, 1.5f, 1.5f };
                table.SetWidths(widths);

               

                string[] encabezado =
                {
                    "#", "Pleno de Circuito", "Asunto", "Denunciante", "Oficio", "Acuerdo admisorio", "Órganos contendientes", "Criterios o tesis contendientes", "Tema o posible tema de la contradicción",
                    "Fecha de resolución", "Resolutivos", "Rubro/Título y subtítulo de tesis prevalenciente(s)"
                };
                PdfPCell cell;

                foreach (string cabeza in encabezado)
                {
                    cell = this.InitializeCell(cabeza, Fuentes.EncabezadoColumna, 1);
                    table.AddCell(cell);
                }

                int consecutivo = 1;
                foreach (Contradicciones contra in listadoContradicciones.Listado)
                {
                    string asunto = (from n in TipoAsuntoSingleton.TipoAsunto
                                     where n.IdTipo == contra.IdTipoAsunto
                                     select n.Descripcion).ToList()[0];

                    if (asunto.Equals("Contradicción de Tesis"))
                        asunto = "CT";

                    cell = this.InitializeCell(consecutivo.ToString(), Fuentes.ContenidoCelda, 1);
                    table.AddCell(cell);

                    //Aqui va el nombre del pleno del Circuito del cual actualmente no existe un catálogo
                    cell = this.InitializeCell(" ", Fuentes.ContenidoCelda, 0);
                    table.AddCell(cell);

                    cell = this.InitializeCell(asunto + " " + contra.ExpedienteNumero + "/" + contra.ExpedienteAnio, Fuentes.ContenidoCelda, 1);
                    table.AddCell(cell);

                    cell = this.InitializeCell(contra.Denunciantes + ".   ", Fuentes.ContenidoCelda, 0);
                    table.AddCell(cell);

                    cell = this.InitializeCell(contra.Oficios, Fuentes.ContenidoCelda, 1);
                    table.AddCell(cell);

                    if (contra.AcAdmisorio != null)
                        cell = this.InitializeCell(contra.AcAdmisorio.Acuerdo, Fuentes.ContenidoCelda, 0);
                    else
                        cell = this.InitializeCell("", Fuentes.ContenidoCelda, 1);
                    table.AddCell(cell);

                    cell = this.InitializeCell(contra.Criterios, Fuentes.ContenidoCelda, 3);
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    foreach (Criterios criterio in contra.Criterios)
                    {
                        Paragraph parat = new Paragraph(criterio.Criterio, Fuentes.ContenidoCelda);
                        parat.Alignment = 3;
                        cell.AddElement(parat);
                    }
                    cell.Border = 1;
                    cell.HorizontalAlignment = 3; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    cell = this.InitializeCell(contra.Tema, Fuentes.ContenidoCelda, 3);
                    table.AddCell(cell);

                    if (contra.MiEjecutoria != null && contra.MiEjecutoria.FechaResolucion != null)
                    {

                        cell = this.InitializeCell(contra.MiEjecutoria.FechaResolucion.Value.ToShortDateString(), Fuentes.ContenidoCelda, 1);
                        //cell = new PdfPCell(new Phrase(contra.MiEjecutoria.FechaResolucion.Value.ToString(format), Fuentes.ContenidoCelda));
                        table.AddCell(cell);
                    }
                    else
                    {
                        cell = this.InitializeCell("  ", Fuentes.ContenidoCelda, 1);
                        //cell = new PdfPCell(new Phrase(contra.MiEjecutoria.FechaResolucion.Value.ToString(format), Fuentes.ContenidoCelda));
                        table.AddCell(cell);
                    }

                    cell = this.InitializeCell(contra.Resolutivo.PuntosResolutivos, Fuentes.ContenidoCelda, 3);
                    table.AddCell(cell);

                    cell = this.InitializeCell(contra.MiTesis,Fuentes.ContenidoCelda,3);
                    table.AddCell(cell);

                    consecutivo++;
                }

                myDocument.Add(table);
                consec++;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            //}
            //finally
            //{
                myDocument.Close();
                System.Diagnostics.Process.Start(filePath);
            //}
        }

        private PdfPCell InitializeCell(string cellContent, Font cellFont, int textAlign)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellContent, cellFont));
            cell.Colspan = 0;
            cell.Border = 1;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthRight = 1;
            cell.BorderWidthTop = 1;
            cell.BorderWidthBottom = 1;
            cell.HorizontalAlignment = textAlign; //0=Left, 1=Centre, 2=Right, 3=Justify

            return cell;
        }

        private PdfPCell InitializeCell(object enumer, Font cellFont, int textAlign)
        {
            PdfPCell cell = new PdfPCell();

            if (enumer is ObservableCollection<Criterios>)
            {
                
                foreach (Criterios criterio in enumer as ObservableCollection<Criterios>)
                {
                    string organoC = (from n in OrganismosSingleton.Colegiados
                                      where n.IdOrganismo == criterio.IdOrgano
                                      select n.Organismo).ToList()[0];

                    Paragraph parart = new Paragraph(organoC, cellFont);
                    parart.Alignment = textAlign;
                    cell.AddElement(parart);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
                }
            }
            else if (enumer is ObservableCollection<PResolutivos>)
            {
                foreach (PResolutivos res in enumer as ObservableCollection<PResolutivos>)
                {
                    Paragraph parat = new Paragraph(res.Resolutivo, Fuentes.ContenidoCelda);
                    parat.Alignment = textAlign;
                    cell.AddElement(parat);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
                }
            }
            else if (enumer is ObservableCollection<Tesis>)
            {
                foreach (Tesis tes in enumer as ObservableCollection<Tesis>)
                {
                    Paragraph parargr = new Paragraph(tes.ClaveIdentificacion, Fuentes.ContenidoCelda);
                    parargr.Alignment = textAlign;
                    cell.AddElement(parargr);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));

                    parargr = new Paragraph(tes.Rubro, Fuentes.ContenidoCelda);
                    parargr.Alignment = textAlign;
                    cell.AddElement(parargr);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));

                    parargr = new Paragraph("REG. IUS  ", Fuentes.ContenidoCelda);
                    parargr.Alignment = textAlign;
                    cell.AddElement(parargr);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
                }
            }
            else if (enumer is ObservableCollection<Oficios>)
            {
                Paragraph parat;
                foreach (Oficios oficio in enumer as ObservableCollection<Oficios>)
                {
                    if (oficio.Oficio.Contains("CORREO") || oficio.FechaOficio == null)
                        parat = new Paragraph(oficio.Oficio, Fuentes.ContenidoCelda);
                    else
                        parat = new Paragraph(oficio.Oficio + ", DE " + DateTimeUtilities.ToLongDateFormat(oficio.FechaOficio).ToUpper(), Fuentes.ContenidoCelda);
                    
                    parat.Alignment = textAlign;
                    cell.AddElement(parat);
                    cell.AddElement(new Paragraph(" ", Fuentes.ContenidoCelda));
                }
            }

            
            cell.Border = 1;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthRight = 1;
            cell.BorderWidthTop = 1;
            cell.BorderWidthBottom = 1;
            cell.HorizontalAlignment = textAlign; //0=Left, 1=Centre, 2=Right, 3=Justify

            return cell;
        }
    }
}