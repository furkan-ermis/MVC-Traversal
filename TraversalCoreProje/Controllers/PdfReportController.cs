using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace TraversalCoreProje.Controllers
{
    /// <summary>
    /// iTextSharp.LGPLv2.Core paketi yüklenmelidir
    /// wwwroot içerisine pdfreports adında klasör oluşturduk 
    /// </summary>
    public class PdfReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StaticPdfReport()
        {
            Random rnd = new Random();
            var i = rnd.Next(10000);
            // kaydedilecek yolu ve dosyanın yazılacağı stream oluşturuldu
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfreports/" + "dosya" + i + ".pdf");

            var stream = new FileStream(path, FileMode.Create);
            // pdf sayfası oluşturuldu ve stream a eklendi
            Document document = new Document(PageSize.A4, 10, 10, 20, 20);
            PdfWriter.GetInstance(document, stream);

            // dosya içeriği için document açılarak istenenler yazıldı
            document.Open();

            Paragraph paragraph = new Paragraph("Traversal Rezervasyon Pdf Raporu");

            document.Add(paragraph);
            document.Close();

            return File("/pdfreports/dosya" + i + ".pdf", "application/pdf", "dosya" + i + ".pdf");
        }
        public IActionResult StaticCustomerReport()
        {
            Random rnd = new Random();
            var i = rnd.Next(10000);
            // kaydedilecek yolu ve dosyanın yazılacağı stream oluşturuldu
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfreports/" + "dosya" + i + ".pdf");

            var stream = new FileStream(path, FileMode.Create);
            // pdf sayfası oluşturuldu ve stream a eklendi
            Document document = new Document(PageSize.A4, 10, 10, 20, 20);
            PdfWriter.GetInstance(document, stream);

            // dosya içeriği için document açılarak istenenler yazıldı
            document.Open();
            PdfPTable pdfTable = new PdfPTable(3);

            pdfTable.AddCell("Misafir Adı");
            pdfTable.AddCell("Misafir Soyadı");
            pdfTable.AddCell("Misafir TC");

            pdfTable.AddCell("Furkan");
            pdfTable.AddCell("Ermis");
            pdfTable.AddCell("51112177500");

            pdfTable.AddCell("Gizem");
            pdfTable.AddCell("Ermis");
            pdfTable.AddCell("11654877293");
            document.Add(pdfTable);
            document.Close();

            return File("/pdfreports/dosya" + i + ".pdf", "application/pdf", "dosya" + i + ".pdf");
        }
    }
}
