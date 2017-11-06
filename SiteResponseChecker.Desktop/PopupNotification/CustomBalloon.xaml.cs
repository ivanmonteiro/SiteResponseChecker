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

namespace SiteResponseChecker.Desktop.NotifyIcon
{
    /// <summary>
    /// Interaction logic for CustomBalloon.xaml
    /// </summary>
    public partial class CustomBalloon : UserControl
    {
        public int SiteId { get; set; }

        public string SiteUri { get; set; }

        private bool _isClosing = false;

        public CustomBalloon()
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
        }

        #region BalloonText dependency property

        /// <summary>
        /// Description
        /// </summary>
        public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                typeof (string),
                typeof (CustomBalloon),
                new FrameworkPropertyMetadata(""));

        /// <summary>
        /// A property wrapper for the <see cref="BalloonTextProperty"/>
        /// dependency property:<br/>
        /// Description
        /// </summary>
        public string BalloonText
        {
            get { return (string) GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        #endregion



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
            Popup pp = (Popup) Parent;
            pp.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start(SiteUri);
            ResponseViewer rv = new ResponseViewer();
            rv.SiteId = SiteId;
            rv.Show();
            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        private void btnOpenSite_Click(object sender, RoutedEventArgs e)
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                //SiteRepository siteRepository = new SiteRepository(session);
                //Site site = siteRepository.Get(SiteId);
                var siteResponseRepository = new SiteRepository(session);
                var site = siteResponseRepository.Get(SiteId);
                if (site != null && !String.IsNullOrEmpty(site.SiteUrl))
                {
                    System.Diagnostics.Process.Start(new Uri(site.SiteUrl).ToString());
                }
            }            
        }
    }
}
