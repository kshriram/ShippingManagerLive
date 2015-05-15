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

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for wndRoleMngt.xaml
    /// </summary>
    public partial class wndRoleMngt : Window
    {
        public wndRoleMngt()
        {
            InitializeComponent();

            
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen _Home = new HomeScreen();
            _Home.Show();
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            lblUserTop.Content = Global.WindowTopUserName.ToString();
            lblLastLoginTime.Content = Global.LastLoginDateTime.ToString("MMM dd, yyyy h:m tt");

            RoleUI _Role = new RoleUI();
            frmMain.Content = _Role;
        }

    }
}
