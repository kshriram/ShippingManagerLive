using Packing_Net.Classes;
using PackingClassLibrary;
using PackingClassLibrary.Commands;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndBoxInfo.xaml
    /// </summary>
    public partial class wndBoxInfoForPallet : Window
    {
        DispatcherTimer dtLoadUpdate;
        DispatcherTimer dtLoadUpdate1;
        cmdCartonInfo carton = new cmdCartonInfo();
        cmdPakingDetails packing = new cmdPakingDetails();
        smController ConObj = new smController();

        string CurrentBox = "";
        /// <summary>
        ///  List<BoxAndPallet> lspallet = new List<BoxAndPallet>();
        /// </summary>
        


        Boolean closeflg = false;

        List<cstPackageDetails> lsdetail = new List<cstPackageDetails>();



        List<cstCartonInfo> lscarton = new List<cstCartonInfo>();

        PackingClassLibrary.smController sm = new PackingClassLibrary.smController();

        int RowCount = 0;


        public wndBoxInfoForPallet()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Closing += new System.ComponentModel.CancelEventHandler(MyWindow_Closing);

                // btnmodifypallet.Visibility = Visibility.Hidden;

                Global.Pallet.Clear();

                Global.CartonOn = "On";

                btnAddNewPallet.Visibility = Visibility.Hidden;

                lblShipmentNumber.Content = "Shipment is :- " + Global.ShippingNumber;

                cstPalletInfo _boxPackage = new cstPalletInfo();
                _boxPackage.palletCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                _boxPackage.Location = cmdLocalFile.ReadString("Location");
                List<cstPalletInfo> lsBox = new List<cstPalletInfo>();
                lsBox.Add(_boxPackage);
                Global.PalletID = Global.controller.SetPallet(lsBox);

                lblPallet.Content = "Your current pallet is " + "  " + Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                Global.palletnumber = Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                Global.PalletNoForReplace = Global.palletnumber;

                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}
                // fillGrid(Global.ShippingNumber);   


                //  grdContent.ItemsSource = lsdetail;

                // fillGrid();

                //if (CheckGray(grdContent))
                //{
                //    btnAddNewPallet.Visibility = Visibility.Visible;
                //    btnAddNewPallet.Content = "Scan Badge";
                //}
                //else
                //{
                //    //btnAddNewPallet.Visibility = Visibility.Hidden;
                //}

                txtBoxNumberScanned.Focus();
                // txtWH.Text = CheckWH(Global.ShippingNumber).Trim();
                //Please wait screen abort.


            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - Page_load", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ShowMassagePopup("Page Load error occurs", 4000);
            }

        }


        void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

                if (closeflg == false)
                {

                    //if (sm.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace).Count != 0)
                    //{
                    if (btnAddNewPallet.Visibility != Visibility.Hidden)
                    {
                        ShowMassagePopup("Please click on Print Pallet", 2000);
                        e.Cancel = true;

                    }
                    else
                    {
                        MsgBox.Show("Error", "Exit", "close Pallet will Exit ." + Environment.NewLine + "Are you sure want to Close Pallet Info?");
                        if (Global.MsgBoxResult == "Ok")
                        {

                            this.Close();
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                            txtBoxNumberScanned.Focus();
                        }
                    }
                }
                else
                {
                    e.Cancel = false;
                }

            }
            catch (Exception)
            {
                ShowMassagePopup("Invalid Operation Closing", 4000);
            }
        }


        public String CheckWH(string ShippingNumber)
        {
            String Retutn = "";
            Retutn = Global.controller.GetShippingInfoFromSage(ShippingNumber, "LTL").First().ToAddressLine3;
            return Retutn;
        }


        public void fillGrid()
        {

            try
            {

                //foreach (var item in sm.GetPackingNum(ShipmentNum))
                //{


                //    foreach (var item1 in packing.GetPackingDetailsBoxNumber(item))
                //    {
                //        //cstPackageDetails singlelist = new cstPackageDetails();

                //        //singlelist.BoxNumber = item1.BoxNumber;
                //        //singlelist.TCLCOD_0 = item1.TCLCOD_0;
                //        //singlelist.TarrifCode = item1.TarrifCode;
                //        //singlelist.SKUQuantity = item1.SKUQuantity;
                //        //singlelist.SKUNumber = item1.SKUNumber;
                //        //singlelist.ShipmentLocation = item1.ShipmentLocation;
                //        //singlelist.ProductName = item1.ProductName;
                //        //singlelist.PackingId = item1.PackingId;
                //        //singlelist.PackingDetailStartDateTime = item1.PackingDetailStartDateTime;
                //        //singlelist.PackingDetailID = item1.PackingDetailID;
                //        //singlelist.MAP_Price = item1.MAP_Price;
                //        //singlelist.ItemName = item1.ItemName;
                //        //singlelist.CountryOfOrigin = item1.CountryOfOrigin;
                //        //singlelist.UnitOfMeasure = item1.UnitOfMeasure;


                //        //lsdetail.Add(singlelist);
                //    }


                //   // lsdetail = packing.GetPackingDetailsBoxNumber(item);
                //}

                //grdContent.ItemsSource = lsdetail;

                //dtLoadUpdate1 = new DispatcherTimer();
                //dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                //dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                ////start the dispacher.
                //dtLoadUpdate1.Start();


                ////grdContent.ItemsSource = lsgrid;
                //txtBoxNumberScanned.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - FillGrid", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                /////  ShowMassagePopup("Please select Box Number For Modify", 4000);
            }

        }


        private void _showPalletNumber()
        {
            try
            {
                WindowThread.start();

                foreach (DataGridRow Row in GetDataGridRows(grdContent))
                {

                    DataGridRow row = (DataGridRow)Row;
                    TextBlock BoxNumber = grdContent.Columns[0].GetCellContent(row) as TextBlock;

                    TextBlock Palletnumber = grdContent.Columns[1].GetCellContent(row) as TextBlock;

                    if (sm.GetPalletInfoByBoxnumberCon(BoxNumber.Text) == "" || sm.GetPalletInfoByBoxnumberCon(BoxNumber.Text) == null)
                    {
                        Palletnumber.Text = "";


                        Row.Background = new SolidColorBrush(Colors.White);

                    }
                    else
                    {
                        Palletnumber.Text = "Added in Pallet -- " + sm.GetPalletInfoByBoxnumberCon(BoxNumber.Text);
                        row.Background = new SolidColorBrush(Colors.Gray);
                        RowCount++;

                        // btnmodifypallet.Visibility = Visibility.Visible;

                    }
                    try
                    {
                        //txtScannSKu.Focus();
                    }
                    catch (Exception Ex)
                    {
                        //Log the Error to the Error Log table
                        ErrorLoger.save("wndBoxInfoForPallet - _showPallet", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                    }

                }

                if (CheckGray(grdContent))
                {
                    btnAddNewPallet.Visibility = Visibility.Visible;
                    btnAddNewPallet.Content = "Print Pallet";
                    btnPrint.Visibility = Visibility.Visible;
                    Global.isAllpack = true;
                }
                else
                {
                    Global.isAllpack = false;
                    btnAddNewPallet.Content = "Add a Pallet";
                    if (sm.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace).Count != 0)
                    {
                        btnAddNewPallet.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnAddNewPallet.Visibility = Visibility.Hidden;
                    }



                    //btnAddNewPallet.Visibility = Visibility.Hidden;
                }

                //Please wait screen abort.
                if (Global.newWindowThread.IsAlive)
                {
                    Global.newWindowThread.Abort();
                }




            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndBoxInfoForPallet - _showPallet", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);

                //ShowMassagePopup("Invalid Barcode", 2000);

            }
        }
        private void dtLoadUpdate1_Tick(object sender, EventArgs e)
        {
            try
            {
                _showPalletNumber();

                dtLoadUpdate1.Stop();

                txtBoxNumberScanned.Focus();

            }
            catch (Exception)
            {

            }
        }


        private void txtBoxNumberScanned_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Enter && txtBoxNumberScanned.Text.Trim() != "")
                {
                    if (txtBoxNumberScanned.Text.Contains("#"))
                    {
                        if (txtBoxNumberScanned.Text == "#addapallet" || txtBoxNumberScanned.Text == "#ADDAPALLET")
                        {
                            txtBoxNumberScanned.Text = "";

                            if (btnAddNewPallet.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAddNewPallet);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                //string ss = "You can not add new box.";
                                ///////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                ////simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                //MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                //mm(ss, 3000);

                            }
                        }
                        if (txtBoxNumberScanned.Text == "#close" || txtBoxNumberScanned.Text == "#CLOSE")
                        {
                            txtBoxNumberScanned.Text = "";

                            if (btnClose.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnClose);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                //string ss = "You can not add new box.";
                                ///////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                ////simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                //MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                //mm(ss, 3000);

                            }
                        }
                        if (txtBoxNumberScanned.Text == "#modifypallet" || txtBoxNumberScanned.Text == "#MODIFYPALLET")
                        {

                            txtBoxNumberScanned.Text = "";

                            if (btnmodifypallet.IsVisible)
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnmodifypallet);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            }
                            else
                            {
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                //string ss = "You can not add new box.";
                                ///////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                ////simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                //MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                //mm(ss, 3000);

                            }

                        }

                    }

                }
                if (txtBoxNumberScanned.Text.Trim() != "" && txtBoxNumberScanned.Text.Trim() != null && e.Key == Key.Enter)
                {


                    //Badge Scanning code
                    if (btnAddNewPallet.Content == "Print Pallet")
                    {
                        List<cstUserMasterTbl> lsUserInfo = Global.controller.GetSelcetedUserMaster(txtBoxNumberScanned.Text);
                        if (lsUserInfo.Count > 0 && lsUserInfo[0].UserID == Global.LoggedUserId)
                        {
                            closeflg = true;
                            ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAddNewPallet);
                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            invokeProv.Invoke();
                        }
                        else
                        {
                            ShowMassagePopup("Invalid Badge", 2000);
                            txtBoxNumberScanned.Text = "";
                        }
                    }

                    //if (carton.GetByBoxNumber(txtBoxNumberScanned.Text.Trim())[0].Printed == 1)
                    //{
                    //    ShowMassagePopup("Label is already printed.", 2000);

                    //    txtBoxNumberScanned.Text = "";
                    //}
                    //else
                    //{


                    //    if (Global.CartonOn == "On")
                    //    {
                    //        Global.counter = 0;
                    //        List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(txtBoxNumberScanned.Text);

                    //        foreach (var item in _packingDetails)
                    //        {
                    //            Global.BoxNumberScanned = txtBoxNumberScanned.Text;
                    //            wndWayfair wayFairlabel = new wndWayfair();
                    //            wayFairlabel.Show();
                    //            Global.counter = Global.counter + 1;
                    //        }


                    //        SavePrinted(txtBoxNumberScanned.Text);
                    //    }


                    //Global.counter = 0;
                    //List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(txtBoxNumberScanned.Text);

                    //foreach (var item in _packingDetails)
                    //{
                    //    Global.BoxNumberScanned = txtBoxNumberScanned.Text;
                    //    wndWayfair wayFairlabel = new wndWayfair();
                    //    wayFairlabel.ShowDialog();
                    //    Global.counter = Global.counter + 1;
                    //}

                    //SavePrinted(txtBoxNumberScanned.Text);

                    //check box already added in to pallet or not
                    if (btnAddNewPallet.Content != "Print Pallet")
                    {
                        if (CheckBoxInvalidForPackageDetail())
                        {
                            ShowMassagePopup("Invalid BoxNumber ", 2000);
                            txtBoxNumberScanned.Text = "";
                            // MessageBox.Show("Invalid BoxNumber");
                        }
                        else
                        {
                            CurrentBox = txtBoxNumberScanned.Text.Trim();
                            if (carton.GetByBoxNumber(txtBoxNumberScanned.Text.Trim())[0].Printed == 1)
                            {
                                ShowMassagePopup("Label is already printed.", 2000);
                                CurrentBox = txtBoxNumberScanned.Text.Trim();
                                txtBoxNumberScanned.Text = "";
                            }
                            else
                            {


                                if (Global.CartonOn == "On")
                                {
                                    Global.counter = 0;
                                    List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(txtBoxNumberScanned.Text);

                                    foreach (var item in _packingDetails)
                                    {
                                        Global.BoxNumberScanned = txtBoxNumberScanned.Text;
                                        wndWayfair wayFairlabel = new wndWayfair();
                                        wayFairlabel.Show();
                                        Global.counter = Global.counter + 1;
                                    }


                                    SavePrinted(txtBoxNumberScanned.Text);
                                }
                            }

                            if (sm.GetpalletDetailsByBoxNumberCon(CurrentBox.ToUpper().Trim()).Count != 0)
                            {
                                ShowMassagePopup("This box is already added. ", 2000);
                                txtBoxNumberScanned.Text = "";
                                //MessageBox.Show("This box is already added.");
                            }
                            else
                            {
                                btnAddNewPallet.Visibility = Visibility.Visible;

                                // btnmodifypallet.Visibility = Visibility.Hidden;

                                cstPalletDetails _palletDetail = new cstPalletDetails();
                                _palletDetail.PalletID = sm.GetPalletInfoByPalletnumberCon(Global.PalletNoForReplace).PalletID; //Global.PalletID;
                                _palletDetail.BoxNumber = CurrentBox;
                                _palletDetail.ShipmentNumber = sm.GetSHNumberByBoxnumber1(CurrentBox);


                                //_palletDetail.lo


                                List<cstPalletDetails> lsBox = new List<cstPalletDetails>();
                                lsBox.Add(_palletDetail);
                                Global.PalletDetailID = Global.controller.SetPalletDetails(lsBox);

                                string palletNumber = Global.PalletNoForReplace;//Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                                BoxAndPallet BoxPall = new BoxAndPallet();
                                BoxPall.BoxNumber = CurrentBox;
                                BoxPall.PalletNumber = Global.PalletNoForReplace;
                                BoxPall.Status = "Added in" + " " + Global.PalletNoForReplace;


                                Global.Pallet.Add(BoxPall);

                                grdContent.ItemsSource = null;

                                grdContent.ItemsSource = Global.Pallet;

                                btnClose.Content = "Submit";

                                //this.Dispatcher.Invoke(new Action(() =>
                                //{
                                //    foreach (DataGridRow row in GetDataGridRows(grdContent))
                                //    {
                                //        TextBlock txtBoxNum = grdContent.Columns[0].GetCellContent(row) as TextBlock;
                                //        if (txtBoxNum.Text == txtBoxNumberScanned.Text.ToUpper().Trim())
                                //        {


                                //            TextBlock txtstatus = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                                //            txtstatus.Text = "Added in Pallet " + " - " + palletNumber;
                                //            row.Background = new SolidColorBrush(Colors.Gray);
                                //            RowCount++;

                                //            btnmodifypallet.Visibility = Visibility.Visible;

                                //            Global.CheckPalletPack = true;
                                //        }
                                //    }
                                //}));

                                //Wayfair commeted as per discussion with Rakesh and Fred 17/12/2014


                                //Global.counter = 0;
                                //List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(txtBoxNumberScanned.Text);

                                //foreach (var item in _packingDetails)
                                //{
                                //    Global.BoxNumberScanned = txtBoxNumberScanned.Text;
                                //    wndWayfair wayFairlabel = new wndWayfair();
                                //    wayFairlabel.Show();
                                //    Global.counter = Global.counter + 1;
                                //}


                                //  SavePrinted(txtBoxNumberScanned.Text);

                                txtBoxNumberScanned.Text = "";
                            }
                        }
                    }

                    //if (CheckGray(grdContent))
                    //{
                    //    btnAddNewPallet.Content = "Scan Badge";
                    //   // closeflg = true;
                    //}

                }
                //}

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - FillGrid", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                ShowMassagePopup("Invalid Scann", 4000);
                txtBoxNumberScanned.Text = "";
            }


        }

        public Boolean CheckBoxInvalid()
        {
            Boolean flag = true;
            try
            {
                foreach (DataGridRow Row in GetDataGridRows(grdContent))
                {
                    TextBlock txtBoxNum = grdContent.Columns[0].GetCellContent(Row) as TextBlock;
                    if (txtBoxNum.Text.ToUpper().Trim() == txtBoxNumberScanned.Text.ToUpper().Trim())
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception)
            {

            }
            return flag;
        }

        public Boolean CheckBoxInvalidForPackageDetail()
        {
            Boolean flag = false;
            try
            {
                if (sm.GetPackingDetailTbl(txtBoxNumberScanned.Text.ToUpper().Trim()).Count == 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {

            }
            return flag;
        }





        public void SavePrinted(String BoxNumber)
        {
            try
            {
                cmdCartonInfo car = new cmdCartonInfo();
                cstCartonInfo _cartonBox = Global.controller.GetAllCartonInfoByBoxNumber(BoxNumber).FirstOrDefault();
                _cartonBox.Printed = 1;
                car.UpdateCartonInfo(_cartonBox);
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - SavePrinted", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }

        }

        private void btnAddNewBox_Click(object sender, RoutedEventArgs e)
        {
            // closeflg = true;
            this.Close();
            //if (sm.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace).Count != 0)
            //{
            //    ShowMassagePopup("Please click on add Pallet/Scan badge", 2000);

            //}
            //else
            //{
            //    MsgBox.Show("Error", "Exit", "close Pallet will Exit ." + Environment.NewLine + "Are you sure want to Close Pallet Info?");
            //    if (Global.MsgBoxResult == "Ok")
            //    {

            //        this.Close();
            //    }
            //}
        }

        public class Gridclass
        {
            public string boxnumber { get; set; }
            public string Status { get; set; }

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

        private void txtWH_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (txtWH.Text.Trim() == "")
            {
                txtBoxNumberScanned.IsEnabled = false;
            }
            else
            {
                Global.WH = txtWH.Text;
                txtBoxNumberScanned.IsEnabled = true;
                // txtBoxNumberScanned.Text = "";
            }
        }


        public Boolean CanClose()
        {
            Boolean flag = false;
            try
            {
                int i = GetDataGridRows(grdContent).Count();
                int printedrow = 0;

                foreach (DataGridRow row in GetDataGridRows(grdContent))
                {
                    TextBlock txtBoxNum = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                    if (txtBoxNum.Text.ToUpper() == "Printed".ToUpper())
                    {
                        printedrow++;
                    }
                }
                if (i == printedrow)
                    flag = true;
            }
            catch (Exception)
            {
            }
            return flag;
        }

        private void txtBoxNumberScanned_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBoxNumberScanned.Text = "";
        }

        private void btnAddNewPallet_Click_1(object sender, RoutedEventArgs e)
        {

            //foreach (DataGridRow row in GetDataGridRows(grdContent))
            //{
            //   // TextBlock txtBoxNum = grdContent.Columns[0].GetCellContent(row) as TextBlock;
            //    if (row.Background == new SolidColorBrush(Colors.Gray))
            //    {
            //        RowCount++;
            //        //TextBlock txtstatus = grdContent.Columns[1].GetCellContent(row) as TextBlock;
            //        //txtstatus.Text = "Added in" + " - " + palletNumber;
            //        // row.Background = new SolidColorBrush(Colors.Gray);
            //    }
            //}

            try
            {

                //if (CheckGray(grdContent))
                //{
                //    Global.palletnumber = Global.PalletNoForReplace;
                //    wndPalletSlip Palletobj = new wndPalletSlip();
                //    Palletobj.ShowDialog();
                //    Global.CheckPalletPack = false;

                //    btnPrint.Visibility = Visibility.Visible;

                //    closeflg = true;
                //    //wndPalletPrintStatus pallet = new wndPalletPrintStatus();
                //    //pallet.ShowDialog();
                //   // this.Close();
                //}
                //else
                //{

                cstPalletInfo pallet = new cstPalletInfo();
                pallet.PalletEndDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());

                List<cstPalletInfo> objPallet = new List<cstPalletInfo>();
                objPallet.Add(pallet);

                Global.controller.UpdateEndTimePallet(objPallet, Global.PalletID);


                Global.palletnumber = Global.PalletNoForReplace;

                cstPalletInfo _boxPackage = new cstPalletInfo();
                _boxPackage.palletCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                _boxPackage.Location = cmdLocalFile.ReadString("Location");
                List<cstPalletInfo> lsBox = new List<cstPalletInfo>();
                lsBox.Add(_boxPackage);
                Global.PalletID = Global.controller.SetPallet(lsBox);

                Global.CheckPalletPack = false;

                // Global.palletnumber = Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                lblPallet.Content = "Your current pallet is  " + "  " + Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                Global.PalletNoForReplace = Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                btnAddNewPallet.Visibility = Visibility.Hidden;

                btnmodifypallet.Visibility = Visibility.Visible;

                wndPalletSlip Palletobj = new wndPalletSlip();
                Palletobj.ShowDialog();

                txtBoxNumberScanned.Focus();

                //ShowMassagePopup("Packing slip printed for Pallet ", 2000);
                //    }
            }
            catch (Exception)
            {

            }



        }


        public Boolean CheckGray(DataGrid ForCheck)
        {
            Boolean check = false;

            int i = GetDataGridRows(grdContent).Count();
            int printegray = 0;
            foreach (DataGridRow row in GetDataGridRows(grdContent))
            {
                TextBlock txtPalletNumber = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                if (txtPalletNumber.Text != "")
                {
                    printegray++;
                }
            }

            if (i == printegray)
            {
                check = true;
            }

            return check;


        }

        public Boolean CheckGrayForOne(DataGrid ForCheck)
        {
            Boolean check = false;

            //int i = GetDataGridRows(grdContent).Count();

            foreach (DataGridRow row in GetDataGridRows(grdContent))
            {
                TextBlock txtPalletNumber = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                if (txtPalletNumber.Text != "")
                {
                    check = true;
                    break;
                    // printegray++;
                }
            }



            return check;


        }


        public void ReturnFromRemove()
        {
            try
            {
                List<cstPalletDetails> PalletDet = ConObj.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace);

                Global.Pallet.Clear();

                foreach (var item in PalletDet)
                {
                    BoxAndPallet BoxPall = new BoxAndPallet();

                    BoxPall.BoxNumber = item.BoxNumber;
                    BoxPall.PalletNumber = Global.PalletNoForReplace;
                    BoxPall.Status = "Added in" + " " + Global.PalletNoForReplace;

                    Global.Pallet.Add(BoxPall);
                }

                grdContent.ItemsSource = null;

                grdContent.ItemsSource = Global.Pallet;

                btnClose.Content = "Submit";

            }
            catch (Exception)
            {


            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                //if (sm.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace).Count == 0)
                //{
                //    ShowMassagePopup("Pallet is empty", 2000);
                //}
                //else
                //{

               // Global.PalletNoForReplace = Global.palletnumber;

                wndModifyPallet nw = new wndModifyPallet();

                nw.RaiseCustomEvent += new EventHandler<wndModifyPallet.myEventArgs>(nw_raiseevnt);

                nw.RaiseCustomEvent3 += new EventHandler<wndModifyPallet.myEventArgs3>(nw_raiseevnt3);

                nw.ShowDialog();

                // If it returns from Remove Boxes from Pallet page
                if (Global.FromRemove == true)
                {
                    ReturnFromRemove();
                    Global.FromRemove = false;
                }



                txtBoxNumberScanned.Focus();
                //}
            }
            catch (Exception)
            {

            }
        }

        private void nw_raiseevnt(object sender, wndModifyPallet.myEventArgs e)
        {

            try
            {
                //try
                //{
                //    if (Global.newWindowThread.IsAlive)
                //    {
                //        Global.newWindowThread.Abort();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                if (e.massage == "Remove")
                {
                    lblPallet.Content = "Your current pallet is " + "   " + Global.PalletNoForReplace;
                    btnAddNewPallet.Visibility = Visibility.Visible;
                    //Global.PalletNoForReplace










                    grdContent.ItemsSource = null;

                    grdContent.ItemsSource = Global.Pallet;


                    //fillGrid(Global.ShippingNumber);




                }
                else
                {


                    if (Global.deleteflag == "deletesingle")
                    {
                        //// cstBoxPallet bxp=new cstBoxPallet();
                        //  cmdPallet dd=new cmdPallet();
                        // grdContent.ItemsSource = dd.GetBoxInforamtionByShipment(Global.ShippingNumber);

                        // Global.PalletNoForReplace = e.massage;

                        cstPalletInfo _boxPackage = new cstPalletInfo();
                        _boxPackage.palletCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                        _boxPackage.Location = cmdLocalFile.ReadString("Location");
                        List<cstPalletInfo> lsBox = new List<cstPalletInfo>();
                        lsBox.Add(_boxPackage);
                        Global.PalletID = Global.controller.SetPallet(lsBox);

                        lblPallet.Content = "Your current pallet is " + "    " + Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                        Global.PalletNoForReplace = Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;
                        Global.CheckPalletPack = false;
                        btnAddNewPallet.Visibility = Visibility.Hidden;

                        grdContent.ItemsSource = null;

                        grdContent.ItemsSource = Global.Pallet;


                        //fillGrid(Global.ShippingNumber);
                    }
                    else
                    {
                        try
                        {
                            cstPalletInfo _boxPackage = new cstPalletInfo();
                            _boxPackage.palletCreatedTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());
                            _boxPackage.Location = cmdLocalFile.ReadString("Location");
                            List<cstPalletInfo> lsBox = new List<cstPalletInfo>();
                            lsBox.Add(_boxPackage);
                            Global.PalletID = Global.controller.SetPallet(lsBox);

                            lblPallet.Content = "Your current pallet is " + "   " + Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;

                            Global.PalletNoForReplace = Global.controller.GetPalletInfoByPalletID(Global.PalletID).PalletNumber;
                            Global.CheckPalletPack = false;
                            btnAddNewPallet.Visibility = Visibility.Hidden;


                            //fillGrid(Global.ShippingNumber);
                            txtBoxNumberScanned.Focus();
                            txtWH.Text = CheckWH(Global.ShippingNumber).Trim();
                        }
                        catch (Exception Ex)
                        {
                            ErrorLoger.save("wndBoxInfo - Page_load", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                            ShowMassagePopup("Page Load error occurs", 4000);
                        }
                    }
                    Global.deleteflag = "";
                }
                //Please wait screen abort.


                //if (CheckGrayForOne(grdContent))
                //{
                //    btnmodifypallet.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    btnmodifypallet.Visibility = Visibility.Hidden;
                //}
            }
            catch (Exception)
            {

            }




        }
        private void nw_raiseevnt3(object sender, wndModifyPallet.myEventArgs3 e)
        {

            try
            {
                Global.PalletNoForReplace = e.massage;

                lblPallet.Content = "Your current pallet is" + "   " + Global.PalletNoForReplace;

                Global.CheckPalletPack = true;
                //fillGrid(Global.ShippingNumber);
                //Please wait screen abort.
                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}
                //if (CheckGrayForOne(grdContent))
                //{
                //    btnmodifypallet.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    btnmodifypallet.Visibility = Visibility.Hidden;
                //}
            }
            catch (Exception)
            {


            }



        }

        ////msbx

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
            txtBoxNumberScanned.Focus();

        }

        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate.Stop();
            txtBoxNumberScanned.Focus();
        }
        #endregion

        private void grdContent_GotFocus_1(object sender, RoutedEventArgs e)
        {
            txtBoxNumberScanned.Focus();
        }

        private void btnPrint_Click_1(object sender, RoutedEventArgs e)
        {
            wndPalletPrintStatus objprint = new wndPalletPrintStatus();
            objprint.ShowDialog();

            this.Close();
        }

        public class BoxAndPallet
        {
            public string BoxNumber { get; set; }
            public string PalletNumber { get; set; }
            public string Status { get; set; }
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                btnON.Visibility = Visibility.Hidden;
                lblOFF.Visibility = Visibility.Hidden;

                btnOFF.Visibility = Visibility.Visible;
                lblON.Visibility = Visibility.Visible;

                Global.CartonOn = "Off";
                txtBoxNumberScanned.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentScan - Label_MouseLeftButtonDown_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }


        }

        private void lblON_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                btnOFF.Visibility = Visibility.Hidden;
                lblON.Visibility = Visibility.Hidden;

                btnON.Visibility = Visibility.Visible;
                lblOFF.Visibility = Visibility.Visible;

                Global.CartonOn = "On";

                txtBoxNumberScanned.Focus();

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentScan - lblON_MouseLeftButtonDown_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }
    }
}
