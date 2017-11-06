using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository
{
    public class NotificationsRepository : BaseRepository<Notification>
    {
        //public NotificationsRepository() { }
        public NotificationsRepository(ISession session) : base(session) { }

        public IList<Notification> GetAllNotSent()
        {
            return Session.QueryOver<Notification>().Where(x => x.IsSent == false).Take(20).List();
        }
    }
}
