using System.Text;
using iTextSharp.text.pdf;

namespace Zugferd
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
        public static void CreatePDF(string pdfaName,string xmlFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string FolderPath = @"C:\Users\thomas.scharf\Zugferd\Zugferd";

            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12);

            ICC_Profile icc = ICC_Profile.GetInstance(System.IO.Path.Combine(FolderPath, "sRGB_v4_ICC_preference.icm"));

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4,5f,5f,16f,100f);
            PdfAWriter writer = PdfAWriter.GetInstance(document, new FileStream(pdfaName,FileMode.Create), PdfAConformanceLevel.PDF_A_3B);
            document.Open();
            writer.CreateXmpMetadata();
            writer.Open();
            writer.SetOutputIntents("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", icc);
            document.AddAuthor("Thomas Scharf");
            document.AddTitle("ZUGFeRD Rechnung");

            PdfDictionary param = new PdfDictionary();
            param.Put(PdfName.MODDATE, new PdfDate());
            param.Put(PdfName.TITLE, new PdfString(" This ist Attachment Title"));
            PdfFileSpecification specification = PdfFileSpecification.FileEmbedded(writer, xmlFile, "ZUGFeRD-invoice.xml", null, "application/xml", param, 0);
            specification.Put(new PdfName("AFRelationship"), new PdfName("Alternative"));
            writer.AddFileAttachment("This is description area for Attachment",specification);
            PdfArray array = new PdfArray();
            array.Add(specification.Reference);
            writer.ExtraCatalog.Put(new PdfName("AF"), array);

            document.Add(new iTextSharp.text.Paragraph("Hello World!",font));

            document.Close();
            writer.Close();


        }

    }
}
