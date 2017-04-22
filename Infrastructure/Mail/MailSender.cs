using Infrastructure.Mail.Interfaces;
using System;
using System.Net.Mail;

namespace Infrastructure.Mail
{
    public class MailSender : IMailSender
    {

        #region props

        private string _host;
        private string _from;
        private string _password;
        private bool _useSsl;
        private int _portNumber;

        public string ErrorMessage { get; set; }

        #endregion

        
        public void SetSetting(string host, string from, string password, bool useSsl, int portNumber)
        {
            _host = host;
            _from = from;
            _password = password;
            _useSsl = useSsl;
            _portNumber = portNumber;
        }


        /// <summary>
        /// without attachment
        /// </summary>
        public bool Send(string title, string to, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(_from, title, System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;

                mail.Priority = MailPriority.High;
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(_from, _password),
                    Port = _portNumber,
                    Host = _host,
                    EnableSsl = _useSsl
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// with cc and Bcc
        /// </summary>
        public bool Send(string title, string to, string[] cc, string[] bcc, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(to);

                // add cc
                if (cc != null && cc.Length > 0)
                    foreach (var t in cc)
                        mail.CC.Add(t);

                // add bcc
                if (bcc != null && bcc.Length > 0)
                    for (int i = 0; i < bcc.Length; i++)
                        mail.Bcc.Add(cc[i]);


                mail.From = new MailAddress(_from, title, System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;

                mail.Priority = MailPriority.High;
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(_from, _password),
                    Port = _portNumber,
                    Host = _host,
                    EnableSsl = _useSsl
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

        }


        /// <summary>
        /// with cc and Bcc and attachment
        /// </summary>
        public bool Send(string title, string to, string[] cc, string[] bcc, string[] attachmentsPhysicalAddress, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(to);

                // add cc
                if (cc != null && cc.Length > 0)
                    foreach (var t in cc)
                        mail.CC.Add(t);

                // add bcc
                if (bcc != null && bcc.Length > 0)
                    for (int i = 0; i < bcc.Length; i++)
                        mail.Bcc.Add(cc[i]);

                mail.From = new MailAddress(_from, title, System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;

                // add attachment
                if(attachmentsPhysicalAddress != null && attachmentsPhysicalAddress.Length > 0)
                    foreach (var t in attachmentsPhysicalAddress)
                        mail.Attachments.Add(new Attachment(t));

                mail.Priority = MailPriority.High;
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(_from, _password),
                    Port = _portNumber,
                    Host = _host,
                    EnableSsl = _useSsl
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

        }

        public bool Send(string title, string to, string subject, string body, Attachment[] attachments)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(to);

                mail.From = new MailAddress(_from, title, System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;

                // add attachment
                if (attachments != null && attachments.Length > 0)
                    foreach (var attach in attachments)
                        mail.Attachments.Add(attach);

                mail.Priority = MailPriority.High;
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(_from, _password),
                    Port = _portNumber,
                    Host = _host,
                    EnableSsl = _useSsl
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Send(string title, string to, string[] cc, string[] bcc, string subject, string body, Attachment[] attachments)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(to);

                // add cc
                if (cc != null && cc.Length > 0)
                    foreach (var t in cc)
                        mail.CC.Add(t);

                // add bcc
                if (bcc != null && bcc.Length > 0)
                    for (int i = 0; i < bcc.Length; i++)
                        mail.Bcc.Add(cc[i]);

                mail.From = new MailAddress(_from, title, System.Text.Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;

                // add attachment
                if (attachments != null && attachments.Length > 0)
                    foreach (var attach in attachments)
                        mail.Attachments.Add(attach);

                mail.Priority = MailPriority.High;
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(_from, _password),
                    Port = _portNumber,
                    Host = _host,
                    EnableSsl = _useSsl
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }

}