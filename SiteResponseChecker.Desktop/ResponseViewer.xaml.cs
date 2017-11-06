using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SiteResponseChecker.ApplicationLogic.Utils;
using SiteResponseChecker.Domain;
using SiteResponseChecker.NhRepository;

namespace SiteResponseChecker.Desktop
{
    /// <summary>
    /// Interaction logic for ResponseViewer.xaml
    /// </summary>
    public partial class ResponseViewer : Window
    {
        public int SiteId { get; set; }

        public ResponseViewer()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                //SiteRepository siteRepository = new SiteRepository(session);
                //Site site = siteRepository.Get(SiteId);
                SiteResponseRepository siteResponseRepository = new SiteResponseRepository(session);
                var lastResponse = siteResponseRepository.GetLastResponse(SiteId);
                if (lastResponse != null)
                {
                    tbHtmlContents.Text = lastResponse.Contents;
                    tbTextContents.Text = SiteHtmlUtil.StripHTMLAdvanced(lastResponse.Contents);
                    //tbDiffContents.Text = lastResponse.Diff;
                    if(String.IsNullOrEmpty(lastResponse.Diff))
                    {
                        webBrowserDiffContents.NavigateToString("No contents... :(");
                    } 
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(NotificationHelper.GetBodyTopHtml());
                        sb.Append(lastResponse.Diff);
                        sb.Append(NotificationHelper.GetBottomHtml());
                        webBrowserDiffContents.NavigateToString(sb.ToString());
                    }
                    

                    if (String.IsNullOrEmpty(lastResponse.Contents))
                    {
                        webBrowser.NavigateToString("No contents... :(");
                    } else
                    {
                        webBrowser.NavigateToString(lastResponse.Contents);
                    }
                    
                    liveBrowser.Navigate(lastResponse.Site.SiteUrl);
                    //webBrowser.Document
                }
            }
        }

        private void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {   
            Util.WebBrowserUtils.SetSilent(webBrowser, true);
        }

        private void liveBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Util.WebBrowserUtils.SetSilent(liveBrowser, true);
        }

        private void webBrowserDiffContents_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Util.WebBrowserUtils.SetSilent(liveBrowser, true);
        }

        private void btnOpenSite_Click(object sender, RoutedEventArgs e)
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                SiteRepository siteRepository = new SiteRepository(session);
                Site site = siteRepository.Get(SiteId);
                if (site != null && !String.IsNullOrEmpty(site.SiteUrl))
                {
                    System.Diagnostics.Process.Start(new Uri(site.SiteUrl).ToString());
                }
            }           
        }

    }

    [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleServiceProvider
    {
      [PreserveSig]
      int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
    }
}
