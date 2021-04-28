using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Kursovik.Controllers
{
    public static class FileHelper
    {

        public static string OpenTxtFile(Stream fileStream)
        {

            Encoding Windows1251 = CodePagesEncodingProvider.Instance.GetEncoding(1251);
            string textFromFile = new StreamReader(fileStream, Windows1251).ReadToEnd();
            return textFromFile;
        }

        public static string OpenDocFile(Stream fileStream)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileStream, false))
            {
                Body body = wordDocument.MainDocumentPart.Document.Body;
                return body.InnerText.ToString();
            }
        }
        public static byte[] CreateDocFile(string fileText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (WordprocessingDocument newWord = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = newWord.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text(fileText));
                }
                return ms.ToArray();
            }
        }
        public static byte[] CreateTxtFile(string text)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(text);
                }
                return ms.ToArray();
            }
        }
    }
}
