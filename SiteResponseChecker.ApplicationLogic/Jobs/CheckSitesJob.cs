using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteResponseChecker.Domain;
using System.Diagnostics;
using SiteResponseChecker.NhRepository;

namespace SiteResponseChecker.ApplicationLogic.Jobs
{
    public sealed class CheckSitesJob : BaseJob
    {
        //public override string JobName { get; set; }

        public CheckSitesJob(bool assync, double minutes) : base("Site Response Checker", assync, minutes)
        {
        }

        public override void DoJob()
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    SiteRepository siteRepository = new SiteRepository(session);
                    SiteResponseRepository siteResponseRepository = new SiteResponseRepository(session);
                    SiteErrorRepository siteErrorRepository = new SiteErrorRepository(session);
                    NotificationsRepository notificationsRepository = new NotificationsRepository(session);
                    
                    List<Action> actions = new List<Action>();

                    foreach (var site in siteRepository.FindAllEnabled())
                    {
                        SiteResponse lastResponse = siteResponseRepository.GetLastResponse(site.Id);//.AsQueryable().Where(x => x.Site.Id == site.Id).OrderByDescending(x => x.CheckDate).FirstOrDefault();

                        if (lastResponse == null || lastResponse.CheckDate.AddMinutes(site.CheckInterval) < DateTime.Now)
                        {
                            ResponseCheckerContext responseContext = new ResponseCheckerContext()
                            {
                                LastResponse = lastResponse,
                                Site = site,
                                SiteRepository = siteRepository,
                                SiteResponseRepository = siteResponseRepository,
                                SiteErrorRepository = siteErrorRepository,
                                NotificationsRepository = notificationsRepository
                            };

                            actions.Add(() => CheckResponse(responseContext));
                        }
                    }

                    if (Assync)
                    {
                        Parallel.Invoke(actions.ToArray());
                    }
                    else
                    {
                        foreach (var action in actions)
                        {
                            action.Invoke();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private void CheckResponse(Object state)
        {
            try
            {
                ResponseCheckerContext responseCheckContext = (ResponseCheckerContext)state;
                ResponseChecker checker = new ResponseChecker(Logger, responseCheckContext.SiteRepository, responseCheckContext.SiteResponseRepository, responseCheckContext.SiteErrorRepository, responseCheckContext.NotificationsRepository);
                checker.CheckResponse(responseCheckContext.Site, responseCheckContext.LastResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}
