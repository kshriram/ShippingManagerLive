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
using Packing_Net.Pages.ReportPages;
using Packing_Net.Pages;
using System.Windows.Threading;
using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for wndReports.xaml
    /// </summary>
    public partial class wndReports : Window
    {
        public wndReports()
        {
            InitializeComponent();

            String RoleName = Global.LoggedUserModel.UserInfo.RoleName;
            
            if (RoleName != "Admin")
            {
                btnHome.Visibility = System.Windows.Visibility.Hidden;
                imgHome.Visibility = System.Windows.Visibility.Hidden;
            }

            pgShipmentTime ShipmentTime = new pgShipmentTime();
            frmReportViwer.Content = ShipmentTime;
            lblLastLoginTime.Content = Global.LastLoginDateTime.ToString("MMM dd, yyyy h:m tt");
            lblUserTop.Content = Global.WindowTopUserName.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Packing_Net.Pages.HomeScreen H = new HomeScreen();
            H.Show();
            this.Close();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen _Home = new HomeScreen();
            _Home.Show();
            this.Close();
        }

        private void btnShipmentScan_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Shipment = new MainWindow();
            this.Close();
            Shipment.ShowDialog();
            
        }
    }
}
