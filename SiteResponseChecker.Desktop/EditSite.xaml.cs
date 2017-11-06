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
using SiteResponseChecker.Domain;
using SiteResponseChecker.NhRepository;

namespace SiteResponseChecker.Desktop
{
    /// <summary>
    /// Interaction logic for EditSite.xaml
    /// </summary>
    public partial class EditSite : Window
    {
        public int SiteId { get; set; }

        public EditSite()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            comboBoxSpecificElementType.ItemsSource = Enum.GetValues(typeof(SpecificElementType));

            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                SiteRepository siteRepository = new SiteRepository(session);
                Site site = siteRepository.Get(SiteId);
                checkBoxEnabled.IsChecked = site.Enabled;
                textBoNotificationEmail.Text = site.NotificationEmail;
                textBoxSiteName.Text = site.SiteName;
                textBoxSiteUrl.Text = site.SiteUrl;
                textBoxCheckInterval.Text = site.CheckInterval.ToString();
                checkBoxCheckSpecificElement.IsChecked = site.CheckSpecificElement;
                comboBoxSpecificElementType.SelectedValue = site.SpecificElementType;
                if (!String.IsNullOrEmpty(site.NotificationEmail))
                {
                    if (!site.NotificationEmail.Equals(site.User.Email))
                    {
                        checkBoxSendtoOtherEmail.IsChecked = true;    
                    }
                }
                textBoxSpecificElement.Text = site.SpecificElement;
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    SiteRepository siteRepository = new SiteRepository(session);
                    Site site = siteRepository.Get(SiteId);
                    site.SiteName = textBoxSiteName.Text;
                    site.SiteUrl = textBoxSiteUrl.Text;
                    site.CheckInterval = Convert.ToInt32(textBoxCheckInterval.Text);
                    if (checkBoxCheckSpecificElement.IsChecked.GetValueOrDefault())
                    {
                        site.CheckSpecificElement = true;
                        site.SpecificElement = textBoxSpecificElement.Text;
                        site.SpecificElementType = (SpecificElementType)comboBoxSpecificElementType.SelectedValue;
                    }
                    site.Enabled = checkBoxEnabled.IsChecked.GetValueOrDefault();
                    if (checkBoxSendtoOtherEmail.IsChecked.GetValueOrDefault())
                    {
                        site.NotificationEmail = textBoNotificationEmail.Text;
                    }
                    else
                    {
                        site.NotificationEmail = site.User.Email;
                    }
                    siteRepository.SaveOrUpdate(site);
                    transaction.Commit();
                }
            }

            this.Close();
        }

        private void checkBoxCheckSpecificElement_Checked(object sender, RoutedEventArgs e)
        {
            labelSpecificElement.Visibility = Visibility.Visible;
            textBoxSpecificElement.Visibility = Visibility.Visible;
            labelSpecificElementType.Visibility = Visibility.Visible;
            comboBoxSpecificElementType.Visibility = Visibility.Visible;
        }

        private void checkBoxCheckSpecificElement_Unchecked(object sender, RoutedEventArgs e)
        {
            labelSpecificElement.Visibility = Visibility.Collapsed;
            textBoxSpecificElement.Visibility = Visibility.Collapsed;
            labelSpecificElementType.Visibility = Visibility.Collapsed;
            comboBoxSpecificElementType.Visibility = Visibility.Collapsed;
        }

        private void checkBoxSendtoOtherEmail_Checked(object sender, RoutedEventArgs e)
        {
            labelNotificationEmail.Visibility = Visibility.Visible;
            textBoNotificationEmail.Visibility = Visibility.Visible;
        }

        private void checkBoxSendtoOtherEmail_Unchecked(object sender, RoutedEventArgs e)
        {
            labelNotificationEmail.Visibility = Visibility.Collapsed;
            textBoNotificationEmail.Visibility = Visibility.Collapsed;
        }
    }
}
