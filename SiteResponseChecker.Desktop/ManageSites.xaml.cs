using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SharpArch.NHibernate;
using SiteResponseChecker.NhRepository;
using NHibernate;
using SiteResponseChecker.Domain;
using SiteResponseChecker.ApplicationLogic;

namespace SiteResponseChecker.Desktop
{
    /// <summary>
    /// Interaction logic for GerenciarSites.xaml
    /// </summary>
    public partial class ManageSites : Window
    {
        //private ISession _currentSession = NHibernateSession.Current;

        public ManageSites()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateDataGrid();
        }

        private void buttonAddSite_Click(object sender, RoutedEventArgs e)
        {
            AddSite addSite = new AddSite();
            addSite.ShowDialog();
            UpdateDataGrid();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Site site = ((FrameworkElement)sender).DataContext as Site;
            EditSite editSite = new EditSite();
            editSite.SiteId = site.Id;
            editSite.ShowDialog();
            UpdateDataGrid();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to permanently delete this site checker?", "Are you sure?", MessageBoxButton.OKCancel);

            if (messageBoxResult == MessageBoxResult.OK)
            {
                using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        SiteRepository siteRepository = new SiteRepository(session);
                        Site siteToDelete = ((FrameworkElement) sender).DataContext as Site;
                        int id = siteToDelete.Id;
                        Site site = siteRepository.Get(id);
                        siteRepository.Delete(site);
                        transaction.Commit();
                    }
                }

                UpdateDataGrid();
            }
        }

        private void btnViewLastResponse_Click(object sender, RoutedEventArgs e)
        {
            Site site = ((FrameworkElement)sender).DataContext as Site;
            ResponseViewer responseViewer = new ResponseViewer();
            responseViewer.SiteId = site.Id;
            responseViewer.ShowDialog();
        }

        private void UpdateDataGrid()
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                SiteRepository siteRepository = new SiteRepository(session);
                dataGrid1.DataContext = siteRepository.GetAll().ToList();
            }
        }

        private void btnCheckNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        SiteRepository siteRepository = new SiteRepository(session);
                        SiteResponseRepository siteResponseRepository = new SiteResponseRepository(session);
                        SiteErrorRepository siteErrorRepository = new SiteErrorRepository(session);
                        NotificationsRepository notificationsRepository = new NotificationsRepository(session);

                        Site siteToUpdate = ((FrameworkElement)sender).DataContext as Site;
                        int id = siteToUpdate.Id;
                        Site site = siteRepository.Get(id);
                        SiteResponse lastResponse = siteResponseRepository.GetLastResponse(site.Id);

                        ResponseChecker checker = new ResponseChecker(ApplicationLogic.Utils.Logger.Instance, siteRepository, siteResponseRepository, siteErrorRepository, notificationsRepository);
                        checker.CheckResponse(site, lastResponse);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLogic.Utils.Logger.Instance.LogError(ex);
            }            

            UpdateDataGrid();
            
        }
    }
}
