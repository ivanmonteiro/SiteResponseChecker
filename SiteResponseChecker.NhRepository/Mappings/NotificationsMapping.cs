using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository.Mappings
{
    public class NotificationsMapping : ClassMap<Notification>
    {
        public NotificationsMapping()
        {
            Table("Notifications");

            Id(x => x.Id, "NotificationsId").UnsavedValue(0);
            Map(x => x.NotificationDate);
            Map(x => x.SendDate);
            Map(x => x.SendError);
            Map(x => x.IsSent);
            Map(x => x.Subject);
            Map(x => x.Contents);

            References(x => x.Site, "SiteId");
        }
    }
}
