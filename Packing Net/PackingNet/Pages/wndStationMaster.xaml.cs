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
using Packing_Net.Classes;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for wndStationMaster.xaml
    /// </summary>
    public partial class wndStationMaster : Window
    {
        public wndStationMaster()
        {
            InitializeComponent();
            //Set Default page.
            defaultPage();
        }
        public void defaultPage()
        {
            pgStationRegistration Station = new pgStationRegistration();
            frmStation.Content = Station;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            lblLastLoginTime.Content = Global.LastLoginDateTime;
            lblUserTop.Content = Global.LoggedUserModel.UserInfo.UserFullName.ToString();
        }

        
    }
}
