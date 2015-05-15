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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndModifyPallet.xaml
    /// </summary>
    public partial class wndModifyPallet : Window
    {
        DispatcherTimer dtLoadUpdate;
        smController Control = new smController();
        List<string> pl = new List<string>();
        public event EventHandler<myEventArgs3> RaiseCustomEvent3;

        int flagforcurrent = 0;

        List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet> PalletCompare = new List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet>();

        

        string ActivePallet = "";

        public event EventHandler<myEventArgs> RaiseCustomEvent;
        public wndModifyPallet()
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
            pl = new List<string>();

            PalletCompare = Global.Pallet;

            if (Control.GetBoxnumberAndQuantity(Global.PalletNoForReplace).Count == 0)
            {

              

                cmbPalletNUmber.Visibility = Visibility.Hidden;
                lblboxSelect.Visibility = Visibility.Visible;
                txtScannSKu.Visibility = Visibility.Visible;

                // lblMessage.Visibility = Visibility.Visible;
                // lblMessage2.Visibility = Visibility.Visible;
                // cmbBoxNUmber.Items.Add(Global.UnPacked);
                lblMessage.Content = "Current pallet " + " " + Global.PalletNoForReplace + " " + "is open ";
                lblMessage2.Content = " select one of the following option.";

                flagforcurrent = 1;

                btnAddsku.Content = "Cancel";
                btnAddsku.Visibility = Visibility.Visible;
                //btndeletesku.Visibility = Visibility.Visible;
                //btnRemovesku.Visibility = Visibility.Visible;


                txtScannSKu.Focus();

            }
            else
            {


                cmbPalletNUmber.Visibility = Visibility.Hidden;
                lblboxSelect.Visibility = Visibility.Hidden;
                txtScannSKu.Visibility = Visibility.Hidden;

                // lblMessage.Visibility = Visibility.Visible;
                // lblMessage2.Visibility = Visibility.Visible;
                // cmbBoxNUmber.Items.Add(Global.UnPacked);
                lblMessage.Content = "Current pallet " + " " + Global.PalletNoForReplace + " " + "is open ";
                lblMessage2.Content = " select one of the following option.";

               // flagforcurrent = 1;

                btnAddsku.Content = "Cancel";
                btnAddsku.Visibility = Visibility.Visible;
                btndeletesku.Visibility = Visibility.Visible;
                btnRemovesku.Visibility = Visibility.Visible;


                txtScannSKu.Focus();






                // pl = Control.GetPalletInfoBySHNumber(Global.ShippingNumber);

                //  lblMessage.Visibility = Visibility.Hidden;
                // lblMessage2.Visibility = Visibility.Hidden;

                //List<cstPalletInfo> lspalletInfo = Control.GetPalletInfoBySHNumber(Global.ShippingNumber);

                ////List<string> lspalletInfo1 = Control.GetPalletInfoBySHNumber(Global.ShippingNumber);

                //string pallet;

                //foreach (var item in lspalletInfo)
                //{
                //    //  cstPalletInfo plt = new cstPalletInfo();
                //    // plt.PalletNumber = ;

                //    pallet = item.PalletNumber;

                //    pl.Add(pallet);
                //}
                //pl.Insert(0, "--select--");
                //pl.Insert(1, "Delete All pallets");

                //cmbPalletNUmber.ItemsSource = pl;

                //// cmbsample.ItemsSource = pl;

                //cmbPalletNUmber.SelectedIndex = 0;

                //txtScannSKu.Focus();

                //Control.GetPalletInfoBySHNumber(Global.ShippingNumber);
                //ShowMassagePopup("Please select Box Number For Modify", 4000);
                // cmbPalletNUmber.DisplayMemberPath = plt.PalletNumber;
                //cmbPalletNUmber.SelectedValuePath = plt.PalletNumber;
            }
        }

        private void btnAddsku_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add and Cancle button for Current PalletNumber or Selected Pallet Number. 

                RaiseCustomEvent3(this, new myEventArgs3(Global.PalletNoForReplace));

                this.Close();
            }
            catch (Exception)
            {

            }

        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
               // WindowThread.start();
                this.Close();
              //  Global.ActivePallet = cmbPalletNUmber.SelectedItem.ToString();
                wndRemoveBoxesFromPallet windowremove = new wndRemoveBoxesFromPallet();
                windowremove.RaiseCustomEvent1 += new EventHandler<wndRemoveBoxesFromPallet.myEventArgs1>(nw_raiseevnt1);

                


                //  wndRemoveSKUFromBox Remove = new wndRemoveSKUFromBox();
                windowremove.ShowDialog();
              
            }
            catch (Exception)
            {

            }

        }


        private void nw_raiseevnt1(object sender, wndRemoveBoxesFromPallet.myEventArgs1 e)
        {
            try
            {
              

                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}

                RaiseCustomEvent(this, new myEventArgs("Remove"));
                //Global.pkdStatus = "You are in packing Mode";
            }
            catch (Exception)
            {

            }
        }


        public void SaveDeletedPallet(String PalletNumber)
        {

            try
            {
                List<cstPackageDetails> lsPackageDetail = new List<cstPackageDetails>();
                lsPackageDetail = Global.controller.GetPackingDetailTbl(PalletNumber);

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





        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            try
            {
                MsgBox.Show("Confirm", "Delete", "Delete Pallet will Remove pallet" + Environment.NewLine + "Are you sure want to Delete pallet?");
                if (Global.MsgBoxResult == "Ok")
                {

                    SplashScreen splashScreen = new SplashScreen("1235.png");
                    splashScreen.Show(true);
                    //WindowThread.start();
                    //if (cmbPalletNUmber.SelectedIndex == 1)
                    //{
                    //    foreach (var item in pl)
                    //    {
                    //        Control.DeletePalletCon(item);
                    //    }
                    //    Global.deleteflag = "deleteAll";
                    //}
                    //else
                    //{
                    Control.DeletePalletCon(Global.PalletNoForReplace);
                    Global.deleteflag = "deletesingle";






                    List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet> PalletCompareNew = new List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet>();
                    foreach (var item in Global.Pallet)
                    {
                        PalletCompareNew.Add(item);
                    }


                    foreach (var item in PalletCompareNew)
                    {
                     
                        if (item.PalletNumber == Global.PalletNoForReplace)
                        {
                            PalletCompare.Remove(item);
                            //break;
                        }
                    }

                    Global.Pallet = PalletCompare;


                    //}
                    // Global.ActivePallet = cmbPalletNUmber.SelectedItem.ToString();
                    RaiseCustomEvent(this, new myEventArgs(Global.PalletNoForReplace));

                    splashScreen.Show(false);

                    this.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        //private void mbox_ok(object sender, RoutedEventArgs e)
        //{

        //}

        private void txtScannSKu_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
            {



                if (txtScannSKu.Text.Trim() != Global.PalletNoForReplace && flagforcurrent == 1)
                {
                    string PalletNumber = Control.GetPalletInfoByPalletnumberCon(txtScannSKu.Text.Trim()).PalletNumber;

                    if (Control.GetPalletInfoByPalletnumberCon(txtScannSKu.Text.Trim()).PalletNumber != txtScannSKu.Text.Trim())
                    {
                        MessageBox.Show("Invalid Scan.");

                        txtScannSKu.Focus();
                    }
                    else
                    {
                        Global.PalletNoForReplace = txtScannSKu.Text.Trim();
                        lblMessage.Content = "Current pallet " + " " + Global.PalletNoForReplace + " " + "is open ";
                        lblMessage2.Content = " select one of the following option.";

                        btnAddsku.Visibility = Visibility.Visible;
                        btndeletesku.Visibility = Visibility.Visible;
                        btnRemovesku.Visibility = Visibility.Visible;

                        txtScannSKu.Focus();
                    }

                }
                else
                {


                    Global.PalletNoForReplace = txtScannSKu.Text.Trim();
                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;
                    //ActivePallet = cmbPalletNUmber.SelectedItem.ToString();
                    //if (Global.isAllpack == true)
                    //{
                    //    btnAddsku.Content = "Cancel";
                    //}
                    //else
                    //{
                    btnAddsku.Content = "Add Box";
                    //}

                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;

                    lblMessage.Content = "Current pallet " + " " + Global.PalletNoForReplace + " " + "is open ";
                    lblMessage2.Content = " select one of the following option.";





                    ShowMassagePopup("Now Your Current Pallet is " + Global.PalletNoForReplace + "", 1000);
                    txtScannSKu.Focus();
                }

            }







            if (txtScannSKu.Text == "#addapallet" || txtScannSKu.Text == "#ADDAPALLET")
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
                    //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                    string ss = "You can not add new box.";
                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                    mm(ss, 3000);

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
                    //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                    string ss = "You can not add new box.";
                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                    mm(ss, 3000);

                }
            }




            if (txtScannSKu.Text == "#removebox" || txtScannSKu.Text == "#REMOVEBOX")
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
                    //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                    string ss = "You can not add new box.";
                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                    mm(ss, 3000);

                }
            }
            if (txtScannSKu.Text == "#deletepallet" || txtScannSKu.Text == "#DELETEPALLET")
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
                    //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                    string ss = "You can not add new box.";
                    /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                    //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                    MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                    mm(ss, 3000);

                }
            }
        }
        #region  Massage box 15-12-2014
        public void ShowMassagePopup(string message, int TimeSpan)
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
        private void dtLoadUpdate_Tick(object sender, EventArgs e)
        {
            //  brdfrist.Background = new SolidColorBrush(Colors.White);
            ////brdfrist.Background = new SolidColorBrush(Colors.Black);
            //  brdfrist.BorderThickness = new Thickness(4,0,0, 0);
            ///   brdMessage.Opacity = 1;
            //    brdMessage.Background = new SolidColorBrush(Colors.Transparent);
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate.Stop();
            txtScannSKu.Focus();

        }

        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate.Stop();
            txtScannSKu.Focus();
        }
        #endregion

        private void cmbPalletNUmber_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
               

                if (cmbPalletNUmber.SelectedIndex == 1)
                {
                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;

                    btndeletesku.Content = "Delete Pallets";
                    btndeletesku.Visibility = Visibility.Visible;

                    btnAddsku.Visibility = Visibility.Hidden;
                    btnRemovesku.Visibility = Visibility.Hidden;
                    txtScannSKu.Focus();
                }
                else if (cmbPalletNUmber.SelectedIndex != 0)
                {

                    Global.PalletNoForReplace = cmbPalletNUmber.SelectedItem.ToString();
                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;
                    //ActivePallet = cmbPalletNUmber.SelectedItem.ToString();
                    if (Global.isAllpack == true)
                    {
                        btnAddsku.Content = "Cancel";
                    }
                    else
                    {
                        btnAddsku.Content = "Add Box";
                    }
                   
                    btnAddsku.Visibility = Visibility.Visible;
                    btndeletesku.Visibility = Visibility.Visible;
                    btnRemovesku.Visibility = Visibility.Visible;



                    ShowMassagePopup("Now Your Current Pallet is " + Global.PalletNoForReplace + "", 1000);
                    txtScannSKu.Focus();
                }
                //if (Global.shipmentclosed == "true")
                //{
                //    btnAddsku.Visibility = Visibility.Hidden;
                //}
                //if (pkdstatus == true)
                //    btnAddsku.Visibility = Visibility.Hidden;


                // txtScannSKu.Focus();
            }
            catch (Exception)
            {
                ShowMassagePopup("Select again", 1000);
            }
        }
        
    }
}
