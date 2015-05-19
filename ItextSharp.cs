
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf.draw;

public class CS
{
    //ItextSharp Version: 5.5.6.0
    
    private static Paragraph FormataTextoCabecalho(string titulo, string valor, float indentation)
    {
        Paragraph paragraph = new Paragraph();
        Chunk tituloNormal = new Chunk(titulo + ": ", FontFactory.GetFont("ARIAL", 8));
        Chunk valorNegrito = new Chunk(valor, FontFactory.GetFont("ARIAL", 8, Font.BOLD));
        paragraph.IndentationLeft = indentation;
        paragraph.Add(tituloNormal);
        paragraph.Add(valorNegrito);
        return paragraph;
    }

    public string GetDictionaryKeyFromValue(Dictionary<string, string> dict, string value)
    {
        var result = dict.Where(p => p.Value == value).Select(p => p.Key);
        foreach (var key in result)
            return key;

        return "";
    }

    private void InsereTotais(ref PdfPTable pdfTable, int qtdColunas, string texto, decimal valorNe, decimal valorFatura)
    {
        PdfPCell pdfPCell = null;
        Font font8 = FontFactory.GetFont("ARIAL", 8, Font.BOLD);

        //Linha em branco
        for (int i = 0; i < qtdColunas; i++)
        {
            pdfPCell = new PdfPCell(new Phrase(new Chunk(string.Empty)));
            pdfPCell.Border = 0;
            pdfTable.AddCell(pdfPCell);
        }

        //valores 
        for (int i = 0; i < qtdColunas; i++)
        {
            if (i == 2)
            {
                pdfPCell = new PdfPCell(new Phrase(new Chunk(texto, font8)));
            }
            else if (i == 3)
            {
                pdfPCell = new PdfPCell(new Phrase(new Chunk(valorNe.ToString("N"), font8)));
            }
            else if (i == 4)
            {
                pdfPCell = new PdfPCell(new Phrase(new Chunk(valorFatura.ToString("N"), font8)));
            }
            else
            {
                pdfPCell = new PdfPCell(new Phrase(new Chunk(string.Empty)));
            }

            pdfPCell.Border = 0;
            pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfTable.AddCell(pdfPCell);
        }
    }

    public void GeraRelatorio(DataTable dataTable, Dictionary<string, string> dict, string Termo)
    {
        if (dataTable == null) return;

        Chunk linebreak = null;
        LineSeparator separator = null;

        Document pdfDoc = new Document(PageSize.A4.Rotate(), 5, 5, 10, 20);
        try
        {
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
            writer.PageEvent = new Footer();
            pdfDoc.Open();

            InsereHeader(ref pdfDoc, ref writer);

            //LineBreak
            separator = new LineSeparator();
            linebreak = new Chunk(separator);

            short counter = 0;
            short contador = 0;
            string valor = null;
            string proximo = null;
            decimal TotalValorNE = 0;
            decimal TotalValorFatura = 0;

            foreach (var key in dict.Values.Distinct()) // Lista de distintos
            {
                valor = key.ToString();
                proximo = dict.Values.Distinct().Skip(++contador).FirstOrDefault();

                //Gera somente relatório do "Termo" do método 
                if (!string.IsNullOrEmpty(Termo))
                {
                    if (valor != Termo)
                    {
                        counter++;
                        continue;
                    }
                }

                //Print
                Paragraph right = new Paragraph("Termo Nº " + counter, FontFactory.GetFont("ARIAL", 10, Font.BOLD));
                right.IndentationLeft = 10;
                right.SpacingAfter = 3f;
                pdfDoc.Add(right);

                pdfDoc.Add
                (
                    ExtraiDadosPorTermo
                    (
                        counter.ToString(),
                        ref dataTable,
                        GetDictionaryKeyFromValue(dict, valor),
                        GetDictionaryKeyFromValue(dict, proximo),
                        ref TotalValorNE,
                        ref TotalValorFatura
                    )
                );

                pdfDoc.Add(linebreak);
                counter++;
            }

            //Gera Total geral quando a consulta solicitar todos os termos
            if (string.IsNullOrEmpty(Termo))
            {
                PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                pdfTable.TotalWidth = PageSize.A4.Rotate().Width - 40;
                pdfTable.LockedWidth = true;
                pdfTable.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] widths = new float[] { 10, 13, 70, 15, 15, 20 };
                pdfTable.SetWidths(widths);
                InsereTotais(ref pdfTable, dataTable.Columns.Count, "TOTAL GERAL:", TotalValorNE, TotalValorFatura);
                pdfDoc.Add(pdfTable);
                //pdfDoc.Add(linebreak);
            }

            pdfDoc.Add(FormataTextoCabecalho("SALDO A ", "8.826.756,38", 300f));
            pdfDoc.Add(FormataTextoCabecalho("SALDO DE ", "8.826.756,38", 300f));
            pdfDoc.Add(FormataTextoCabecalho("SALDO DO ", "8.826.756,38", 300f));

            pdfDoc.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename= RelPag_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf");

            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            System.Web.HttpContext.Current.Response.Write(pdfDoc);
            Response.Flush();
            Response.End();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();  
        }
        catch (DocumentException de)
        {
            System.Web.HttpContext.Current.Response.Write(de.Message);
        }
        catch (IOException ioEx)
        {
            System.Web.HttpContext.Current.Response.Write(ioEx.Message);
        }
        catch (Exception ex)
        {
            System.Web.HttpContext.Current.Response.Write(ex.Message);
        }
    }

    private void InsereHeader(ref Document pdfDoc, ref PdfWriter writer)
    {
        Phrase phrase = null;
        PdfPCell cell = null;
        PdfPTable table = null;

        Chunk linebreak = null;
        LineSeparator separator = null;

        separator = new LineSeparator();
        linebreak = new Chunk(separator);

        //Header Table
        table = new PdfPTable(2);
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        table.SetWidths(new float[] { 300f, 200f });

        table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

        //Logo
        cell = ImageCell("~/images/Logomarca.jpg", 50f, PdfPCell.ALIGN_LEFT);
        table.AddCell(cell);

        //Heather
        phrase = new Phrase();
        phrase.Add(new Chunk("Relatório de Pagamentos\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)));

        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER);
        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
        table.AddCell(cell);
        pdfDoc.Add(table);

        pdfDoc.Add(linebreak);

        pdfDoc.Add(FormataTextoCabecalho("Empresa", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", 10f));
        pdfDoc.Add(FormataTextoCabecalho("CNPJ", "X", 10f));
        pdfDoc.Add(FormataTextoCabecalho("Modalidade", "Contrato", 10f));
        pdfDoc.Add(FormataTextoCabecalho("Número", "X", 10f));
        pdfDoc.Add(FormataTextoCabecalho("Valor do Contrato", "X", 10f));
        pdfDoc.Add(FormataTextoCabecalho("Vigência", "01/03/2011 a 28/03/2016", 10f));

        PdfContentByte cb = writer.DirectContent;
        ColumnText column1 = new ColumnText(cb);
        //                      With(x)  y(invertido) x(pos)   Width(y)invertido
        column1.SetSimpleColumn(700,     535,         400,     200);
        column1.AddElement(FormataTextoCabecalho("Campo", "X", 0f));
        column1.Alignment = Element.ALIGN_JUSTIFIED;
        column1.Go();

        pdfDoc.Add(linebreak);
    }

    private PdfPTable ExtraiDados(string NumTa, ref DataTable dt, string NumIni, string NumProx, ref decimal TotalValorNE, ref decimal TotalValorFatura)
    {
        decimal tempTotalValorNE = 0;
        decimal tempTotalValorFatura = 0;

        Font font8 = FontFactory.GetFont("ARIAL", 8);
        PdfPTable pdfPTable = new PdfPTable(dt.Columns.Count);

        pdfPTable.TotalWidth = PageSize.A4.Rotate().Width - 40;
        pdfPTable.LockedWidth = true;

        pdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
        float[] widths = new float[] { 10, 13, 70, 15, 15, 20 };
        pdfPTable.SetWidths(widths);

        string cloName = null;
        float HeaderTextSize = 9;

        PdfPCell pdfPCell = null;

        //Insere nome das colunas
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            cloName = dt.Columns[i].ColumnName;
            pdfPCell = new PdfPCell(new Phrase(new Chunk(cloName, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD))));

            if (cloName.ToUpper().StartsWith("V") || cloName.ToUpper().StartsWith("P"))
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            pdfPCell.Border = 0;
            pdfPTable.AddCell(pdfPCell);
        }

        decimal value = 0;
        bool insere = false;

        if (string.IsNullOrEmpty(NumProx)) NumProx = "nulo";

        //Insere dados coluna à coluna
        for (int rows = 0; rows < dt.Rows.Count; rows++)
        {
            if (dt.Rows[rows][1].ToString() == NumProx)
                break;
            else if (dt.Rows[rows][1].ToString() == NumIni)
                insere = true;

            if (insere)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    if (dt.Columns[column].ColumnName.ToUpper().StartsWith("V"))
                    {
                        value = Convert.ToDecimal(dt.Rows[rows][column].ToString());
                        pdfPCell = new PdfPCell(new Phrase(new Chunk(value.ToString("N"), font8)));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                        if (dt.Columns[column].ColumnName.ToUpper() == "VALOR ")
                            tempTotalValorNE += value;

                        if (dt.Columns[column].ColumnName.ToUpper() == "VALOR F")
                            tempTotalValorFatura += value;
                    }
                    else if (dt.Columns[column].ColumnName.ToUpper().StartsWith("D"))
                    {
                        cloName = Convert.ToDateTime(dt.Rows[rows][column].ToString()).ToShortDateString();
                        pdfPCell = new PdfPCell(new Phrase(new Chunk(cloName, font8)));
                    }
                    else if (dt.Columns[column].ColumnName.ToUpper().StartsWith("P"))
                    {
                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    else
                    {
                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }

                    pdfPCell.Border = 0;
                    pdfPTable.AddCell(pdfPCell);
                    value = 0;
                }
            }
        }

        InsereTotais(ref pdfPTable, dt.Columns.Count, "TOTAL nº " + Num, tempTotalValorNE, tempTotalValorFatura);

        TotalValorNE += tempTotalValorNE;
        TotalValorFatura += tempTotalValorFatura;

        pdfPTable.SpacingBefore = 15f;
        pdfPTable.SpacingAfter = 15f;

        return pdfPTable;
    }

    private static PdfPCell ImageCell(string path, float scale, int align)
    {
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
        image.ScalePercent(scale);
        PdfPCell cell = new PdfPCell(image);
        cell.BorderColor = BaseColor.WHITE;
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

        cell.PaddingBottom = 0f;
        cell.PaddingTop = 0f;
        cell.PaddingLeft = 10f;
        return cell;
    }

    private static PdfPCell PhraseCell(Phrase phrase, int align)
    {
        PdfPCell cell = new PdfPCell(phrase);
        cell.BorderColor = BaseColor.WHITE;
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        cell.HorizontalAlignment = align;
        cell.PaddingBottom = 2f;
        cell.PaddingTop = 0f;
        return cell;
    }
    
    public Dictionary<string, string> Classifica(int Ide)
    {
        DataTable dt = new DataTable();
        DataTable dtTermos = new DataTable();

        string query = @"Select ";

        dt = GetData(String.Format(query, IdeCto));

        string query2 =
        @"SELECT DISTINCT ";

        dtTermos = GetData(String.Format(query2, IdeCto));

        Queue filaTermos = new Queue();

        foreach (DataRow item in dtTermos.Rows)
        {
            filaTermos.Enqueue(item["NumCto"].ToString());
        }

        Dictionary<string, string> dict = new Dictionary<string, string>();

        foreach (DataRow item in dt.Rows)
        {
            if (filaTermos.Peek().ToString() == "0")
            {
                //Busca INICIAL
                if (item["NumCto"] == DBNull.Value && item["Modalidade"] == DBNull.Value)
                    dict.Add(item["Num"].ToString(), "0");

                //Se houver Modalidade = 2
                if (item["Modalidade"].ToString() == "2")
                    dict.Add(item["Num"].ToString(), "0");

                if (item["Modalidade"].ToString() == "1")
                {
                    filaTermos.Dequeue();

                    if (filaTermos.Peek().ToString() == item["Num"].ToString())
                    {
                        dict.Add(item["Num"].ToString(), filaTermos.Peek().ToString());
                    }
                }
            }
            else
            {
                if (filaTermos.Peek().ToString() == item["NumCto"].ToString())
                {
                    dict.Add(item["Num"].ToString(), filaTermos.Peek().ToString());
                }
                else
                {
                    if (item["Modalidade"].ToString() == "2")
                        dict.Add(item["Num"].ToString(), filaTermos.Peek().ToString());

                    if (item["Modalidade"].ToString() == "1")
                    {
                        filaTermos.Dequeue();

                        if (filaTermos.Peek().ToString() == item["Num"].ToString())
                        {
                            dict.Add(item["Num"].ToString(), filaTermos.Peek().ToString());
                        }
                    }
                }
            }
        }

        return dict;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

public class Footer : PdfPageEventHelper
{
    PdfContentByte cb;
    PdfTemplate template;
    BaseFont bf = null;
    DateTime PrintTime = DateTime.Now;

    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            PrintTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }
        catch (DocumentException de)
        {
        }
        catch (System.IO.IOException ioe)
        {
        }
    }

    public override void OnEndPage(PdfWriter writer, Document doc)
    {
        /*
        Paragraph footer = new Paragraph(writer.PageNumber.ToString(), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
        footer.Alignment = Element.ALIGN_RIGHT;
        PdfPTable footerTbl = new PdfPTable(1);
        footerTbl.TotalWidth = 300;
        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell cell = new PdfPCell(footer);
        cell.Border = 0;
        cell.PaddingLeft = 10;

        footerTbl.AddCell(cell);
        footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);       
        */
        
        base.OnEndPage(writer, doc);
        
        int pageN = writer.PageNumber;
        String text = pageN + " de ";       

        float len = bf.GetWidthPoint(text, 8);
        Rectangle pageSize = doc.PageSize;
        cb.SetRGBColorFill(100, 100, 100);
        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetTextMatrix(pageSize.GetLeft(20), pageSize.GetBottom(10));        
        cb.ShowText(text);
        cb.EndText();        
        cb.AddTemplate(template, pageSize.GetLeft(20) + len, pageSize.GetBottom(10));

        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, PrintTime.ToString(), pageSize.GetRight(40), pageSize.GetBottom(10), 0);
        cb.EndText();        
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);
        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.SetTextMatrix(0, 0);
        template.ShowText("" + (writer.PageNumber - 1));
        template.EndText();
    }
}
