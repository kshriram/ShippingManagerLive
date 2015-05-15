 #region Assemblies
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
using PackingClassLibrary;
using Packing_Net.Classes;
using System.Windows.Resources;
using PackingClassLibrary.CustomEntity;
using System.Collections;
using System.Windows.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using PackingClassLibrary.BusinessLogic;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Runtime.InteropServices;
using PackingNet.Classes;
using PackingNet.Pages;
using PackingNet;
using Microsoft.Windows.Controls.Primitives;

#endregion
namespace Packing_Net.Pages
{
    public delegate void simpledelegate();
    public delegate void MessageDelegate(string msg,int time);
    public partial class ShipmentScreen : Window
    {
        #region Declaration
        //for avoid problem of same sku in diffrnt box
        Boolean flgForWrongMsg = false;
        Boolean chk = true;
        //Get model of shipment;
        model_Shipment _shipment = Global.controller.getModelShipment(Global.ShippingNumber);

        List<Run> lsScroll1 = new List<Run>();

        //Datatime Veriables for packing details Start and end date.
        DateTime StartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
        DateTime LastEndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
        DispatcherTimer timer;
        DispatcherTimer timer2;
        DispatcherTimer timer3;

        int OrderedQuantiy = 0;
        int PakedQuantitiy = 0;
        int BoxQuantity = 1;

        //Override mode 2 Flage maintainer is row scanning automatic or by user to save reocred or update record.
        Boolean ISRowAutoScaned = false;
        Boolean IsManualEntry = false;
        DateTime itemPackedTime = DateTime.UtcNow;

        //Work with gridview fill complete event.
        BackgroundWorker Worker = new BackgroundWorker();

        //String application location 
        String ApplicationLocation = Global.controller.ApplicationLocation();

        //Grid Data items
        List<cstShipment> Bindedshipment = new List<cstShipment>();

        smController ConObj = new smController();

        //CartonNumber for save on add Box
        int CartonNumber = 0;
        int previousCarton = 0;
        List<KeyValuePair<int, Guid>> lsRowAndPackingDetailsiD = new List<KeyValuePair<int, Guid>>();
        Boolean IsFirstTime = true;

        DispatcherTimer dtLoadUpdate;
        /// <summary>
        /// For Showing Correct Qty For Same Sku diff Box
        /// </summary>
        /// 
        int Count = 0;
        string rcntsku = "";
        string prvssku = "";
        public class myinfo
        {
            public string Skuname { get; set; }
            public int Qty { get; set; }
        }
        List<myinfo> lsinfo = new List<myinfo>();
        myinfo mm = new myinfo();
        //End
        #endregion

        public ShipmentScreen()
        {

           

            InitializeComponent();

            this.Closing += new System.ComponentModel.CancelEventHandler(MyWindow_Closing);

            try
            {
                txtScannSKu.Focus();

                

                //Hide the Error Label
                BErrorMsg.Visibility = System.Windows.Visibility.Hidden;

                //Show the satation name at the title
                lblStationName.Content = Global.StationName;

                //Define DispacherTimer to Show clock at the Top;
                timer = new DispatcherTimer();
                timer.Tick += timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();

                DateTime Dt = Convert.ToDateTime(Global.LastLoginDateTime);
               // lblLastLoginTime.Content = Dt.ToString("MMM dd, yyyy h:mm tt ").ToString();
                lblLastLoginTime.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Dt, "Eastern Standard Time").ToString("MMM dd, yyyy").TrimStart('0').ToString();
                //Add event to the BackGround worker.
                Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);

               // splashScreen.Show(false);
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - ShipmentScreen", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Global.UnPacked != "" && Global.UnPacked != null)
                {
                    e.Cancel = true;
                    ShowMassagePopup("Please you have to Pack this Box \n" + Global.UnPacked + " First", 4000);
                    //ShowMassagePopup("If you choose to exit, \n your open box  " + Global.UnPacked + " is deleted.", 4000);
                    txtScannSKu.Focus();
                }
                else
                {
                    e.Cancel = false;
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - MyWindow_Closing", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                ShowMassagePopup("Invalid Operation Closing", 4000);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                ///  lblTime.Content = DateTime.UtcNow.ToString("MMM dd, yyyy h:mm tt ").ToString();
                lblTime.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                /// lblTime.Content = DateTime.UtcNow.ToString("MMM dd, yyyy h:mm tt ").ToString();
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - timer_Tick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        #region Page load

        /// <summary>
        /// Window Load functions Placed here.
        /// Data Binding to the Gridview is done on the window load from the appropriate Funcation call to 
        /// Dll included in the project.
        /// </summary>
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {

                //SplashScreen splashScreen = new SplashScreen("loading_desktop.gif");
                //splashScreen.Show(true);


                lblblinlText.Text = Global.pkdStatus;
                //log 
                Thread t = new Thread(() => { SaveUserLogsTbl.logThis(csteActionType.ShipmentScreenLoaded.ToString(), Global.ShippingNumber.ToString()); });

                //Add Box Button hide
                btnAddNewBox.Visibility = Visibility.Hidden;
                btnRemoveSKuBox.Visibility = Visibility.Hidden;
                btnSummary.Visibility = Visibility.Hidden;
               // btnSummary.Visibility = Visibility.Hidden;
                //Clear recently packed packing ID;
                Global.RecentlyPackedID = Guid.Empty;
                Boolean IsAdmin = false;
                //start sesseion start
                Thread t1 = new Thread(() => { SessionManager.StartTime(); });
                //Show Scroll Messages in message stack

               

                this.Dispatcher.Invoke(new Action(() => { _showListStrings(); }));
                //Hide and Show the Error Strip to animate the Error Label
                ScrollMsg("please Scan item", Colors.Green);
                //lblCurrentBox.Content = Global.pkdStatus;
                // Load the shipment Grid view depending on conditions, set Mode.
                #region Load GridView
                try
                {
                    List<cstShipment> shipment = new List<cstShipment>();

                    //Manager Override.
                    #region Manager Override

                    //Hide top label.
                    ///  lblOverride.Visibility = System.Windows.Visibility.Hidden;
                    
                    /////For Testing Admin seeboth location skus 24-12-2014
                     List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
                    var rolnm=(from v in lsUserInfo  
                              where v.UserID==Global.LoggedUserId 
                              select v.RoleName).FirstOrDefault();
                    if (Global.controller.IsSuperUser(Global.LoggedUserId))
                    {

                        shipment = Global.controller.GetShipment_SPCKD(Global.ShippingNumber, true);
                        lblAdminMessage.Content = "You are looking both location SKU's NYWT and NYWH";
                        ShowMassagePopup("You are Open Both Location SKU's \n NYWT and NYTH", 4000);
                        //Save New Record To the BoxPackage table
                        cstBoxPackage _boxPackage = new cstBoxPackage();
                        _boxPackage.PackingID = Global.PackingID;
                        ///_boxPackage.BoxCreatedTime = DateTime.UtcNow;
                        _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());
                        List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                        lsBox.Add(_boxPackage);

                        //if (Global.flgBx == "AddBx")
                        //{
                        Global.PrintBoxID = Global.controller.SetBox(lsBox, _boxPackage.PackingID);
                        Global.flgBx = "";
                        //}
                        //else if (Global.flgBx == "NotAddBx")
                        //{
                        //    Global.flgBx = "";
                        //}
                        IsAdmin = true;
                    }
                    else
                    {
                        if (Global.Mode == "Override")
                        {
                            shipment = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);

                            //  lblOverride.Visibility = System.Windows.Visibility.Visible;
                            //   lblOverride.Content = Global.controller.ConvetLanguage("This Shipment is in override mode.( Manager: " + Global.ManagerName + ")", Global.LanguageFileName);
                            lblUserTop.Content = Global.WindowTopUserName;

                            //Save New Record To the BoxPackage table
                            cstBoxPackage _boxPackage = new cstBoxPackage();
                            _boxPackage.PackingID = Global.PackingID;
                            _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());
                            List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                            lsBox.Add(_boxPackage);
                            if (Global.flgBx == "AddBx")
                            {
                                Global.PrintBoxID = Global.controller.SetBox(lsBox, _boxPackage.PackingID);
                                Global.flgBx = "";

                            }
                            else if (Global.flgBx == "NotAddBx")
                            {
                                Global.flgBx = "";
                            }
                            //delete previous information
                            Global.controller.DeleteCartonInfo(Global.ShippingNumber);
                        }
                        else if (Global.Mode == "SameUser")
                        {
                            shipment = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);
                            btnRemoveSKuBox.Visibility = Visibility.Hidden;
                            ///  lblOverride.Visibility = System.Windows.Visibility.Visible;
                            //   lblOverride.Content = Global.controller.ConvetLanguage("This Shipment is in re-packing Mode.", Global.LanguageFileName);
                            lblUserTop.Content = Global.WindowTopUserName;

                            try
                            {
                                cstBoxPackage _boxPackage = new cstBoxPackage();
                                _boxPackage.PackingID = Global.PackingID;
                                /// _boxPackage.BoxCreatedTime = DateTime.UtcNow;
                                _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());
                                List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                                lsBox.Add(_boxPackage);
                                if (Global.flgBx == "AddBx")
                                {
                                    Global.PrintBoxID = Global.controller.SetBox(lsBox, _boxPackage.PackingID);
                                    Global.flgBx = "";
                                }
                                else if (Global.flgBx == "NotAddBx")
                                {
                                    Global.flgBx = "";
                                }
                                string boxnum = Global.controller.GetBoxPackageByPackingID(Global.SameUserpackingID).Max(bx => bx.BOXNUM);

                                Global.PrintBoxID = Global.controller.GetBoxIDByBoxNumber(boxnum);
                                //Global.PrintBoxID = Global.controller.GetBoxPackageByPackingID(Global.SameUserpackingID).Max(bx => bx.ROWID);
                            }
                            catch (Exception)
                            {
                                cstBoxPackage _boxPackage = new cstBoxPackage();
                                //    _boxPackage.PackingID = Global.PackingID;
                                //    _boxPackage.BoxCreatedTime = DateTime.UtcNow;
                                //    List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                                //    lsBox.Add(_boxPackage);
                                //    Global.PrintBoxID = Global.controller.SetBox(lsBox);
                            }

                        }
                        else
                        {
                            shipment = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);

                            //Save New Record To the BoxPackage table
                            cstBoxPackage _boxPackage = new cstBoxPackage();
                            _boxPackage.PackingID = Global.PackingID;
                            ///_boxPackage.BoxCreatedTime = DateTime.UtcNow;
                            _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());
                            List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                            lsBox.Add(_boxPackage);

                            if (Global.flgBx == "AddBx")
                            {
                                Global.PrintBoxID = Global.controller.SetBox(lsBox, _boxPackage.PackingID);
                                Global.flgBx = "";
                            }
                            else if (Global.flgBx == "NotAddBx")
                            {
                                Global.flgBx = "";
                            }
                        }
                    }

                    if (Global.ShippingNumber != "")
                    {
                        //Fetch Shipping nuber information..

                        if (shipment != null)
                        {
                            lblShipmentId.Content = Global.ShippingNumber.ToString();

                            //add comboIdTO the shipment Information.
                            CobmoIDGenrator _generate = new CobmoIDGenrator();
                            shipment = _generate.SetComboNumbers(shipment);
                            this.Dispatcher.Invoke(new Action(() => { grdContent.ItemsSource = shipment; }));
                            Bindedshipment = shipment;
                            if (IsAdmin)
                                grdContent.Columns[9].Visibility = Visibility.Visible;
                        }
                    }
                    #endregion
                    //set packing Start date when this screen loaded.
                    //StartTime = DateTime.UtcNow;
                    StartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    //Display the contern at the top;
                 //   lblTime.Content = DateTime.UtcNow.ToLongDateString();
                    lblTime.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    if (Global.LoggedUserModel.UserInfo.UserFullName != "")
                    {
                        lblUserName.Content = Global.WindowTopUserName;
                        lblUserTop.Content = Global.WindowTopUserName;
                    }

                    // Code for total today
                    lblTotalToday.Content = Global.controller.GetTotalToday(Global.LoggedUserId)[0].Value;

                    // Code for average time
                    TimeSpan Tm = TimeSpan.FromSeconds(Global.controller.GetAverageTime(Global.LoggedUserId)[0].Value);
                    string min = Tm.Minutes.ToString();
                    string sec = Tm.Seconds.ToString();
                    sec = sec.TrimStart(new char[] { '0' }) + "";
                    if (sec != "")
                    {
                        sec = "" + sec + "sec";
                    }
                    min = min.TrimStart(new char[] { '0' }) + "";
                    if (min != "")
                    {
                        min = min + "min:";
                    }
                    lblAverageBoxTime.Content = min + sec;


                 


                }
                catch (Exception Ex)
                {
                    //Log the Error to the Error Log table
                    ErrorLoger.save("wndShipmentDetailPage - Window_Loaded_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                }
                #endregion



                FnForCurrentBoxID();

                txtScannSKu.Focus();

                //Language Change:
                WindowLanguages.Convert(this);

                //For Chage Status
                //inprogress();
                Global.splashScreen.Show(false);

            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
               // ErrorLoger.save("wndShipmentDetailPage - Window_Loaded_1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndShipmentDetailPage - Window_Loaded_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }


        }
        public void FnForCurrentBoxID()
        { //For Showing Which is Current Box

            try
            {
                if (Global.FlgaddInBox == "FillInSelectedBox")
                {
                    lblCurrentBox.Content = Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM;
                    Global.PrintBoxID = Global.selectedBoxID;
                }
                else
                {
                    lblCurrentBox.Content = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                }
                MessageDelegate mms = new MessageDelegate(ShowMassagePopup);
                //mms("Please Scan item ", 3000);

            }
            catch (Exception Ex)
            {

                ErrorLoger.save("wndShipmentDetailPage - FnForCurrentBoxID()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

          
            //End

        }

        //public void inprogress()
        //{
        //    foreach (DataGridRow row in GetDataGridRows(grdContent))
        //    {
        //        if (row.IsEnabled && grdContent.Items.Count > 1)
        //        {
        //            try
        //            {
        //                ContentPresenter cp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
        //                DataTemplate myDataTemplate = cp.ContentTemplate;
        //                TextBox t = (TextBox)myDataTemplate.FindName("gtxtBox", cp);
        //                //t.Text = (Convert.ToInt32(t.Text) + 1).ToString();
        //                TextBlock QUantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
        //                TextBlock pckdQUantity = grdContent.Columns[4].GetCellContent(row) as TextBlock;

        //                if (QUantity.Text == pckdQUantity.Text)
        //                {
        //                    t.Text = "";
        //                }
        //                else
        //                {
        //                    t.Text = "in progress";
        //                }



        //            }
        //            catch (Exception)
        //            { }
        //        }
        //    }
        //}

        /// <summary>
        /// Set The upate mode shipment and call the key press event of txtSkuSacn to paint rows.
        /// </summary>
        private void _sameUserRepacking()
         {
            try
            {

                List<cstPackageDetails> _lsPackedDetailTableInfo = new List<cstPackageDetails>();
                List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
                    var rolnm=(from v in lsUserInfo
                              where v.UserID==Global.LoggedUserId 
                              select v.RoleName).FirstOrDefault();

                    if (Global.controller.IsSuperUser(Global.LoggedUserId))
                    {
                        _lsPackedDetailTableInfo = Global.controller.GetPackingDetailTblByShipment(Global.ShippingNumber);
                    }
                    else
                    {
                        _lsPackedDetailTableInfo = Global.controller.GetPackingDetailTbl(Global.PackingID);
                    }
                foreach (cstPackageDetails _PackItem in _lsPackedDetailTableInfo)
                {
                    int itemQuty = Convert.ToInt32(_PackItem.SKUQuantity);//Repacking Quantity
                    for (int i1 = 0; i1 < itemQuty; i1++)//Loop for quantity
                    {
                        //Set Auto Sacannig Flag true. 
                        ISRowAutoScaned = true;

                        //Set text of textbox to upc code.
                        txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == _PackItem.SKUNumber).UPCCode;

                        //Set Previously packed time of item to show in the Grid.
                        itemPackedTime = _PackItem.PackingDetailStartDateTime;

                        //Set Previously packed time as packing time. for package table
                        StartTime = _shipment.PackingInfo[0].StartTime;

                        var key = Key.Enter;                    // Key to send
                        var target = txtScannSKu;   // Target element
                        var routedEvent = Keyboard.KeyDownEvent; // Event to send
                        target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                         System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
                        //if (Global.shipmentclosed == "true")
                        //    btnAddNewBox.Visibility = Visibility.Hidden;
                        //else
                        //    btnAddNewBox.Visibility = Visibility.Visible;


                    }
                }

                ////this code need to change when work with manager overide 12-10-2014
               // List<cstPackageTbl> _lsPackedDetailTableInfo2 = Global.controller.GetPackingList(Global.PackingID,true);
                cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
              ///  int tt = _lsPackedDetailTableInfo2[0].PackingStatus;
                if (packing.PackingStatus == 0)
                {
                    ////shipment is closed
                    Global.pkdStatus = "Shipment is Closed.";
                    btnAddNewBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    Global.pkdStatus = "You are in Packing Mode.";
                    ///btnAddNewBox.Visibility = Visibility.Visible;
                }
                ////END

                ////foreach (cstPackageTbl _PackItem2 in _lsPackedDetailTableInfo2)
                ////{
                ////    int tt = _PackItem2.PackingStatus;
                ////}

                ISRowAutoScaned = false;
                Global.SameUserpackingID = Guid.Empty;
            }
            catch (Exception Ex)
            {
                ShowMassagePopup("wndShipmentDetailPage Error Message" + Ex.Message + "", 2000);
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - _sameUserRepacking()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        #endregion

        public bool IsLineType6(String SKU)
        {
            Boolean _return = false;
            foreach (var item in Bindedshipment)
            {
                if (item.SKU.ToString() == SKU.ToString() && item.LineType == 6)
                {
                    _return = true;
                }
            }
            return _return;
        }

        /// <summary>
        ///Show items Packing Remaining and Items Packed label.
        /// </summary>
        public void showQuantityLabel()
        {
            try
            {
                OrderedQuantiy = 0;
                PakedQuantitiy = 0;
                //this forech for each Quantity Count.
                foreach (DataGridRow row in GetDataGridRows(grdContent))
                {
                    try
                    {
                        TextBlock txtSku = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                        TextBlock txtOrderdQuantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                        if (!IsLineType6(txtSku.Text))
                            OrderedQuantiy = Convert.ToInt32(txtOrderdQuantity.Text) + Convert.ToInt32(OrderedQuantiy);
                        TextBlock txtPakedQuantity = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                        PakedQuantitiy = Convert.ToInt32(txtPakedQuantity.Text) + Convert.ToInt32(PakedQuantitiy);
                    }
                    catch (Exception Ex)
                    {
                        ErrorLoger.save("ShipmentScreen.showQuantityLabel()", Ex.Message.ToString());
                        PakedQuantitiy += 0;
                    }
                    tbkStatus.Text = "Status Activity : " + PakedQuantitiy + Global.controller.ConvetLanguage(" item(s) packed out of ", Global.LanguageFileName) + OrderedQuantiy + Global.controller.ConvetLanguage(" item(s).", Global.LanguageFileName);
                    if (PakedQuantitiy == OrderedQuantiy)
                    {
                        //btnAddNewBox.Visibility = Visibility.Hidden;
                        Global.shipmentclosed = "true";
                        Global.FlagAllshippckd = "flagTrue";
                        tbkStatus.Text = "All items of this shipment are packed. Please scan your badge to continue.";
                        bdrStatus.Background = new SolidColorBrush(Colors.Green);

                    }
                    else if (PakedQuantitiy >= OrderedQuantiy / 2)
                    {
                        bdrStatus.Background = new SolidColorBrush(Color.FromRgb(38, 148, 189));
                        tbkStatus.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        bdrStatus.Background = new SolidColorBrush(Color.FromRgb(38, 148, 189));
                        tbkStatus.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage -showQuantityLabel()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
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

        /// <summary>
        /// Validation for each row is checked for complete or not
        /// </summary>
        /// <returns>Boolean true is one or more then one row not checked else false</returns>
        public Boolean Done_ButtonValidation_TocheckAllRowsCompleted()
        {
            Boolean _retuen = false;
            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContent))
                {
                    if (row.IsEnabled == true)
                    {
                        _retuen = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - Done_ButtonValidation_TocheckAllRowsCompleted()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            return _retuen;
        }

        /// <summary>
        /// Avinash:
        /// This Code is for for pluse button in Gridview tamplet field button with increments the value of textbox by 1.
        /// this coce is very IMP for next project.
        /// </summary>
        /// <param name="sender"> parameter of click event</param>
        /// <param name="e"> paramenter of click evet</param> 
        private void gButton_onclick(object sender, RoutedEventArgs e)
        {
            //For problem of same sku on diff box

            //CartonObj.get

            //int flag = 1;

            //if (ConObj.GetAllCartonInfoByBoxNumber(Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM).Count > 0)
            //{
            //    flag = 0;
            //}
            //else
            //{
            //    flag = 1;
            //}
            try
            {
                List<myinfo> lsinfo = new List<myinfo>();
                myinfo mm = new myinfo();
                Count = 0;
                //end
                //chk = true;

              


                btnAddNewBox.Visibility = Visibility.Hidden;

                ///15-11-2014
                ///
                txtScannSKu.Focus();//Set Focus on textbox of sku UPC Code Scanning.
                if (Global.FlgaddInBox == "FillInSelectedBox")
                {
                    Global.PrintBoxID = Global.selectedBoxID;
                    String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM;

                    if (BoxNumber == Global.UnPacked)
                    {
                        Global.UnPacked = "";
                    }


                    SaveToCartonInfo(BoxNumber, CartonNumber);

                    wndBoxSlip _boxSlip = new wndBoxSlip();
                    _boxSlip.ShowDialog();
                    this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                }
                else
                {
                    String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                    if (BoxNumber == Global.UnPacked)
                    {
                        Global.UnPacked = "";
                    }
                    SaveToCartonInfo(BoxNumber, CartonNumber);

                    wndBoxSlip _boxSlip = new wndBoxSlip();
                    _boxSlip.ShowDialog();
                    this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                }
                ///end


                string BOXNumberUpdate = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                DateTime EndTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());

                ConObj.UpdateBox(EndTime, BOXNumberUpdate);




                //save box and Replace Global BoxID with new One
                cstBoxPackage _boxPackage = new cstBoxPackage();
                _boxPackage.PackingID = Global.PackingID;
                _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());
                List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                lsBox.Add(_boxPackage);
                Global.PrintBoxID = Global.controller.SetBox(lsBox,_boxPackage.PackingID);

                //if (flag == 0)
                //{
                //    ShowMassagePopup("Modified label printed", 4000);
                //}
                //else
                //{
                //ShowMassagePopup("Packing Slip Printed For Packed Box \n" + Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM, 2000);
               // ShowMassagePopup("Packing Slip Printed For Packed Box ", 2000);
                //}




                Global.FlgaddInBox = "";

                //Set Error message on the Sctrip and Visible it to animate.
                ScrollMsg("Packing Slip: Packing Slip Printed For Packed Box", Colors.Green);

                ///For Curent Box Selection 19-11-2014
                ///
                //// Global.PrintBoxID = Global.controller.GetBoxPackageByPackingID(Global.RecentlyPackedID).Max(bx => bx.BoxID);
                lblCurrentBox.Content = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                ///end




                //if (Global.IsActiveFlag)
                //{



                //    //Print old Box Id slip 

                //    Global.IsActiveFlag = false;

                //    wndBoxSlip _boxSlip = new wndBoxSlip();
                //    _boxSlip.ShowDialog();
                //    this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                //}
                //else
                //{


                //Increment Row number
                //foreach (DataGridRow row in GetDataGridRows(grdContent))
                //{
                //    if (row.IsEnabled && grdContent.Items.Count > 1)
                //    {
                //        try
                //        {
                //            //ContentPresenter cp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                //            //DataTemplate myDataTemplate = cp.ContentTemplate;
                //            //TextBox t = (TextBox)myDataTemplate.FindName("gtxtBox", cp);
                //            ////t.Text = (Convert.ToInt32(t.Text) + 1).ToString();
                //            //TextBlock QUantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                //            //TextBlock pckdQUantity = grdContent.Columns[4].GetCellContent(row) as TextBlock;

                //            //if (QUantity.Text == pckdQUantity.Text)
                //            //{
                //            //    t.Text = "";
                //            //}
                //            //else
                //            //{
                //            //    t.Text = "in progress";
                //            //    //t.Foreground=
                //            //}



                //        }
                //        catch (Exception)
                //        { }
                //    }
                //}
                //txtScannSKu.Focus();//Set Focus on textbox of sku UPC Code Scanning.

                ////String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                ////SaveToCartonInfo(BoxNumber, CartonNumber);

                ////Print old Box Id slip 

                //wndBoxSlip _boxSlip = new wndBoxSlip();
                //_boxSlip.ShowDialog();
                //this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));


                //}
            }
            catch (Exception Ex)
            {
                ShowMassagePopup(""+Ex.Message, 3000);
                ErrorLoger.save("wndShipmentDetailPage - gButton_onclick()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }




        }

        private void ImgSKU_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Worker.IsBusy)
                {
                    Worker.RunWorkerAsync();
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - ImgSKU_Loaded", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        /// <summary>
        /// Background worker method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Show images on the screen.
              //  this.Dispatcher.Invoke(new Action(_fillImage));

                //Please wait screen abort.
                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}

                //Barcode show hide 
                Global.ISBarcodeShow = Global.controller.ReadFromLocalFile("ISBarcodeShow");
                if (Global.ISBarcodeShow == "Yes")
                {
                    this.Dispatcher.Invoke(new Action(_showBarcode));
                }
                else
                {
                    this.Dispatcher.Invoke(new Action(_hideBarcodes));
                }

                //Same User Packing Call Update row Event Key press textbox sku scan.
                if (Global.SameUserpackingID != Guid.Empty || Global.SameUserpackingID != null) //Already packed items mark to packed.(Update mode shipment open.)
                {
                    this.Dispatcher.Invoke(new Action(_sameUserRepacking));
                }

                //mark the Combo Product and hide its borcodes.
                this.Dispatcher.Invoke(new Action(() => { BoldFontandHideCombp(Bindedshipment); }));
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - Worker_DoWork", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        //Combo Product Bold and Barcode hide them
        public void BoldFontandHideCombp(List<cstShipment> lsShipment)
        {
            try
            {
                foreach (var item in lsShipment)
                {
                    if (item.LineType == 6)
                    {
                        foreach (DataGridRow row in GetDataGridRows(grdContent))
                        {
                            TextBlock txtSKUName = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                            if (txtSKUName.Text.ToUpper() == item.SKU.ToUpper())
                            {
                                TextBlock txtproductName = grdContent.Columns[2].GetCellContent(row) as TextBlock;
                                txtproductName.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                                txtproductName.FontWeight = FontWeight.FromOpenTypeWeight(400);
                                TextBlock txtQuantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                                txtQuantity.Foreground = new SolidColorBrush(Colors.Gray);
                                txtQuantity.Text = "0";
                                txtSKUName.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                                txtSKUName.FontWeight = FontWeight.FromOpenTypeWeight(400);
                                row.Background = new SolidColorBrush(Colors.Gray);
                                TextBlock txtPacked = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                                txtPacked.Foreground = new SolidColorBrush(Colors.Gray);
                                ContentPresenter sp1 = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                                DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                                TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);
                                myTextBlock.Visibility = Visibility.Hidden;
                                myTextBlock.Foreground = new SolidColorBrush(Colors.Gray);
                                //Hode Quantity Equal Barcode
                                ContentPresenter _contentPar = grdContent.Columns[7].GetCellContent(row) as ContentPresenter;
                                DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                                Image _imgBarcode = (Image)_dataTemplate.FindName("imgBarCode", _contentPar);
                                TextBlock txtComboNumber = (TextBlock)_dataTemplate.FindName("txtGroupID", _contentPar);
                                _imgBarcode.Visibility = Visibility.Hidden;
                                txtComboNumber.Text = "";
                                ContentPresenter sp = grdContent.Columns[4].GetCellContent(row) as ContentPresenter;
                                DataTemplate myDataTemplate2 = sp.ContentTemplate;
                                Button btn = (Button)myDataTemplate2.FindName("btnComplete", sp);
                                btn.Visibility = Visibility.Hidden;

                                Border bdr = (Border)myDataTemplate2.FindName("bdrImage", sp);
                                bdr.Visibility = Visibility.Hidden;
                                /// btn.
                                /// btn.Background = new SolidColorBrush(Colors.Gray);
                                /// 
                                //ContentPresenter _contentPar2 = grdContent.Columns[5].GetCellContent(row) as ContentPresenter;
                                //DataTemplate _dataTemplate2 = _contentPar2.ContentTemplate;
                                //Image _imgBarcode2 = (Image)_dataTemplate2.FindName("greenimage", _contentPar2);

                                //_imgBarcode2.Visibility = Visibility.Hidden;

                                row.IsEnabled = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridRow row in GetDataGridRows(grdContent))
                        {
                            TextBlock txtQuantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                            TextBlock txtPacked = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                            ContentPresenter sp1 = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                            DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                            TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);

                            if (txtQuantity.Text == txtPacked.Text && item.LineType != 6)
                            {
                                /// myTextBlock.Visibility = Visibility.Hidden;
                                myTextBlock.Text = "Complete";
                                myTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                            }
                            else
                            {
                                myTextBlock.Text = "To Process";

                            }
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - BoldFontandHideCombp()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

            }

          
        }



        /// <summary>
        /// Fill images for the SKU in data grid view.
        /// </summary>
        private void _fillImage()
        {

            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContent))
                {
                    ContentPresenter sp = grdContent.Columns[0].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate = sp.ContentTemplate;
                    Image ImgSKUset = (Image)myDataTemplate.FindName("ImgSKU", sp);
                    ImgSKUset.Height = 50;
                    ImgSKUset.Width = 60;
                    TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                    try
                    {
                        ImgSKUset.Source = BitmapFrame.Create(new Uri(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "//Images//MEDIA//" + SKUNo.Text + ".jpg", UriKind.Relative));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ImgSKUset.Source = BitmapFrame.Create(new Uri(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "//Images//MEDIA//" + SKUNo.Text + "-1" + ".jpg", UriKind.Relative));
                        }
                        catch (Exception)
                        { }
                    }

                    try
                    {
                        txtScannSKu.Focus();
                    }
                    catch (Exception Ex)
                    {
                        Thread ti = new Thread(() =>
                        {
                            //Log the Error to the Error Log table
                            ErrorLoger.save("wndShipmentDetailPage - _fillImage()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                        });
                    }
                }
            }
            catch (Exception Ex)
            {
                Thread ti = new Thread(() =>
                         { //Log the Error to the Error Log table
                             ErrorLoger.save("wndShipmentDetailPage - _fillImage()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                         });
            }
        }

        /// <summary>
        /// Barcode Show in Grid
        /// </summary>
        #region
        private void _showBarcode()
        {
            ///This Commented By Deepak on 20-11-2014 for speed up application as per told by Rakesh Sir

            try
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                foreach (DataGridRow Row in GetDataGridRows(grdContent))
                {

                    DataGridRow row = (DataGridRow)Row;
                    TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;

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
                        ContentPresenter sp = grdContent.Columns[7].GetCellContent(row) as ContentPresenter;
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
                            txtScannSKu.Focus();
                        }
                        catch (Exception Ex)
                        {
                            //Log the Error to the Error Log table
                            ErrorLoger.save("wndShipmentDetailPage - _showBarcode()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                        }

                        ///fn for Inprogress
                        ///
                        //ContentPresenter cp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                        //DataTemplate myDataTemplate2 = cp.ContentTemplate;
                        //TextBox t = (TextBox)myDataTemplate2.FindName("gtxtBox", cp);
                        ////t.Text = (Convert.ToInt32(t.Text) + 1).ToString();
                        //TextBlock QUantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                        //TextBlock pckdQUantity = grdContent.Columns[4].GetCellContent(row) as TextBlock;

                        //if (QUantity.Text == pckdQUantity.Text && row.IsEnabled == true)
                        //{
                        //    t.Text = "";
                        //}
                        //else if (row.IsEnabled == true)
                        //{
                        //    t.Text = "To Process";
                        //    t.Foreground = new SolidColorBrush(Colors.Orange);
                        //}

                        ///End


                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage -  _showBarcode()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        #endregion
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.MsgBoxMessage = "Are you sure want to logout? ";
                Global.MsgBoxTitle = "Logout";
                Global.MsgBoxType = "Error";
                Umsgbox msg = new Umsgbox();
                msg.ShowDialog();
                if (Global.MsgBoxResult == "Ok")
                {
                    HomeScreen _Home = new HomeScreen();
                    _Home.Show();
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - btnHome_Click", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        /// <summary>
        /// Save record code and roll Back code.
        /// </summary>
        private void _saveButtonClick(int PackingStatus, DataGridRow Row)
        {
            try
            {
                previousCarton = CartonNumber;
                ///d
                ///
                //if (Global.p != null)
                //{
                try
                {
                    int Rowindex = Row.GetIndex();
                    IsFirstTime = false;
                    Guid UserID = Guid.Empty;
                    UserID = Global.LoggedUserModel.UserInfo.UserID;
                    if (Global.Mode == "Override") UserID = Global.ManagerID;
                     
                    List<cstPackageTbl> _lsPacking = new List<cstPackageTbl>();
                    cstPackageTbl _packingCustom = new cstPackageTbl();
                    _packingCustom.ShippingNum = lblShipmentId.Content.ToString();
                    _packingCustom.UserID = UserID;
                    _packingCustom.StartTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(StartTime, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                    _packingCustom.EndTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                    _packingCustom.StationID = Global.controller.GetStationMasterByName(Global.StationName).StationID;
                    _packingCustom.ShippingID = Global.controller.GetShippingTbl(lblShipmentId.Content.ToString()).ShippingID;
                    _packingCustom.ShipmentLocation = ApplicationLocation;
                    if (Global.Mode == "Override")
                    {
                        _packingCustom.MangerOverride = 1;
                    }
                    else if (Global.Mode == "SameUser")
                    {
                        _packingCustom.MangerOverride = 2;
                    }

                    //Status: 1 - Under Packing Process.
                    //Status: 0 - Packing Complete
                    _packingCustom.PackingStatus = PackingStatus;
                    _lsPacking.Add(_packingCustom);

                    //Imp Code Avinash 7-May2013
                    //save in transaction table Packing Details.
                    String _result = "";//RollBack Transaction veriable.
                    try
                    {
                        ///Commented by deepak for problem of frist time loading shows wrong skus and qty
                        ///1-11-2014


                        //List<cstPackageDetails> lsPackingCustom = new List<cstPackageDetails>();
                        //DataGridRow row = Row;
                        //TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                        //TextBlock ProductName = grdContent.Columns[2].GetCellContent(row) as TextBlock;
                        //TextBlock QUantity = grdContent.Columns[4].GetCellContent(row) as TextBlock;
                        //ContentPresenter sp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                        //DataTemplate myDataTemplate = sp.ContentTemplate;
                        //TextBox myTextBlock = (TextBox)myDataTemplate.FindName("gtxtBox", sp);
                        //TextBlock Endtime = grdContent.Columns[7].GetCellContent(row) as TextBlock;
                        //string[] _TempEndTimeUser = Endtime.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        //String EndTimeUser = _TempEndTimeUser[1];

                        //// Custom Entity Boject of Packing Details.
                        //cstPackageDetails _packingC = new cstPackageDetails();
                        //_packingC.PackingDetailID = Guid.NewGuid();
                        //_packingC.PackingId = Global.PackingID;
                        //_packingC.SKUNumber = SKUNo.Text;

                        ////First Sku Items Paking DateTime is Shipment Allocation Datetime.
                        //_packingC.PackingDetailStartDateTime = Convert.ToDateTime(EndTimeUser);


                        ////   _packingC.SKUQuantity = Convert.ToInt32(QUantity.Text);
                        //_packingC.SKUQuantity = Convert.ToInt32("1");



                        //_packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                        //_packingC.ShipmentLocation = ApplicationLocation;
                        //_packingC.ProductName = ProductName.Text;

                        ////Added Columns information in packing Details.
                        //cstViewExtraColumns _view = Global.controller.GetViewColumnInfo(lblShipmentId.Content.ToString(), SKUNo.Text);

                        //_packingC.ItemName = _view.ItemName;
                        //_packingC.UnitOfMeasure = _view.UnitOfMeasure;
                        //_packingC.CountryOfOrigin = _view.CountryOfOrigin;
                        //_packingC.MAP_Price = _view.MAP_Price;
                        //_packingC.TCLCOD_0 = _view.TCLCOD_0;
                        //_packingC.TarrifCode = _view.TarrifCode;

                        //lsPackingCustom.Add(_packingC);

                        ////Convert message to te applition language
                        //Global.MsgBoxTitle = Global.controller.ConvetLanguage(Global.controller.SetPackingTable(_lsPacking, Global.PackingID), Global.LanguageFileName);

                        ////Save information inpacking detail table.
                        //_result = Global.controller.SetPackingDetails(lsPackingCustom);

                        ////Hide and Show the Error Strip to animate the Error Label
                        //ScrollMsg("Ok. \'" + SKUNo.Text + "\' Item Packed.", Colors.Green);
                        //btnAddNewBox.Visibility = Visibility.Visible;

                        ////save to key value pair.
                        //lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, _packingC.PackingDetailID));

                        ///End Commented on 1-11-2014
                        ///deepak

                        savePackingDatilsOnly(Row);
                    }
                    catch (Exception Ex)
                    {
                        //Log the Error to the Error Log table
                        ErrorLoger.save("wndShipmentDetailPage - _saveButtonClick()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

                        //RollBack Transaction
                        //RollBack Transaction function Call.
                        Boolean _Tranc = Global.controller.RollBackPakingMaster(lblShipmentId.Content.ToString());
                        Global.MsgBoxMessage = Global.controller.ConvetLanguage("Save Fail! Transaction rollback successful.", Global.LanguageFileName);
                        Global.MsgBoxTitle = Global.controller.ConvetLanguage("Warning", Global.LanguageFileName);
                    }
                    //Imp Code Avinash 7-May2013
                    if (Global.MsgBoxTitle == "Warning" || Global.MsgBoxTitle == null)
                    {
                        Global.MsgBoxTitle = Global.controller.ConvetLanguage("Warning", Global.LanguageFileName);
                        Global.MsgBoxType = "Warning";
                    }



                }
                catch (Exception Ex)
                {
                    //Log the Error to the Error Log table
                    ErrorLoger.save("wndShipmentDetailPage - _saveButtonClick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);

                    //Set Error message on the Sctrip and Visible it to animate.
                    ScrollMsg("Warning: Unexpected Error! Force Closed.", Color.FromRgb(222, 87, 24));

                    MsgBox.Show("Warning", "Unexpected Error!", "Force Closed.");
                    MainWindow Shipmentwnd = new MainWindow();
                    Shipmentwnd.Show();
                    this.Close();
                }
                //} 
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - _saveButtonClick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }


        private void savePackingDatilsOnly(DataGridRow Row)
        {
            try
            {
                int flg = 0;
                int Rowindex = Row.GetIndex();
                myinfo mm = new myinfo();

                List<cstPackageDetails> lsPackingCustom = new List<cstPackageDetails>();
                DataGridRow row = Row;
                TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                TextBlock ProductName = grdContent.Columns[2].GetCellContent(row) as TextBlock;
                TextBlock QUantity = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                ContentPresenter sp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                DataTemplate myDataTemplate = sp.ContentTemplate;
                TextBox myTextBlock = (TextBox)myDataTemplate.FindName("gtxtBox", sp);
              //  TextBlock Endtime = grdContent.Columns[7].GetCellContent(row) as TextBlock;
                //string[] _TempEndTimeUser = Endtime.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
               // String EndTimeUser = _TempEndTimeUser[1];
                ContentPresenter _contentPar = grdContent.Columns[8].GetCellContent(row) as ContentPresenter;
                DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                TextBlock RowNumber = (TextBlock)_dataTemplate.FindName("txtRowNumber", _contentPar);


                string sdfdsf = RowNumber.Text;
                // Custom Entity Boject of Packing Details.
                cstPackageDetails _packingC = new cstPackageDetails();
                _packingC.PackingDetailID = Guid.NewGuid();
                _packingC.PackingId = Global.PackingID;
                _packingC.SKUNumber = SKUNo.Text;
                _packingC.RowNumber = Convert.ToInt32(RowNumber.Text);
                //First Sku Items Paking DateTime is Shipment Allocation Datetime.
                _packingC.PackingDetailStartDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"));
                ///   _packingC.SKUQuantity = Convert.ToInt32(QUantity.Text);
                if (Global.FlgaddInBox == "FillInSelectedBox")
                {
                    _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM;
                }
                else
                {
                    _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                }
                _packingC.ShipmentLocation = ApplicationLocation;
                _packingC.ProductName = ProductName.Text;

                ////Commented by Deepak on 5-11-2014

                /////////   for problem of Qty same sku in diff boxes

                ////if (CartonNumber != previousCarton)
                ////{

                ////    _packingC.PackingDetailID = Global.pkDetailID;

                ////    Guid ppkkdd = Global.controller.GetPackingDetailTblbyskuandBoxnumber(SKUNo.Text, _packingC.BoxNumber);
                ////    lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex,ppkkdd));
                ////   // _packingC.PackingDetailID =Global.P
                ////}
                ////else
                ////{
                ////    _packingC.PackingDetailID = Guid.NewGuid();
                ////    Global.pkDetailID = _packingC.PackingDetailID;
                ////}
                int skuQty = 0;
                try
                {

                    skuQty = Global.controller.GetSKUQtyByBoxSKU(SKUNo.Text, _packingC.BoxNumber);
                }
                catch (Exception)
                {
                    skuQty = 0;

                }
                if (skuQty == 0)
                {
                    //Count++;
                    //mm.Skuname = SKUNo.Text;
                    //mm.Qty = Count;
                    //lsinfo.Add(mm);
                    Count = 1;
                    _packingC.SKUQuantity = Count;
                }

                else
                {

                    // skuQty = Global.controller.GetSKUQtyByBoxSKU(SKUNo.Text, _packingC.BoxNumber);

                    if (skuQty != null)
                        skuQty = skuQty + 1;


                    Guid ppkkdd = Global.controller.GetPackingDetailTblbyskuandBoxnumber(SKUNo.Text, _packingC.BoxNumber);
                    _packingC.PackingDetailID = ppkkdd;
                    lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, ppkkdd));

                    //if (CartonNumber != previousCarton)
                    //{

                    //    /// _packingC.PackingDetailID = Global.pkDetailID;

                    //     ppkkdd = Global.controller.GetPackingDetailTblbyskuandBoxnumber(SKUNo.Text, _packingC.BoxNumber);
                    //    _packingC.PackingDetailID = ppkkdd;
                    //    lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, ppkkdd));
                    //    // _packingC.PackingDetailID =Global.P
                    //}
                    Count++;
                    int _rowindex = row.GetIndex();
                    UpdatePackingDatilsOnlyforRescanQty(Row, GetGuidOfRow(lsRowAndPackingDetailsiD, _rowindex), skuQty);

                    flg = 1;


                    //// rcntsku = SKUNo.Text;
                    ////myinfo mm = new myinfo();

                    //if (Count > 1)
                    //{
                    //    foreach (var n in lsinfo)
                    //    {
                    //        if (n.Skuname == SKUNo.Text)
                    //        {
                    //            //_packingC.SKUQuantity = Count;
                    //            //Count++;

                    //            //For Different Combo in 
                    //            if (CartonNumber != previousCarton)
                    //            {

                    //              /// _packingC.PackingDetailID = Global.pkDetailID;

                    //                Guid ppkkdd = Global.controller.GetPackingDetailTblbyskuandBoxnumber(SKUNo.Text, _packingC.BoxNumber);
                    //                _packingC.PackingDetailID = ppkkdd;
                    //                lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, ppkkdd));
                    //                // _packingC.PackingDetailID =Global.P
                    //            }
                    //            ///end


                    //            int _rowindex = row.GetIndex();


                    //            var qt= (from d in lsinfo
                    //                    where d.Skuname==n.Skuname select d.Qty).Max();

                    //           // Count = mm.Qty;
                    //            Count = qt;
                    //            Count++;
                    //            UpdatePackingDatilsOnlyforRescanQty(Row, GetGuidOfRow(lsRowAndPackingDetailsiD, _rowindex), Count);
                    //            flg = 1;
                    //           ////flg = 1;
                    //            break;
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    foreach (var m in lsinfo)
                    //    {
                    //        if (m.Skuname == SKUNo.Text)
                    //        {
                    //            int _rowindex = row.GetIndex();
                    //           // Count = mm.Qty;

                    //            //For Different Combo in 
                    //            if (CartonNumber != previousCarton)
                    //            {

                    //                /// _packingC.PackingDetailID = Global.pkDetailID;

                    //                Guid ppkkdd = Global.controller.GetPackingDetailTblbyskuandBoxnumber(SKUNo.Text, _packingC.BoxNumber);
                    //                _packingC.PackingDetailID = ppkkdd;
                    //                lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, ppkkdd));
                    //                // _packingC.PackingDetailID =Global.P
                    //            }
                    //            ///end


                    //            Count++;
                    //            UpdatePackingDatilsOnlyforRescanQty(Row, GetGuidOfRow(lsRowAndPackingDetailsiD, _rowindex), Count);
                    //            flg = 1;
                    //            break;
                    //          //  _packingC.SKUQuantity = Count;

                    //        }
                    //        else
                    //        {
                    //            _packingC.SKUQuantity = Count;

                    //        }

                    //    }


                    //}
                    //mm.Skuname = SKUNo.Text;
                    //mm.Qty = Count;
                    // lsinfo.Add(mm);
                }
                //if (_packingC.SKUQuantity == 0 && flg!=1)
                //{
                //    Count = 1;
                //    _packingC.SKUQuantity = 1;
                //    mm.Skuname = SKUNo.Text;
                //    mm.Qty = Count;
                //    lsinfo.Add(mm);
                //}

                ////  end
                if (flg == 0)
                {
                    ////  Added Columns information in packing Details.

                    //End
                    cstViewExtraColumns _view = Global.controller.GetViewColumnInfo(lblShipmentId.Content.ToString(), SKUNo.Text);

                    _packingC.ItemName = _view.ItemName;
                    _packingC.UnitOfMeasure = _view.UnitOfMeasure;
                    _packingC.CountryOfOrigin = _view.CountryOfOrigin;
                    _packingC.MAP_Price = _view.MAP_Price;
                    _packingC.TCLCOD_0 = _view.TCLCOD_0;
                    _packingC.TarrifCode = _view.TarrifCode;

                    lsPackingCustom.Add(_packingC);

                    ////Convert message to te applition language
                    //Global.MsgBoxTitle = Global.controller.ConvetLanguage(Global.controller.SetPackingTable(_lsPacking, Global.PackingID), Global.LanguageFileName);

                    //Save information inpacking detail table.
                    Global.controller.SetPackingDetails(lsPackingCustom);

                    //Hide and Show the Error Strip to animate the Error Label
                    ScrollMsg("Ok. \'" + SKUNo.Text + "\' Item Packed.", Colors.Green);
                    btnAddNewBox.Visibility = Visibility.Visible;

                    //save to key value pair.
                    lsRowAndPackingDetailsiD.Add(new KeyValuePair<int, Guid>(Rowindex, _packingC.PackingDetailID));

                }

                ///fn for Inprogress
                ///
                ContentPresenter cp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                DataTemplate myDataTemplate2 = cp.ContentTemplate;
                TextBox t = (TextBox)myDataTemplate2.FindName("gtxtBox", cp);
                //t.Text = (Convert.ToInt32(t.Text) + 1).ToString();
                TextBlock QUantity2 = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                TextBlock pckdQUantity = grdContent.Columns[5].GetCellContent(row) as TextBlock;

                if (QUantity2.Text == pckdQUantity.Text && row.IsEnabled == true)
                {
                    t.Text = "";
                }
                else if (row.IsEnabled == true)
                {
                    //t.Text = "To progress";
                    t.Text = "In Process";
                    t.Foreground = new SolidColorBrush(Colors.Red);
                }
                ///For Restrict Unpacking
                Global.UnPacked = _packingC.BoxNumber;
                ///
                ///End
                ///
                //ShowMassagePopup("Ok. \'" + SKUNo.Text + "\' Item Packed.", 5000);

                //MessageDelegate mms = new MessageDelegate(ShowMassagePopup);
                //mms("Ok. \'" + SKUNo.Text + "\' Item Packed.", 3000);
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - savePackingDatilsOnly()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            
            }
        }


        private void UpdatePackingDatilsOnly(DataGridRow Row, Guid PackingDetailsID)
        {
            try
            {
                int Rowindex = Row.GetIndex();

                List<cstPackageDetails> lsPackingCustom = new List<cstPackageDetails>();
                DataGridRow row = Row;
                TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                TextBlock ProductName = grdContent.Columns[2].GetCellContent(row) as TextBlock;
                TextBlock QUantity = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                ContentPresenter sp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                DataTemplate myDataTemplate = sp.ContentTemplate;
                TextBox myTextBlock = (TextBox)myDataTemplate.FindName("gtxtBox", sp);
                //TextBlock Endtime = grdContent.Columns[7].GetCellContent(row) as TextBlock;
                //string[] _TempEndTimeUser = Endtime.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //String EndTimeUser = _TempEndTimeUser[1];

                ContentPresenter _contentPar = grdContent.Columns[8].GetCellContent(row) as ContentPresenter;
                DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                TextBlock RowNumber = (TextBlock)_dataTemplate.FindName("txtRowNumber", _contentPar);


                string sdfdsf = RowNumber.Text;
                // Custom Entity Boject of Packing Details.
                cstPackageDetails _packingC = new cstPackageDetails();
                _packingC.PackingDetailID = PackingDetailsID;
                _packingC.PackingId = Global.PackingID;
                _packingC.SKUNumber = SKUNo.Text;
                _packingC.RowNumber = Convert.ToInt32(RowNumber.Text);
                //First Sku Items Paking DateTime is Shipment Allocation Datetime.
                _packingC.PackingDetailStartDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                _packingC.SKUQuantity = Convert.ToInt32(QUantity.Text);

                _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                _packingC.ShipmentLocation = ApplicationLocation;
                _packingC.ProductName = ProductName.Text;

                //Added Columns information in packing Details.
                cstViewExtraColumns _view = Global.controller.GetViewColumnInfo(lblShipmentId.Content.ToString(), SKUNo.Text);

                _packingC.ItemName = _view.ItemName;
                _packingC.UnitOfMeasure = _view.UnitOfMeasure;
                _packingC.CountryOfOrigin = _view.CountryOfOrigin;
                _packingC.MAP_Price = _view.MAP_Price;
                _packingC.TCLCOD_0 = _view.TCLCOD_0;
                _packingC.TarrifCode = _view.TarrifCode;

                lsPackingCustom.Add(_packingC);

                ////Convert message to te applition language
                //Global.MsgBoxTitle = Global.controller.ConvetLanguage(Global.controller.SetPackingTable(_lsPacking, Global.PackingID), Global.LanguageFileName);

                //Save information inpacking detail table.
                Global.controller.UpdatePackingDetails(lsPackingCustom);

                //Hide and Show the Error Strip to animate the Error Label
                ScrollMsg("Ok. \'" + SKUNo.Text + "\' Item Packed.", Colors.Green);
                btnAddNewBox.Visibility = Visibility.Visible;
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - UpdatePackingDatilsOnly()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        private void UpdatePackingDatilsOnlyforRescanQty(DataGridRow Row, Guid PackingDetailsID, int qty)
        {
            try
            {
                int Rowindex = Row.GetIndex();

                List<cstPackageDetails> lsPackingCustom = new List<cstPackageDetails>();
                DataGridRow row = Row;
                TextBlock SKUNo = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                TextBlock ProductName = grdContent.Columns[2].GetCellContent(row) as TextBlock;
                TextBlock QUantity = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                ContentPresenter sp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                DataTemplate myDataTemplate = sp.ContentTemplate;
                TextBox myTextBlock = (TextBox)myDataTemplate.FindName("gtxtBox", sp);
              //  TextBlock Endtime = grdContent.Columns[7].GetCellContent(row) as TextBlock;
              //  string[] _TempEndTimeUser = Endtime.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
               // String EndTimeUser = _TempEndTimeUser[1];
                ContentPresenter _contentPar = grdContent.Columns[8].GetCellContent(row) as ContentPresenter;
                DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                TextBlock RowNumber = (TextBlock)_dataTemplate.FindName("txtRowNumber", _contentPar);


                string sdfdsf = RowNumber.Text;
                // Custom Entity Boject of Packing Details.
                cstPackageDetails _packingC = new cstPackageDetails();
                _packingC.PackingDetailID = PackingDetailsID;
                _packingC.PackingId = Global.PackingID;
                _packingC.SKUNumber = SKUNo.Text;
                _packingC.RowNumber = Convert.ToInt32(RowNumber.Text);

                //First Sku Items Paking DateTime is Shipment Allocation Datetime.
                _packingC.PackingDetailStartDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                //  _packingC.SKUQuantity = Convert.ToInt32(QUantity.Text);
                _packingC.SKUQuantity = qty;

                if (Global.FlgaddInBox == "FillInSelectedBox")
                {
                    _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM;
                }
                else
                {
                    _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                }



                // _packingC.BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                _packingC.ShipmentLocation = ApplicationLocation;
                _packingC.ProductName = ProductName.Text;

                //Added Columns information in packing Details.
                cstViewExtraColumns _view = Global.controller.GetViewColumnInfo(lblShipmentId.Content.ToString(), SKUNo.Text);

                _packingC.ItemName = _view.ItemName;
                _packingC.UnitOfMeasure = _view.UnitOfMeasure;
                _packingC.CountryOfOrigin = _view.CountryOfOrigin;
                _packingC.MAP_Price = _view.MAP_Price;
                _packingC.TCLCOD_0 = _view.TCLCOD_0;
                _packingC.TarrifCode = _view.TarrifCode;

                lsPackingCustom.Add(_packingC);

                ////Convert message to te applition language
                //Global.MsgBoxTitle = Global.controller.ConvetLanguage(Global.controller.SetPackingTable(_lsPacking, Global.PackingID), Global.LanguageFileName);

                //Save information inpacking detail table.
                Global.controller.UpdatePackingDetails(lsPackingCustom);

                //Hide and Show the Error Strip to animate the Error Label
                ScrollMsg("Ok. \'" + SKUNo.Text + "\' Item Packed.", Colors.Green);
                btnAddNewBox.Visibility = Visibility.Visible;
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - UpdatePackingDatilsOnlyforRescanQty()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void txtScannSKu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //Avinash: Detect the Key Pres that is scanned Code and enter at the last.
                if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
                {
                    if (txtScannSKu.Text.Contains("#"))
                    {
                        if (txtScannSKu.Text == "#addbox" || txtScannSKu.Text == "#ADDBOX")
                        {
                            txtScannSKu.Text = "";

                            if (btnAddNewBox.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAddNewBox);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not add new box.";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                mm(ss, 3000);

                            }
                        }
                            if (txtScannSKu.Text == "#modify" || txtScannSKu.Text == "#MODIFY")
                        {
                            txtScannSKu.Text = "";

                            if (btnRemoveSKuBox.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnRemoveSKuBox);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                ScrollMsg("You can not modify box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not modify box.";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                mm(ss, 3000);

                            }
                        }
                            if (txtScannSKu.Text == "#summary" || txtScannSKu.Text == "#SUMMARY")
                            {
                                txtScannSKu.Text = "";

                                if (btnSummary.IsVisible)
                                {
                                    ButtonAutomationPeer peer = new ButtonAutomationPeer(btnSummary);
                                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                    invokeProv.Invoke();
                                }
                                else
                                {
                                    ScrollMsg("Summary Can not Show", Color.FromRgb(222, 87, 24));
                                    string ss = "Summary Can not Show.";
                                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                    mm(ss, 3000);

                                }
                            }
                            if (txtScannSKu.Text == "#exitshipment" || txtScannSKu.Text == "#EXITSHIPMENT")
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
                                     ScrollMsg("Can not allow to exit", Color.FromRgb(222, 87, 24));
                                    string ss = "Can not allow to exit.";
                                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                    mm(ss, 3000);

                                }
                            }
                            txtScannSKu.Text = "";
                    }
                    else
                    {
                       // btnSummary.Visibility = Visibility.Visible;
                        btnRemoveSKuBox.Visibility = Visibility.Visible;
                        btnSummary.Visibility = Visibility.Visible;
                        //Hide the Combo text 
                        ComboWarningText.Visibility = Visibility.Hidden;

                        String _tempUPCStore = txtScannSKu.Text;
                        //Logout expire timer RE-start
                        SessionManager.StartTime();
                        ///2-12-2014  for showing correct UI when return from REmove or Delete opration when all are packed
                        ///
                        if (Global.FlagAllshippckd == "ReturnTrue")
                        {
                            tbkStatus.Text = "return from Remove or Delete opration";
                            Global.FlagAllshippckd = "flagTrue";
                        }
                        ///end
                        if (!(tbkStatus.Text == "All items of this shipment are packed. Please scan your badge to continue."))
                        {
                            Global.shipmentclosed = "False";
                            //Log
                            SaveUserLogsTbl.logThis(csteActionType.Shipment_RowScan.ToString(), _tempUPCStore);

                            if (txtScannSKu.Text != "" || txtScannSKu.Text == null)
                            {
                                //Convert the UPC Code to the Sku Name and assign it to the text Box.
                                string Str = txtScannSKu.Text.TrimStart('0').ToString();
                                //txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.UPCCode == Str).SKU.ToString();
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
                                int LineType = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.UPCCode == Str).LineType;
                                //---------

                                //Hide and Show the Error Strip to animate the Error Label
                                ScrollMsg("New item Scanned. \'" + txtScannSKu.Text + "\'", Colors.WhiteSmoke);

                                Boolean _SkuInShipment = false;
                                int _AllRowCount = grdContent.Items.Count;
                                int _EnableRowCount = 0;
                                int _SKUEnableFalseCount = 0;

                                foreach (DataGridRow row in GetDataGridRows(grdContent))
                                {
                                    TextBlock txtSKUName = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                                    TextBlock txtPckDate = grdContent.Columns[7].GetCellContent(row) as TextBlock;
                                    TextBlock txtPacked = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                                    TextBlock txtQuantity = grdContent.Columns[3].GetCellContent(row) as TextBlock;
                                    ContentPresenter _contentPar = grdContent.Columns[7].GetCellContent(row) as ContentPresenter;
                                    DataTemplate _dataTemplate = _contentPar.ContentTemplate;
                                    TextBlock txtComboNumber = (TextBlock)_dataTemplate.FindName("txtGroupID", _contentPar);
                                    TextBlock txtSKUName2 = grdContent.Columns[2].GetCellContent(row) as TextBlock;

                                    if (row.IsEnabled == true)//Row color wihte for enabled true rows
                                    {
                                        this.Dispatcher.Invoke(new Action(() => { row.Background = new SolidColorBrush(Colors.White); }));
                                        //Scroll item to view of grid view..
                                        //  this.Dispatcher.Invoke(new Action(() => { grdContent.ScrollIntoView(row.Item); }));
                                    }
                                    if (txtScannSKu.Text.ToUpper() == txtSKUName.Text.ToUpper())//check SKU is Present in Shiment
                                    {
                                        //btnAddNewBox.Visibility = Visibility.Visible;
                                        // this cosider as Sku is present
                                        _SkuInShipment = true;
                                        if (row.IsEnabled == true)
                                        {
                                            //Scroll item to view of grid view..
                                            //this.Dispatcher.Invoke(new Action(() => { grdContent.ScrollIntoView(row.Item); }));
                                            //grdContent.ScrollIntoView(row);

                                            //set caton number
                                            CartonNumber = Convert.ToInt32(txtComboNumber.Text);


                                            #region Enable true
                                            if (Convert.ToInt32(txtPacked.Text) < Convert.ToInt32(txtQuantity.Text))
                                            {
                                                grdContent.ScrollIntoView(row.Item);
                                                //if scanned item is not combo
                                                txtPacked.Text = Convert.ToString(Convert.ToInt32(txtPacked.Text) + 1);
                                                this.Dispatcher.Invoke(new Action(() =>
                                                {
                                                    row.Background = new SolidColorBrush(Color.FromRgb(106, 188, 236));//Change Back Color  blck
                                                    //if (!ISRowAutoScaned)//Update mode Flag check.
                                                    //{
                                                    //    txtPckDate.Text = "";
                                                    //    //txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + (DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss tt"));
                                                    //    txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                                                    //}
                                                    //else
                                                    //{
                                                    //    txtPckDate.Text = "";
                                                    //   // txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + (itemPackedTime.ToString("MMM dd, yyyy hh:mm:ss tt"));
                                                    //    txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(itemPackedTime, "Eastern Standard Time");
                                                    //}

                                                }));
                                                ////Save Recored to Database.
                                                if (LineType != 6)
                                                {
                                                    ///problem
                                                    //foreach (var m in lsinfo)
                                                    //{
                                                    //    if (m.Skuname == txtSKUName2.Text)
                                                    //    {
                                                    //        //chk = false;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //chk = true;
                                                    //    }
                                                    //}
                                                    ///End
                                                    if (!ISRowAutoScaned)
                                                    {
                                                        int _rowindex = row.GetIndex();
                                                        if (IsFirstTime)
                                                            this.Dispatcher.Invoke(new Action(() => { _saveButtonClick(1, row); }));
                                                        else if (IsRowPresentInColumn(lsRowAndPackingDetailsiD, _rowindex) && chk == false)
                                                            UpdatePackingDatilsOnly(row, GetGuidOfRow(lsRowAndPackingDetailsiD, _rowindex));
                                                        else
                                                            savePackingDatilsOnly(row);

                                                        btnSummary.Visibility = Visibility.Visible;
                                                        btnRemoveSKuBox.Visibility = Visibility.Visible;

                                                    }

                                                }
                                                //ComboWarningText.Visibility = Visibility.Hidden;
                                            }
                                            if (txtPacked.Text == txtQuantity.Text && row.IsEnabled == true)
                                            {
                                                grdContent.ScrollIntoView(row.Item);

                                                TextBlock txtnn = grdContent.Columns[5].GetCellContent(row) as TextBlock;
                                                ContentPresenter sp = grdContent.Columns[4].GetCellContent(row) as ContentPresenter;
                                                DataTemplate myDataTemplate = sp.ContentTemplate;
                                                Button btn = (Button)myDataTemplate.FindName("btnComplete", sp);
                                                this.Dispatcher.Invoke(new Action(() =>
                                                {
                                                    btn.Visibility = System.Windows.Visibility.Hidden;
                                                    row.Background = new SolidColorBrush(Color.FromRgb(192, 192, 192));
                                                    row.IsEnabled = false;
                                                    //// txtnn.Visibility = Visibility.Hidden;

                                                    ContentPresenter sp1 = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                                                    DataTemplate myDataTemplate12 = sp1.ContentTemplate;
                                                    TextBox myTextBlock = (TextBox)myDataTemplate12.FindName("gtxtBox", sp1);
                                                    myTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                                                    myTextBlock.Text = "Complete";
                                                    // myTextBlock.Visibility = Visibility.Hidden;



                                                }));
                                                //if (!ISRowAutoScaned)//Update mode Flag check.
                                                //{
                                                //   // txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + (DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss tt"));
                                                //    //TimeZoneInfo.ConvertTimeBySystemTimeZoneId(itemPackedTime, "Eastern Standard Time")
                                                //    txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                                                //}
                                                //else
                                                //{
                                                //   // txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + (itemPackedTime.ToString("MMM dd, yyyy hh:mm:ss tt"));
                                                //    txtPckDate.Text = lblUserName.Content + "-" + Environment.NewLine + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(itemPackedTime, "Eastern Standard Time");
                                                //}

                                                //Save Row to the Database
                                                if (!ISRowAutoScaned)//Update mode flage check
                                                {
                                                    //Add Box Button Show
                                                    btnAddNewBox.Visibility = Visibility.Visible;

                                                    //Hode Quantity Equal Barcode
                                                    ContentPresenter _contentPar1 = grdContent.Columns[7].GetCellContent(row) as ContentPresenter;
                                                    DataTemplate _dataTemplate2 = _contentPar1.ContentTemplate;
                                                    Image _imgBarcode = (Image)_dataTemplate2.FindName("imgBarCode", _contentPar1);

                                                    _imgBarcode.Visibility = Visibility.Hidden;

                                                    ////Save Recored to Database.
                                                    //if (LineType != 6)
                                                    //{
                                                    //    this.Dispatcher.Invoke(new Action(() => { _saveButtonClick(1, row); }));
                                                    //}
                                                }
                                            }
                                            txtScannSKu.Text = "";
                                            #endregion Enable true
                                        }
                                        else
                                        {
                                            _SKUEnableFalseCount = _SKUEnableFalseCount + 1;
                                        }
                                    }
                                    if (row.IsEnabled == false)
                                    {
                                        _EnableRowCount = _EnableRowCount + 1;//this count used to save function call.

                                    }
                                }//foreach end.

                                //Save call..
                                if (_EnableRowCount == _AllRowCount)
                                {
                                    //Global.FlgaddInBox = "FillInSelectedBox";
                                    btnAddNewBox.Visibility = System.Windows.Visibility.Hidden;

                                    //Avinash:4May - Display How many items remain to Pack.
                                    this.Dispatcher.Invoke(new Action(() => { showQuantityLabel(); }));
                                }
                                if (_SKUEnableFalseCount == SkuRowsInGridCount(txtScannSKu.Text.ToUpper()) && SkuRowsInGridCount(txtScannSKu.Text.ToUpper()) != 0)
                                {
                                    //Log
                                    SaveUserLogsTbl.logThis(csteActionType.ExtraProductScan__00.ToString(), _tempUPCStore);

                                    ScrollMsg("Warning: Extra Packing!\n Required quantity of this product is packed.\n Please do not pack this product. ", Color.FromRgb(222, 87, 24));
                                    MessageDelegate sm = new MessageDelegate(ShowMassagePopup);
                                    sm("Warning: Extra Packing! Required quantity of this product is packed.\n Please do not pack this product.", 4000);
                                    txtScannSKu.Text = "";
                                }
                                //if sku not found in any row 
                                if (_SkuInShipment == false)
                                {
                                    if (flgForWrongMsg == false)
                                    {
                                        //Log
                                        SaveUserLogsTbl.logThis(csteActionType.WrongProductScan_00.ToString(), _tempUPCStore);
                                        MessageDelegate sm = new MessageDelegate(ShowMassagePopup);
                                        sm("Warning: Wrong Product! \n This product is not belongs to current shipment.\n Please do not pack this product.", 4000);
                                        ScrollMsg("Warning: Wrong Product! This product is not belongs to current shipment. Please do not pack this product.", Color.FromRgb(222, 87, 24));
                                        Global.SKUName = "";
                                        txtScannSKu.Text = "";
                                    }
                                }
                                //if (Global.shipmentclosed == "true")
                                //{
                                //    btnAddNewBox.Visibility = Visibility.Hidden;
                                //}
                                //else
                                //{
                                //    btnAddNewBox.Visibility = Visibility.Visible;
                                //}
                            }
                            else
                            {
                                //Log
                                SaveUserLogsTbl.logThis(csteActionType.WrongProductScan_00.ToString(), _tempUPCStore);
                                MessageDelegate sm = new MessageDelegate(ShowMassagePopup);
                                sm("Warning: Product not found! \"" + _tempUPCStore + "\"  Wrong UPC Code. \n Please check the code again.", 4000);
                                //Strip Error Show...
                                ScrollMsg("Warning: Product not found! \"" + _tempUPCStore + "\"  Wrong UPC Code. Please check the code again.", Color.FromRgb(222, 87, 24));
                                txtScannSKu.Clear();
                            }
                        }
                        //Badge Scan Request..
                        else if (tbkStatus.Text == "All items of this shipment are packed. Please scan your badge to continue.")
                        {



                            lblblinlText.Text = "Shipment is Closed.";
                            Global.shipmentclosed = "True";
                           // MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                           // mm("All items of this shipment are packed.\n  Please scan your badge to continue ", 4000);
                            //ShowMassagePopup("All items of this shipment are packed. Please scan your badge to continue ",8000);
                            //Add Box Button disable
                            btnAddNewBox.Visibility = System.Windows.Visibility.Hidden;
                            //badge Scan request...
                            List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(txtScannSKu.Text);
                            if (lsUserInfo.Count > 0 && lsUserInfo[0].UserID == Global.LoggedUserId)
                            {
                                // save If packed Quantity equal to the order Quantity.
                                //Save package Detail End time HERE==================

                                //set Packing Status 0 From 1 in package table
                                cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                                packing.PackingStatus = 0;
                                packing.EndTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                                List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                                _lsNewPacking.Add(packing);

                                Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);

                                //Print packing Slip
                                Global.RecentlyPackedID = Global.PackingID;
                                #region Save to Carton Info table
                                ///for wrong showing Current Box added for Testing purpose only Dated 11/11/2014
                                ///for select Box Print
                                ///
                                ///
                                ///

                                Global.UnPacked = "";
                                String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                                wndBoxSlip _boxSlip = new wndBoxSlip();
                                _boxSlip.ShowDialog();
                                this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                                SaveToCartonInfo(BoxNumber, CartonNumber);

                                string BOXNumberUpdate = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                                DateTime EndTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm:ss tt ").TrimStart('0').ToString());

                                ConObj.UpdateBox(EndTime, BOXNumberUpdate);

                                ///Commented 1-12-2014
                                //if (Global.FlgaddInBox == "FillInSelectedBox")
                                //{
                                //    //if (Global.PrintBoxID != Guid.Empty && Global.PrintBoxID != null)
                                //    //{
                                //        Global.PrintBoxID = Global.selectedBoxID;
                                //        String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.selectedBoxID).BOXNUM;

                                //        //  SaveToCartonInfo(BoxNumber, CartonNumber);
                                //        if (Global.UnPacked != "" && Global.UnPacked != null)
                                //        {
                                //            Global.PrintBoxID = Global.controller.GetBoxPackageByPackingID(Global.RecentlyPackedID).Max(bx => bx.BoxID);
                                //            BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                                //        }
                                //        if (BoxNumber == Global.UnPacked)
                                //        {
                                //            Global.UnPacked = "";
                                //        }
                                //        wndBoxSlip _boxSlip = new wndBoxSlip();
                                //        _boxSlip.ShowDialog();
                                //        this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                                //        SaveToCartonInfo(BoxNumber, CartonNumber);
                                //    //}
                                //}
                                //else
                                //{
                                //    Global.PrintBoxID = Global.controller.GetBoxPackageByPackingID(Global.RecentlyPackedID).Max(bx => bx.BoxID);
                                //    String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                                //    if (BoxNumber == Global.UnPacked)
                                //    {
                                //        Global.UnPacked = "";
                                //    }
                                //    ///  SaveToCartonInfo(BoxNumber, CartonNumber);

                                //    wndBoxSlip _boxSlip = new wndBoxSlip();
                                //    _boxSlip.ShowDialog();
                                //    this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
                                //    SaveToCartonInfo(BoxNumber, CartonNumber);
                                //}
                                ///end
                                ////End


                                //    Global.PrintBoxID = Global.controller.GetBoxPackageByPackingID(Global.RecentlyPackedID).Max(bx => bx.BoxID);

                                // PrintSlip _printslip = new PrintSlip();
                                //_printslip.PrintPckingSlip(Global.PrintBoxID);



                                //wndBoxSlip _boxSlip = new wndBoxSlip();
                                //_boxSlip.ShowDialog();
                                //this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));

                                //#region Save to Carton Info table

                                //String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;

                                ///save to carton number table
                                //SaveToCartonInfo(BoxNumber, CartonNumber);
                                #endregion

                                ScrollMsg("Shipment Packed. Shipment ID = " + Global.ShippingNumber, Color.FromRgb(38, 148, 189));

                                #region Clear Global Veriables
                                //clear all Global Veriabels
                                // Global.RecentlyPackedID = Guid.Empty;
                                Global.Mode = "";
                                Global.ShippingNumber = "";
                                Global.ManagerID = Guid.Empty;
                                Global.ManagerName = "";
                              //  Global.PackingID = Guid.Empty;
                                Global.SameUserpackingID = Guid.Empty;

                                #endregion


                                //save shipment Number fot WayFair.
                                Global.ShippingNumber = lblShipmentId.Content.ToString();


                                MainWindow Shipmentwnd = new MainWindow();
                                Shipmentwnd.Show();
                                this.Close();

                                if (Global.LoginType == "LTL")
                                {
                                    if (Global.CartonPallet == "Carton")
                                    {
                                        wndBoxInfo _wayfairBox = new wndBoxInfo();
                                        _wayfairBox.ShowDialog();
                                    }
                                    else if (Global.CartonPallet == "Pallet")
                                    {
                                        wndBoxInfoForPallet _AmazoneBox = new wndBoxInfoForPallet();
                                        _AmazoneBox.ShowDialog();
                                    }
                                    //wndBoxInfo _wayfairBox = new wndBoxInfo();
                                    //_wayfairBox.Show();
                                }


                            }
                            else
                            {
                                txtScannSKu.Text = "";
                                //Strip Error Show...
                                //Log
                                SaveUserLogsTbl.logThis(csteActionType.Login_Invalid_User_00.ToString(), _tempUPCStore);
                                ScrollMsg("Warning: Incorrect User Scan! \n Please scan your badge again.", Color.FromRgb(222, 87, 24));
                            }
                        }
                    }
                    //Avinash:4May - Display How many items remain to Pack.
                    // This Function must be execute after all operation.
                    showQuantityLabel();
                   
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ////ErrorLoger.save("wndShipmentDetailPage - txtScannSKu_KeyDown", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndShipmentDetailPage - txtScannSKu_KeyDown", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                //Strip Error Show...
                //Log
                SaveUserLogsTbl.logThis(csteActionType.WrongProductScan_00.ToString(), txtScannSKu.Text);
                MessageDelegate sm = new MessageDelegate(ShowMassagePopup);
                sm("Warning: Product not found!\n Wrong UPC Code.\n  Please check the code again", 4000);
                ScrollMsg("Warning: Product not found!\n Wrong UPC Code.\nPlease check the code again.", Color.FromRgb(222, 87, 24));
                txtScannSKu.Clear();
            }
        }

        /// <summary>
        /// Count number of rows repeted in DataGrid 
        /// </summary>
        /// <param name="SKUName">Sku Name To be check</param>
        /// <returns>int value count</returns>
        public int SkuRowsInGridCount(String SKUName)
        {
            int Count = 0;
            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContent))
                {
                    TextBlock txtSkuName = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                    if (SKUName == txtSkuName.Text.ToUpper())
                    {
                        Count = Count + 1;
                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                //ErrorLoger.save("wndShipmentDetailPage - txtScannSKu_KeyDown", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndShipmentDetailPage - SkuRowsInGridCount()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            return Count;
        }

        private void btnExitShipment_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Global.UnPacked != "" && Global.UnPacked != null)
                {
                    ShowMassagePopup("Please you have to Pack this Box \n" + Global.UnPacked + " First", 4000);
                   ///// ShowMassagePopup("If you choose to exit, \n your open box  " + Global.UnPacked + " is deleted.", 4000);
                   // ShowMassagePopup("If you choose to exit, \n your open box is deleted.", 4000);
                    txtScannSKu.Focus();
                }
                else
                {
                    //Log
                    SaveUserLogsTbl.logThis(csteActionType.Shipment_ForceExit__00.ToString(), lblShipmentId.Content.ToString());

                    MsgBox.Show("Error", "Exit", "Exiting shipment will clear this shipment information." + Environment.NewLine + "Are you sure want to exit shipment?");
                    if (Global.MsgBoxResult == "Ok")
                    {
                        Global.Mode = "";
                        Global.ShippingNumber = "";
                        Global.ManagerID = Guid.Empty;
                        Global.ManagerName = "";
                        Global.PackingID = Guid.Empty;
                        MainWindow _main = new MainWindow();
                        _main.Show();
                        this.Close();
                    }
                    else
                    {
                        txtScannSKu.Focus();
                    }
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - btnExitShipment_Click", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        ////Function For Showing New Message Box 25-11-2014

        public void ShowMassagePopup(string message, int TimeSpan)
        {
            try
            {
                // brdMessage.Background = new SolidColorBrush(Colors.Black);
                //brdfrist.Background = new SolidColorBrush(Colors.Black);
                //  brdMessage.Opacity = 0.5;

                lblmessage.Content = message;
                //brdMessage.Background = new SolidColorBrush(Colors.Black);
                //brdMessage.Opacity = 0.5;
                brdMessage2.Visibility = System.Windows.Visibility.Visible;

                dtLoadUpdate = new DispatcherTimer();
                dtLoadUpdate.Interval = new TimeSpan(0, 0, 0, 0, TimeSpan);
                dtLoadUpdate.Tick += dtLoadUpdate_Tick;
                //start the dispacher.
                dtLoadUpdate.Start();
                //txtScannSKu.Focus();
            }
            catch (Exception ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - ShowMassagePopup()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        private void dtLoadUpdate_Tick(object sender, EventArgs e)
        {
            //  brdfrist.Background = new SolidColorBrush(Colors.White);
            ////brdfrist.Background = new SolidColorBrush(Colors.Black);
            //  brdfrist.BorderThickness = new Thickness(4,0,0, 0);
            ///   brdMessage.Opacity = 1;
            //    brdMessage.Background = new SolidColorBrush(Colors.Transparent);
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate.Stop();
            if (IsManualEntry == false)
            {
                txtScannSKu.Focus();
            }
            else
            {
                txtQunatity.Focus();
            }
        }
        ////End





        /// <summary>
        /// Check shipment is allocated for multiple locations 
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>int value of Box respect to how may locations already packed this shipment</returns>
        public int MultilocationShipmentPacked(String ShipmentID)
        {
            int _returnBox = 1;
            try
            {
                if (Global.controller.ISShipMultiLocationExist(Global.ShippingNumber.ToString()) == true)
                {
                    List<cstShipmentLocationWise> lsShipmentLocation = Global.controller.ShipmentLocationList(Global.ShippingNumber.ToString());
                    foreach (var _Lovationitem in lsShipmentLocation)
                    {
                        List<cstPackageTbl> _lsPackig = Global.controller.GetPackingList(Global.ShippingNumber.ToString(), _Lovationitem.LocationName.ToString());
                        if (_lsPackig.Count > 0 && _lsPackig[0].ShipmentLocation != ApplicationLocation)
                        {
                            _returnBox++;
                        }
                    }

                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - MultilocationShipmentPacked()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            return _returnBox;
        }

        private void gtxtBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //TextBox txtBox = (TextBox)e.Source;
                //txtBox.Text = BoxQuantity.ToString();


                //foreach (DataGridRow row in GetDataGridRows(grdContent))
                //{
                //    if (row.IsEnabled && grdContent.Items.Count > 1)
                //    {
                //        try
                //{
                //ContentPresenter cp = grdContent.Columns[6].GetCellContent(row) as ContentPresenter;
                //DataTemplate myDataTemplate = cp.ContentTemplate;
                //TextBox t = (TextBox)myDataTemplate.FindName("gtxtBox", cp);
                //t.Text = (Convert.ToInt32(t.Text) + 1).ToString();
                //TextBlock QUantity = grdContent.Columns[3].GetCellContent(e) as TextBlock;
                //            TextBlock pckdQUantity = grdContent.Columns[4].GetCellContent(e) as TextBlock;

                //            if (QUantity.Text == pckdQUantity.Text)
                //            {
                //                txtBox.Text = "";
                //            }
                //            else
                //            {
                //                txtBox.Text = "in progress";
                //            }



                //}
                //catch (Exception)
                //{ }
                //}
                //}
                // inprogress();
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - gtxtBox_Loaded", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        #region Scroll message set
        public void ScrollMsg(string Message, Color clr)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                   {
                       BErrorMsg.Visibility = System.Windows.Visibility.Hidden;
                       BErrorMsg.Visibility = System.Windows.Visibility.Visible;
                       lblErrorMsg.Foreground = new SolidColorBrush(clr);
                       //lblErrorMsg.Text = "SKU Scan -[" + (DateTime.UtcNow.ToString("hh:mm:ss tt")) + "]▶ " + Message.ToString();
                    
                           lblErrorMsg.Text = "SKU Scan -[" + (  TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" ) )+ "]▶ " + Message.ToString();
                       txtblStack.Inlines.Add(new Run { Text = lblErrorMsg.Text + Environment.NewLine, Foreground = new SolidColorBrush(clr) });
                      // Global.lsScroll.Add(new Run { Text = lblErrorMsg.Text + Environment.NewLine, Foreground = new SolidColorBrush(clr) });

                       lsScroll1.Add(new Run { Text = lblErrorMsg.Text + Environment.NewLine, Foreground = new SolidColorBrush(clr) });

                       svStack.ScrollToBottom();
                   }));
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - ScrollMsg()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        //Load Previous Scroll messages in Sroll messgae stack of this page.
        private void _showListStrings()
        {
            try
            {
                foreach (Run r in lsScroll1)
                {
                    txtblStack.Inlines.Add(r);
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - _showListStrings()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        #endregion

        private void grdContent_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtScannSKu.Focus();

                // nw.RaiseCustomEvent5 += new EventHandler<wndModifyPopup.myEventArgs>(nw_raiseevnt5);

                //if (grdContent.SelectedIndex != -1)
                //{
                // int selectedIndex = grdContent.SelectedIndex;

                //DataGridCell cell = GetCell(1, 7);
                //ContentPresenter CntPersenter = cell.Content as ContentPresenter;
                //DataTemplate DataTemp = CntPersenter.ContentTemplate;
                //TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);

                //    txtReturnGuid.Text = "HI";

                //}
                //TextAutomationPeer peer = new TextAutomationPeer(txtReturnGuid);
                //IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                //invokeProv.Invoke();

                //var key = Key.Enter;                    // Key to send
                //var target = txtReturnGuid;   // Target element
                //var routedEvent = Keyboard.KeyDownEvent; // Event to send
                //target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                // System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - grdContent_GotFocus", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                
            }


        }

        /// <summary>
        /// Hide Barcode column from the Grid
        /// </summary>
        private void _hideBarcodes()
        {
            try
            {
                grdContent.Columns[7].Width = 0;
                grdContent.Columns[7].Header = "";
                grdContent.Columns[8].Width = 490;
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ////ErrorLoger.save("wndShipmentDetailPage - _hideBarcodes", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndShipmentDetailPage - _hideBarcodes()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void svStack_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtScannSKu.Focus();
                txtScannSKu.Focusable = true;
            }
            catch (Exception ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - svStack_GotFocus", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        /// <summary>
        /// save carton information with default parameter.
        /// </summary>
        /// <param name="BoxNumber"></param>
        /// <param name="CartonNumber"></param>
        /// <returns></returns>
        private Guid SaveToCartonInfo(String BoxNumber, int CartonNumber = 0)
        {
           
            
                cstCartonInfo _Carton = new cstCartonInfo();
                _Carton.CartonID = Guid.NewGuid();
                _Carton.BOXNumber = BoxNumber;
                _Carton.ShipmentNumber = lblShipmentId.Content.ToString();
                _Carton.CartonNumber = CartonNumber;
                _Carton.Printed = 0;

                return Global.controller.SaveCartonInfo(_Carton);
           
        }


        /// <summary>
        /// Check that row is previously save in database.
        /// </summary>
        /// <param name="lsKayVal"></param>
        /// <param name="RowIndex"></param>
        /// <returns></returns>
        private bool IsRowPresentInColumn(List<KeyValuePair<int, Guid>> lsKayVal, int RowIndex)
        {
            Boolean _return = false;
            foreach (var item in lsRowAndPackingDetailsiD)
            {
                if (item.Key == RowIndex)
                    _return = true;
            }
            return _return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsKayVal"></param>
        /// <param name="RowIndex"></param>
        /// <returns></returns>
        private Guid GetGuidOfRow(List<KeyValuePair<int, Guid>> lsKayVal, int RowIndex)
        {
            Guid _return = Guid.Empty;
            foreach (var item in lsRowAndPackingDetailsiD)
            {
                if (item.Key == RowIndex)
                    _return = item.Value;
            }
            return _return;
        }

        private void btnRemoveSKuBox_Click_1(object sender, RoutedEventArgs e)
        {
            //wndRemoveSKUFromBox Remove = new wndRemoveSKUFromBox();
            //Remove.ShowDialog();
            //
            //int co = ConObj.GetPackingDetailTbl(Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM).Count;

            //if (ConObj.GetPackingDetailTbl(Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM).Count == 0)
            //{
            //    ShowMassagePopup("This box is empty,can't modified.", 2000);
            //}
            //else
            //{





            try
            {
                List<cstPackageDetails> _lsPacking = new List<cstPackageDetails>();
                _lsPacking = ConObj.GetPackingDetailTbl(lblCurrentBox.Content.ToString());
                if (_lsPacking.Count==0)
                {
                    Global.CheckRemoveButton = true;
                }

                Global.previouscurrentbox = lblCurrentBox.Content.ToString();
                wndModifyPopup nw = new wndModifyPopup();
                nw.RaiseCustomEvent += new EventHandler<wndModifyPopup.myEventArgs>(nw_raiseevnt);
                nw.RaiseCustomEvent3 += new EventHandler<wndModifyPopup.myEventArgs3>(nw_raiseevnt3);
                //wndRemoveSKUFromBox windowremove = new wndRemoveSKUFromBox();
                //windowremove.RaiseCustomEvent1 += new EventHandler<wndRemoveSKUFromBox.myEventArgs1>(nw_raiseevnt1);

                nw.ShowDialog();

                txtScannSKu.Focus();

                //   }
                // wndModifyPopup poup = new wndModifyPopup();
                // poup.Owner = this;
                //// this.Hide();
                // poup.ShowDialog();
                //  txtScannSKu.Focus();
                //this.Close();
            }
            catch (Exception d)
            {
                ShowMassagePopup("" + d.Message, 2000);
                ErrorLoger.save("wndShipmentDetailPage - btnRemoveSKuBox_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + d.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void grdContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mbox_cancel(object sender, RoutedEventArgs e)
        {
            try
            {
                /// brdMessage.Opacity = 1;
                ///  brdMessage.Background = new SolidColorBrush(Colors.Transparent);
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
            }
            catch(Exception ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - mbox_cancel", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            try
            {
                 /// brdMessage.Opacity = 1;
            /// brdMessage.Background = new SolidColorBrush(Colors.Transparent);
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate.Stop();
            txtScannSKu.Focus();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - mbox_ok", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //PackingNet.Pages.ReportPages.ReportForm ReportObj = new PackingNet.Pages.ReportPages.ReportForm();
                //ReportObj.ShowDialog();
                wndSummaryReports reports = new wndSummaryReports();
                reports.ShowDialog();
                txtScannSKu.Focus();
                //wndModifyPopup nw = new wndModifyPopup();
                //nw.RaiseCustomEvent += new EventHandler<wndModifyPopup.myEventArgs>(nw_raiseevnt);
                //nw.Show();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - Button_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

        private void nw_raiseevnt(object sender, wndModifyPopup.myEventArgs e)
        {
            try
            {
                ReportController r = new ReportController();
                if (r.GetPackingDetailsByBoxNumForReport(Global.ShippingNumber).Count == 0)
                {
                    btnRemoveSKuBox.Visibility = Visibility.Hidden;
                    btnSummary.Visibility = Visibility.Hidden;

                }
                else
                {
                    btnAddNewBox.Visibility = Visibility.Visible;
                    btnRemoveSKuBox.Visibility = Visibility.Visible;
                    btnSummary.Visibility = Visibility.Visible;
                }
                if (ConObj.GetPackingDetailTbl(Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM).Count == 0)
                {
                    //btnAddNewBox.Visibility = Visibility.Hidden;
                    btnAddNewBox.Visibility = Visibility.Hidden;
                    if (Global.FromRemoveFlag == true)
                    {
                        //ScrollMsg("Your Box = " + Global.PrintBoxID + " is opened. \n Please Scan SKU. \n Otherwise Delete open Box", Color.FromRgb(38, 148, 189));
                        ShowMassagePopup("Please pack current Box " + Global.previouscurrentbox + "  \n Otherwise Delete it.", 10000);
                    }
                    else if (Global.FromRemoveFlag == false)
                    {

                    }
                }

                //if (Global.flgChkdeletebx == false)
                //    ShowMassagePopup("Some of Boxes are not deleted,\n beacause They have Tracking Numbers", 100000);

                // lblblinlText.Text=
                this.lblCurrentBox.Content = e.massage;
                List<cstShipment> shipment1 = new List<cstShipment>();

                /////For Testing Admin seeboth location skus 24-12-2014
                List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
                var rolnm = (from v in lsUserInfo
                             where v.UserID == Global.LoggedUserId
                             select v.RoleName).FirstOrDefault();
                if (Global.controller.IsSuperUser(Global.LoggedUserId))
                {
                    CobmoIDGenrator _generate = new CobmoIDGenrator();
                  
                    shipment1 = Global.controller.GetShipment_SPCKD(Global.ShippingNumber, true);
                    shipment1 = _generate.SetComboNumbers(shipment1);
                }
                else
                {
                    CobmoIDGenrator _generate = new CobmoIDGenrator();
                  
                    shipment1 = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);
                    shipment1 = _generate.SetComboNumbers(shipment1);
                }
                grdContent.ItemsSource = "";
                this.Dispatcher.Invoke(new Action(() => { grdContent.ItemsSource = shipment1; }));

                cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                packing.PackingStatus = 1;
                List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                _lsNewPacking.Add(packing);

                Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);

                lblblinlText.Text = Global.pkdStatus;
                tbkStatus.Text = "Status";
                // Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
                flgForWrongMsg = true;
                timer2 = new DispatcherTimer();
                timer2.Tick += timer2_Tick;
                timer2.Interval = new TimeSpan(0, 0, 0, 1);
                timer2.Start();
                _sameUserRepacking();
                txtScannSKu.Focus();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - nw_raiseevnt", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

            //  this.Dispatcher.Invoke(new Action(() => { BoldFontandHideCombp(Bindedshipment); }));
        }
        private void nw_raiseevnt3(object sender, wndModifyPopup.myEventArgs3 e)
        {
            try
            {
                grdContent.ItemsSource = "";
                ReportController r = new ReportController();
                if (r.GetPackingDetailsByBoxNumForReport(Global.ShippingNumber).Count == 0)
                {
                    btnRemoveSKuBox.Visibility = Visibility.Hidden;
                    btnSummary.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnRemoveSKuBox.Visibility = Visibility.Visible;
                    btnSummary.Visibility = Visibility.Visible;
                }
                ////int co = ConObj.GetPackingDetailTbl(Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM).Count;
                if (ConObj.GetPackingDetailTbl(Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM).Count == 0)
                {
                    btnAddNewBox.Visibility = Visibility.Hidden;
                }


                lblCurrentBox.Content = Global.BoxActive;
                this.lblCurrentBox.Content = e.massage;
                List<cstShipment> shipment1 = new List<cstShipment>();
                /////For Testing Admin seeboth location skus 24-12-2014
                List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
                var rolnm = (from v in lsUserInfo
                             where v.UserID == Global.LoggedUserId
                             select v.RoleName).FirstOrDefault();
                if (Global.controller.IsSuperUser(Global.LoggedUserId))
                {
                    CobmoIDGenrator _generate = new CobmoIDGenrator();
                  
                    shipment1 = Global.controller.GetShipment_SPCKD(Global.ShippingNumber, true);
                    shipment1 = _generate.SetComboNumbers(shipment1);
                }
                else
                {
                    CobmoIDGenrator _generate = new CobmoIDGenrator();
                 
                    shipment1 = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);
                    shipment1 = _generate.SetComboNumbers(shipment1);
                }
                this.Dispatcher.Invoke(new Action(() => { grdContent.ItemsSource = shipment1; }));

                cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                packing.PackingStatus = 1;
                List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                _lsNewPacking.Add(packing);

                Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);
                lblblinlText.Text = Global.pkdStatus;
                tbkStatus.Text = "Status";
                // Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
                flgForWrongMsg = true;
                timer2 = new DispatcherTimer();
                timer2.Tick += timer2_Tick;

                timer2.Interval = new TimeSpan(0, 0, 0, 1);
                timer2.Start();
                //////Global.SameUserpackingID = Global.PackingID;
                _sameUserRepacking();
                txtScannSKu.Focus();

              //  if (Global.newWindowThread.IsAlive)
             //   {
             //       Global.newWindowThread.Abort();
             //   }




                //  this.Dispatcher.Invoke(new Action(() => { BoldFontandHideCombp(Bindedshipment); }));
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - nw_raiseevnt3", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        void timer2_Tick(object sender, EventArgs e)
        {
          
            Global.SameUserpackingID = Global.PackingID;
            //List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
            //var rolnm = (from v in lsUserInfo
            //             where v.UserID == Global.LoggedUserId
            //             select v.RoleName).FirstOrDefault();
            //if (Global.controller.IsSuperUser(Global.LoggedUserId) == false || rolnm.ToString() != "Manager")
            //{
          
            //}
            timer2.Stop();
             //if (Global.newWindowThread.IsAlive)
             //   {
             //       Global.newWindowThread.Abort();
             //   }
        }

        private void txtEnterQty_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void txtEnterQty_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    grdContent.ScrollIntoView(rowContainer, grdContent.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)grdContent.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                grdContent.UpdateLayout();
                grdContent.ScrollIntoView(grdContent.Items[index]);
                row = (DataGridRow)grdContent.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        int selectedIndex;

        private void btnadd_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedIndex = grdContent.SelectedIndex;
                IsManualEntry = true;
                //   DataGridCell cell = GetCell(selectedIndex, 7);

                //DataGridRow rowContainer = GetRow(selectedIndex);

                ////TextBox RMAStatus = (TextBox)grdContent.Columns[7].GetCellContent(rowContainer) as TextBox;

                //ContentPresenter CntPersenter = grdContent.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
                //DataTemplate DataTemp = CntPersenter.ContentTemplate;
                //TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);
                //dtLoadUpdate.Stop();
                popup.IsOpen = true;
                btnAdd.Visibility = Visibility.Hidden;
                txtQunatity.Focus();

                //  txtReturnGuid.Text = "HI SHRIRAM RAJARAM";
                // txtReturnGuid.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - btnadd_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
             
        }

        private void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                #region
                if (e.Key == Key.Enter && txtQunatity.Text.Trim() != "")
                {
                    btnAdd.Visibility = Visibility.Visible;
                    IsManualEntry = false;
                    if (txtQunatity.Text.Contains("#"))
                    {
                        if (txtQunatity.Text.StartsWith("#"))
                        {
                            if (txtQunatity.Text.Trim() != "#close" && txtQunatity.Text.Trim() != "#CLOSE" && txtQunatity.Text.Trim() != "#submit" && txtQunatity.Text.Trim() != "#SUBMIT")
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }
                            else if (txtQunatity.Text.Trim() == "#close" || txtQunatity.Text.Trim() == "#CLOSE" || txtQunatity.Text.Trim() == "#submit" || txtQunatity.Text.Trim() == "#SUBMIT")
                            {
                                if (txtQunatity.Text == "#close" || txtQunatity.Text == "#CLOSE")
                                {
                                    txtQunatity.Text = "";
                                    popup.IsOpen = false;
                                    txtScannSKu.Focus();
                                }

                                if (txtQunatity.Text == "#submit" || txtQunatity.Text == "#SUBMIT")
                                {
                                    popup.IsOpen = false;
                                    MessageBox.Show("Invalid Quantity.");
                                    popup.IsOpen = true;
                                    txtQunatity.Text = "";
                                    txtQunatity.Focus();
                                    btnAdd.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                        else
                        {
                            // Imaitem.Split(new char[] { '\\' }).Last().ToString()
                            // string[] ReturnReasons = retuen.ReturnReason.Split(new char[] { '.' });
                            // string split = "";
                            try
                            {
                                string split = txtQunatity.Text.Split(new char[] { '#' })[0];

                                string Check = txtQunatity.Text.Split(new char[] { '#' })[1];




                                if (Check == "submit" || Check == "SUBMIT")
                                {
                                    #region GreaterQtyChk
                                    int cheeee = Convert.ToInt32(split);
                                    DataGridRow rowContainer = GetRow(selectedIndex);

                                    //TextBox RMAStatus = (TextBox)grdContent.Columns[7].GetCellContent(rowContainer) as TextBox;

                                    ContentPresenter CntPersenter = grdContent.Columns[8].GetCellContent(rowContainer) as ContentPresenter;
                                    DataTemplate DataTemp = CntPersenter.ContentTemplate;
                                    TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);

                                    txtReturnGuid.Visibility = Visibility.Visible;

                                    //ContentPresenter CntPersenter1 = grdContent.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
                                    //DataTemplate DataTemp1 = CntPersenter.ContentTemplate;
                                    //TextBox txtReturnGuid1 = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter1);

                                    TextBlock txtSku = grdContent.Columns[1].GetCellContent(rowContainer) as TextBlock;

                                    TextBlock txtQty = grdContent.Columns[3].GetCellContent(rowContainer) as TextBlock;
                                    TextBlock txtPkd = grdContent.Columns[5].GetCellContent(rowContainer) as TextBlock;


                                    int chk = 0;
                                    chk = Convert.ToInt32(txtPkd.Text) + Convert.ToInt32(split);
                                    if (Convert.ToInt32(txtQty.Text) >= chk && Convert.ToInt32(split) != 0)
                                    {
                                        txtReturnGuid.Text = split;
                                        try
                                        {

                                            for (int i1 = 0; i1 < Convert.ToInt16(txtReturnGuid.Text); i1++)//Loop for quantity
                                            {
                                                //Set Auto Sacannig Flag true. 
                                                ISRowAutoScaned = false;

                                                //Set text of textbox to upc code.
                                                txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == txtSku.Text).UPCCode;

                                                //Set Previously packed time of item to show in the Grid.
                                                //itemPackedTime = _PackItem.PackingDetailStartDateTime;
                                                //////itemPackedTime =Convert.ToDateTime( detail);
                                                //Set Previously packed time as packing time. for package table
                                                StartTime = _shipment.PackingInfo[0].StartTime;

                                                var key = Key.Enter;                    // Key to send
                                                var target = txtScannSKu;   // Target element
                                                var routedEvent = Keyboard.KeyDownEvent; // Event to send
                                                target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                                                 System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
                                                //if (Global.shipmentclosed == "true")
                                                //    btnAddNewBox.Visibility = Visibility.Hidden;
                                                //else
                                                //    btnAddNewBox.Visibility = Visibility.Visible;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            txtScannSKu.Focus();
                                        }

                                        popup.IsOpen = false;

                                        txtQunatity.Text = "";

                                        txtScannSKu.Focus();
                                    }
                                    else
                                    {
                                        popup.IsOpen = false;
                                        MessageBox.Show("Invalid Quantity.");
                                        popup.IsOpen = true;
                                        txtQunatity.Text = "";
                                        txtQunatity.Focus();
                                        btnAdd.Visibility = Visibility.Hidden;
                                    }

                                    #endregion
                                }
                                else if (Check == "close" || Check == "CLOSE")
                                {
                                    txtQunatity.Text = "";
                                    popup.IsOpen = false;
                                    txtScannSKu.Focus();
                                }
                                else
                                {
                                    popup.IsOpen = false;
                                    MessageBox.Show("Invalid Quantity.");
                                    popup.IsOpen = true;
                                    txtQunatity.Text = "";
                                    txtQunatity.Focus();
                                    btnAdd.Visibility = Visibility.Hidden;
                                }
                            }
                            catch (Exception)
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }





                        }
                    }
                    else
                    {
                        try
                        {

                            #region GreaterQtyChk

                            DataGridRow rowContainer = GetRow(selectedIndex);

                            //TextBox RMAStatus = (TextBox)grdContent.Columns[7].GetCellContent(rowContainer) as TextBox;

                            ContentPresenter CntPersenter = grdContent.Columns[8].GetCellContent(rowContainer) as ContentPresenter;
                            DataTemplate DataTemp = CntPersenter.ContentTemplate;
                            TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);

                            txtReturnGuid.Visibility = Visibility.Visible;

                            //ContentPresenter CntPersenter1 = grdContent.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
                            //DataTemplate DataTemp1 = CntPersenter.ContentTemplate;
                            //TextBox txtReturnGuid1 = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter1);

                            TextBlock txtSku = grdContent.Columns[1].GetCellContent(rowContainer) as TextBlock;

                            TextBlock txtQty = grdContent.Columns[3].GetCellContent(rowContainer) as TextBlock;
                            TextBlock txtPkd = grdContent.Columns[5].GetCellContent(rowContainer) as TextBlock;


                            int chk = 0;
                            chk = Convert.ToInt32(txtPkd.Text) + Convert.ToInt32(txtQunatity.Text);
                            if (Convert.ToInt32(txtQty.Text) >= chk && Convert.ToInt32(txtQunatity.Text.Trim()) != 0 && Convert.ToInt32(txtQunatity.Text.Trim()) > 0)
                            {
                                txtReturnGuid.Text = txtQunatity.Text;
                                try
                                {

                                    for (int i1 = 0; i1 < Convert.ToInt16(txtReturnGuid.Text); i1++)//Loop for quantity
                                    {
                                        //Set Auto Sacannig Flag true. 
                                        ISRowAutoScaned = false;

                                        //Set text of textbox to upc code.
                                        txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == txtSku.Text).UPCCode;

                                        //Set Previously packed time of item to show in the Grid.
                                        //itemPackedTime = _PackItem.PackingDetailStartDateTime;
                                        //////itemPackedTime =Convert.ToDateTime( detail);
                                        //Set Previously packed time as packing time. for package table
                                        StartTime = _shipment.PackingInfo[0].StartTime;

                                        var key = Key.Enter;                    // Key to send
                                        var target = txtScannSKu;   // Target element
                                        var routedEvent = Keyboard.KeyDownEvent; // Event to send
                                        target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                                         System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
                                        //if (Global.shipmentclosed == "true")
                                        //    btnAddNewBox.Visibility = Visibility.Hidden;
                                        //else
                                        //    btnAddNewBox.Visibility = Visibility.Visible;
                                    }
                                }
                                catch (Exception)
                                {
                                    txtScannSKu.Focus();
                                }

                                popup.IsOpen = false;

                                txtQunatity.Text = "";

                                txtScannSKu.Focus();
                            }
                            else
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                IsManualEntry = true;
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }

                            #endregion


                        }
                        catch (Exception)
                        {
                            popup.IsOpen = false;
                            MessageBox.Show("Invalid Quantity.");
                            IsManualEntry = true;
                            popup.IsOpen = true;
                            txtQunatity.Text = "";
                            txtQunatity.Focus();
                            btnAdd.Visibility = Visibility.Hidden;
                        }

                    }
                }
                //else
                //{
                //    popup.IsOpen = false;
                //    MessageBox.Show("Invalid Quantity.");
                //    popup.IsOpen = true;
                //    txtQunatity.Text = "";
                //}
                #endregion


                //if (txtQunatity.Text.Trim() != "#close" && txtQunatity.Text.Trim() != "#CLOSE" && txtQunatity.Text.Trim() != "#submit" && txtQunatity.Text.Trim() != "#SUBMIT")
                //{



                //}
                //else
                //{
                //    MessageBox.Show("Invalid");
                //}

                // }
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - UpdatePackingDatilsOnly", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
               
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                IsManualEntry = false;
                txtQunatity.Text = "";
                popup.IsOpen = false;
                txtScannSKu.Focus();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - btnClose_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

        private void btnAdd_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                IsManualEntry = false;
                #region
                if (txtQunatity.Text.Trim() != "")
                {
                    if (txtQunatity.Text.Contains("#"))
                    {
                        if (txtQunatity.Text.StartsWith("#"))
                        {
                            if (txtQunatity.Text.Trim() != "#close" && txtQunatity.Text.Trim() != "#CLOSE" && txtQunatity.Text.Trim() != "#submit" && txtQunatity.Text.Trim() != "#SUBMIT")
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                IsManualEntry = true;
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }
                            else if (txtQunatity.Text.Trim() == "#close" || txtQunatity.Text.Trim() == "#CLOSE" || txtQunatity.Text.Trim() == "#submit" || txtQunatity.Text.Trim() == "#SUBMIT")
                            {
                                if (txtQunatity.Text == "#close" || txtQunatity.Text == "#CLOSE")
                                {
                                    txtQunatity.Text = "";
                                    popup.IsOpen = false;
                                    txtScannSKu.Focus();

                                }

                                if (txtQunatity.Text == "#submit" || txtQunatity.Text == "#SUBMIT")
                                {
                                    popup.IsOpen = false;
                                    MessageBox.Show("Invalid Quantity.");
                                    IsManualEntry = true;
                                    popup.IsOpen = true;
                                    txtQunatity.Text = "";
                                    txtQunatity.Focus();
                                    btnAdd.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                        else
                        {
                            // Imaitem.Split(new char[] { '\\' }).Last().ToString()
                            // string[] ReturnReasons = retuen.ReturnReason.Split(new char[] { '.' });
                            // string split = "";
                            try
                            {
                                string split = txtQunatity.Text.Split(new char[] { '#' })[0];

                                string Check = txtQunatity.Text.Split(new char[] { '#' })[1];




                                if (Check == "submit" || Check == "SUBMIT")
                                {
                                    #region GreaterQtyChk
                                    int cheeee = Convert.ToInt32(split);
                                    DataGridRow rowContainer = GetRow(selectedIndex);

                                    //TextBox RMAStatus = (TextBox)grdContent.Columns[7].GetCellContent(rowContainer) as TextBox;

                                    ContentPresenter CntPersenter = grdContent.Columns[8].GetCellContent(rowContainer) as ContentPresenter;
                                    DataTemplate DataTemp = CntPersenter.ContentTemplate;
                                    TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);

                                    txtReturnGuid.Visibility = Visibility.Visible;

                                    //ContentPresenter CntPersenter1 = grdContent.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
                                    //DataTemplate DataTemp1 = CntPersenter.ContentTemplate;
                                    //TextBox txtReturnGuid1 = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter1);

                                    TextBlock txtSku = grdContent.Columns[1].GetCellContent(rowContainer) as TextBlock;

                                    TextBlock txtQty = grdContent.Columns[3].GetCellContent(rowContainer) as TextBlock;
                                    TextBlock txtPkd = grdContent.Columns[5].GetCellContent(rowContainer) as TextBlock;


                                    int chk = 0;
                                    chk = Convert.ToInt32(txtPkd.Text) + Convert.ToInt32(split);
                                    if (Convert.ToInt32(txtQty.Text) >= chk && Convert.ToInt32(split) != 0)
                                    {
                                        txtReturnGuid.Text = split;
                                        try
                                        {

                                            for (int i1 = 0; i1 < Convert.ToInt16(txtReturnGuid.Text); i1++)//Loop for quantity
                                            {
                                                //Set Auto Sacannig Flag true. 
                                                ISRowAutoScaned = false;

                                                //Set text of textbox to upc code.
                                                txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == txtSku.Text).UPCCode;

                                                //Set Previously packed time of item to show in the Grid.
                                                //itemPackedTime = _PackItem.PackingDetailStartDateTime;
                                                //////itemPackedTime =Convert.ToDateTime( detail);
                                                //Set Previously packed time as packing time. for package table
                                                StartTime = _shipment.PackingInfo[0].StartTime;

                                                var key = Key.Enter;                    // Key to send
                                                var target = txtScannSKu;   // Target element
                                                var routedEvent = Keyboard.KeyDownEvent; // Event to send
                                                target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                                                 System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
                                                //if (Global.shipmentclosed == "true")
                                                //    btnAddNewBox.Visibility = Visibility.Hidden;
                                                //else
                                                //    btnAddNewBox.Visibility = Visibility.Visible;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            txtScannSKu.Focus();
                                        }

                                        popup.IsOpen = false;

                                        txtQunatity.Text = "";

                                        txtScannSKu.Focus();
                                    }
                                    else
                                    {
                                        popup.IsOpen = false;
                                        MessageBox.Show("Invalid Quantity.");
                                        IsManualEntry = true;
                                        popup.IsOpen = true;
                                        txtQunatity.Text = "";
                                        txtQunatity.Focus();
                                        btnAdd.Visibility = Visibility.Hidden;
                                    }

                                    #endregion
                                }
                                else if (Check == "close" || Check == "CLOSE")
                                {
                                    txtQunatity.Text = "";
                                    popup.IsOpen = false;
                                    txtScannSKu.Focus();
                                }
                                else
                                {
                                    popup.IsOpen = false;
                                    MessageBox.Show("Invalid Quantity.");
                                    IsManualEntry = true;
                                    popup.IsOpen = true;
                                    txtQunatity.Text = "";
                                    txtQunatity.Focus();
                                    btnAdd.Visibility = Visibility.Hidden;
                                }
                            }
                            catch (Exception)
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                IsManualEntry = true;
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }





                        }
                    }
                    else
                    {
                        try
                        {

                            #region GreaterQtyChk

                            DataGridRow rowContainer = GetRow(selectedIndex);

                            //TextBox RMAStatus = (TextBox)grdContent.Columns[7].GetCellContent(rowContainer) as TextBox;

                            ContentPresenter CntPersenter = grdContent.Columns[8].GetCellContent(rowContainer) as ContentPresenter;
                            DataTemplate DataTemp = CntPersenter.ContentTemplate;
                            TextBox txtReturnGuid = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter);

                            txtReturnGuid.Visibility = Visibility.Visible;

                            //ContentPresenter CntPersenter1 = grdContent.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
                            //DataTemplate DataTemp1 = CntPersenter.ContentTemplate;
                            //TextBox txtReturnGuid1 = (TextBox)DataTemp.FindName("txtEnterQty", CntPersenter1);

                            TextBlock txtSku = grdContent.Columns[1].GetCellContent(rowContainer) as TextBlock;

                            TextBlock txtQty = grdContent.Columns[3].GetCellContent(rowContainer) as TextBlock;
                            TextBlock txtPkd = grdContent.Columns[5].GetCellContent(rowContainer) as TextBlock;


                            int chk = 0;
                            chk = Convert.ToInt32(txtPkd.Text) + Convert.ToInt32(txtQunatity.Text);
                            if (Convert.ToInt32(txtQty.Text) >= chk && Convert.ToInt32(txtQunatity.Text.Trim()) != 0 && Convert.ToInt32(txtQunatity.Text.Trim()) > 0)
                            {
                                txtReturnGuid.Text = txtQunatity.Text;
                                try
                                {

                                    for (int i1 = 0; i1 < Convert.ToInt16(txtReturnGuid.Text); i1++)//Loop for quantity
                                    {
                                        //Set Auto Sacannig Flag true. 
                                        ISRowAutoScaned = false;

                                        //Set text of textbox to upc code.
                                        txtScannSKu.Text = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU == txtSku.Text).UPCCode;

                                        //Set Previously packed time of item to show in the Grid.
                                        //itemPackedTime = _PackItem.PackingDetailStartDateTime;
                                        //////itemPackedTime =Convert.ToDateTime( detail);
                                        //Set Previously packed time as packing time. for package table
                                        StartTime = _shipment.PackingInfo[0].StartTime;

                                        var key = Key.Enter;                    // Key to send
                                        var target = txtScannSKu;   // Target element
                                        var routedEvent = Keyboard.KeyDownEvent; // Event to send
                                        target.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice,
                                         System.Windows.PresentationSource.FromVisual((Visual)target), 0, key) { RoutedEvent = routedEvent });
                                        //if (Global.shipmentclosed == "true")
                                        //    btnAddNewBox.Visibility = Visibility.Hidden;
                                        //else
                                        //    btnAddNewBox.Visibility = Visibility.Visible;
                                    }
                                }
                                catch (Exception)
                                {
                                    txtScannSKu.Focus();
                                }

                                popup.IsOpen = false;

                                txtQunatity.Text = "";

                                txtScannSKu.Focus();
                            }
                            else
                            {
                                popup.IsOpen = false;
                                MessageBox.Show("Invalid Quantity.");
                                IsManualEntry = true;
                                popup.IsOpen = true;
                                txtQunatity.Text = "";
                                txtQunatity.Focus();
                                btnAdd.Visibility = Visibility.Hidden;
                            }

                            #endregion


                        }
                        catch (Exception)
                        {
                            popup.IsOpen = false;
                            MessageBox.Show("Invalid Quantity.");
                            IsManualEntry = true;
                            popup.IsOpen = true;
                            txtQunatity.Text = "";
                            txtQunatity.Focus();
                            btnAdd.Visibility = Visibility.Hidden;
                        }

                    }
                }
                //else
                //{
                //    popup.IsOpen = false;
                //    MessageBox.Show("Invalid Quantity.");
                //    popup.IsOpen = true;
                //    txtQunatity.Text = "";
                //}
                #endregion
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - btnAdd_Click_2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

        private void btnQty_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
                btnAdd.Visibility = Visibility.Visible;
                
                txtQunatity.Focus();

            }
           catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - btnQty_SelectionChanged", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }

    }
}