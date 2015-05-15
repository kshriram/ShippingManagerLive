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
using PackingClassLibrary.CustomEntity;
using System.Windows.Threading;
using PackingClassLibrary.Models;
namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for Umsgbox.xaml
    /// </summary>
    public partial class Umsgbox : Window
    {
        //Message Box Autoclick Timer time.
        int OkPresstime = 5;
        //Dispacher for the MessageBox ok Click Button
        DispatcherTimer TimerOk = new DispatcherTimer();
        public Umsgbox()
        {
            InitializeComponent();

            TimerOk.Interval = new TimeSpan(0, 0, 1);
            TimerOk.Start();
            TimerOk.Tick += TimerOk_Tick;

        }

        /// <summary>
        /// Ok Button Click Time logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerOk_Tick(object sender, EventArgs e)
        {
            //Split string Of msg box.
            string c = btnOK.Content.ToString();
            String[] cnt = c.Split(new char[] { ' ' });

            //Add timer on the Button of the message box.
            btnOK.Content = cnt[0] + " (" + (OkPresstime - 1).ToString() + ")";
            OkPresstime = OkPresstime - 1;
            if (OkPresstime == 0)
            {
                TimerOk.Stop();
                Global.MsgBoxResult = "Ok";
                imgSKu.Visibility = System.Windows.Visibility.Hidden;
                this.Close();
            }
        }
        
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Auto logout timer start
            SessionManager.StartTime();

            //message Box Types.
            try
            {
                btnOK.Focus();
                Global.MsgBoxResult = "";
                lblmsg.Text = Global.MsgBoxMessage;
                LblTitle.Content = Global.MsgBoxTitle;
                if (Global.MsgBoxType == "Warning")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgWarning.png", UriKind.Relative);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                }
                if (Global.MsgBoxType == "WarningImage")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgWarning.png", UriKind.Relative);
                    String Path = Global.controller.GetSKuUrl(Global.SKUName);
                    ImageFromUrl.GetImageFrom(imgSKu, Path);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                    imgSKu.Visibility = Visibility.Visible;
                }
                if (Global.MsgBoxType == "Error")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgWarning.png", UriKind.Relative);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                    btnOK.Content = "Yes";
                    btnCancel.Content = "No";
                }
                if (Global.MsgBoxType == "Ok")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgCorrectGreen.png", UriKind.Relative);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                }
                if (Global.MsgBoxType == "Shipment")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgWarning.png", UriKind.Relative);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                    btnCancel.Content = "Continue any Way";
                    btnCancel.Background = new SolidColorBrush(Color.FromRgb(197, 69, 0));
                    btnOK.Background = new SolidColorBrush(Colors.Green);

                }

                if (Global.MsgBoxType == "Continue")
                {
                    Uri ImagesUri = new Uri("/PackingNet;component/Images/imgWarning.png", UriKind.Relative);
                    ImgMsgImage.Source = new BitmapImage(ImagesUri);
                    btnOK.Visibility = System.Windows.Visibility.Hidden;
                    txtUserName.Focus();
                }
            }
            catch (Exception)
            { }

            //Convert message Box to selected language.
            WindowLanguages.Convert(this);
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Global.MsgBoxResult = "Ok";
            imgSKu.Visibility = System.Windows.Visibility.Hidden;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Global.MsgBoxResult = "Cancel";
            imgSKu.Visibility = System.Windows.Visibility.Hidden;
            this.Close();
        }
        
        public void SaveShippingInformation(String ShippingNumber)
        {
            try
            {
                ////Get Shipping Information from sage.
               // List<cstShippingTbl> lsSageInfo = Global.controller.GetShippingInfoFromSage(ShippingNumber);

                ////Save to local Database.
                //Global.controller.SetShippingTbl(lsSageInfo);

                //picked from wayfair

                List<cstShippingTbl> lsSageInfo = new List<cstShippingTbl>();
                //Get Shipping Information from sage.

                if (Global.LTLLogin == "LTL")
                {
                    lsSageInfo = Global.controller.GetShippingInfoFromSage(ShippingNumber, Global.LTLLogin);
                }
                else if (Global.LTLLogin == "UPSFEDEX")
                {
                    lsSageInfo = Global.controller.GetShippingInfoFromSage(ShippingNumber, Global.LTLLogin);
                }

                //Save to local Database.
                Global.controller.SetShippingTbl(lsSageInfo);


            }
            catch (Exception)
            { }
        }

        private void ShipmentLock(int OverrideMode)
        {
            try
            {
                //save Shipping Infromtaion.
                SaveShippingInformation(Global.ShippingNumber);

                List<cstPackageTbl> lsPacking = new List<cstPackageTbl>();
                cstPackageTbl _packingCustom = new cstPackageTbl();
                _packingCustom.ShippingNum = Global.ShippingNumber;
                _packingCustom.UserID = Global.ManagerID;
                _packingCustom.StartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                _packingCustom.EndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                _packingCustom.ShippingID = Global.controller.GetShippingTbl(Global.ShippingNumber).ShippingID;
                _packingCustom.StationID = Global.controller.GetStationMaster().SingleOrDefault(i => i.StationName == Global.StationName).StationID;
                _packingCustom.ShipmentLocation = Global.controller.ApplicationLocation();
                //Status: 1 - Under Packing Process.
                //Status: 0 - Packing Complete
                _packingCustom.PackingStatus = 1;
                _packingCustom.MangerOverride = OverrideMode;

                if (Global.LoginType == "LTL")
                {
                    _packingCustom.LTL_flg = 1;
                    _packingCustom.UPS_FEDEX_flg = 0;
                }
                else
                {
                    _packingCustom.LTL_flg = 0;
                    _packingCustom.UPS_FEDEX_flg = 1;
                }

                if (Global.CartonPallet == "Pallet")
                {
                    _packingCustom.pallet_flg = 1;
                }
                else
                {
                    _packingCustom.pallet_flg = 0;
                }




                lsPacking.Add(_packingCustom);
                Global.PackingID = Global.controller.SetPackingTable(lsPacking);

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("MainWindow.ShipmentLock()", Ex.ToString());
            }
        }
      
        /// <summary>
        /// manager Override Option Result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            //Logout expire timer RE-start
            SessionManager.StartTime();
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtUserName.Text != "")
                    {
                       
                        List<cstUserMasterTbl> _lsUser = Global.controller.GetSelcetedUserMaster(txtUserName.Text);

                        if (Global.controller.CanOverrideShipment(_lsUser[0].UserID))
                        {
                            Global.ManagerID = _lsUser[0].UserID;
                            Global.ManagerName = _lsUser[0].UserFullName;
                            Global.Mode = "Override";

                            SessionManager.Autotimer.Stop();
                            //wait screen;
                            WindowThread.start();
                            ShipmentLock(1);
                            ShipmentScreen _ShipmentScreen = new ShipmentScreen();
                            _ShipmentScreen.Show();

                            App.Current.Windows[0].Close();
                            this.Close();
                        }
                        else
                        {
                            Global.ManagerName = "";
                            LblTitle.Content = Global.controller.ConvetLanguage("Access Denied.", Global.LanguageFileName);
                            lblmsg.Text = Environment.NewLine + Environment.NewLine + Global.controller.ConvetLanguage("         You are not authorized to override. ", Global.LanguageFileName);
                            txtUserName.Text = "";
                            btnCancel.Visibility = System.Windows.Visibility.Hidden;
                            btnOK.Visibility = System.Windows.Visibility.Visible;
                            btnOK.Focus();
                        }
                    }
                    else
                    {
                        lblmsg.Text = Environment.NewLine + Environment.NewLine + Global.controller.ConvetLanguage("               Please rescan badge. ", Global.LanguageFileName);
                        txtUserName.Text = "";
                    }
                }
            }
            catch (Exception)
            { }
        }

    }
}
