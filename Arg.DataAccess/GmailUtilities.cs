using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.Net;

namespace Arg.DataAccess
{
    public class GmailUtilities
    {
        public void GenerateDraft(string hostName, int port, string userName, string password,
         string emailAddress, string subject, string content, string attachmentFile, string sendToMailAddresses)
        {
            using (var client = new ImapClient())
            {
                var credentials = new NetworkCredential(userName, password);

                var msg = new MimeMessage();
                var bb = new BodyBuilder
                {
                    HtmlBody = content
                };
                if (!string.IsNullOrWhiteSpace(sendToMailAddresses))
                {
                    if (sendToMailAddresses.Contains(';'))
                    {
                        sendToMailAddresses = sendToMailAddresses.Replace(",", ";").Replace(" ", "");

                        foreach (var address in sendToMailAddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(MailboxAddress.Parse(address.Trim()));
                        }
                    }
                    else
                    {
                        msg.To.Add(MailboxAddress.Parse(sendToMailAddresses));
                    }

                }

                msg.From.Add(MailboxAddress.Parse(emailAddress));
                bb.Attachments.Add(attachmentFile);
                msg.Subject = subject;
                msg.Body = bb.ToMessageBody();

                using var cancel = new CancellationTokenSource();
                client.Connect(hostName, port, true);
                client.Authenticate(credentials, cancel.Token);

                var draftFolder = client.GetFolder(SpecialFolder.Drafts);
                if (draftFolder != null)
                {
                    draftFolder.Open(FolderAccess.ReadWrite);

                    draftFolder.Append(msg, MessageFlags.Draft);
                    draftFolder.Expunge();
                }
                else
                {
                    var toplevel = client.GetFolder(client.PersonalNamespaces[0]);
                    var DraftFolder = toplevel.Create(SpecialFolder.Drafts.ToString(), true);

                    DraftFolder.Open(FolderAccess.ReadWrite);
                    DraftFolder.Append(msg, MessageFlags.Draft);
                    DraftFolder.Expunge();
                }
                client.Disconnect(true, cancel.Token);
            }
        }
    }
}
