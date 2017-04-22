using System.Collections.Generic;
using System.Net.Mail;

namespace Infrastructure.Mail.Interfaces
{
    public interface IMailSender
    {
        string ErrorMessage { get; set; }

        void SetSetting(string host, string from, string password, bool useSsl, int portNumber);

        bool Send(string title, string to, string subject, string body);
        bool Send(string title, string to, string[] cc, string[] bcc, string subject, string body);
        bool Send(string title, string to, string[] cc, string[] bcc, string[] attachmentsPhysicalAddress, string subject, string body);
        bool Send(string title, string to, string subject, string body, Attachment[] attachments);
        bool Send(string title, string to, string[] cc, string[] bcc, string subject, string body, Attachment[] attachments);
    }
}