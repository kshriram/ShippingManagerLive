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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Packing_Net.Classes;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System.Windows.Threading;

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for pgStationRegistration.xaml
    /// </summary>
    public partial class pgStationRegistration : Page
    {
        DispatcherTimer timer;
        String _deviceNumber = Global.controller.getDeviceMAC();

        public pgStationRegistration()
        {
            
            InitializeComponent();
            //time on top of window
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

        }
        void timer_Tick(object sender, EventArgs e)
        {
           
          string sec = DateTime.UtcNow.Second.ToString();
          if (sec == "30")
          {
              if (CheckStationRegistred.IsRegistered())
              {
                  int wcount = Application.Current.Windows.Count;
                  if (wcount > 0)
                  {
                      Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "StationMaster");
                      if (win != null)
                      {
                          win.Close();
                      }
                  }
              }
          }
        }
        public Boolean SaveStation()
        {
            Boolean _return = false;
            try
            {
               
                List<cstStationMasterTbl> lsStationMaster = Global.controller.GetStationMaster(_deviceNumber);
                if (lsStationMaster.Count <=0)
                {
                    List<cstStationMasterTbl> _lsStation = new List<cstStationMasterTbl>();
                    cstStationMasterTbl Station = new cstStationMasterTbl();
                    Station.StationID = Guid.Empty;
                    Station.StationName = txtStationName.Text;
                    Station.DeviceNumber = _deviceNumber;
                    Station.RequestedUserID =Global.LoggedUserId;
                    Station.StationAlive = 0;
                    Station.RegistrationDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    Station.StaionLocation = Global.controller.ApplicationLocation();
                    _lsStation.Add(Station);
                   _return= Global.controller.SaveStationMaster(_lsStation);
                }
            }
            catch (Exception)
            {}
            return _return;
        }

        private void txtStationName_KeyDown(object sender, KeyEventArgs e)
        {
            if (lblMsg.Text.StartsWith(">>"))
            {
                lblMsg.Text = Global.controller.ConvetLanguage("Press 'Enter' to send registration request.", Global.LanguageFileName);
                bdrMsg.Visibility = Visibility.Hidden;
                bdrMsg.Visibility = Visibility.Visible;
                bdrMsg.Background = new SolidColorBrush(Color.FromRgb(34,135,19));
            }

            if (e.Key == Key.Enter)
            {
               Boolean saved= SaveStation();
               if (saved== true)
               {
                   MsgBox.Show("Ok", "Save", "Save successfully!");
                   bdrMsg.Visibility = Visibility.Hidden;
                   bdrMsg.Visibility = Visibility.Visible;
                   lblMsg.Text = Global.controller.ConvetLanguage("Station is not activated. Waiting for activation.", Global.LanguageFileName);
                   bdrMsg.Background = new SolidColorBrush(Color.FromRgb(199, 90, 28));
               }
               
               txtStationName.Text = "";
               txtStationName.IsEnabled = false;
            }
            
        }

        private void txtStationName_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

       

        private void cvsMain_Loaded(object sender, RoutedEventArgs e)
        {
            txtStationName.Focus();

            List<cstStationMasterTbl> lsStationMaster = Global.controller.GetStationMaster(_deviceNumber);
            bdrMsg.Visibility = Visibility.Hidden;
            if (lsStationMaster.Count <= 0)
            {
                bdrMsg.Visibility = Visibility.Visible;
                bdrMsg.Background = new SolidColorBrush(Color.FromRgb(199, 90, 28));

                
            }
            else if (lsStationMaster[0].StationAlive == 0)
            {
                lblMsg.Text = Global.controller.ConvetLanguage("Station not activated. Waiting for activation.", Global.LanguageFileName);
                
                bdrMsg.Visibility = Visibility.Visible;
                bdrMsg.Background = new SolidColorBrush(Color.FromRgb(199, 90, 28));
                txtStationName.IsEnabled = false;
            }
            else

            {
                bdrMsg.Visibility = Visibility.Hidden;
                bdrMsg.Background = new SolidColorBrush(Color.FromRgb(199, 90, 28));
            }
        }
    }
}
