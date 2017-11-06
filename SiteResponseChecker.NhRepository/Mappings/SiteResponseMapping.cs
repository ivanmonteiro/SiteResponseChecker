using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteResponseChecker.Domain;
using FluentNHibernate.Mapping;

namespace SiteResponseChecker.NhRepository.Mappings
{
    public class SiteResponseMapping : ClassMap<SiteResponse>
    {
        public SiteResponseMapping()
        {
            Table("SiteResponses");
            
            Id(x => x.Id, "SiteResponseId").UnsavedValue(0);
            Map(x => x.Hash);
            Map(x => x.Contents);
            Map(x => x.Diff);
            Map(x => x.StatusCode);
            Map(x => x.CheckDate);

            References(x => x.Site, "SiteId");
        }
    }
}
