using Queue.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RECEIVE.Helper
{
    public   class EmailService
    {

        public bool SendEmail(Emails model)
        {
            var streamList = new List<Stream>();
            try
            {

                var mail = new MailMessage();
                var fromAddress = "sendemailadress@dizayn34.com";
                var displayName = "dizayn 34";


                var htmlView = AlternateView.CreateAlternateViewFromString(
                                        model.Content,
                                        Encoding.UTF8,
                                        MediaTypeNames.Text.Html);

                    if (model.attachmentDatas == null || model.attachmentDatas.Length == 0)
                    {
                        mail.Body = model.Content;
                    }
                    else
                    {
                        string mediaType = MediaTypeNames.Image.Jpeg;
                        for (int i = 0; i < model.attachmentDatas.Length; i++)
                        {
                            var ms = new MemoryStream(model.attachmentDatas[i]);
                            streamList.Add(ms);
                            var img = new LinkedResource(ms, mediaType)
                            {
                                ContentId = "@@Embedded" + i,
                                TransferEncoding = TransferEncoding.Base64
                            };
                            img.ContentType.MediaType = mediaType;
                            img.ContentType.Name = img.ContentId;
                            img.ContentLink = new Uri("cid:" + img.ContentId);
                            htmlView.LinkedResources.Add(img);

                        }
                        mail.AlternateViews.Add(htmlView);
                    }

                    if (model.fileAttachments != null && model.fileAttachments.Any())
                        foreach (var file in model.fileAttachments)
                            mail.Attachments.Add(new Attachment(new MemoryStream(file.data), file.fileNameWithExtension));

 

                    mail.From = new MailAddress(fromAddress, displayName);
                    mail.To.Add(model.To);
                    mail.Subject = model.Subject;
                    mail.IsBodyHtml = true;

                    
                        var smtpMailClient = new SmtpClient("smtp.smtpaddress.com", int.Parse("587"))
                        {
                            UseDefaultCredentials = true,
                            Credentials =
                                new NetworkCredential("emailsendadress@dizayn34.com",
                                    "password"),
                            EnableSsl = true,
                            Timeout = 50000
                        };
                          smtpMailClient.Send(mail);
                        Console.WriteLine($"mail ok : {model.To}");
                        return true;
                     
                   
                
            }
            catch (Exception ex)
            {
                
                return false;
            }
            finally
            {
                streamList.ForEach(c => c?.Dispose());
            }
        }

    }
}
