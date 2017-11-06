using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SharpArch.NHibernate;
using NHibernate.Linq;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository
{
    public class SiteResponseRepository : BaseRepository<SiteResponse>
    {
        //public SiteResponseRepository() { }
        public SiteResponseRepository(ISession session) : base(session) { }

        public SiteResponse GetLastResponse(int siteId)
        {
            return
                Session.QueryOver<SiteResponse>().Where(x => x.Site.Id == siteId).OrderBy(x => x.CheckDate).Desc.Take(1).
                    List().FirstOrDefault();
        }
    }
}
