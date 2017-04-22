using System;
using System.IO;
using System.Windows.Forms;
using Infrastructure.Security;
using Infrastructure.Mail.Interfaces;
using Infrastructure.Mail;
using System.Net.Mail;
using System.Net.Mime;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipFileMailSender
{
    public partial class Form1 : Form
    {

        private const string baseFilePath = "D:\\Temp Projects\\ZipFileMailSender\\ZipFileMailSender\\App_Data\\hash\\";


        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var isSuccess = true;

            // ZipBySharpZip(new string[] { "1.hs", "2.hs" });

            // ZipByGZipStream(etImageBytes("2.hs"));

            // CreateImage("2.hs", "image2.jpg");

            // isSuccess = AttachImageAndSendEmail("1.hs");

            isSuccess = AttachZipAndSendEmail(new string[] { "1.hs", "2.hs" });


            MessageBox.Show(isSuccess ? "Email sent successfully" : "An error occored");
        }


        private void ZipByGZipStream(byte[] imageBytes)
        {
            string zipFileName = baseFilePath + "sampleGZip.zip";
            using (var fs = new FileStream(zipFileName, FileMode.Create))
            {
                using (var gz = new GZipStream(fs, CompressionMode.Compress, false))
                {
                    gz.Write(imageBytes, 0, imageBytes.Length);
                }
            }
        }


        private MemoryStream ZipBySharpZip(string[] encryptedFileNames)
        {
            var ms = new MemoryStream();
            using (var zipOStream = new ZipOutputStream(ms))
            {
                int bytesRead = 0;

                foreach (var encryptedFileName in encryptedFileNames)
                {
                    var imageBytes = GetImageBytes(encryptedFileName);

                    ZipEntry entry = new ZipEntry((encryptedFileName.Replace(".hs", ".jpg")));
                    zipOStream.PutNextEntry(entry);
                    using (var entryMS = new MemoryStream(imageBytes))
                    {
                        byte[] transferBuffer = new byte[1024];
                        do
                        {
                            bytesRead = entryMS.Read(transferBuffer, 0, transferBuffer.Length);
                            zipOStream.Write(transferBuffer, 0, bytesRead);
                        }
                        while (bytesRead > 0);
                    }
                }
            }

            return ms;
        }


        private bool AttachZipAndSendEmail(string[] encryptedFileNames)
        {
            string filePath = string.Empty;

            // Create attachment
            var attachmentStream = ZipBySharpZip(encryptedFileNames);
            var attachFile = new Attachment(attachmentStream, new ContentType(MediaTypeNames.Application.Zip));
            attachFile.ContentDisposition.FileName = "cards.zip";

            var disposition = attachFile.ContentDisposition;
            disposition.CreationDate = DateTime.UtcNow.AddHours(-5);
            disposition.ModificationDate = DateTime.UtcNow.AddHours(-5);
            disposition.ReadDate = DateTime.UtcNow.AddHours(-5);

            // Send Email
            var isSuccess = SendEmail(attachFile);

            attachmentStream.Dispose();
            attachFile.Dispose();

            return isSuccess;
        }

        private bool AttachImageAndSendEmail(string encryptedFileName)
        {
            byte[] imageBytes = GetImageBytes(encryptedFileName);

            using (var ms = new MemoryStream(imageBytes))
            {
                // Create attachment
                using (var attachFile = new Attachment(ms, new ContentType(MediaTypeNames.Image.Jpeg)))
                {
                    attachFile.ContentDisposition.FileName = encryptedFileName.Replace(".hs", ".jpg");

                    var disposition = attachFile.ContentDisposition;
                    disposition.CreationDate = DateTime.UtcNow.AddHours(-5);
                    disposition.ModificationDate = DateTime.UtcNow.AddHours(-5);
                    disposition.ReadDate = DateTime.UtcNow.AddHours(-5);

                    // Send Email
                    var isSuccess = SendEmail(attachFile);

                    return isSuccess;
                }
            }
        }

        private void CreateImage(string encryptedFileName, string destinationImageFileName)
        {
            byte[] imageBytes = GetImageBytes(encryptedFileName);

            using (var ms = new MemoryStream(imageBytes))
            using (var fs = new FileStream(baseFilePath + destinationImageFileName, FileMode.Create))
            {
                ms.Position = 0;
                ms.WriteTo(fs);
            }
        }

        private byte[] GetImageBytes(string encryptedFileName)
        {
            var filePath = baseFilePath + encryptedFileName;
            var encryptedFileContent = File.ReadAllText(filePath); // base64 encrypted string
            var decryptedContent = DataEncryptor.DecryptWithSalt(encryptedFileContent); // base64 decrypted string
            byte[] imageBytes = Convert.FromBase64String(decryptedContent);

            return imageBytes;
        }

        private bool SendEmail(Attachment file)
        {
            IMailSender mailSender = new MailSender();
            mailSender.SetSetting("mail.1sp.ir", "no_reply@1sp.ir", "uoFSPb]4hk(;", false, 587);
            var isSuccess = mailSender.Send(
                "sample email title",
                "h.moradof@gmail.com",
                "sample subject",
                "sample body",
                new Attachment[] { file });

            return isSuccess;
        }

    }
}
