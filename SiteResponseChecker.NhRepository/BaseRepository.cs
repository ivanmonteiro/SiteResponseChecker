using NHibernate;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;

namespace SiteResponseChecker.NhRepository
{
    public class BaseRepository<T> : NHibernateRepositoryWithTypedId<T, int>, INHibernateRepository<T>
    {
        //public BaseRepository()
        //{
        //}

        public BaseRepository(ISession session)
        {
            _provided_session = session;
        }

        private ISession _provided_session;

        protected override ISession Session
        {
            get
            {
                if (_provided_session != null)
                    return _provided_session;
                else
                    return base.Session;
            }
        }
    }
}