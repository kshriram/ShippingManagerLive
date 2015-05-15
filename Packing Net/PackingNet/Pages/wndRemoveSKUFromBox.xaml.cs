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
using PackingClassLibrary.Models;
using PackingClassLibrary.BusinessLogic;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.Commands;
using PackingNet.Classes;
using Packing_Net.Classes;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Windows.Threading;
using Packing_Net.Pages;

using System.Windows.Media.Animation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;



namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndRemoveSKUFromBox.xaml
    /// </summary>

    public partial class wndRemoveSKUFromBox : Window
    {    //String application location 
        string bxnmr = "";
        cmdPakingDetails cd = new cmdPakingDetails();
        cmdBox box = new cmdBox();

        String ApplicationLocation = Global.controller.ApplicationLocation();

        public event EventHandler<myEventArgs1> RaiseCustomEvent1;

        public Boolean closeflg = false;


        smController Control = new smController();
        model_Shipment _shipment = Global.controller.getModelShipment(Global.ShippingNumber);
        List<cstPackageDetails> PackageDetail = new List<cstPackageDetails>();
        List<cstBoxPackage> bxpckgDetail = new List<cstBoxPackage>();
        List<cstPackageTbl> packid = new List<cstPackageTbl>();
        DispatcherTimer dtLoadUpdate;
        DispatcherTimer dtLoadUpdate1;
        DispatcherTimer dtLoadUpdate2;
        List<string> updatedboxnumber = new List<string>();
        List<string> boxnumber = new List<string>();
        int flg = 0;
        string chk = "NotValid";
        public wndRemoveSKUFromBox()
        {
            InitializeComponent();
        }
        public class myEventArgs1 : EventArgs
        {
            public myEventArgs1(string s)
            {
                msg = s;
            }
            private string msg;
            public string massage
            {
                get
                {
                    return msg;
                }
            }
        }
        //public void blink1()
        //{
        //    DoubleAnimation widthAnimation = new DoubleAnimation(120, 300, TimeSpan.FromSeconds(5));

        //    widthAnimation.RepeatBehavior = RepeatBehavior.Forever;
        //    widthAnimation.AutoReverse = true;
        //    labelblink.BeginAnimation(Button.WidthProperty, widthAnimation);
        //}

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                lblShipmentNo.Content = "Shipment Number " + Global.ShippingNumber;

                this.Closing += new System.ComponentModel.CancelEventHandler(MyWindow_Closing);
                try
                {
                    txtScannSKu.Focus();

                    PackageDetail = new List<cstPackageDetails>();
                    //// blink1();
                    fillGridSelectedBox();
                    //showstatus();
                    lblBoxNumber.Content = Global.BoxActive;
                    // ShowMassagePopup("Please Scan  Item To Remove from " + Global.BoxActive + "", 4000);
                }
                catch (Exception d)
                {
                    ShowMassagePopup("" + d.Message, 2000);
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - Window_Loaded_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }



        }


        void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (closeflg == false)
                {

                    if (btnCancelShipment.Visibility == Visibility.Visible)
                    {
                        e.Cancel = false;
                        //ShowMassagePopup("Please you have to Pack this Box \n" + Global.UnPacked + " First", 4000);
                        //ShowMassagePopup("If you choose to exit, \n your open box  " + Global.UnPacked + " is deleted.", 4000);
                        txtScannSKu.Focus();
                    }
                    else
                    {
                        ShowMassagePopup("click on submit", 4000);
                        e.Cancel = true;

                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
            catch (Exception Ex)
            {
                ShowMassagePopup("Invalid Operation Closing", 4000);
                ErrorLoger.save("wndRemoveSKUFromBox - MyWindow_Closing", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        public void fillGridSelectedBox()
        {
            try
            {
                ////bxnmr = Global.selectedBoxID;
                bxnmr = Global.BoxActive;
                //tbAssembly.Text = "";
                //var filter2 = from n in PackageDetail
                //              where n.BoxNumber == bxnmr
                //              select n;


                PackageDetail = cd.GetPackingDetailsByBoxNum(bxnmr);

                grdContentRemove.ItemsSource = cd.GetPackingDetailsByBoxNum(bxnmr);


                dtLoadUpdate = new DispatcherTimer();
                dtLoadUpdate.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dtLoadUpdate.Tick += dtLoadUpdate_Tick;
                //start the dispacher.
                dtLoadUpdate.Start();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - fillGridSelectedBox()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }


        private void _showBarcode()
        {
            try
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                foreach (DataGridRow Row in GetDataGridRows(grdContentRemove))
                {

                    DataGridRow row = (DataGridRow)Row;
                    TextBlock SKUNo = grdContentRemove.Columns[0].GetCellContent(row) as TextBlock;

                    if (Global.controller.IsSKUShowBarcode(SKUNo.Text))
                    {
                        String SkuName = SKUNo.Text.ToString();

                        //Convert SKU name to UPC COde;
                        String UPC_Code = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == SkuName).UPCCode;

                        if (UPC_Code.StartsWith("NA"))
                        {
                            UPC_Code = UPC_Code.Remove(0, 2);
                            UPC_Code = UPC_Code + "00";
                        }


                        if (UPC_Code.Trim() == "") UPC_Code = "00000000000";

                        //clGlobal.call.SKUnameToUPCCode(SKUNo.Text.ToString());
                        ContentPresenter sp = grdContentRemove.Columns[4].GetCellContent(row) as ContentPresenter;
                        DataTemplate myDataTemplate = sp.ContentTemplate;
                        Image ImgbarcodSet = (Image)myDataTemplate.FindName("imgBarCode", sp);
                        System.Drawing.Image Barcodeimg = null;
                        try
                        {
                            Barcodeimg = b.Encode(BarcodeLib.TYPE.UPCA, UPC_Code, System.Drawing.Color.Black, System.Drawing.Color.White, 300, 60);
                        }
                        catch (Exception Ex)
                        {
                            //Log the Error to the Error Log table
                            ErrorLoger.save("wndShipmentDetailPage - _showBarcode_Sub1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                        }
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();

                        // Save to a memory stream...
                        Barcodeimg.Save(ms, ImageFormat.Bmp);

                        // Rewind the stream...
                        ms.Seek(0, SeekOrigin.Begin);

                        // Tell the WPF image to use this stream...
                        bi.StreamSource = ms;
                        bi.EndInit();
                        ImgbarcodSet.Source = bi;

                        try
                        {
                            //txtScannSKu.Focus();
                        }
                        catch (Exception Ex)
                        {
                            //Log the Error to the Error Log table
                            ////  ErrorLoger.save("wndShipmentDetailPage - _showBarcode_Sub2", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                            ErrorLoger.save("wndRemoveSKUFromBox - _showBarcode()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                // ErrorLoger.save("wndShipmentDetailPage - _showBarcode", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndRemoveSKUFromBox - _showBarcode()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                ShowMassagePopup("Invalid Barcode", 2000);

            }
        }

        /// <summary>
        /// This is to all return DataGridRows Object
        /// </summary>
        /// <param name="grid"> Grid View object</param>
        /// <returns>DataGridRow</returns>
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        public void showstatus()
        {
            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContentRemove))
                {
                    //    row.Background = new SolidColorBrush(Color.FromRgb(192, 192, 192));

                    ContentPresenter sp1 = grdContentRemove.Columns[3].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                    TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);


                    /// myTextBlock.Visibility = Visibility.Hidden;
                    myTextBlock.Text = "ToProcess";
                    myTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - showstatus()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }



        private void dtLoadUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                _showBarcode();

                dtLoadUpdate.Stop();

                txtScannSKu.Focus();

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - dtLoadUpdate_Tick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }



        private void grdContentRemove_GotFocus_1(object sender, RoutedEventArgs e)
        {
            try
            {
                txtScannSKu.Focus();

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - grdContentRemove_GotFocus_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void btnExitShipment_Click_1(object sender, RoutedEventArgs e)
        {
            //ShipmentScreen _Shipment = new ShipmentScreen();
            //_Shipment.Show();
            //this.Close();
            //_saveClick();

            try
            {


                SplashScreen splashScreen = new SplashScreen("1235.png");
                splashScreen.Show(true);


                Guid dd = Guid.Empty;
                ///print 
                /// //Print  Box Id slip 
                /// 
                closeflg = true;
                //List<string> gd = new List<string>();

                if (Global.FlagAllshippckd == "flagTrue")
                {
                    Global.FlagAllshippckd = "ReturnTrue";
                }


                Global.PrintBox = box.GetBoxIDByBoxNumber(Global.BoxActive);

                Global.PrintBoxID = Global.PrintBox;
                //  wndBoxSlip _boxSlip = new wndBoxSlip();
                // _boxSlip.ShowDialog();
                // this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));

                //      simpledelegate sm = new simpledelegate(_saveClick);
                //sm();
                //      this.Dispatcher.Invoke(new Action(() => { sm(); }));
                // _saveClick();

                //IsActiveFlag Change here
                RaiseCustomEvent1(this, new myEventArgs1(Global.BoxActive));
                splashScreen.Show(false);
                this.Close();

                Global.IsActiveFlag = true;



                //Not To Add New Box
                Global.flgBx = "NotAddBx";


            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - btnExitShipment_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }

        private void _saveClick()
        {
            // BErrorMsg.Visibility = System.Windows.Visibility.Hidden;
            try
            {
                Global.PackingID = Guid.Empty;
                // Global.ShippingNumber = txtShipmentId.Text.ToUpper();

                //Get model of shipment;
                model_Shipment modelshipment = Global.controller.getModelShipment(Global.ShippingNumber);
                String ApplicationLocation = Global.controller.ApplicationLocation();
                Boolean ISSameLocationPacked = modelshipment.ShipmentFucntions.IsShipmentPackedAtSameLocation(modelshipment.PackedLocations, ApplicationLocation, modelshipment.IsAlreadyPacked);
                if (modelshipment.IsShipmentValidated)
                {
                    if (modelshipment.IsAlreadyPacked && ISSameLocationPacked)
                    {
                        List<cstPackageTbl> _lsPck = modelshipment.PackingInfo;
                        int PackingStatus = modelshipment.PackingInfo.FirstOrDefault(i => i.ShipmentLocation == ApplicationLocation).PackingStatus;
                        Guid UserId = modelshipment.PackingInfo.FirstOrDefault(i => i.ShipmentLocation == ApplicationLocation).UserID;
                        String UserName = modelshipment.UserInfo_ShipmentPackedBy.FirstOrDefault(i => i.UserID == UserId).UserFullName;
                        String StationName = Global.controller.GetStationMaster(modelshipment.PackingInfo[0].StationID).StationName;
                        DateTime PakedDate = modelshipment.PackingInfo[0].EndTime;

                        #region single location packed
                        if (modelshipment.IsMultiLocation == false)
                        {
                            // if (PackingStatus == 1)
                            // {
                            //log.
                            SaveUserLogsTbl.logThis(csteActionType.ShipmentID_UnderPacking_Scan_00.ToString(), _lsPck[0].PackingId.ToString());

                            //if (UserId == Global.LoggedUserId)
                            if (true)
                            {
                                //if (modelshipment.CanOverride)
                                //{
                                //    //set mod to the SameUser packing the Shipment;
                                Global.Mode = "SameUser";
                                Global.SameUserpackingID = modelshipment.PackingInfo[0].PackingId;
                                this.Dispatcher.Invoke(new Action(() => { _show_Shipment(); }));
                                //    }
                                //    else
                                //    {
                                //        //Show Error message on the Strip;
                                //        _scrollMsg("Warning:\"" + txtShipmentId.Text + "\" is already overridden by you. Can not override shipment more than one.", Color.FromRgb(222, 87, 24));
                                //    }
                                //}
                                //else
                                //{
                                //    _scrollMsg("Warning: " + "'" + txtShipmentId.Text.ToUpper() + "' Shipment is under packing. user " + UserName + " is packing The Shipment at station  " + StationName, Color.FromRgb(222, 87, 24));
                                //    MsgBox.Show("Shipment", "Warning", "'" + txtShipmentId.Text.ToUpper() + "'" + Environment.NewLine + " Shipment is under packing." + Environment.NewLine + "user " + UserName + " is packing The Shipment at station  " + StationName);

                                //}
                                //_actMessageResult();
                                // txtShipmentId.Text = "";
                            }
                            else if (PackingStatus == 0)
                            {
                                //log.
                                SaveUserLogsTbl.logThis(csteActionType.ShipmentID_AlreadyPacked_Scan_00.ToString(), _lsPck[0].PackingId.ToString());
                                ////Distroy Shipment Object;
                                //modelshipment = new model_Shipment();
                                //_scrollMsg("Warning: " + "'" + txtShipmentId.Text + "'" + " Shipment is already packed  by  " + UserName + " on " + PakedDate.ToString("dddd,MMM dd, yyyy") + " at " + PakedDate.ToString("hh:mm:ss tt"), Color.FromRgb(222, 87, 24));
                                //MsgBox.Show("Shipment", "Warning", "'" + txtShipmentId.Text + "'" + " Shipment is already packed " + Environment.NewLine + " by  " + UserName + Environment.NewLine + " on " + PakedDate.ToString("dddd,MMM dd, yyyy") + Environment.NewLine + " at " + PakedDate.ToString("hh:mm:ss tt"));
                                //_actMessageResult();
                                //txtShipmentId.Text = "";
                            }
                        }
                        #endregion
                        else if (modelshipment.IsMultiLocation)
                        {
                            //log.
                            SaveUserLogsTbl.logThis(csteActionType.MultiLocation_ShipmentIDScan.ToString());
                            Boolean LocationFound = false;
                            foreach (cstPackageTbl _packed in modelshipment.PackingInfo)
                            {
                                if (_packed.ShipmentLocation == ApplicationLocation)
                                {
                                    LocationFound = true;
                                    // if (_packed.PackingStatus == 1)
                                    //  {
                                    //log.
                                    SaveUserLogsTbl.logThis(csteActionType.ShipmentID_UnderPacking_Scan_00.ToString(), _lsPck[0].PackingId.ToString());

                                    //same User Override 
                                    // if (UserId == Global.LoggedUserId)
                                    // {
                                    //set mod to the SameUser packing the Shipment;
                                    Global.Mode = "SameUser";
                                    Global.SameUserpackingID = _packed.PackingId;
                                    _show_Shipment();
                                    //  }
                                    //  else
                                    //  {
                                    //      _scrollMsg("Warning: " + "'" + txtShipmentId.Text.ToUpper() + "' Shipment is under packing. user " + UserName + " is packing The Shipment at station  " + StationName, Color.FromRgb(222, 87, 24));
                                    //      MsgBox.Show("Shipment", "Warning", "'" + txtShipmentId.Text.ToUpper() + "'" + Environment.NewLine + " Shipment is under packing." + Environment.NewLine + "user " + UserName + " is packing The Shipment at station  " + StationName);
                                    //  }
                                    //  _actMessageResult();
                                    // txtShipmentId.Text = "";
                                    // break;
                                    // }
                                    if (_packed.PackingStatus == 0)
                                    {
                                        //log.
                                        SaveUserLogsTbl.logThis(csteActionType.ShipmentID_AlreadyPacked_Scan_00.ToString(), _lsPck[0].PackingId.ToString());
                                        //Distroy Shipment Object;
                                        //modelshipment = new model_Shipment();
                                        //MsgBox.Show("Shipment", "Warning", "'" + txtShipmentId.Text + "'" + " Shipment is already packed " + Environment.NewLine + " by  " + UserName + Environment.NewLine + " on " + PakedDate.ToString("dddd,MMM dd, yyyy") + Environment.NewLine + " at " + PakedDate.ToString("hh:mm:ss tt"));
                                        //_scrollMsg("Warning: " + "'" + txtShipmentId.Text + "'" + " Shipment is already packed  by  " + UserName + " on " + PakedDate.ToString("dddd,MMM dd, yyyy") + " at " + PakedDate.ToString("hh:mm:ss tt"), Color.FromRgb(222, 87, 24));
                                        //_actMessageResult();
                                        //txtShipmentId.Text = "";
                                        break;
                                    }
                                }
                            }
                            if (LocationFound == false)
                            {
                                //Open Shipment Screen Directly.
                                _show_Shipment();
                            }
                        }
                    }
                    else if (modelshipment.ShipmentDetailSage == null || modelshipment.ShipmentDetailSage.Count <= 0)
                    {
                        //log.
                        SaveUserLogsTbl.logThis(csteActionType.Invalid_ShipmentScan__00.ToString(), Global.ShippingNumber);
                        //show error
                        // _catchErrorInCondition();
                    }
                    else
                    {
                        Boolean Shown = false;
                        foreach (var sage in modelshipment.ShipmentDetailSage)
                        {
                            if (sage.Location == ApplicationLocation)
                            {
                                Shown = true;
                                _show_Shipment();
                                break;
                            }
                        }
                        if (Shown == false)
                        {
                            //Distroy Shipment Object;
                            //modelshipment = new model_Shipment();

                            ////Show Error message on the Strip;
                            //_scrollMsg("Warning: Please check the shipment ID! - \"" + txtShipmentId.Text + "\" is not belongs to this location.", Color.FromRgb(222, 87, 24));

                            ////clear the shipment textbox
                            //txtShipmentId.Text = "";
                        }
                    }
                }
                else if (!modelshipment.IsShipmentValidated && modelshipment.ShipmentDetailSage.Count > 0)
                {
                    //Show error message.
                    //_scrollMsg("Warning: Shipment Number: " + txtShipmentId.Text + " is already Shipped.", Color.FromRgb(222, 87, 24));
                    ////clear the shipment textbox
                    //txtShipmentId.Text = "";
                    //Distroy Shipment Object;
                    model_Shipment _shipment = new model_Shipment();
                }
                else
                {
                    //Show error message.
                    //_scrollMsg("Warning: Shipment Number: " + txtShipmentId.Text + " is not valid. Please check Shipment Number.", Color.FromRgb(222, 87, 24));
                    ////clear the shipment textbox
                    //txtShipmentId.Text = "";
                    //Distroy Shipment Object;
                    model_Shipment _shipment = new model_Shipment();
                }
            }
            catch (Exception Ex)
            {
                //Show error
                //  _catchErrorInCondition();
                //Log the Error to the Error Log table
                ////ErrorLoger.save("wndShipmentScanPage - _saveClick", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndRemoveSKUFromBox - _saveClick()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            //this.Close();
        }

        private void _show_Shipment()
        {
            try
            {
                Boolean _return = false;
                //lock the shipment that is under packing process.
                if (Global.Mode == "SameUser")
                {
                    _return = _shipmentLock(2);
                }
                else if (Global.Mode == "Override")
                {
                    _return = _shipmentLock(1);
                }
                else
                {
                    _return = _shipmentLock(0);
                }
                if (_return)
                {
                    //Start please wait screen in saprate thread.
                    //WindowThread.start();

                    //Set the Global Shiment Number
                    // Global.ShippingNumber = txtShipmentId.Text.ToUpper();

                    ShipmentScreen shipmentScreen = new ShipmentScreen();

                    //loger add log.
                    SaveUserLogsTbl.logThis(csteActionType.ShipmentID_Scan.ToString(), Global.ShippingNumber.ToString());
                    //     _scrollMsg("Valid Shipment Scanned. Shipment ID =" + Global.ShippingNumber, Color.FromRgb(38, 148, 189));

                    shipmentScreen.Show();

                    //close thi screen.
                    this.Close();
                }
                else
                {
                    //_scrollMsg("Warning: shipping information not available. Please scan another shipment.", Color.FromRgb(222, 87, 24));
                    //txtShipmentId.Text = "";
                }

            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                //ErrorLoger.save("wndShipmentScanPage - _show_Shipment", " [" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndRemoveSKUFromBox - _show_Shipment", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }


        private Boolean _shipmentLock(int OverrideMode)
        {
            Boolean _return = false;
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    //save Shipping Infromtaion.
                    _return = _saveShippingInformation(Global.ShippingNumber);

                    List<cstPackageTbl> lsPacking = new List<cstPackageTbl>();
                    cstPackageTbl _packingCustom = new cstPackageTbl();
                    _packingCustom.ShippingNum = Global.ShippingNumber;
                    _packingCustom.UserID = Global.LoggedUserModel.UserInfo.UserID;
                    _packingCustom.StartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    _packingCustom.EndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    _packingCustom.StationID = Global.controller.GetStationMasterByName(Global.StationName).StationID;
                    _packingCustom.ShippingID = Global.controller.GetShippingTbl(Global.ShippingNumber).ShippingID;
                    _packingCustom.ShipmentLocation = Global.controller.ApplicationLocation();

                    //Status: 1 - Under Packing Process.
                    //Status: 0 - Packing Complete
                    _packingCustom.PackingStatus = 1;

                    //Status: 0 - Not Manger override packing
                    //Status: 1 - Manger Override packing
                    //Status: 2 - Same User Repacking
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
                    if (OverrideMode == 2)
                    {
                        //No save No update just pass the old Packing Id to next operations.
                        Global.PackingID = Global.SameUserpackingID;
                    }
                    else
                    {
                        Global.PackingID = Global.controller.SetPackingTable(lsPacking);
                    }

                }
                catch (Exception Ex)
                {
                    //Log the Error to the Error Log table
                    //ErrorLoger.save("wndShipmentScanPage - _shipmentLock", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                    ErrorLoger.save("wndRemoveSKUFromBox - _shipmentLock", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                }
            }));
            return _return;
        }

        private Boolean _saveShippingInformation(String ShippingNumber)
        {
            Boolean _return = false;
            try
            {
                //Get Shipping Information from sage.
                List<cstShippingTbl> lsSageInfo = Global.controller.GetShippingInfoFromSage(ShippingNumber);
                if (lsSageInfo.Count > 0)
                {
                    //Save to local Database.
                    Global.controller.SetShippingTbl(lsSageInfo);
                    _return = true;
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ////ErrorLoger.save("wndShipmentScanPage - _saveShippingInformation", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndRemoveSKUFromBox - _saveShippingInformation", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            return _return;
        }

        private void btnCancelShipment_Click_1(object sender, RoutedEventArgs e)
        {
            //For Not Adding New Box
            ////Global.flgBx = "NotAddBx";
            ////ShipmentScreen _Shipment = new ShipmentScreen();
            ////_Shipment.Show();
            ////this.Close();
            try
            {
                //_saveClick();
                this.Close();
                //Not To Add New Box
                Global.flgBx = "NotAddBx";
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - btnCancelShipment_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        public void checkforsku(String Boxnum)
        {
            try
            {
                List<cstPackageDetails> PackageDetailTemp = new List<cstPackageDetails>();
                PackageDetail = new List<cstPackageDetails>();
                List<string> boxnumber = new List<string>();
                string Tenpboxnumber = "";

                foreach (var item in Control.GetPackingNum(Global.ShippingNumber))
                {
                    /// List<cstPackageDetails> PackageDetailTemp = new List<cstPackageDetails>();
                    PackageDetailTemp = Control.GetPackingDetailTbl(item);

                    foreach (var item1 in PackageDetailTemp)
                    {
                        if (item1.ShipmentLocation == ApplicationLocation)
                        {
                            cstPackageDetails tempdetail = new cstPackageDetails();
                            tempdetail.BoxNumber = item1.BoxNumber;

                            // tempdetail.
                            tempdetail.ItemName = item1.ItemName;
                            tempdetail.MAP_Price = item1.MAP_Price;
                            tempdetail.PackingDetailID = item1.PackingDetailID;
                            tempdetail.PackingDetailStartDateTime = item1.PackingDetailStartDateTime;
                            tempdetail.ProductName = item1.ProductName;
                            tempdetail.ShipmentLocation = item1.ShipmentLocation;
                            tempdetail.SKUNumber = item1.SKUNumber;
                            tempdetail.SKUQuantity = item1.SKUQuantity;
                            tempdetail.TarrifCode = item1.TarrifCode;
                            tempdetail.TCLCOD_0 = item1.TCLCOD_0;
                            tempdetail.UnitOfMeasure = item1.UnitOfMeasure;

                            Tenpboxnumber = item1.BoxNumber;

                            //if (boxnumber.Count == 0)
                            //{
                            //    boxnumber.Add(Tenpboxnumber);
                            //}

                            //for (int i = 0; i < boxnumber.Count; i++)
                            //{
                            //    if (boxnumber[i] != Tenpboxnumber)
                            //    {
                            //        boxnumber.Add(Tenpboxnumber);
                            //    }
                            //}

                            //  boxnumber.Add(Tenpboxnumber);
                            PackageDetail.Add(tempdetail);
                        }
                    }
                    PackageDetailTemp = cd.GetPackingDetailsBoxNumber(item);
                    cstPackageDetails tempdetails = new cstPackageDetails();
                    foreach (var item11 in PackageDetailTemp)
                    {

                        ////  cstPackageDetails tempdetail = new cstPackageDetails();
                        tempdetails.BoxNumber = item11.BoxNumber;
                        Tenpboxnumber = item11.BoxNumber;
                        boxnumber.Add(Tenpboxnumber);
                    }


                }
                foreach (var a in boxnumber)
                {
                    if (a == Boxnum)
                    {
                        chk = "valid";
                        break;
                    }
                    else
                    {
                        chk = "NotValid";
                    }

                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - checkforsku", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }




            //tbAssembly.ItemsSource = boxnumber;
        }




        private void ApplyEffect(Window win)
        {
            try
            {
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                objBlur.Radius = 4;
                win.Effect = objBlur;
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - ApplyEffect", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

            }

        }
        private void ClearEffect(Window win)
        {
            try
            {
                win.Effect = null;
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - ClearEffect()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                MyPopup.IsOpen = false;
                ClearEffect(this);
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - Button_Click_2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

            }
        }

        private void yes_Click_1(object sender, RoutedEventArgs e)
        {
            txtScannSKu.Focus();
        }




        public Boolean checkQty()
        {
            Boolean fk = false;

            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContentRemove))
                {
                    TextBlock txtQty = grdContentRemove.Columns[2].GetCellContent(row) as TextBlock;

                    if (Convert.ToInt16(txtQty.Text) > 0)
                    {
                        fk = true;
                        break;
                    }


                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - checkQty()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }


            return fk;
        }

        private void txtScannSKu_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
                {
                    if (txtScannSKu.Text.Contains("#"))
                    {
                        if (txtScannSKu.Text == "#submit" || txtScannSKu.Text == "#SUBMIT")
                        {
                            txtScannSKu.Text = "";

                            if (btnExitShipment.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnExitShipment);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                /// ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not Exit.";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                mm(ss, 3000);

                            }
                        }
                        if (txtScannSKu.Text == "#exit" || txtScannSKu.Text == "#EXIT")
                        {
                            txtScannSKu.Text = "";

                            if (btnCancelShipment.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnCancelShipment);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                /// ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "Can not allowed to cancel";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                mm(ss, 3000);

                            }
                        }
                    }
                    else
                    {

                        bool Invalid = true;
                        Boolean rmveflf = false;
                        String _tempUPCStore = txtScannSKu.Text;
                        if (txtScannSKu.Text != "" || txtScannSKu.Text == null)
                        {
                            string Str = txtScannSKu.Text.TrimStart('0').ToString();
                            //txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.UPCCode == Str).SKU.ToString();
                            ////03-01-2015  for the skus Not Having Barcode Starts with NA
                            try
                            {
                                txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.UPCCode == Str).SKU.ToString();
                            }
                            catch (Exception)
                            {
                                Str = "NA" + Str;
                                //Str=Str.TrimEnd('0').ToString();
                                //Str = Str.TrimEnd('0').ToString();
                                Str = Str.Remove(12);

                                txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.UPCCode == Str).SKU.ToString();
                            }



                            foreach (DataGridRow row in GetDataGridRows(grdContentRemove))
                            {
                                TextBlock txtSKUName = grdContentRemove.Columns[0].GetCellContent(row) as TextBlock;
                                TextBlock txtQuantity = grdContentRemove.Columns[2].GetCellContent(row) as TextBlock;

                                ContentPresenter _contentPar = grdContentRemove.Columns[3].GetCellContent(row) as ContentPresenter;
                                DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                                TextBox Location = (TextBox)_dataTemplate.FindName("txtLocation", _contentPar);

                                if (txtScannSKu.Text == txtSKUName.Text)
                                {
                                    if (Convert.ToInt16(txtQuantity.Text) == 1) //for delete
                                    {
                                        MsgBox.Show("Confirm", "Remove", "Remove SKU from the current Box" + Environment.NewLine + "Are you sure want to Remove SKU?");
                                        if (Global.MsgBoxResult == "Ok")
                                        {
                                            Invalid = false;

                                            //var packindatialID = (from package in PackageDetail
                                            //                      where package.SKUNumber == txtSKUName.Text && package.BoxNumber == cmbBoxNUmber.SelectedItem.ToString()
                                            //                      select package.PackingDetailID).SingleOrDefault();

                                            SaveDeletesBox(Global.BoxActive, Location.Text, txtSKUName.Text);

                                            var packindatialID = (from package in PackageDetail
                                                                  where package.SKUNumber == txtSKUName.Text && package.BoxNumber == bxnmr
                                                                  select package.PackingDetailID).SingleOrDefault();

                                            Control.deleteSKUFromBoxsm((Guid)packindatialID);

                                            cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                                            packing.PackingStatus = 1;
                                            List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                                            _lsNewPacking.Add(packing);

                                            Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);


                                            rmveflf = true;
                                            txtQuantity.Text = "0";
                                            ContentPresenter sp1 = grdContentRemove.Columns[3].GetCellContent(row) as ContentPresenter;
                                            DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                                            TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);


                                            /// myTextBlock.Visibility = Visibility.Hidden;
                                            myTextBlock.Text = "Removed";
                                            myTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                                            ShowMassagePopup("Item" + txtSKUName.Text + " is  Removed", 5000);
                                            Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, bxnmr);

                                        }
                                    }
                                    else if (Convert.ToInt16(txtQuantity.Text) > 1) //for update
                                    {
                                        Invalid = false;

                                        MsgBox.Show("Confirm", "Remove", "Remove SKU from the current Box" + Environment.NewLine + "Are you sure want to Remove SKU?");
                                        if (Global.MsgBoxResult == "Ok")
                                        {
                                            //var packindatialID = (from package in PackageDetail
                                            //                      where package.SKUNumber == txtSKUName.Text && package.BoxNumber==cmbBoxNUmber.SelectedItem.ToString()
                                            //                      select package.PackingDetailID).SingleOrDefault();


                                            SaveDeletesBox(Global.BoxActive, Location.Text, txtSKUName.Text);

                                            var packindatialID = (from package in PackageDetail
                                                                  where package.SKUNumber == txtSKUName.Text && package.BoxNumber == bxnmr
                                                                  select package.PackingDetailID).SingleOrDefault();

                                            int Qty = Convert.ToInt16(txtQuantity.Text) - 1;

                                            Control.updateSKUFromBoxsm((Guid)packindatialID, Qty);


                                            cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                                            packing.PackingStatus = 1;
                                            List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                                            _lsNewPacking.Add(packing);

                                            Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);





                                            rmveflf = true;
                                            txtQuantity.Text = Qty.ToString();

                                            ContentPresenter sp1 = grdContentRemove.Columns[3].GetCellContent(row) as ContentPresenter;
                                            DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                                            TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);


                                            /// myTextBlock.Visibility = Visibility.Hidden;
                                            myTextBlock.Text = "Removed";
                                            myTextBlock.Foreground = new SolidColorBrush(Colors.Red);


                                            ShowMassagePopup("Item" + txtSKUName.Text + " is  Removed", 3000);
                                            Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, bxnmr);
                                        }
                                    }
                                    else if (Convert.ToInt16(txtQuantity.Text) == 0) //for update
                                    {//For Showing Message 
                                        Invalid = false;

                                        ShowMassagePopup("Item" + txtSKUName.Text + " is Already Removed", 5000);
                                    }
                                    ///For invisible Cancel Button
                                    btnCancelShipment.Visibility = Visibility.Hidden;
                                    //Global.shipmentclosed ="False";
                                    ///For print updated BoxNumber

                                    if (flg == 1)
                                    {
                                        //foreach (var bx in updatedboxnumber)
                                        //{
                                        int cnt = updatedboxnumber.Count;
                                        for (int i = 0; i < cnt; i++)
                                            if (updatedboxnumber[i].ToString() != bxnmr)
                                                updatedboxnumber.Add(bxnmr);
                                        //}
                                    }
                                    else
                                    {
                                        updatedboxnumber.Add(bxnmr);
                                    }
                                    flg = 1;
                                    ///updatedboxnumber.Add(bxnmr);
                                }
                                //else
                                //{
                                //    ShowMassagePopup("Invalid scan", 5000);
                                //}
                                //grdContentRemove.ScrollIntoView(row.Item);
                                //row.Background = new SolidColorBrush(Color.FromRgb(192, 192, 192));
                                //ContentPresenter sp1 = grdContentRemove.Columns[3].GetCellContent(row) as ContentPresenter;
                                //DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                                //TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);

                                //if (  rmveflf == true)
                                //{
                                //    /// myTextBlock.Visibility = Visibility.Hidden;
                                //    myTextBlock.Text = "Removed";
                                //    myTextBlock.Foreground = new SolidColorBrush(Colors.Red);




                                //}
                                //else
                                //{
                                //    myTextBlock.Text = "";

                                //}


                            }
                            if (checkQty() == false)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnExitShipment);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();

                            }
                        }
                        txtScannSKu.Text = "";
                        if (Invalid == true)
                        {
                            ShowMassagePopup("Invalid scan", 5000);
                        }

                    }
                }

            }
            catch (Exception Ex)
            {
                txtScannSKu.Text = "";
                ShowMassagePopup("Invalid scan", 3000);
                ErrorLoger.save("wndRemoveSKUFromBox - txtScannSKu_KeyDown_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }
        #region Message
        ////Function For Showing New Message Box 25-11-2014

        public void ShowMassagePopup(string message, int TimeSpan)
        {
            //  brdMessage.Background = new SolidColorBrush(Colors.Black);
            //brdfrist.Background = new SolidColorBrush(Colors.Black);
            /// brdMessage.Opacity = 0.5;
            try
            {
                lblmessage.Content = message;
                //brdMessage.Background = new SolidColorBrush(Colors.Black);
                //brdMessage.Opacity = 0.5;
                brdMessage2.Visibility = System.Windows.Visibility.Visible;

                dtLoadUpdate1 = new DispatcherTimer();
                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, TimeSpan);
                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                //start the dispacher.
                dtLoadUpdate1.Start();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - ShowMassagePopup", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

            }
        }
        private void dtLoadUpdate1_Tick(object sender, EventArgs e)
        {
            try
            {
                //  brdfrist.Background = new SolidColorBrush(Colors.White);
                ////brdfrist.Background = new SolidColorBrush(Colors.Black);
                //  brdfrist.BorderThickness = new Thickness(4,0,0, 0);
                // brdMessage.Opacity = 1;
                // brdMessage.Background = new SolidColorBrush(Colors.Transparent);
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - dtLoadUpdate1_Tick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

            }

        }
        #endregion
        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            try
            {
                //  brdMessage.Opacity = 1;
                //  brdMessage.Background = new SolidColorBrush(Colors.Transparent);
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
                txtScannSKu.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - mbox_ok", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void mbox_cancel(object sender, RoutedEventArgs e)
        {
            try
            {
                //  brdMessage.Opacity = 1;
                //  brdMessage.Background = new SolidColorBrush(Colors.Transparent);
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveSKUFromBox - mbox_cancel", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        ////End

        public void SaveDeletesBox(String BoxNumber, string ShipmentLocation, String SKUNumber)
        {

            try
            {
                //List<cstPackageDetails> lsPackageDetail = new List<cstPackageDetails>();
                //lsPackageDetail = Global.controller.GetPackingDetailTbl(BoxNumber);

                List<cstDeletedBoxSave> ListDeletedBox = new List<cstDeletedBoxSave>();


                //foreach (var item in lsPackageDetail)
                //{
                cstDeletedBoxSave delete = new cstDeletedBoxSave();
                delete.BoxNumber = BoxNumber;
                delete.SKUQuantity = 1;
                delete.Location = ShipmentLocation;
                delete.StationName = Global.StationName;
                delete.DeleteDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                delete.DeletedBoxID = Guid.NewGuid();
                delete.UserID = Global.LoggedUserId;
                delete.SKUName = SKUNumber;
                delete.ActionStatus = "Remove";


                ListDeletedBox.Add(delete);
                // }

                Global.controller.SetDeletedBoxDetails(ListDeletedBox);
            }
            catch (Exception)
            {

            }

        }
    }
}
