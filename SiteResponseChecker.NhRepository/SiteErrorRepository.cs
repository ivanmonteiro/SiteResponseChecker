using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SharpArch.NHibernate;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository
{
    public class SiteErrorRepository : BaseRepository<SiteError>
    {
        //public SiteErrorRepository() { }
        public SiteErrorRepository(ISession session) : base(session) { }

        public IList<SiteError> FindTop10BySite(Site site)
        {
            return Session.QueryOver<SiteError>()
                .Where(x => x.Site.Id == site.Id).OrderBy(x => x.Date)
                .Desc
                .Take(10)
                .List();
        }

        public void DeleteOld(Site site)
        {
            //Session.CreateQuery(
            //    "delete from SiteError se where se.Site.Id = :siteId and se.Id not in (select top 99 se2.Id from SiteError se2 where se2.Site.Id = :siteId order by se2.Date desc)")
            //       .SetInt32("siteId", site.Id)
            //       .ExecuteUpdate();

            Session.CreateQuery("delete from SiteError se where se.Id in (select old.Id from SiteError old where old.Date < :date)")
                .SetDateTime("date", DateTime.Now.Subtract(TimeSpan.FromDays(31)))
                .ExecuteUpdate();
        }
    }
}
