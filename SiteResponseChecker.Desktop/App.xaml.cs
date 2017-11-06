using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using Hardcodet.Wpf.TaskbarNotification;
using SiteResponseChecker.ApplicationLogic.Jobs;
using SiteResponseChecker.Desktop.Util;
using SiteResponseChecker.Domain;
using log4net.Appender;
using System.Timers;

namespace SiteResponseChecker.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IList<BaseJob> Jobs;
        public static IList<string> Logs = new List<string>();
        public static bool IsShowingNotification = false;
        private TaskbarIcon _notifyIcon;
        System.Timers.Timer timerTest = new System.Timers.Timer();
        System.Timers.Timer timerShowStackedNotifications = new System.Timers.Timer();
        
        Stack<Action> notificationsToProcess = new Stack<Action>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Debugger.IsAttached)
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            Configure();
        }

        private void Configure()
        {
            ApplicationLogic.Utils.Logger.Instance.OnInfoLogged += info => Logs.Insert(0, info);
            ApplicationLogic.Utils.Logger.Instance.OnErrorLogged += info => Logs.Insert(0, info);
            ApplicationLogic.Utils.Logger.Instance.OnSiteChangeNotificationLogged += (notification, site, message, diff) =>
                {
                    Action del = () => OnSiteChangeNotificationLogged(site.Id, site.SiteUrl, site.SiteName, diff);
                    notificationsToProcess.Push(del);
                    //_notifyIcon.Dispatcher.BeginInvoke(del);
                    //OnSiteChangeNotificationLogged(site.SiteUrl, message);
                };

            log4net.Config.XmlConfigurator.Configure();
            
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\appdb.db"))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\appdb.db");
            }

            ServiceLocatorInitializer.Init();

            Initializer.Init();
            
            Jobs = new List<BaseJob>()
                {
                    new CheckSitesJob(false, 2),
                    new SnapshotJob(false, 25),
                    new SendNotificationsEmailsJob(false, 5)
                };


            GC.KeepAlive(Jobs);

            timerShowStackedNotifications.Interval = 3000;
            timerShowStackedNotifications.Elapsed += timerShowStackedNotifications_Elapsed;
            timerShowStackedNotifications.Start();

            if (Debugger.IsAttached)
            {
                timerTest.Interval = 3000;
                timerTest.Elapsed += timerTest_Elapsed;
                timerTest.Start();
            }           

            //Action test1 = () => OnSiteChangeNotificationLogged(1, "http://www.google.com", "Google", "-1 \n-2");
            //Action test2 = () => OnSiteChangeNotificationLogged(1, "http://www.uol.com.br", "Uol", "-1sdsd \n-2rsrs");
            //notificationsToProcess.Push(test1);
            //notificationsToProcess.Push(test2);

        }

        void timerShowStackedNotifications_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsShowingNotification)
            {
                if (notificationsToProcess.Count > 0)
                {
                    var action = notificationsToProcess.Pop();
                    if (action != null)
                    {
                        _notifyIcon.Dispatcher.BeginInvoke(action);
                        IsShowingNotification = true;
                    }
                }
            }            
        }

        void timerTest_Elapsed(object sender, ElapsedEventArgs e)
        {
            PopupNotificationModel model_test = new PopupNotificationModel()
            {
                SiteId = 1,
                SiteName = "test",
                SiteUri = "http://www.gmail.com/",
                TextDiff = "<br><br><br><br><br><br><p class='added'><a name='anchor'>+</a> added</p>" +
                           "<br><br><br><br><p class='notchanged'>Not changed</p>" +
                           "<br><br><br><br><p class='removed'>- removed</p>" +
                           "<br><br><br><br><br><br><br><br><br>"
            };

            Action del = () => {
                PopupNotificationUC notification = new PopupNotificationUC(model_test);
                _notifyIcon.ShowCustomBalloon(notification, PopupAnimation.Slide, null);                
            };
            _notifyIcon.Dispatcher.BeginInvoke(del);
            timerTest.Stop();
        }

        private void OnSiteChangeNotificationLogged(int id, string url, string siteName, string textDiff)
        {
            PopupNotificationUC notification = new PopupNotificationUC(new PopupNotificationModel() 
            {
                SiteId = id,
                TextDiff = textDiff,
                SiteName = siteName,
                SiteUri = url
            });
            _notifyIcon.ShowCustomBalloon(notification, PopupAnimation.Slide, null);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner

            if (Jobs != null && Jobs.Count > 0)
            {
                foreach (var job in Jobs)
                {
                    job.Stop();
                }
            } 

            base.OnExit(e);
            
        }
    }
}
