using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository.Mappings
{
    public class SiteErrorMapping : ClassMap<SiteError>
    {
        public SiteErrorMapping()
        {
            Table("SiteErrors");

            Id(x => x.Id, "SiteErrorId").UnsavedValue(0);
            Map(x => x.ErrorType);
            Map(x => x.ErrorDetails);
            Map(x => x.IsRecurring);
            Map(x => x.Date);

            References(x => x.Site, "SiteId");
        }
    }
}
