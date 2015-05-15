using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Packing_Net.Pages;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System.Windows.Threading;
using PackingNet.Pages;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public HomeScreen()
        {
            InitializeComponent();

            txtScan.Focus();

            //time on top of window
            if (Global.ISStateTimer == false)
            {
                timer.Tick += timer_Tick;
                timer.Interval = new TimeSpan(0, 0,30);
                timer.Start();
                Global.ISStateTimer = true;
            }

            lblUserTop.Content = Global.WindowTopUserName;
            lblLastLoginTime.Content = Convert.ToString(Global.LastLoginDateTime.ToString("dd mmm, yyyy h:m tt"));

            //Auto Timer Restart..
            SessionManager.StartTime();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            
                if (CheckStationRegistred.IsRegistered() == false)
                {
                    wndStationMaster _Station = new wndStationMaster();
                    _Station.ShowDialog();
                    txtScan.Focus();
                }
            
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //log
            SaveUserLogsTbl.logThis(csteActionType.Home_Button_Shipment.ToString());
            MainWindow _Shipment = new MainWindow();
            _Shipment.Show();
            this.Close();

            txtScan.Focus();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //log
            SaveUserLogsTbl.logThis(csteActionType.Home_Button_UserManagement.ToString());

            UserManagement _UserRegis = new UserManagement();
            _UserRegis.Show();
            this.Close();
            txtScan.Focus();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            //log
            SaveUserLogsTbl.logThis(csteActionType.Home_Button_Setting.ToString());
            SettingWindow _setting = new SettingWindow();
            _setting.Show();
            this.Close();
            txtScan.Focus();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            //log
            SaveUserLogsTbl.logThis(csteActionType.Logout.ToString());
            Global.MsgBoxResult = "";
            MsgBox.Show("Error", "Logout", Environment.NewLine + " Are you sure want to logout? ");
            if (Global.MsgBoxResult == "Ok")
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            txtScan.Focus();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //log
            SaveUserLogsTbl.logThis(csteActionType.Home_Button_Exit.ToString());
            Global.MsgBoxResult = "";
            MsgBox.Show("Error", "Exit", Environment.NewLine+" Are you sure want to Exit?");
            if (Global.MsgBoxResult=="Ok")
            {
                Application.Current.Shutdown();
            }
            txtScan.Focus();
           
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
           //Log .
            SaveUserLogsTbl.logThis(csteActionType.HomeScreenLoaded.ToString());

            //Auto Timer Restart..
            SessionManager.StartTime();
            if (Global.ISTimerTickInitialised==false)
            {
                SessionManager.Autotimer.Tick += Autotimer_Tick;
                Global.ISTimerTickInitialised = true;       
            }
            //Convert Language
            WindowLanguages.Convert(this);
         
        }

        private void Autotimer_Tick(object sender, EventArgs e)
        {

            if (Global.ISTimerRaised == false)
            {
                //Log .
                SaveUserLogsTbl.logThis(csteActionType.SessionTimeOut.ToString());

                Global.ISTimerRaised = true;
                Login _login = new Login();
                _login.ShowDialog();
            }

        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
         //Log.
            SaveUserLogsTbl.logThis(csteActionType.HomeScreenClosed.ToString());
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndReports reports = new wndReports();
                reports.ShowDialog();
                this.Close();
            }
            catch (Exception)
            {}
            
        }

        private void btnRoleManagement_Click(object sender, RoutedEventArgs e)
        {
            wndRoleMngt _role = new wndRoleMngt();
            _role.Show();
            this.Close();
        }

        private void btnPallet_Click_1(object sender, RoutedEventArgs e)
        {
            wndShipmentForPallet _pallet = new wndShipmentForPallet();
            _pallet.ShowDialog();
        }

        private void btnBOLNumber_Click_1(object sender, RoutedEventArgs e)
        {
            wndPalletPrintStatus _print = new wndPalletPrintStatus();
            _print.ShowDialog();
        }

        private void txtScan_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtScan.Text.Trim() != "")
            {
                if (txtScan.Text.Contains("#"))
                {
                    if (txtScan.Text == "#shipment" || txtScan.Text == "#SHIPMENT")
                    {
                        txtScan.Text = "";

                        if (btnshpment.IsVisible)
                        {
                            ButtonAutomationPeer peer = new ButtonAutomationPeer(btnshpment);
                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            invokeProv.Invoke();
                        }
                        else
                        {
                           // ShowMassagePopup("You can not add new box.", 3000);
                            //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));

                        }
                    }
                    else
                    {
                        MsgBox.Show("Error", "Invalid", "Invalid Scan");
                    }
                }
                else
                {
                    MsgBox.Show("Error", "Invalid", "Invalid Scan");
                
                }

            }
        }
    }
}