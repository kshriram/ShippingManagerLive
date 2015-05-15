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
using System.Windows.Automation.Provider;
using System.Windows.Automation.Peers;
using System.Windows.Interop;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndRemoveBoxesFromPallet.xaml
    /// </summary>
    public partial class wndRemoveBoxesFromPallet : Window
    {
        DispatcherTimer dtLoadUpdate;
        smController ConObj = new smController();

        List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet> PalletCompare = new List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet>();

        public Boolean closeflg = false;

        public event EventHandler<myEventArgs1> RaiseCustomEvent1;

        public wndRemoveBoxesFromPallet()
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
        private void btnExitShipment_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.FromRemove = true;

                SplashScreen splashScreen = new SplashScreen("1235.png");
                splashScreen.Show(true);
                closeflg = true;
               // WindowThread.start();
                RaiseCustomEvent1(this, new myEventArgs1("Remove"));
                splashScreen.Show(false);
                this.Close();
            }
            catch (Exception)
            {
               
            }

           
        }

        private void btnCancelShipment_Click_1(object sender, RoutedEventArgs e)
        {
            Global.PalletNoForReplace = Global.palletnumber;
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            PalletCompare = Global.Pallet;
            try
            {
                this.Closing += new System.ComponentModel.CancelEventHandler(MyWindow_Closing);

                //lblShipmentNo.Content = "Shipment Number " + Global.ShippingNumber;

                lblBoxNumber.Content = Global.PalletNoForReplace;

                List<cstPalletDetails> PalletDet = ConObj.GetPalletDetailByPalletNumberCon(Global.PalletNoForReplace);

                grdContentRemove.ItemsSource = PalletDet;
                ShowMassagePopup("please scan Box for remove from pallet. ", 3000);
                txtScannSKu.Focus();

                dtLoadUpdate = new DispatcherTimer();
                dtLoadUpdate.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dtLoadUpdate.Tick += dtLoadUpdate_Tick1;
                //start the dispacher.
                dtLoadUpdate.Start();


                //Please wait screen abort.
                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}
            }
            catch (Exception ex)
            {
                ShowMassagePopup(ex.Message, 3000);
            }
        }

        private void dtLoadUpdate_Tick1(object sender, EventArgs e)
        {
            try
            {
                _showBarcode();

                dtLoadUpdate.Stop();

                txtScannSKu.Focus();

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndRemoveBOXFromPallet - dtLoadUpdate_Tick", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
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



                    //clGlobal.call.SKUnameToUPCCode(SKUNo.Text.ToString());
                    ContentPresenter sp = grdContentRemove.Columns[2].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate = sp.ContentTemplate;
                    Image ImgbarcodSet = (Image)myDataTemplate.FindName("imgBarCode", sp);
                    System.Drawing.Image Barcodeimg = null;

                    var sBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, SKUNo.Text, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);

                    var bitmapBox = new System.Drawing.Bitmap(sBoxNumber);

                    var bBoxSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBox.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    bitmapBox.Dispose();

                    //imgBoxNumber.Source = bBoxSource;

                    //try
                    //{
                    //    Barcodeimg = b.Encode(BarcodeLib.TYPE.UPCA, SKUNo.Text, System.Drawing.Color.Black, System.Drawing.Color.White, 300, 60);
                    //}
                    //catch (Exception Ex)
                    //{
                    //    //Log the Error to the Error Log table
                    //    ErrorLoger.save("wndShipmentDetailPage - _showBarcode_Sub1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                    //}
                    //BitmapImage bi = new BitmapImage();
                    //bi.BeginInit();
                    //MemoryStream ms = new MemoryStream();

                    //// Save to a memory stream...
                    //Barcodeimg.Save(ms, ImageFormat.Bmp);

                    //// Rewind the stream...
                    //ms.Seek(0, SeekOrigin.Begin);

                    //// Tell the WPF image to use this stream...
                    //bi.StreamSource = ms;
                    //bi.EndInit();
                    ImgbarcodSet.Source = bBoxSource;

                   // imgBoxNumber.Source = bBoxSource;

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
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                // ErrorLoger.save("wndShipmentDetailPage - _showBarcode", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
               // ErrorLoger.save("wndRemoveSKUFromBox - _showBarcode()", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
               // ShowMassagePopup("Invalid Barcode", 2000);

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
            catch (Exception)
            {
                ShowMassagePopup("Invalid Operation Closing", 4000);
            }
        }


        private void yes_Click_1(object sender, RoutedEventArgs e)
        {
            txtScannSKu.Focus();
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
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not add new box.";
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
                                //ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                string ss = "You can not add new box.";
                                /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                                MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                                mm(ss, 3000);

                            }
                        }
                    }
                    if (!CheckBox())
                    {
                        ShowMassagePopup(txtScannSKu.Text + " Invalid Box  ", 3000);
                        txtScannSKu.Text = "";
                    }
                    else
                    {
                        MsgBox.Show("Error", "Remove", "Remove Box will remove its information from Pallet." + Environment.NewLine + "Are you sure want to remove Box?");
                        if (Global.MsgBoxResult == "Ok")
                        {

                            ConObj.RemoveBoxFromPalletCon(txtScannSKu.Text.ToUpper().Trim());

                            //cstPackageTbl packing = Global.controller.GetPackingList(Global.PackingID, true);
                            //packing.PackingStatus = 1;
                            //List<cstPackageTbl> _lsNewPacking = new List<cstPackageTbl>();
                            //_lsNewPacking.Add(packing);

                            //Global.controller.SetPackingTable(_lsNewPacking, Global.PackingID);



                            txtScannSKu.Focus();

                            foreach (DataGridRow row in GetDataGridRows(grdContentRemove))
                            {
                                TextBlock txtBoxNumber = grdContentRemove.Columns[0].GetCellContent(row) as TextBlock;
                                TextBlock txtStatus = grdContentRemove.Columns[1].GetCellContent(row) as TextBlock;

                                if (txtStatus.Text == "Removed")
                                {
                                    ShowMassagePopup(txtScannSKu.Text + " allready removed. ", 3000);
                                }
                                else
                                {
                                    if (txtBoxNumber.Text.ToUpper().Trim() == txtScannSKu.Text.ToUpper().Trim())
                                    {
                                        txtStatus.Text = "Removed";

                                        btnCancelShipment.Visibility = Visibility.Hidden;

                                        ShowMassagePopup(txtScannSKu.Text + " Box is Removed ", 4000);


                                        List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet> PalletCompareNew = new List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet>();
                                        foreach (var item in Global.Pallet)
                                        {
                                            PalletCompareNew.Add(item);
                                        }

                                        foreach (var item in Global.Pallet)
                                        {
                                            if (item.BoxNumber.ToUpper().Trim() == txtBoxNumber.Text.ToUpper().Trim())
                                            {

                                                PalletCompare.Remove(item);

                                            }
                                        }

                                        Global.Pallet = PalletCompare;

                                    }
                                }
                            }

                        }
                    }
                    txtScannSKu.Text = "";

                }
            }
            catch (Exception)
            {
               
            }


          
        }



        private Boolean CheckBox()
        {
            Boolean flag = false;

            try
            {
                foreach (DataGridRow row in GetDataGridRows(grdContentRemove))
                {
                    TextBlock txtBoxNumber = grdContentRemove.Columns[0].GetCellContent(row) as TextBlock;

                    if (txtBoxNumber.Text.ToUpper().Trim() == txtScannSKu.Text.ToUpper().Trim())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return flag;
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

        //private void mbox_ok(object sender, RoutedEventArgs e)
        //{

        //}

        private void grdContentRemove_GotFocus_1(object sender, RoutedEventArgs e)
        {
            txtScannSKu.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

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
    }
}
