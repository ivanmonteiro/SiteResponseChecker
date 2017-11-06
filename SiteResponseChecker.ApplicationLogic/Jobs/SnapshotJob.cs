using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteResponseChecker.NhRepository;
using SiteResponseChecker.Domain;
using SiteResponseChecker.ApplicationLogic.Utils;

namespace SiteResponseChecker.ApplicationLogic.Jobs
{
    public class SnapshotJob : BaseJob
    {
        public SnapshotJob(bool assync, double minutes) : base("Snapshot Email Sender", assync, minutes)
        {
        }

        public override void DoJob()
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    UserRepository userRepository = new UserRepository(session);
                    SiteResponseRepository siteResponseRepository = new SiteResponseRepository(session);
                    IList<User> usersWithPendingSnapshots = userRepository.FindWithPendingSnapshots();//.AsQueryable()
                    //.Include("Site")
                    //.Where(x => (x.LastSnapshotDate == null || EntityFunctions.AddDays(x.LastSnapshotDate, x.SnapshotInterval) <=    DateTime.Now) && x.Site.Count(s => s.Enabled) > 0);

                    foreach (var user in usersWithPendingSnapshots)
                    {
                        Logger.LogInfo("Sending snapshot...");
                        new EmailSender().SendSnapshotEmail(user, BuildSnapshotNotification(siteResponseRepository, user, user.Sites.Where(x => x.Enabled)));
                        user.LastSnapshotDate = DateTime.Now;
                        userRepository.SaveOrUpdate(user);
                        //UnitOfWork.Commit();
                    }

                    transaction.Commit();
                }
            }
        }

        private static string BuildSnapshotNotification(SiteResponseRepository siteResponseRepository, User user, IEnumerable<Site> sites)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var site in sites)
            {
                string snapshotContents = null;

                var lastResponse = siteResponseRepository.GetLastResponse(site.Id);

                if (lastResponse != null)
                    snapshotContents = SiteHtmlUtil.StripHTMLAdvanced(lastResponse.Contents);
                else
                    snapshotContents = "No response available right now.";

                snapshotContents = snapshotContents.Replace("(\r|\n|\r\n)+", "<br //>");

                sb.AppendFormat(@"Snapshot for <b><a href='{0}'>{1}</a></b><br /><br />{2}<br /><br />", site.SiteUrl,
                                site.SiteName, snapshotContents);

                var siteErrorsForPeriod =
                    site.SiteErrors.Where(x => x.Date >= DateTime.Now.Subtract(TimeSpan.FromDays(user.SnapshotInterval))).OrderBy(x => x.Date).Take(10).ToList();

                if (siteErrorsForPeriod.Count > 0)
                {
                    sb.Append("Errors since last snapshot: <br />");

                    foreach (var siteError in siteErrorsForPeriod)
                    {
                        sb.AppendFormat("Em {0} ocorreu o erro: {1}. <br />", siteError.Date, siteError.ErrorDetails);
                    }
                }

                sb.Append("<br /><br />");
            }

            return sb.ToString();
        }
    }
}
