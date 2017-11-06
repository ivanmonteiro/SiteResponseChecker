using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SharpArch.NHibernate;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.NhRepository
{
    public class SiteRepository : BaseRepository<Site>
    {
        //public SiteRepository() { }
        public SiteRepository(ISession session) : base(session) { }

        public IList<Site> FindAllEnabled()
        {
            return Session.QueryOver<Site>().Where(x => x.Enabled == true).List();
        }
    }
}
