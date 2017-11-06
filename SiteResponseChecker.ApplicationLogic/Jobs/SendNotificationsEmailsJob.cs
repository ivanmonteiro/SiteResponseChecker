using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteResponseChecker.ApplicationLogic.Utils;
using SiteResponseChecker.Domain;
using SiteResponseChecker.NhRepository;

namespace SiteResponseChecker.ApplicationLogic.Jobs
{
    public class SendNotificationsEmailsJob : BaseJob
    {
        public SendNotificationsEmailsJob(bool assync, double minutes)
            : base("Notification Email Sender", assync, minutes)
        {
        }

        public override void DoJob()
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    NotificationsRepository notificationsRepository = new NotificationsRepository(session);
                    IList<Notification> notSent = notificationsRepository.GetAllNotSent();
                    foreach (var notification in notSent)
                    {
                        try
                        {
                            new EmailSender().SendEmail(notification.Site.User.Email, notification.Subject, notification.Contents);
                            notification.SendDate = DateTime.Now;
                            notification.IsSent = true;
                            notificationsRepository.SaveOrUpdate(notification);
                        }
                        catch (Exception ex)
                        {
                            notification.SendError = ex.Message;
                            notificationsRepository.SaveOrUpdate(notification);
                            Logger.LogError(ex);
                        }
                    }

                    transaction.Commit();
                }
            }
        }
    }
}