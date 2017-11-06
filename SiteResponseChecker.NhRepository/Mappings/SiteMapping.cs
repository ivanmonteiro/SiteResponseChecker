using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository.Mappings
{
    public class SiteMapping : ClassMap<Site>
    {
        public SiteMapping()
        {
            Table("Sites");

            Id(x => x.Id, "SiteId").UnsavedValue(0);
            Map(x => x.NotificationEmail);
            Map(x => x.SiteName);
            Map(x => x.SiteUrl);
            Map(x => x.CheckInterval);
            Map(x => x.CheckSpecificElement);
            Map(x => x.SpecificElement);
            Map(x => x.Enabled);
            Map(x => x.SpecificElementType);

            References(x => x.User, "UserId");
            HasMany(x => x.SiteResponses)
                .KeyColumn("SiteId")
                .Inverse()
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.SiteErrors)
                .KeyColumn("SiteId")
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}
