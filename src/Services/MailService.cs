using System.Net;
using System.Text;
using System.Net.Mail;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using System.Text.RegularExpressions;
using MailService.Controller;

namespace MailService.Services
{
    class Mail
    {
        private string? Host;
        private int Port;

        private string? ImapHost;

        private int ImapPort;
        private string Subject = "";
        private string Content = "";

        private string User = "";

        private string Password = "";

        public void SendMail(bool isHTML, string mailEncoding, string destinatary)
        {
            try
            {
                var mailMessage = new MailMessage(this.User, destinatary);
                mailMessage.Subject = this.Subject;
                mailMessage.IsBodyHtml = isHTML;
                mailMessage.SubjectEncoding =  Encoding.GetEncoding(mailEncoding);
                mailMessage.BodyEncoding = Encoding.GetEncoding(mailEncoding);
                mailMessage.Body = this.Content;

                var smtpClient = new SmtpClient(this.Host, this.Port);
                smtpClient.Credentials = new NetworkCredential(this.User, this.Password);
                smtpClient.EnableSsl = true;
                if(mailEncoding != null)
                {
                    smtpClient.Send(mailMessage);
                }
            }
            catch(Exception e)
            {
                Log.AppendLog($" \n ! An error has occured:\n\n {e} ! \n ");
            }
        }

        public object ReadMail()
        {
            var cancel = new CancellationTokenSource();
            var imapClient = new ImapClient();
            imapClient.Connect (this.ImapHost, this.ImapPort, true, cancel.Token);
            imapClient.AuthenticationMechanisms.Remove ("XOAUTH");
            imapClient.Authenticate(this.User, this.Password, cancel.Token);
            var inbox = imapClient.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly, cancel.Token);
            string[][] message = new string[20][];
            for(int i = 0; i <= inbox.Count -1; i++)
            {
                string[] mail = {inbox.GetMessage(i).Subject, Regex.Replace(inbox.GetMessage(i).HtmlBody, "<[^>]*>", "")};
                message[i] = mail;
            }
            imapClient.Disconnect(true);
            return message;
        }

        public void SetMailConf(string host, int port, string subject, string content, string user, string password)
        {
            this.Host = host;
            this.Port = port;
            this.Subject = subject;
            this.Content = content;
            this.User = user;
            this.Password = password;
        }

        public void SetInboxConf(string user, string password, string host, int port)
        {
            this.User = user;
            this.Password = password;
            this.ImapHost = host;
            this.ImapPort = port;
        }

    }
}