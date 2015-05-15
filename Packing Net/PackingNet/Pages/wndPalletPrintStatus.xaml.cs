using Packing_Net.Classes;
using PackingClassLibrary;
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
    /// Interaction logic for wndPalletPrintStatus.xaml
    /// </summary>
    public partial class wndPalletPrintStatus : Window
    {
        smController _Contro = new smController();
        List<cstPalletInfo> _lspallet = new List<cstPalletInfo>();

        List<cstPackageTbl> lstpackage = new List<cstPackageTbl>();
        DispatcherTimer dtLoadUpdate1;
        DispatcherTimer dtLoadUpdate2;
        int cartoncount; 


        public wndPalletPrintStatus()
        {
            InitializeComponent();
        }

        private void btnAddNewBox_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            try
            {





                cartoncount = 0;
                
                _lspallet = _Contro.GetPalletInfoBySHNumber(Global.ShippingNumber);
                grdContent.ItemsSource = _lspallet;



                lstpackage = _Contro.GetPackingList(Global.ShippingNumber, _Contro.ApplicationLocation());

                txtBOLNumber.Text = lstpackage[0].PLT_BOL;
                txtCarrierName.Text = lstpackage[0].PLT_carrier;
                txtPRONumber.Text = lstpackage[0].PLT_PRO;

                dtLoadUpdate2 = new DispatcherTimer();
                dtLoadUpdate2.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dtLoadUpdate2.Tick += dtLoadUpdate2_Tick;
                //start the dispacher.
                dtLoadUpdate2.Start();



                Global.ShipmentNumberforferguson = Global.ShippingNumber;
                //Please wait screen abort.
                if (Global.newWindowThread.IsAlive)
                {
                    Global.newWindowThread.Abort();
                }
            }
            catch (Exception ex)
            {

                ShowMassagePopup(ex.Message, 3000);
            }

        }

        private void dtLoadUpdate2_Tick(object sender, EventArgs e)
        {
            try
            {
                _showPrinted();
                dtLoadUpdate2.Stop();
            }
            catch (Exception)
            {

            }
        }




        private void _showPrinted()
        {
            try
            {
                //WindowThread.start();

                foreach (DataGridRow Row in GetDataGridRows(grdContent))
                {

                    DataGridRow row = (DataGridRow)Row;
                    TextBlock Palletnumber = grdContent.Columns[0].GetCellContent(row) as TextBlock;

                    TextBlock printStatus = grdContent.Columns[1].GetCellContent(row) as TextBlock;

                    if (printStatus.Text == "1")
                    {
                        printStatus.Text = "Printed";
                        cartoncount++;
                    }


                    
                    //btnAddNewPallet.Visibility = Visibility.Hidden;
                }

           }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndPalletPrintStatus - _showPrinted", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);

                //ShowMassagePopup("Invalid Barcode", 2000);

            }
        }

        private void txtBoxNumberScanned_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBoxNumberScanned.Text = "";
        }

        private void txtBoxNumberScanned_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Enter)
                {
                    //this.Dispatcher.Invoke(new Action(() =>
                    //{

                    Boolean InvalidPallet = false;

                    foreach (DataGridRow row in GetDataGridRows(grdContent))
                    {
                        TextBlock txtBoxNum = grdContent.Columns[0].GetCellContent(row) as TextBlock;
                        if (txtBoxNum.Text == txtBoxNumberScanned.Text.ToUpper().Trim())
                        {
                            TextBlock txtstatus = grdContent.Columns[1].GetCellContent(row) as TextBlock;

                            if (txtstatus.Text == "Printed")
                            {
                                ShowMassagePopup("Already Printed.", 3000);
                            }
                            else
                            {
                                TextBlock txtstatus1 = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                                txtstatus1.Text = "Printed";

                                InvalidPallet = true;

                               

                                cstPackageTbl packing = Global.controller.GetPackingList(lstpackage[0].PackingId, true);
                                packing.PLT_BOL = txtBOLNumber.Text.Trim();
                                packing.PLT_carrier = txtCarrierName.Text.Trim();
                                packing.PLT_PRO = txtPRONumber.Text.Trim();
                                List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                                _lsNewPacking.Add(packing);

                                Guid palletID = Guid.NewGuid();

                                foreach (var item in _lspallet)
                                {
                                    if (txtBoxNum.Text == item.PalletNumber && txtBoxNumberScanned.Text.ToUpper().Trim() == item.PalletNumber)
                                    {
                                        palletID = item.PalletID;
                                    }
                                }



                                Global.controller.SetPackingTable(_lsNewPacking, lstpackage[0].PackingId);

                                cstPalletInfo pallet=new cstPalletInfo();
                                pallet.PrintStatus = 1;

                                List<cstPalletInfo> objPallet=new List<cstPalletInfo>();
                                objPallet.Add(pallet);

                                _Contro.UpdatePallet(objPallet, palletID);


                                //string pallet = txtBoxNum.Text.Split(new char[] { 'PLT' }).Last();

                                int Original = 0;
                                int Original1 = 0;
                                int Original2 = 0;
                                int Original3 = 0;
                                int Original4 = 0;
                                int Original5 = 0;
                                int Original6 = 0;

                                int FinalNumber;


                                string mystring = txtBoxNum.Text.Substring(txtBoxNum.Text.Length - 7);

                                for (int i = 0; i < 6; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            string f = mystring[i].ToString();
                                            Original = Convert.ToInt32(f) * 3;


                                            break; /* optional */
                                        case 1:
                                            Original1 = Convert.ToInt16(mystring[i].ToString()) * 1;
                                            break; /* optional */

                                        case 2:
                                            Original2 = Convert.ToInt16(mystring[i].ToString()) * 3;
                                            break;

                                        case 3:
                                            Original3 = Convert.ToInt16(mystring[i].ToString()) * 1;
                                            break; /* optional */
                                        case 4:
                                            Original4 = Convert.ToInt16(mystring[i].ToString()) * 3;
                                            break; /* optional */

                                        case 5:
                                            Original5 = Convert.ToInt16(mystring[i].ToString()) * 1;
                                            break;

                                        case 6:
                                            Original6 = Convert.ToInt16(mystring[i].ToString()) * 3;
                                            break;

                                        /* you can have any number of case statements */
                                        default: /* Optional */
                                            break;
                                    }
                                }

                                FinalNumber = Original + Original1 + Original2 + Original3 + Original4 + Original5 + Original6;

                                int FinalTotal = 71 + FinalNumber;
                                int temp = 0;

                                if (FinalTotal % 10 == 0)
                                {
                                    temp = 0;
                                }
                                else
                                {
                                    temp = ((FinalTotal / 10) + 1) * 10 - FinalTotal;
                                }

                                Global.ssccNumber = "00181267" + mystring + temp;


                                Global.TotalSKUQuantity = _Contro.GetTotalSKUByPalletNUmberCon(txtBoxNumberScanned.Text.Trim());
                                Global.BOLNumber = txtBOLNumber.Text;
                                Global.PRONumber = txtPRONumber.Text;
                                Global.CarrierName = txtCarrierName.Text;

                                int palletcount = cartoncount + 1;

                                Global.Carton = palletcount + " of " + grdContent.Items.Count;
                                txtBoxNumberScanned.Text = "";
                                wndPalletInfo pall = new wndPalletInfo();
                                // Global.palletnumber = Global.ShippingNumber;
                                pall.ShowDialog();
                            }

                        }
                        //else
                        //{
                        //    ShowMassagePopup("Invalide Pallet Number", 3000);
                        //    txtBoxNumberScanned.Text = "";
                        //}
                    }

                    if (!InvalidPallet)
                    {
                         ShowMassagePopup("Invalide Pallet Number.", 3000);
                         txtBoxNumberScanned.Text = "";
                         InvalidPallet = false;
                    }


                    if (CanClose())
                    {
                        ShowMassagePopup("All Labels are printed.", 2000);
                        this.Close();
                    }

                    //  }));
                }
            }
            catch (Exception)
            {


            }

           
        }

        #region Message
        ////Function For Showing New Message Box 25-11-2014

        public void ShowMassagePopup(string message, int TimeSpan)
        {
            //  brdMessage.Background = new SolidColorBrush(Colors.Black);
            //brdfrist.Background = new SolidColorBrush(Colors.Black);
            /// brdMessage.Opacity = 0.5;

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
        private void dtLoadUpdate1_Tick(object sender, EventArgs e)
        {
            //  brdfrist.Background = new SolidColorBrush(Colors.White);
            ////brdfrist.Background = new SolidColorBrush(Colors.Black);
            //  brdfrist.BorderThickness = new Thickness(4,0,0, 0);
            // brdMessage.Opacity = 1;
            // brdMessage.Background = new SolidColorBrush(Colors.Transparent);
            brdMessage2.Visibility = System.Windows.Visibility.Hidden;
            dtLoadUpdate1.Stop();

        }
        #endregion
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
        private void txtSHNumber_KeyDown_1(object sender, KeyEventArgs e)
        {
           
            //if (e.Key == Key.Enter)
            //{
            //   List<cstPalletInfo> _lspallet = new List<cstPalletInfo>();
            //    _lspallet=_Contro.GetPalletInfoBySHNumber(txtSHNumber.Text);
            //    grdContent.ItemsSource = _lspallet;

            //    Global.ShipmentNumberforferguson = txtSHNumber.Text;

            //}
        }

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

        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            txtBoxNumberScanned.Focus();
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
                        if (txtScannSKu.Text == "#close" || txtScannSKu.Text == "#CLOSE")
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
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not add new box.";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                //MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                //mm(ss, 3000);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //private void btnPrint_Click_1(object sender, RoutedEventArgs e)
        //{

        //    //foreach (var item in collection)
        //    //{
                
        //    //}


        //    wndPalletInfo pall = new wndPalletInfo();
        //    //Global.palletnumber = txtSHNumber.Text;
        //    pall.ShowDialog();
        //}
    }
}
