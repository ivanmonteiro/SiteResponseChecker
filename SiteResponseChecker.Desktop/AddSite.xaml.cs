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
    /// Interaction logic for AddSite.xaml
    /// </summary>
    public partial class AddSite : Window
    {
        public AddSite()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            comboBoxSpecificElementType.ItemsSource = Enum.GetValues(typeof(SpecificElementType));
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var session = SharpArch.NHibernate.NHibernateSession.GetDefaultSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    UserRepository userRepository = new UserRepository(session);
                    SiteRepository siteRepository = new SiteRepository(session);
                    Site site = new Site();
                    
                    site.SiteName = textBoxSiteName.Text;
                    site.SiteUrl = textBoxSiteUrl.Text;
                    site.CheckInterval = Convert.ToInt32(textBoxCheckInterval.Text);
                    if (checkBoxCheckSpecificElement.IsChecked.GetValueOrDefault())
                    {
                        site.CheckSpecificElement = true;
                        site.SpecificElement = textBoxSpecificElement.Text;
                        site.SpecificElementType = (SpecificElementType) comboBoxSpecificElementType.SelectedValue;
                    }
                    User currentUser = userRepository.GetAll().FirstOrDefault();
                    site.User = currentUser;
                    site.Enabled = checkBoxEnabled.IsChecked.GetValueOrDefault();
                    if (checkBoxSendtoOtherEmail.IsChecked.GetValueOrDefault())
                    {
                        site.NotificationEmail = textBoNotificationEmail.Text;
                    }
                    else
                    {
                        site.NotificationEmail = currentUser.Email;
                    }
                    siteRepository.Save(site);
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
