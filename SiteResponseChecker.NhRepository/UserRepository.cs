using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.PersistenceSupport;
using NHibernate.Linq;
using SiteResponseChecker.Domain;
using NHibernate.Criterion;
using NHibernate;

namespace SiteResponseChecker.NhRepository
{
    public class UserRepository : BaseRepository<User>
    {
        //public UserRepository() { }
        public UserRepository(ISession session) : base(session) { }

        public IList<User> FindWithPendingSnapshots()
        {
            return Session.QueryOver<User>().List()
                .Where(x=> (x.LastSnapshotDate == null ||
                     x.LastSnapshotDate.GetValueOrDefault().AddDays(x.SnapshotInterval) <= DateTime.Now)).ToList();
            //&& x.Sites.Count(s => s.Enabled) > 0).List();
        }
    }
}
