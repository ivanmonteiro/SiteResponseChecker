using System;
using System.Data;
using SharpArch.NHibernate;
using SiteResponseChecker.ApplicationLogic.Utils;

namespace SiteResponseChecker.ApplicationLogic.Jobs
{
    public abstract class BaseJob
    {
        private System.Timers.Timer timer;
        public ILogger Logger { get; set; }
        public bool Assync { get; set; }
        private static object sync_lock = new object();
        public string JobName { get; set; }

        public BaseJob(string jobName, bool assync, double minutes)
        {
            JobName = jobName;
            Assync = assync;
            Logger = Utils.Logger.Instance;
            timer = new System.Timers.Timer();
            timer.Interval = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            timer.Start();
            Logger.LogInfo("Initialized job " + JobName + ".");
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //TODO: better locking
            lock (sync_lock)
            {
                //using (var currentSession = NHibernateSession.Current)
                //{
                    try
                    {
                        //currentSession.Transaction.Begin(IsolationLevel.ReadCommitted);
                        DoJob();
                        //currentSession.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        //currentSession.Transaction.Rollback();
                    }
                //}
            }
        }

        public abstract void DoJob();

        public void Stop()
        {
            Logger.LogInfo("Stopping job " + JobName + ".");

            if(timer != null)
            {
                timer.Enabled = false;
            }
        }

        public void Start()
        {
            Logger.LogInfo("Starting job " + JobName + ".");
            
            if (timer != null)
            {
                timer.Enabled = true;
            }
        }
    }
}
