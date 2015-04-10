using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using BaseType;
using NLog;
using NLog.Fluent;
using WebMTHR.Settings;

namespace WebMTHR.EmailSender
{
    public class SenderTaskMail : IMailSender
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public SmtpClient SmtpClient { get; set; }

        public ApplicationDbContext DbContext { get; set; }

        public string TemplateString { get; set; }

        public SenderTaskMail(ISenderParam param)
        {
            SmtpClient = new SmtpClient
            {
                Host = param.Host,
                Port = param.Port,
                EnableSsl = param.IsEnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = param.IsUseDefaultCredentials,
                Credentials = new NetworkCredential(param.UserName, param.Password)
            };
            TemplateString = SenderUtils.GetTemplate(param.NameTemplate);
            DbContext = new ApplicationDbContext();
        }
        public async void SendEmail()
        {
            int errorCount = 0;
            try
            {
                var date = DateTime.Now.AddMinutes(-4);
                var notivication =
                    await DbContext.Notivications.Where(w => w.DateCreate > date && w.NotivicationStatus != NotivicationStatus.Delivered && w.NotivicationStatus != NotivicationStatus.Passed).ToListAsync();

                foreach (Notivication item in notivication)
                {
                    try
                    {
                        using (var message = new MailMessage(new MailAddress(item.From.Email, item.From.UserName),
                                new MailAddress(item.To.Email, item.To.UserName)))
                        {
                            message.Subject = item.Task.NameTask;
                            message.Body = GetMessageTaskBody(item);
                            message.IsBodyHtml = true;
                            SmtpClient.Send(message);
                                  item.NotivicationStatus=NotivicationStatus.Delivered;
                            item.TimeSend = DateTime.Now;
                            DbContext.Notivications.AddOrUpdate(item);
                        }
                    }
                    catch (SmtpException ex)
                    {
                        _log.Error(ex);
                        item.NotivicationStatus = NotivicationStatus.Failed;
                        item.TimeSend = DateTime.Now;
                        DbContext.Notivications.AddOrUpdate(item);
                    }
                    catch (IOException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                        item.NotivicationStatus = NotivicationStatus.Failed;
                        item.TimeSend = DateTime.Now;
                        DbContext.Notivications.AddOrUpdate(item);
                    }
                }
                DbContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                _log.FatalException("Отправка сообщений не возможна", ex);
                throw;
            }
        }

        private string GetMessageTaskBody(Notivication notivication)
        {
            return string.Format(TemplateString, notivication.To.UserName, notivication.Task.NameTask,
                notivication.Task.Status,
                string.Format("{0}Task/OpenTask?idTask={1}", WebMTHRConfiguration.Config.CommonSettings.DestinationUrl,
                    notivication.Task.IdTask), notivication.Description, notivication.From.UserName);
        }
    }
}