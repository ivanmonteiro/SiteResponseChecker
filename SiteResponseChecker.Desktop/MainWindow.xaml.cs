using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Castle.Windsor;
using SiteResponseChecker.ApplicationLogic.Jobs;
using SiteResponseChecker.Desktop.Util;
using System.Diagnostics;
using System.IO;
using Hardcodet.Wpf.TaskbarNotification;
using System.Timers;

namespace SiteResponseChecker.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }
        
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxLog();
            Timer timer = new Timer(3000);
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Action del = UpdateTextBoxLog;
            textBox1.Dispatcher.BeginInvoke(del);
        }

        private void UpdateTextBoxLog()
        {
            if (textBox1.Text == null)
                textBox1.Text = "";
            //limits text to total 499 lines
            textBox1.Text = String.Join(Environment.NewLine, App.Logs.Take(499));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //var result =
            //    MessageBox.Show("If you close this application the sites won't be checked for changes.\r\nDo you really want to quit?", "Warning",
            //        MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None);

            //if (result == MessageBoxResult.No)
            //    e.Cancel = true;
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void buttonManageSites_Click(object sender, RoutedEventArgs e)
        {
            if (App.Jobs != null && App.Jobs.Count > 0)
            {
                foreach (var job in App.Jobs)
                {
                    job.Stop();
                }
            }

            ManageSites manageSites = new ManageSites();
            manageSites.ShowDialog();

            if (App.Jobs != null && App.Jobs.Count > 0)
            {
                foreach (var job in App.Jobs)
                {
                    job.Start();
                }
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            var result =
                MessageBox.Show("If you close this application the sites won't be checked for changes.\r\nDo you really want to quit?", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, MessageBoxOptions.None);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to clear the log?", "Warning", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                App.Logs.Clear();
                UpdateTextBoxLog();
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {            
            Clipboard.SetText(textBox1.Text);
        }
    }
}
