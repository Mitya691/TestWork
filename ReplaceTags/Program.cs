using System.IO;
using NanoXLSX;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DocumentFormat.OpenXml.Vml;
using static System.Net.Mime.MediaTypeNames;
using MailKit;
using MimeKit;
using Multipart = MimeKit.Multipart;
using Parameter = MimeKit.Parameter;


namespace ReplaceTags
{
    public class Program
    {
        static void Main(string[] args)
        {
        }
    }
    public class Document
    {
        private Dictionary<string, string> _table_info = new Dictionary<string, string>();
        private string fileNamePattern = "";
        private string folderName = "";
        private string fileName = "";
        private string fileNamePatternxlsx = "";
        private string fullResultPath = "";

        public string FileNamePattern { get => fileNamePattern; set => fileNamePattern = value; }

        public string FolderName { get => folderName; set => folderName = value; }

        public string FileName { get => fileName; set => fileName = value; }

        private string MakeFileNamePatternXlsx()
        {
            int pointIndex = fileNamePattern.LastIndexOf('.');
            int size = fileNamePattern.Length;
            string copy = fileNamePattern;
            return copy.Substring(0, pointIndex + 1) + "xlsx";
        }

        private async Task<NanoXLSX.Workbook> LoadAndProcessWorkbookAsync()
        {
            fileNamePatternxlsx = MakeFileNamePatternXlsx();
            NanoXLSX.Workbook workbook = null;
            try
            {
                // Загрузка файла асинхронно
                workbook = await Task.Run(() => NanoXLSX.Workbook.Load(fileNamePatternxlsx));
                // Обработка файла (предположим, что это занимает время)
                await Task.Run(() => ProcessWorkbook(workbook));
            }
            catch (Exception ex)
            {
            }
            return workbook;
        }

        private void ProcessWorkbook(NanoXLSX.Workbook workbook)
        {
            NanoXLSX.Worksheet worksheet = workbook.Worksheets[0];
            int rowNumber = 0;
            while (worksheet.HasCell(0, rowNumber))
            {
                string cellMark = worksheet.GetCell(0, rowNumber).Value.ToString();

                try
                {
                    if (worksheet.HasCell(1, rowNumber) == false)
                    {
                        throw new Exception("Выбранная ячейка не входит в данный лист");
                    }
                    else
                    {
                        string cellContent = worksheet.GetCell(1, rowNumber).Value.ToString();
                        _table_info.Add(cellMark, cellContent);
                    }
                }
                catch (Exception e)
                {
                    
                }
                rowNumber++;
            }
            rowNumber = 0;
        }

        public async Task ExecuteAsync()
        {
            NanoXLSX.Workbook workbook = await LoadAndProcessWorkbookAsync();

            if (workbook != null)
            {
                fullResultPath = folderName + "\\\\" + fileName + ".docx";
                File.Copy(fileNamePattern, fullResultPath);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fullResultPath, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;
                    foreach (var info in _table_info)
                    {
                        var tags = body.Descendants<Tag>().Where(t => t.Val == info.Key);
                        foreach (var tag in tags)
                        {
                            var parentElement = tag.Parent;
                            var mainElement = parentElement.Parent;
                            var textElements = mainElement.Descendants<Text>();
                            foreach (var textElement in textElements)
                            {
                                textElement.Text = info.Value;
                            }
                        }
                    }
                    wordDoc.Save();
                }
            }
        }
    }

    public class MailSend
    {
        public void SendLetter()
        {
            string header = $"header-of-letter";
            MimeMessage message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse("recipient-mail"));

            message.Subject = "TEST";
            string messageText = "text";
            TextPart body = new TextPart("plain") { Text = messageText };
            Multipart multipart = new Multipart("mixed");
            multipart.Add(body);

            MimePart attachment = new MimePart("application", "vnd.ms-word")
            {
                Content = new MimeContent(File.OpenRead("file-path"), ContentEncoding.Default),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = System.IO.Path.GetFileName("file-path")
            };

            //замена кодировки чтобы имя вложенного в письмо файла читалось корректно
            foreach (Parameter parameter in attachment.ContentType.Parameters) parameter.EncodingMethod = ParameterEncodingMethod.Rfc2047;
            foreach (Parameter parameter in attachment.ContentDisposition.Parameters) parameter.EncodingMethod = ParameterEncodingMethod.Rfc2047;

            multipart.Add(attachment);
            message.Body = multipart;

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.Connect("smtp.yandex.ru", 465, true);
                client.Authenticate("your-mail", "the password issued by mail to your application");
                client.Send(message);
            }
            catch (Exception ex) 
            {
                    Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

    }
          
}
