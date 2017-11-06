using System;
using log4net;
using SiteResponseChecker.Domain;

namespace SiteResponseChecker.ApplicationLogic.Utils
{
    public interface ILogger
    {
        void LogError(Exception myError);
        void LogInfo(string info);
        void LogSiteChanged(Notification notification, Site site, string message, string diff);
        event StringHandler OnInfoLogged;
        event StringHandler OnErrorLogged;
        event SiteChangeNotificationHandler OnSiteChangeNotificationLogged;
    }

    public delegate void SiteChangeNotificationHandler(Notification notification, Site site, string message, string diff);
    public delegate void StringHandler(string info);

    public class Logger : ILogger
    {
        private static ILogger _instance;

        private Logger() { }

        public static ILogger Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
       }

        public event StringHandler OnInfoLogged;
        public event StringHandler OnErrorLogged;
        public event SiteChangeNotificationHandler OnSiteChangeNotificationLogged;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ILogger).Assembly.GetName().Name);

        public void LogSiteChanged(Notification notification, Site site, string message, string diff)
        {
            Log.Info(message);

            if (OnSiteChangeNotificationLogged != null)
            {
                OnSiteChangeNotificationLogged(notification, site, message, diff);
            }
        }

        public void LogInfo(string info)
        {
            Log.Info(info);

            if (OnInfoLogged != null)
            {
                OnInfoLogged(info);
            }
        }

        public void LogError(Exception myError)
        {
            if (myError != null)
            {
                string myErrorMessage = "Erro:\r\n" +  myError.Message + "\r\n\r\n";

                if (myError.Source != null)
                    myErrorMessage += "Source:\r\n" + myError.Source + "\r\n\r\n";

                if (myError.TargetSite != null)
                    myErrorMessage += "Target site:\r\n" + myError.TargetSite + "\r\n\r\n";

                if (myError.StackTrace != null)
                    myErrorMessage += "Stack trace:\r\n" + myError.StackTrace + "\r\n\r\n";

                if (myError.InnerException != null)
                {
                    myErrorMessage += "Inner Exception:\r\n" + myError.InnerException.Message + "\r\n\r\n";

                    if (myError.InnerException.Source != null)
                        myErrorMessage += "Inner Exception Source:\r\n" + myError.InnerException.Source + "\r\n\r\n";

                    if(myError.InnerException.TargetSite != null)
                        myErrorMessage += "Inner Exception Target site:\r\n" + myError.InnerException.TargetSite + "\r\n\r\n";

                    if(myError.InnerException.StackTrace != null)
                        myErrorMessage += "Inner Exception Stack trace:\r\n" + myError.InnerException.StackTrace + "\r\n\r\n";
                }

                Log.Debug(myErrorMessage, myError);

                if (OnErrorLogged != null)
                {
                    OnErrorLogged(myErrorMessage);
                }
            }
        }
    }
}
