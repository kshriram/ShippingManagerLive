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
    /// Interaction logic for wndModifyPopup.xaml
    /// </summary>
    public partial class wndModifyPopup : Window
    {
        bool flgScrap = false;
        cmdPakingDetails cd = new cmdPakingDetails();
        cmdBox box = new cmdBox();
        Boolean deleteclik;
        String ApplicationLocation = Global.controller.ApplicationLocation();
        string ActiveBox = "";
        DispatcherTimer dtLoadUpdate;
        DispatcherTimer dtLoadUpdate1;
        smController Control = new smController();
        model_Shipment _shipment = Global.controller.getModelShipment(Global.ShippingNumber);
        List<cstPackageDetails> PackageDetail = new List<cstPackageDetails>();
        List<cstBoxPackage> bxpckgDetail = new List<cstBoxPackage>();
        List<cstPackageTbl> packid = new List<cstPackageTbl>();
        List<string> updatedboxnumber = new List<string>();
        List<string> boxnumber = new List<string>();
        Boolean pkdstatus;
        public event EventHandler<myEventArgs3> RaiseCustomEvent3;
        public event EventHandler<myEventArgs> RaiseCustomEvent;

        public wndModifyPopup()
        {
            InitializeComponent();
        }
        public class myEventArgs3 : EventArgs
        {
            public myEventArgs3(string s)
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
        public class myEventArgs : EventArgs
        {
            public myEventArgs(string s)
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
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

          
            /////////  cmbBoxNUmber.SelectedIndex = 0;
        }

        //private void cmbBoxNUmber_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
                
        //        lblMessage2.Content = "";
        //        if (cmbBoxNUmber.SelectedIndex == 0)
        //        {
        //            btndeletesku.Visibility = Visibility.Hidden;

        //            btnAddsku.Visibility = Visibility.Hidden;
        //            btnRemovesku.Visibility = Visibility.Hidden;
        //        }
        //       else if (cmbBoxNUmber.SelectedIndex == 1)
        //        {
        //            btndeletesku.Content = "Delete Boxes";
        //            btndeletesku.Visibility = Visibility.Visible;

        //            btnAddsku.Visibility = Visibility.Hidden;
        //            btnRemovesku.Visibility = Visibility.Hidden;
        //        }
        //        else if(cmbBoxNUmber.SelectedIndex>1)
        //        {

        //            //ActiveBox = cmbBoxNUmber.SelectedItem.ToString();
        //            //btnAddsku.Visibility = Visibility.Visible;
        //            //btndeletesku.Visibility = Visibility.Visible;
        //            //btnRemovesku.Visibility = Visibility.Visible;

        //            ActiveBox = cmbBoxNUmber.SelectedItem.ToString();
        //            List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
        //            lsttrcking = cd.CheckDeleteBoxFromPackingDetail(ActiveBox);

        //            foreach (var item in lsttrcking)
        //            {
        //                if (item.DeleteFlag == "N")
        //                {
        //                    lblMessage2.Visibility = Visibility.Visible;
        //                    lblMessage2.Content = "This Box Having Tracking Number " + item.TrackingNum + "";
        //                    /// btnAddsku.Visibility = Visibility.Hidden;
        //                    btndeletesku.Visibility = Visibility.Hidden;
        //                    btnAddsku.Visibility = Visibility.Visible;
        //                    btnRemovesku.Visibility = Visibility.Hidden;
        //                    flgScrap = true;
        //                }
        //                else
        //                {
        //                    lblMessage2.Content = "";
        //                    btnAddsku.Content = "Cancel";
        //                    btnAddsku.Visibility = Visibility.Visible;
        //                    btndeletesku.Visibility = Visibility.Visible;
        //                    btnRemovesku.Visibility = Visibility.Visible;
        //                    flgScrap = false;
        //                }
        //            }

        //            //ShipmentScreen ss = new ShipmentScreen();

        //            //simpledelegate ms = new simpledelegate(ss.FnForCurrentBoxID);
        //            //ms();

        //            //simpledelegate sd = new simpledelegate();
        //           // ShowMassagePopup("Now Your Current Box Is " + ActiveBox + "", 1000);
        //        }
        //        //if (Global.shipmentclosed == "true")
        //        //{
        //        //    btnAddsku.Visibility = Visibility.Hidden;
        //        //}
        //        if (pkdstatus == true)
        //            btnAddsku.Visibility = Visibility.Hidden;

        //        if (flgScrap == true)
        //        {
        //            btnAddsku.Content = "Cancel";
        //            btnAddsku.Visibility = Visibility.Visible;
        //        }

        //        txtScannSKu.Focus();
        //    }
        //    catch (Exception Ex)
        //    {
        //        ShowMassagePopup("Select again", 1000);
        //        ErrorLoger.save("wndModifyPopup - cmbBoxNUmber_SelectionChanged_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
        //    }

        //}
        private void cmbBoxNUmber_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbBoxNUmber.SelectedIndex == 0)
                {
                    btndeletesku.Visibility = Visibility.Hidden;

                    btnAddsku.Visibility = Visibility.Hidden;
                    btnRemovesku.Visibility = Visibility.Hidden;
                }
                else if (cmbBoxNUmber.SelectedIndex == 1)
                {
                    btndeletesku.Content = "Delete Boxes";
                    btndeletesku.Visibility = Visibility.Visible;

                    btnAddsku.Visibility = Visibility.Hidden;
                    btnRemovesku.Visibility = Visibility.Hidden;
                }
                else if (cmbBoxNUmber.SelectedIndex > 1)
                {

                    if (Global.controller.IsSuperUser(Global.LoggedUserId) && cmbBoxNUmber.SelectedIndex == 2 || cmbBoxNUmber.SelectedIndex == 3)
                    {
                        btndeletesku.Content = "Delete Boxes";
                        btndeletesku.Visibility = Visibility.Visible;

                        btnAddsku.Visibility = Visibility.Hidden;
                        btnRemovesku.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        ActiveBox = cmbBoxNUmber.SelectedItem.ToString();
                        btnAddsku.Visibility = Visibility.Visible;
                        btndeletesku.Visibility = Visibility.Visible;
                        btnRemovesku.Visibility = Visibility.Visible;



                        //ShipmentScreen ss = new ShipmentScreen();

                        //simpledelegate ms = new simpledelegate(ss.FnForCurrentBoxID);
                        //ms();

                        //simpledelegate sd = new simpledelegate();
                      //  ShowMassagePopup("Now Your Current Box Is " + ActiveBox + "", 1000);
                    }
                }
                //if (Global.shipmentclosed == "true")
                //{
                //    btnAddsku.Visibility = Visibility.Hidden;
                //}
                if (pkdstatus == true)
                    btnAddsku.Visibility = Visibility.Hidden;


                txtScannSKu.Focus();
            }
            catch (Exception Ex)
            {
                ShowMassagePopup("Select again", 1000);
                ErrorLoger.save("wndModifyPopup - cmbBoxNUmber_SelectionChanged_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }

        private void txtScannSKu_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {

                //Avinash: Detect the Key Pres that is scanned Code and enter at the last.
                if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
                {
                    if (txtScannSKu.Text.Contains("#"))
                    {
                        if (txtScannSKu.Text == "#addsku" || txtScannSKu.Text == "#ADDSKU")
                        {
                            txtScannSKu.Text = "";

                            if (btnAddsku.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAddsku);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                ShowMassagePopup("You can not add new box.", 3000);
                                //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                
                            }

                        }

                        if (txtScannSKu.Text == "#deleteallboxes" || txtScannSKu.Text == "#DELETEALLBOXES")
                        {
                            txtScannSKu.Text = "";

                            if (cmbBoxNUmber.Visibility == Visibility.Visible)
                            {
                                cmbBoxNUmber.SelectedIndex = 1;
                                cmbBoxNUmber.SelectedItem = "Delete All boxes";

                                //ButtonAutomationPeer peer = new ButtonAutomationPeer(btndeletesku);
                                //IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                //invokeProv.Invoke();

                            }
                            else
                            {
                                ShowMassagePopup("You can not add new box.", 3000);
                                //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));

                            }

                        }





                        if (txtScannSKu.Text == "#cancel" || txtScannSKu.Text == "#CANCEL")
                        {
                            txtScannSKu.Text = "";

                            if (btnAddsku.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAddsku);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                ShowMassagePopup("You can not add new box.", 3000);
                                //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));

                            }

                        }


                        if (txtScannSKu.Text == "#removesku" || txtScannSKu.Text == "#REMOVESKU")
                        {
                            txtScannSKu.Text = "";

                            if (btnRemovesku.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnRemovesku);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                ShowMassagePopup("You can not Remove box.", 3000);
                            }
                        }
                        if (txtScannSKu.Text == "#deletebox" || txtScannSKu.Text == "#DELETEBOX")
                        {
                            txtScannSKu.Text = "";

                            if (btndeletesku.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btndeletesku);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                //////ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                ShowMassagePopup("You can not delete box.", 3000);
                            }
                        }
                        //txtScannSKu.Text = "";
                        //txtScannSKu.Focus();
                    }
                    else
                    {
                       // String _tempUPCStore = txtScannSKu.Text;
                        //Logout expire timer RE-start
                        SessionManager.StartTime();
                        if (txtScannSKu.Text != "" || txtScannSKu.Text == null)
                        {
                            foreach (var item in boxnumber)
                            {
                                if (item == txtScannSKu.Text)
                                {
                                    cmbBoxNUmber.SelectedItem = txtScannSKu.Text;
                                    Global.BoxActive = txtScannSKu.Text;
                                }
                            }
                            txtScannSKu.Text = "";
                            txtScannSKu.Focus();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - txtScannSKu_KeyDown_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.BoxActive = ActiveBox;
               /// Global.BoxActive = cmbBoxNUmber.SelectedItem.ToString();
                //Global.BoxActive = ActiveBox;
                Global.selectedBoxID = Global.controller.GetBoxIDByBoxNumber(ActiveBox);
                Global.PrintBoxID = Global.selectedBoxID;
                Global.FlgaddInBox = "FillInSelectedBox";

                this.Close();

                wndRemoveSKUFromBox windowremove = new wndRemoveSKUFromBox();
                windowremove.RaiseCustomEvent1 += new EventHandler<wndRemoveSKUFromBox.myEventArgs1>(nw_raiseevnt1);

                windowremove.ShowDialog();

               
              //  wndRemoveSKUFromBox Remove = new wndRemoveSKUFromBox();



                //windowremove.ShowDialog();
                //this.Close();



//                this.Dispatcher.Invoke(new Action(() => { Remove.ShowDialog(); }));

               
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - Button_Click_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void nw_raiseevnt1(object sender, wndRemoveSKUFromBox.myEventArgs1 e)
        {

            try
            {
                Global.pkdStatus = "You are in packing Mode";
                RaiseCustomEvent(this, new myEventArgs(Global.BoxActive));
                //Global.pkdStatus = "You are in packing Mode";
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - nw_raiseevnt1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

          

         //   MessageBox.Show("HI");



            //this.lblCurrentBox.Content = e.massage;
            //List<cstShipment> shipment1 = new List<cstShipment>();
            //shipment1 = Global.controller.GetShipment_SPCKD(Global.ShippingNumber);
            //this.Dispatcher.Invoke(new Action(() => { grdContent.ItemsSource = shipment1; }));
            //// Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);

            //timer2 = new DispatcherTimer();
            //timer2.Tick += timer2_Tick;
            //timer2.Interval = new TimeSpan(0, 0, 0, 1);
            //timer2.Start();



            //  this.Dispatcher.Invoke(new Action(() => { BoldFontandHideCombp(Bindedshipment); }));
        }
        public class bxnum
        {
            public String BoxNumber { get; set; }
        }
        private void Window_Loaded_2(object sender, RoutedEventArgs e)
        {
            txtScannSKu.Focus();
            //this.Owner.Hide();
            lblMessage.Visibility = Visibility.Hidden;
            lblMessage2.Visibility = Visibility.Hidden;

            btnAddsku.Visibility = Visibility.Hidden;
            btndeletesku.Visibility = Visibility.Hidden;
            btnRemovesku.Visibility = Visibility.Hidden;

            PackageDetail = new List<cstPackageDetails>();
            //// blink1();

            bxnum bxn = new bxnum();
            boxnumber = new List<string>();
            string Tenpboxnumber = "";
            #region combofilling
            try
            {
                ///For Manager override  24-12-2014
                    List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(Global.LoggedUserId);
                    var rolnm=(from v in lsUserInfo
                              where v.UserID==Global.LoggedUserId 
                              select v.RoleName).FirstOrDefault();
                    ////if (Global.controller.IsSuperUser(Global.LoggedUserId) == true || rolnm.ToString() == "Manager")
                    ////{
                    ////}





                foreach (var item in Control.GetPackingNum(Global.ShippingNumber))
                {
                    List<cstPackageDetails> PackageDetailTemp = new List<cstPackageDetails>();
                    PackageDetailTemp = Control.GetPackingDetailTbl(item);
                  
                    var bxnum = (from bxns in PackageDetailTemp
                                 select bxns).GroupBy(a => a.BoxNumber).Select(g=>g.First()).ToList();

                    foreach (var item1 in bxnum)
                    {

                        if (item1.ShipmentLocation == ApplicationLocation && Global.controller.IsSuperUser(Global.LoggedUserId) != true)
                        {


                            bxn.BoxNumber = item1.BoxNumber;
                           
                            boxnumber.Add(item1.BoxNumber);


                        }

                        else if (Global.controller.IsSuperUser(Global.LoggedUserId) ) 
                        {
                            bxn.BoxNumber = item1.BoxNumber;
                            boxnumber.Add(item1.BoxNumber);

                        }
                    }

                    //foreach (var item1 in PackageDetailTemp)
                    //{

                    //    if (item1.ShipmentLocation == ApplicationLocation && Global.controller.IsSuperUser(Global.LoggedUserId) != true)
                    //    {


                    //        bxn.BoxNumber = item1.BoxNumber;
                    //        boxnumber.Add(item1.BoxNumber);

                    //    }

                    //    else if (Global.controller.IsSuperUser(Global.LoggedUserId))
                    //    {
                    //        bxn.BoxNumber = item1.BoxNumber;
                    //        boxnumber.Add(item1.BoxNumber);

                    //    }
                    //}


                   
                }



                boxnumber.Insert(0, "--select--");
                boxnumber.Insert(1, "Delete All boxes");

                if (Global.controller.IsSuperUser(Global.LoggedUserId))
                {
                    boxnumber.Insert(2, "Delete All boxes From NYWT");
                    boxnumber.Insert(3, "Delete All boxes From NYWH");
                }

                if (Global.UnPacked != "" && Global.UnPacked != null)
                {
                    ActiveBox = Global.UnPacked;
                    lblMessage.Visibility = Visibility.Visible;
                    lblMessage2.Visibility = Visibility.Visible;
                    // cmbBoxNUmber.Items.Add(Global.UnPacked);
                    lblMessage.Content = "Current box " + " " + Global.UnPacked + " " + "is open ";
                    lblMessage2.Content = " select one of the following option.";
                    cmbBoxNUmber.SelectedItem = Global.UnPacked;
                    cmbBoxNUmber.Visibility = Visibility.Hidden;
                    lblboxSelect.Visibility = Visibility.Hidden;
                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;
                }
                //boxnumber.Insert(1, Global.UnPacked);
                ///cmbBoxNUmber.ItemsSource = Global.UnPacked;
                else
                {
                    cmbBoxNUmber.ItemsSource = boxnumber;
                    btnAddsku.Content = "Add SKU";
                }
                //if (Global.shipmentclosed == "true")
                //{
                //    btnAddsku.Visibility = Visibility.Hidden;
                //}

                if (Global.CheckRemoveButton == true)
                {
                    if (Global.UnPacked != "" && Global.UnPacked != null)
                    {
                        List<cstPackageDetails> _lsPacking = new List<cstPackageDetails>();
                        _lsPacking = Control.GetPackingDetailTbl(Global.UnPacked);
                        if (_lsPacking.Count == 0)
                        {
                            btnRemovesku.Visibility = Visibility.Hidden;
                            btnAddsku.Visibility = Visibility.Visible;
                            btndeletesku.Visibility = Visibility.Visible;
                        }
                    }
                }

                cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                ///  int tt = _lsPackedDetailTableInfo2[0].PackingStatus;
                if (packing.PackingStatus == 0)
                {
                    pkdstatus = true;
                    ////shipment is closed
                    Global.pkdStatus = "Shipment is Closed.";
                    btnAddsku.Visibility = Visibility.Hidden;
                }
                else
                {
                    pkdstatus = false;
                    Global.pkdStatus = "You are in Packing Mode.";
                    ///btnAddNewBox.Visibility = Visibility.Visible;
                }
               // ShowMassagePopup("select or scan Boxnumber to Modify", 1000);
                cmbBoxNUmber.SelectedIndex = 0;
                txtScannSKu.Focus();
            }

            catch (Exception Ex)
            {
                ShowMassagePopup("" + Ex.Message, 2000);
                ErrorLoger.save("wndModifyPopup - Window_Loaded_2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            } 
            #endregion
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                SplashScreen splashScreen = new SplashScreen("1235.png");

                MsgBox.Show("Confirm", "Delete", "Delete Box and Remove all SKU's." + Environment.NewLine + "Are you sure want to Delete Box?");
                if (Global.MsgBoxResult == "Ok")
                {
                    Global.FromRemoveFlag = false;
                   // WindowThread.start();
                    if (Global.FlagAllshippckd == "flagTrue")
                    {
                        Global.FlagAllshippckd = "ReturnTrue";
                    }

                    //// Global.BoxActive = "";
                    //cd.DeleteBoxFromPackingDetail(ActiveBox);
                    ////cmbBoxNUmber.SelectedItem = "--select--";
                    ////cmbBoxNUmber.SelectedIndex = 0;
                    ///// cmbBoxNUmber.ItemsSource = boxnumber;
                    //deleteclik = true;
                    //if (Global.UnPacked != "" && Global.UnPacked != null)
                    //{ //save box and Replace Global BoxID with new One
                    //    cstBoxPackage _boxPackage = new cstBoxPackage();
                    //    _boxPackage.PackingID = Global.PackingID;
                    //    _boxPackage.BoxCreatedTime = DateTime.UtcNow;
                    //    List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                    //    lsBox.Add(_boxPackage);
                    //    Global.PrintBoxID = Global.controller.SetBox(lsBox);
                    //    ActiveBox = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                    //    Global.UnPacked = "";


                    //}
                    //else
                    //{
                    //    ActiveBox = Global.previouscurrentbox;
                    //}
                    //Global.pkdStatus = "You are in packing Mode";
                    //RaiseCustomEvent(this, new myEventArgs(ActiveBox));
                    //this.Close();
                    ////  _saveClick();
                    //// simpledelegate sm = new simpledelegate(_saveClick);
                    ////sm();
                    if (cmbBoxNUmber.SelectedIndex == 1 || cmbBoxNUmber.SelectedIndex == 2 || cmbBoxNUmber.SelectedIndex == 3)
                    {

                        splashScreen.Show(true);

                        string CHECK = "";

                        switch (cmbBoxNUmber.SelectedIndex)
                        {
                            case 1:
                                foreach (var item in boxnumber)
                                {
                                    //List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                                    //lsttrcking = cd.CheckDeleteBoxFromPackingDetail(item);
                                    //if (lsttrcking.Count > 0)
                                    //    CHECK = (lsttrcking.FirstOrDefault().DeleteFlag);

                                    //if (CHECK == "N")
                                    //{
                                    //    Global.flgChkdeletebx = false;

                                    //}
                                    //else
                                    //{
                                    //    Global.flgChkdeletebx = true;
                                    //}
                                    SaveDeletesBox(item);
                                    cd.DeleteBoxFromPackingDetail(item);
                                    Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, item);


                                }
                                break;
                            case 2:
                                foreach (var item in boxnumber)
                                {
                                    //List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                                    //lsttrcking = cd.CheckDeleteBoxFromPackingDetail(item, "NYWT");
                                    //if (lsttrcking.Count > 0)
                                    //    CHECK = (lsttrcking.FirstOrDefault(i => i.DeleteFlag == "N").DeleteFlag);
                                    //if (CHECK == "N")
                                    //{
                                    //    Global.flgChkdeletebx = false;

                                    //}
                                    //else
                                    //{
                                    //    Global.flgChkdeletebx = true;
                                    //}
                                    SaveDeletesBox(item);
                                    cd.DeleteBoxFromPackingDetail(item, "NYWT");
                                    Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, item);

                                }
                                break;
                            case 3:
                                foreach (var item in boxnumber)
                                {
                                    //List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                                    //lsttrcking = cd.CheckDeleteBoxFromPackingDetail(item, "NYWH");
                                    //if (lsttrcking.Count > 0)
                                    //    CHECK = (lsttrcking.FirstOrDefault(i => i.DeleteFlag == "N").DeleteFlag);
                                    //if (CHECK == "N")
                                    //{
                                    //    Global.flgChkdeletebx = false;

                                    //}
                                    //else
                                    //{
                                    //    Global.flgChkdeletebx = true;
                                    //}
                                    SaveDeletesBox(item);
                                    cd.DeleteBoxFromPackingDetail(item, "NYWH");
                                    Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, item);

                                }
                                break;
                            default:
                                break;
                        }


                        //foreach (var item in boxnumber)
                        //{
                        //    cd.DeleteBoxFromPackingDetail(item);

                        //}

                        //set Packing Status 0 From 1 in package table
                        cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                        packing.PackingStatus = 1;
                        List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                        _lsNewPacking.Add(packing);

                        Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);

                        deleteclik = true;
                        if (Global.UnPacked != "" && Global.UnPacked != null)
                        { //save box and Replace Global BoxID with new One
                            cstBoxPackage _boxPackage = new cstBoxPackage();
                            _boxPackage.PackingID = Global.PackingID;
                            _boxPackage.BoxCreatedTime = DateTime.UtcNow;
                            List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                            lsBox.Add(_boxPackage);
                            Global.PrintBoxID = Global.controller.SetBox(lsBox);
                            ActiveBox = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                            Global.UnPacked = "";
                        }
                        else
                        {
                            ActiveBox = Global.previouscurrentbox;
                        }
                        Global.pkdStatus = "You are in packing Mode";
                        Global.shipmentclosed = "False";
                        RaiseCustomEvent(this, new myEventArgs(ActiveBox));
                        splashScreen.Show(false);
                        this.Close();
                    }

                    else
                    {
                        splashScreen.Show(true);
                        SaveDeletesBox(ActiveBox);
                        // Global.BoxActive = "";
                        cd.DeleteBoxFromPackingDetail(ActiveBox);
                        Control.DeleteCartonInfoByBoxNum(Global.ShippingNumber, ActiveBox);

                        cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                        packing.PackingStatus = 1;
                        List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                        _lsNewPacking.Add(packing);

                        Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);
                        //cmbBoxNUmber.SelectedItem = "--select--";
                        //cmbBoxNUmber.SelectedIndex = 0;
                        /// cmbBoxNUmber.ItemsSource = boxnumber;
                        deleteclik = true;
                        if (Global.UnPacked != "" && Global.UnPacked != null)
                        { //save box and Replace Global BoxID with new One
                            cstBoxPackage _boxPackage = new cstBoxPackage();
                            _boxPackage.PackingID = Global.PackingID;
                            _boxPackage.BoxCreatedTime = DateTime.UtcNow;
                            List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                            lsBox.Add(_boxPackage);
                            Global.PrintBoxID = Global.controller.SetBox(lsBox);
                            ActiveBox = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
                            Global.UnPacked = "";
                        }
                        else
                        {
                            ActiveBox = Global.previouscurrentbox;
                        }
                        Global.pkdStatus = "You are in packing Mode";
                        Global.shipmentclosed = "False";
                        RaiseCustomEvent(this, new myEventArgs(ActiveBox));
                        splashScreen.Show(false);
                        this.Close();
                        //  _saveClick();
                        // simpledelegate sm = new simpledelegate(_saveClick);
                        //sm();
                    }

                }
                else
                {
                    ShowMassagePopup("Your Operation is cancled", 1000);

                }
            }
            catch (Exception Ex)
            {
                ShowMassagePopup("Invalid Operation", 3000);
                ErrorLoger.save("wndModifyPopup - Button_Click_2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }


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
                ///ErrorLoger.save("wndShipmentScanPage - _saveShippingInformation", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndModifyPopup - _saveShippingInformation()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
            return _return;
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
                    ErrorLoger.save("wndModifyPopup - _shipmentLock()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
                    //ErrorLoger.save("wndShipmentScanPage - _shipmentLock", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                }
            }));
            return _return;
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
                   // WindowThread.start();

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
                ErrorLoger.save("wndModifyPopup - _show_Shipment()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
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
                //ErrorLoger.save("wndShipmentScanPage - _saveClick", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ErrorLoger.save("wndModifyPopup - _saveClick()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }
        public void moveOnDetail()
        {
            try
            {
                _saveClick();
                Global.flgBx = "NotAddBx";
            }
            catch(Exception Ex)
            {
                 ErrorLoger.save("wndModifyPopup - moveOnDetail()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
          
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (deleteclik)
           //  moveOnDetail();

            //wndModifyPopup modify = new wndModifyPopup();
            //modify.Close();
     
        }

        private void btnAddsku_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                SplashScreen splashScreen = new SplashScreen("1235.png");
                splashScreen.Show(true);

                string boxNum = ActiveBox;
                Global.BoxActive = boxNum;
                Global.selectedBoxID = Global.controller.GetBoxIDByBoxNumber(boxNum);

                if (flgScrap == true)
                {
                    cstBoxPackage _boxPackage = new cstBoxPackage();
                    _boxPackage.PackingID = Global.PackingID;
                    _boxPackage.BoxCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                    List<cstBoxPackage> lsBox = new List<cstBoxPackage>();
                    lsBox.Add(_boxPackage);
                    Global.PrintBoxID = Global.controller.SetBox(lsBox);
                    ActiveBox = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;


                    Global.selectedBoxID = Global.controller.GetBoxIDByBoxNumber(ActiveBox);

                    Global.PrintBoxID = Global.selectedBoxID;
                    Global.FlgaddInBox = "FillInSelectedBox";
                    // moveOnDetail();
                    // Window_Closing_1();
                    Global.pkdStatus = "You are in packing Mode";
                    RaiseCustomEvent3(this, new myEventArgs3(ActiveBox));


                }
                else
                {

                    Global.PrintBoxID = Global.selectedBoxID;
                    Global.FlgaddInBox = "FillInSelectedBox";
                    // moveOnDetail();
                    // Window_Closing_1();
                    Global.pkdStatus = "You are in packing Mode";
                    RaiseCustomEvent3(this, new myEventArgs3(boxNum));
                    //wndModifyPopup modify = new wndModifyPopup();
                    //modify.Close();
                }


                splashScreen.Show(false);

                //  this.Owner.Show();
                this.Close();
                //this.Close();
            }
            catch (Exception Ex)
            {
                ShowMassagePopup("Invalid Operation.", 1000);
                ErrorLoger.save("wndModifyPopup - btnAddsku_Click", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
        }

        private void Window_Closing_2(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // this.Owner.Show();

        }

        private void mbox_cancel(object sender, RoutedEventArgs e)
        {

            brdMessage.Visibility = System.Windows.Visibility.Hidden;
        }

        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            brdMessage.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                //// gridMain.Visibility = System.Windows.Visibility.Hidden;

                //brdMessage.Background = new SolidColorBrush(Colors.Black);
                //brdMessage.Opacity = 0.5;
                //brdMessage.Visibility =  System.Windows.Visibility.Visible;

                //dtLoadUpdate = new DispatcherTimer();
                //dtLoadUpdate.Interval = new TimeSpan(0, 0, 0, 0, 1100);
                //dtLoadUpdate.Tick += dtLoadUpdate_Tick;
                ////start the dispacher.
                //dtLoadUpdate.Start()
                this.Close();

                ShipmentScreen ss = new ShipmentScreen();
                ss.Show();
                MessageDelegate ms = new MessageDelegate(ss.ShowMassagePopup);
                ms("HIIIII ", 7000);
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - Button_Click_3", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
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

                lblMessageDialog.Content = message;
                //brdMessage.Background = new SolidColorBrush(Colors.Black);
                //brdMessage.Opacity = 0.5;
                brdMessage.Visibility = System.Windows.Visibility.Visible;

                dtLoadUpdate1 = new DispatcherTimer();
                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, TimeSpan);
                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                //start the dispacher.
                dtLoadUpdate1.Start();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - ShowMassagePopup()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
           
        }
        private void dtLoadUpdate1_Tick(object sender, EventArgs e)
        {
            try
            {
                  //  brdfrist.Background = new SolidColorBrush(Colors.White);
            ////brdfrist.Background = new SolidColorBrush(Colors.Black);
            //  brdfrist.BorderThickness = new Thickness(4,0,0, 0);
            ///   brdMessage.Opacity = 1;
            //    brdMessage.Background = new SolidColorBrush(Colors.Transparent);
            brdMessage.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate1.Stop();
            }
            catch(Exception Ex)
            {
                ErrorLoger.save("wndModifyPopup - dtLoadUpdate1_Tick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }
          

        }
        ////End
        private void dtLoadUpdate_Tick(object sender, EventArgs e)
        {
            //brdMessage.Visibility = System.Windows.Visibility.Hidden;
            //dtLoadUpdate.Stop();
        }

        public void SaveDeletesBox(String BoxNumber)
        {

            try
            {
                List<cstPackageDetails> lsPackageDetail = new List<cstPackageDetails>();
                lsPackageDetail = Global.controller.GetPackingDetailTbl(BoxNumber);

                List<cstDeletedBoxSave> ListDeletedBox = new List<cstDeletedBoxSave>();


                foreach (var item in lsPackageDetail)
                {
                    cstDeletedBoxSave delete = new cstDeletedBoxSave();
                    delete.BoxNumber = item.BoxNumber;
                    delete.SKUQuantity = item.SKUQuantity;
                    delete.Location = item.ShipmentLocation;
                    delete.StationName = Global.StationName;
                    delete.DeleteDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                    delete.DeletedBoxID = Guid.NewGuid();
                    delete.UserID = Global.LoggedUserId;
                    delete.SKUName = item.SKUNumber;
                    delete.ActionStatus = "Delete";


                    ListDeletedBox.Add(delete);
                }

                Global.controller.SetDeletedBoxDetails(ListDeletedBox);
            }
            catch (Exception)
            {

            }

        }


    }
}
