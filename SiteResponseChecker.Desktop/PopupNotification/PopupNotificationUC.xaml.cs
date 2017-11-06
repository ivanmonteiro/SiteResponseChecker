using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using SiteResponseChecker.NhRepository;
using SiteResponseChecker.ApplicationLogic.Utils;
using MSHTML;

namespace SiteResponseChecker.Desktop
{
    public partial class PopupNotificationUC : UserControl
    {
        private bool _isClosing = false;

        public PopupNotificationModel Model { get; set; }

        public PopupNotificationUC(PopupNotificationModel model)
        {
            Model = model;
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
        }
        
        private void me_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model != null && !String.IsNullOrEmpty(Model.TextDiff))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(NotificationHelper.GetBodyTopHtml());
                sb.Append(Model.TextDiff);
                sb.Append(NotificationHelper.GetBottomHtml());
                webBrowserNotificationBaloon.NavigateToString(sb.ToString());                
            }
            else
            {
                webBrowserNotificationBaloon.NavigateToString("No contents... :(");
            }
        }
        
        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the custom fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true; //suppresses the popup from being closed immediately
            _isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
            App.IsShowingNotification = false;
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (_isClosing) return;

            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }


        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }
        
        private void btnOpenSite_Click(object sender, RoutedEventArgs e)
        {
            if (Model != null && !String.IsNullOrEmpty(Model.SiteUri))
            {
                System.Diagnostics.Process.Start(new Uri(Model.SiteUri).ToString());
            }
        }

        private void btnMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start(SiteUri);
            ResponseViewer rv = new ResponseViewer();
            rv.SiteId = Model.SiteId;
            rv.Show();
            //brings window to top
            rv.Activate();
            rv.Topmost = true;  // important
            rv.Topmost = false; // important
            rv.Focus();         // important


            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        private void webBrowserNotificationBaloon_Navigated(object sender, NavigationEventArgs e)
        {
            Util.WebBrowserUtils.SetSilent(webBrowserNotificationBaloon, true);
            IHTMLDocument2 document = (IHTMLDocument2)webBrowserNotificationBaloon.Document;
            IHTMLElementCollection elements = document.body.all;
            foreach (MSHTML.IHTMLElement element in elements)
            {
                if (element.getAttribute("Name").ToString().Contains("anchor"))
                {
                    element.scrollIntoView(true);
                    break;
                }
            }
            //HtmlElement sectionAnchor = 
        }
    }
}
