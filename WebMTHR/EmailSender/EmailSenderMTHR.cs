using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using NLog;

namespace WebMTHR.EmailSender
{
    public static class EmailSenderMTHR
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private static Timer NotificationTimer { get; set; }
        private static object _locker = new object();
        private static bool IsFirstCall { get; set; }

        internal static void EmailNotivicationInit()
        {
            IsFirstCall = true;
            _log.Info("EmailSenderMTHR иницализирован");
            NotificationTimer = new Timer(TimerCall, null, TimeSpan.FromMinutes(0.5), TimeSpan.FromMinutes(0.5));
        }

        private static void TimerCall(object state)
        {
            try
            {
                lock (_locker)
                {
                    if (IsFirstCall)
                    {
                        _log.Info("EmailSenderMTHR запущен первый вызов таймера Task");
                        ISenderParam settings = EmailSenderConfiguration.Config.Senders.GetSenderInstance("Task");
                        if (settings == null) throw new ArgumentNullException("Object EmailSender not found 'Task'");
                        var mailFirst = new SendMailFirst(settings);
                        mailFirst.SendEmail();
                        IsFirstCall = false;
                    }
                    else
                    {
                        _log.Info("EmailSenderMTHR вызов таймера Task");
                        var senderTask =
                            new SenderTaskMail(EmailSenderConfiguration.Config.Senders.GetSenderInstance("Task"));
                        senderTask.SendEmail();
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _log.ErrorException(
                    "Ошибка отправки сообщений таймер остановлен, проверте конфигурационный файл и перезапустите службу",
                    ex);
                NotificationTimer.Dispose();
            }
            catch (IOException ex)
            {
                _log.ErrorException(
                    "Ошибка отправки сообщений таймер остановлен, проверте доступ к файлу шаблона и перезапустите службу",
                    ex);
                NotificationTimer.Dispose();
            }
            catch (Exception ex)
            {
                _log.ErrorException("Ошибка выполения операции", ex);
            }
        }
    }
}