using Packing_Net.Classes;
using PackingClassLibrary.Commands;
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
using PackingClassLibrary.CustomEntity;
namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for PgTimeSetting.xaml
    /// </summary>
    public partial class PgTimeSetting : Page
    {
        public PgTimeSetting()
        {
            InitializeComponent();
            // fill Location Combo Box.
            FillLocationCmb();
            //fill Language Combo Box
            FillLanguageCombo();

            //Default Location and time.
            DefaultSetting();
            
            //Barcode Show in the Grid
            Global.ISBarcodeShow = Global.controller.ReadFromLocalFile("ISBarcodeShow");
            if (Global.ISBarcodeShow=="No")
            {
                RbtnNo.IsChecked = true;
            }
            

        }
        //Barcode show Buttons
        public void setBarcodeShow()
        {
            Global.ISBarcodeShow = "Yes";
            if (RbtnNo.IsChecked==true)
            {
                Global.ISBarcodeShow = "No";
            }
        }
        private void DefaultSetting()
        {
            cmbLocation.SelectedIndex = -1;
            cmbLocation.Text = "";
            lblCLocation.Visibility = System.Windows.Visibility.Visible;
            lblCLocation.Text = Global.controller.ApplicationLocation();
            lblHoures.Content = GetHrorMinorSec("HH");
            lblMin.Content = GetHrorMinorSec("MM");
            lblSec.Content = GetHrorMinorSec("SS");
        }

        /// <summary>
        /// Avinash
        /// Split the Time returned from locationsetting File in to hours , Min, Sec 
        /// </summary>
        /// <param name="returnHHorMMorSS">String maintion HH to return Hour value same for Min-MM and for Sec value =SS</param>
        /// <returns>String depending on selection</returns>
        private string GetHrorMinorSec(String returnHHorMMorSS)
        {
            String _retuen = "";
            try
            {
                String _Time = Global.controller.ReadFromLocalFile("LogoutTime");
                if (_Time != null || _Time != "")
                {
                    var Str = _Time.Split(new Char[] { ':' });
                    switch (returnHHorMMorSS)
                    {
                        case "HH":
                            _retuen = Str[0];
                            break;
                        case "MM":
                            _retuen = Str[1];
                            break;
                        case "SS":
                            _retuen = Str[2];
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.GetHrorMinorSec()", Ex.ToString());
            }
            return _retuen;
        }

        private void AddClick(Label lbl, int Duration)
        {
            try
            {
                int time = Convert.ToInt32(lbl.Content);
                if (time == Duration)
                {
                    lbl.Content = "00";
                }
                else if (Convert.ToInt32(lbl.Content) < 9)
                {
                    lbl.Content = "0" + (Convert.ToInt32(lbl.Content) + 1);
                }
                else
                {
                    lbl.Content = Convert.ToInt32(lbl.Content) + 1;
                }

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimerSetting.AddClick()", Ex.ToString());
            }
        }

        private void MinusClick(Label lbl, int Duration)
        {
            try
            {
                int time = Convert.ToInt32(lbl.Content);
                if (time == Duration)
                {
                    lbl.Content = "00";
                }
                else if (Convert.ToInt32(lbl.Content) < 11 && Convert.ToInt32(lbl.Content) != 0)
                {
                    lbl.Content = "0" + (Convert.ToInt32(lbl.Content) - 1);
                }
                else if (Convert.ToInt32(lbl.Content) !=0)
                {
                    lbl.Content = Convert.ToInt32(lbl.Content) - 1;
                }

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.MinusClick()", Ex.ToString());
            }
        }

        private void btnHrAdd_Click(object sender, RoutedEventArgs e)
        {
            AddClick(lblHoures,24);

        }
        #region Button Clicks
        private void btnMinAdd_Click(object sender, RoutedEventArgs e)
        {
            AddClick(lblMin, 60);
        }

        private void btnSecAdd_Click(object sender, RoutedEventArgs e)
        {
            AddClick(lblSec, 60);
        }

        private void btnHrSub_Click(object sender, RoutedEventArgs e)
        {
            MinusClick(lblHoures, 24);
        }

        private void btnMinSub_Click(object sender, RoutedEventArgs e)
        {
            MinusClick(lblMin, 60);
        }

        private void btnSecSub_Click(object sender, RoutedEventArgs e)
        {
            MinusClick(lblSec, 60);
        }
        #endregion
        public void FillLocationCmb()
        {
            try
            {
                List<Locations> _lsLocation = new List<Locations>();
                Locations _Loc = new Locations();
                _Loc.LocationID = 1;
                _Loc.LocationName = "NYWH";
                _lsLocation.Add(_Loc);
                Locations _Loc1 = new Locations();
                _Loc1.LocationID = 2;
                _Loc1.LocationName = "NYWT";
                _lsLocation.Add(_Loc1);
                Locations _Loc2 = new Locations();
                _Loc2.LocationID = 3;
                _Loc2.LocationName = "N/A";
                _lsLocation.Add(_Loc2);
                Locations _Loc3 = new Locations();
                _Loc3.LocationID = 4;
                _Loc3.LocationName = "WH1";
                _lsLocation.Add(_Loc3);
                cmbLocation.ItemsSource = _lsLocation;

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.FillLocationCmb()", Ex.ToString());
            }
        }
        public void FillLanguageCombo()
        {
            try
            {
                List<Languages> _lsLanguages = new List<Languages>();
                Languages _languages = new Languages();
                _languages.LanguageID = 1;
                _languages.Language = "English";
                _lsLanguages.Add(_languages);
                Languages _languages1 = new Languages();
                _languages1.LanguageID = 1;
                _languages1.Language = "Chinese";
                _lsLanguages.Add(_languages1);
                Languages _languages2 = new Languages();
                _languages2.LanguageID = 1;
                _languages2.Language = "Russian";
                _lsLanguages.Add(_languages2);
                cmbLanguage.ItemsSource = _lsLanguages;
                lblCLanguage.Text = Global.controller.ReadFromLocalFile("Language");
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.FillLanguageCombo()", Ex.ToString());
            }
        }

        private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblCLocation.Visibility = cmbLocation.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
            //Log
            SaveUserLogsTbl.logThis(csteActionType.Setting_LocationChanged.ToString(), Global.ShippingNumber);
        }

        /// <summary>
        /// Change station location when Device Location updated
        /// </summary>
        /// <param name="Location">String Location Code</param>
        /// <returns>null</returns>
        public void UpdateStationLocation(String Location)
        {
            try
            {
                String DeviceNumber = Global.controller.getDeviceMAC();
                List<cstStationMasterTbl> lsStationMaster = Global.controller.GetStationMaster(DeviceNumber);
                cstStationMasterTbl Station = lsStationMaster[0];
                Station.StaionLocation = Location;

                List<cstStationMasterTbl> lsNewLocStation = new List<cstStationMasterTbl>();
                lsNewLocStation.Add(Station);
                Global.controller.SaveStationMaster(lsNewLocStation, DeviceNumber);
            }
            catch (Exception)
            { }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //Log
                SaveUserLogsTbl.logThis(csteActionType.Setting_LocalSettingUpdated.ToString(), Global.ShippingNumber);

                String Location = "";
                String LogoutTime = lblHoures.Content + ":" + lblMin.Content + ":" + lblSec.Content;
                String Language = "";
                if (cmbLocation.Text == "" || cmbLocation.Text == null)
                {
                    Location = lblCLocation.Text;
                }
                else
                {
                    Location = cmbLocation.Text;
                }
                if (cmbLanguage.Text == "" || cmbLanguage.Text == null)
                {
                    Language = lblCLanguage.Text;
                }
                else
                {
                    Language = cmbLanguage.Text;
                }
               
                SetGlobalFileName(Language);

                //Reset the Timer Intervals .
                int _Hour = Convert.ToInt32(lblHoures.Content.ToString());
                int _Min = Convert.ToInt32(lblMin.Content.ToString());
                int _Sec = Convert.ToInt32(lblSec.Content.ToString());
                
                    MsgBox.Show("Error", "Update", Environment.NewLine + "To apply the settings application need to restart." + Environment.NewLine + " Are you sure want to restart application?");
                    if (Global.MsgBoxResult =="Ok")
                    {
                        //Change station location from device location changed
                        UpdateStationLocation(Location);
                        //seve to the text file overrites the existing contents.
                        Boolean _return = Global.controller.WriteStringTofile(Location, LogoutTime, Language,Global.ISBarcodeShow);
                        SessionManager.Autotimer.Interval = new TimeSpan(_Hour, _Min, _Sec);
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }    
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.UpdateBtn()", Ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DefaultSetting();
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Log
            SaveUserLogsTbl.logThis(csteActionType.Setting_LanguageChanged.ToString(), Global.ShippingNumber);

              lblCLanguage.Visibility = cmbLanguage.SelectedItem == null ? Visibility.Visible : System.Windows.Visibility.Hidden;
              if (cmbLanguage.Text =="" || cmbLanguage.Text ==null )
              {
                  SetGlobalFileName(lblCLanguage.Text);
              }
              else
              {
                  SetGlobalFileName(cmbLanguage.Text);
              }
        }
        private void SetGlobalFileName(String LanguageString)
        {
            try
            {
                switch (LanguageString)
                {
                    case "English":
                        Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\English_US.sys";
                        break;
                    case "Chinese":
                        Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\Chinese.sys";
                        break;
                    case "Russian":
                        Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\Russian.sys";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgTimeSetting.GetGlobalFile()", Ex.ToString());
            }
            
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Change language
            WindowLanguages.Convert(this);
        }

        private void RbtnYes_Click(object sender, RoutedEventArgs e)
        {
            setBarcodeShow();
        }

        private void RbtnNo_Click(object sender, RoutedEventArgs e)
        {
            setBarcodeShow();
        }
    }
    class Locations
    {
        public int LocationID { get; set; }
        public String LocationName { get; set; }
    }

    class Languages
    {
        public int LanguageID { get; set; }
        public String Language { get; set; }
    }
}
