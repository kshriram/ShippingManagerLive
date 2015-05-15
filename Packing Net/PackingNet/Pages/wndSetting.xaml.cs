using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Threading;
using Packing_Net.Classes;
using Packing_Net.Pages.ReportPages;
namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {

        DispatcherTimer timer;
        public SettingWindow()
        {


            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();


            lblUserTop.Content = Global.WindowTopUserName;
            //Auto Timer Restart..
            SessionManager.StartTime();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen _home = new HomeScreen();
            _home.Show();
            this.Close();
        }

        private void btnTimeSetting_Click(object sender, RoutedEventArgs e)
        {
            //Auto Timer Restart..
            SessionManager.StartTime();
            PgTimeSetting _time = new PgTimeSetting();
            frmMain.Content = _time;
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {

            PgTimeSetting _time = new PgTimeSetting();
            frmMain.Content = _time;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Change Language
            WindowLanguages.Convert(this);
        }
    }
}
