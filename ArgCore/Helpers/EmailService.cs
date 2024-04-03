using Arg.DataModels;
using System.Net.Mail;
using System.Net;
using static QRCoder.PayloadGenerator;

namespace ArgCore.Helpers
{
    public class EmailService : IDisposable
    {
        private bool disposedValue = false;
        public void sendMessage(Email EM)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                string address = System.Configuration.ConfigurationManager.AppSettings["smtpfrom"];
                string singlerecipient = EM.SingleRecipient;
                mailMessage.From = new MailAddress(address);
                mailMessage.To.Add(singlerecipient);
                mailMessage.Subject = EM.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = EM.Body;
                SmtpClient smtpClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtphost"]);
                smtpClient.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["smtpusername"], System.Configuration.ConfigurationManager.AppSettings["smtppassword"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpport"]);
                smtpClient.Send(mailMessage);
                insertemailrecord(mailMessage, EM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void insertemailrecord(MailMessage mail, Email EM)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
