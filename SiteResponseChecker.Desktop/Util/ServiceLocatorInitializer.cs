using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using SharpArchContrib.Castle.CastleWindsor;

namespace SiteResponseChecker.Desktop.Util
{
    public static class ServiceLocatorInitializer
    {
        public static void Init()
        {
            IWindsorContainer container = new WindsorContainer();
            //Register all the Contrib Components  
            ComponentRegistrar.AddComponentsTo(container);

            container.Register(
                  Component
                  .For(typeof(ISessionFactoryKeyProvider))
                  .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
                  .Named("sessionFactoryKeyProvider"));

            container.Register(
                    Component
                        .For(typeof(IEntityDuplicateChecker))
                        .ImplementedBy(typeof(EntityDuplicateChecker))
                        .Named("entityDuplicateChecker"));

            container.Register(
                    Component
                        .For(typeof(IRepository<>))
                        .ImplementedBy(typeof(NHibernateRepository<>))
                        .Named("repositoryType"));

            container.Register(
                    Component
                        .For(typeof(INHibernateRepository<>))
                        .ImplementedBy(typeof(NHibernateRepository<>))
                        .Named("nhibernateRepositoryType"));

            container.Register(
                    Component
                        .For(typeof(IRepositoryWithTypedId<,>))
                        .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
                        .Named("repositoryWithTypedId"));

            container.Register(
                    Component
                        .For(typeof(INHibernateRepositoryWithTypedId<,>))
                        .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
                        .Named("nhibernateRepositoryWithTypedId"));

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }  
    }
}
