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


using System.Windows.Controls.Primitives;

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
//using System.Windows.Forms;
//using System.Data;


namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndReprintAll.xaml
    /// </summary>
    public partial class wndReprintAll : Window
    {

        DispatcherTimer dtLoadUpdate;

        DispatcherTimer dtLoadUpdate1;

        int cartoncount;

        smController sm = new smController();
        public class test
        {
            //public string ShipmentNumber { get; set; }
            //public string PalletNumber { get; set; }
            //public string BOL { get; set; }
            //public string Carrier { get; set; }
            //public string PRONumber { get; set; }
            /// public string Count { get; set; }
            //public string BoxType { get; set; }
            //public Double BoxWeight { get; set; }
            //public Double BoxLength { get; set; }
            public string ShipmentNumber { get; set; }

            public string PalletNumber { get; set; }
            public string PLT_Carrier { get; set; }
            public string PLT_BOL { get; set; }
            public string PLT_PRO { get; set; }

            public string Location { get; set; }
            public Guid PalletID { get; set; }
        }


        List<PaaletDetail> ship = new List<PaaletDetail>();
        List<test> ss = new List<test>();
        public wndReprintAll()
        {
            try
            {
                InitializeComponent();
                Css();
            }
            catch (Exception)
            {
            }
        }
        public void Css()
        {
            PackingClassLibrary.ReportController ReportObj = new PackingClassLibrary.ReportController();

            if (Global.BOLProcessFlag == "MyBol")
            {
                txtBol.Text = "My pending shipment's";
                ship = ReportObj.GetAllShipmentAndPalletPerStationRC(Global.controller.GetStationMasterByName(Global.StationName).StationID);
            }
            else
            {
                txtBol.Text = "Pending shipment's";
                ship = ReportObj.GetAllShipmentAndPallet();
            }



            if (ship.Count > 0)
            {

                grdtest.ItemsSource = ship;


                //grdtest.ItemsSource = Customers;

                dtLoadUpdate1 = new DispatcherTimer();
                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                //start the dispacher.
                dtLoadUpdate1.Start();

            }
            else
            {
                if (Global.BOLProcessFlag == "MyBol")
                {
                    txtBol.Text = "No my peniding shipment's";
                }
                else
                {
                    txtBol.Text = "No peniding shipment's";
                }
                //MessageBox.Show("No Peniding Shipment's Found.");

            }

        }

        private void txtScanned_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                PackingClassLibrary.ReportController ReportObj = new PackingClassLibrary.ReportController();

                if (e.Key == Key.Enter && txtScanned.Text.Trim() != "")
                {
                    if (txtScanned.Text.Trim().Contains("PLT"))
                    {
                        if (Global.BOLProcessFlag == "MyBol")
                        {
                            ship = ReportObj.GetAllShipmentAndPalletByPalletWithStationRC(txtScanned.Text.Trim(), Global.controller.GetStationMasterByName(Global.StationName).StationID);

                            if (ship.Count > 0)
                            {

                                grdtest.ItemsSource = ship;

                                btnDefault.Visibility = Visibility.Visible;

                                //grdtest.ItemsSource = Customers;

                                dtLoadUpdate1 = new DispatcherTimer();
                                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                                //start the dispacher.
                                dtLoadUpdate1.Start();
                                txtScanned.Text = "";

                            }
                            else
                            {
                                // txtBol.Text = "No Shipment Found.";
                                MessageBox.Show("This pallet doesn't have information.");
                                txtScanned.Text = "";

                            }

                        }
                        else
                        {
                            ship = ReportObj.GetAllShipmentAndPalletByPalletRC(txtScanned.Text.Trim());

                            if (ship.Count > 0)
                            {

                                grdtest.ItemsSource = ship;

                                btnDefault.Visibility = Visibility.Visible;

                                //grdtest.ItemsSource = Customers;

                                dtLoadUpdate1 = new DispatcherTimer();
                                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                                //start the dispacher.
                                dtLoadUpdate1.Start();
                                txtScanned.Text = "";

                            }
                            else
                            {
                                // txtBol.Text = "No Shipment Found.";
                                MessageBox.Show("This pallet doesn't have information.");
                                txtScanned.Text = "";
                            }

                        }
                    }
                    else if (txtScanned.Text.Trim().Contains("SH"))
                    {
                        if (Global.BOLProcessFlag == "MyBol")
                        {
                            ship = ReportObj.GetAllShipmentAndPalletByShipmentWithStationRC(txtScanned.Text.Trim(), Global.controller.GetStationMasterByName(Global.StationName).StationID);

                            if (ship.Count > 0)
                            {

                                grdtest.ItemsSource = ship;

                                btnDefault.Visibility = Visibility.Visible;

                                //grdtest.ItemsSource = Customers;

                                dtLoadUpdate1 = new DispatcherTimer();
                                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                                //start the dispacher.
                                dtLoadUpdate1.Start();
                                txtScanned.Text = "";

                            }
                            else
                            {
                                //txtBol.Text = "No Shipment Found.";
                                MessageBox.Show("This shipment doesn't have information.");
                                txtScanned.Text = "";
                            }

                        }
                        else
                        {
                            ship = ReportObj.GetAllShipmentAndPalletByShipmentRC(txtScanned.Text.Trim());

                            if (ship.Count > 0)
                            {

                                grdtest.ItemsSource = ship;

                                btnDefault.Visibility = Visibility.Visible;

                                //grdtest.ItemsSource = Customers;

                                dtLoadUpdate1 = new DispatcherTimer();
                                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                                //start the dispacher.
                                dtLoadUpdate1.Start();
                                txtScanned.Text = "";
                            }
                            else
                            {
                                // txtBol.Text = "No Shipment Found.";
                                MessageBox.Show("This shipment doesn't have information.");
                                txtScanned.Text = "";
                            }

                        }

                    }
                    else
                    {

                        if (txtScanned.Text.Trim().Contains("#close") || txtScanned.Text.Trim().Contains("#CLOSE"))
                        {
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sorry..Invalid Scan.");
                            txtScanned.Text = "";
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }






        private void dtLoadUpdate1_Tick(object sender, EventArgs e)
        {
            foreach (DataGridRow row in GetDataGridRows(grdtest))
            {
                TextBlock txtPalletNumber = grdtest.Columns[0].GetCellContent(row) as TextBlock;

                if (txtPalletNumber.Text == "" || txtPalletNumber.Text == null)
                {
                    //TextBox txtBolNumber = grdtest.Columns[2].GetCellContent(row) as TextBox;

                    ContentPresenter cp = grdtest.Columns[2].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate = cp.ContentTemplate;
                    TextBox txtBolNumber = (TextBox)myDataTemplate.FindName("txtstkTxt", cp);


                    txtBolNumber.Visibility = Visibility.Hidden;

                    ContentPresenter cp1 = grdtest.Columns[3].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate1 = cp1.ContentTemplate;
                    TextBox txtCarrier = (TextBox)myDataTemplate1.FindName("txtCarrier", cp1);

                    // TextBox txtCarrier = grdtest.Columns[3].GetCellContent(row) as TextBox;

                    txtCarrier.Visibility = Visibility.Hidden;

                    ContentPresenter cp2 = grdtest.Columns[4].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate2 = cp2.ContentTemplate;
                    TextBox txtProNumber = (TextBox)myDataTemplate2.FindName("txtPRONumber", cp2);



                    //TextBox txtProNumber = grdtest.Columns[4].GetCellContent(row) as TextBox;

                    txtProNumber.Visibility = Visibility.Hidden;


                    ContentPresenter sp = grdtest.Columns[6].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate21 = sp.ContentTemplate;
                    Button btn = (Button)myDataTemplate21.FindName("btnadd", sp);


                    btn.Visibility = Visibility.Hidden;


                    ContentPresenter sp3 = grdtest.Columns[7].GetCellContent(row) as ContentPresenter;
                    DataTemplate myDataTemplate213 = sp3.ContentTemplate;
                    Button Print = (Button)myDataTemplate213.FindName("btnprint", sp3);


                    Print.Visibility = Visibility.Hidden;




                }
            }
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


        public System.Windows.Controls.DataGridCell GetCell(int row, int column)
        {

            DataGridRow rowContainer = GetRow(row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                System.Windows.Controls.DataGridCell cell = (System.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    grdtest.ScrollIntoView(rowContainer, grdtest.Columns[column]);
                    cell = (System.Windows.Controls.DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;


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
        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)grdtest.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                grdtest.UpdateLayout();
                grdtest.ScrollIntoView(grdtest.Items[index]);
                row = (DataGridRow)grdtest.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)sender);
                //TextBox tb = (TextBox)((Grid)parent).Children[0];
                //tb.Text = "5";
                //DataGrid dr=

                //  string shipping = grdtest.
                //Global.ShippingNumber = shipping;

                //I have specified rowIndex as 2 as an example

                int selectedindex = grdtest.SelectedIndex;

                System.Windows.Controls.DataGridCell cell = GetCell(selectedindex, 1) as System.Windows.Controls.DataGridCell;

                // string valu = cell.t

                DataGridRow rowContainer = GetRow(selectedindex);

                TextBlock ProgressFlag = grdtest.Columns[1].GetCellContent(rowContainer) as TextBlock;

                string ssss = ProgressFlag.Text;


                // TextBlock CntPersenter = cell.FindName("txtRGANumber") as TextBlock;

                //Presenter CntPersenter1 = cell. as ContentPresenter;
                // cell.Style.

                //ContainerControl con=CntPersenter.fin



                //DataTemplate DataTemp = CntPersenter.con

                //  TextBlock txtReturnGuid = CntPersenter.FindName("txtRGANumber") as TextBlock;//(TextBlock)FindName("txtRGANumber", CntPersenter);
                // string d = CntPersenter.ContentStringFormat;
                //DataTemplate DataTemp = CntPersenter.ContentTemplate;
                ///  System.Windows.Controls.TextBox txtReturnGuid = (System.Windows.Controls.TextBox)DataTemp.FindName("txtRGANumber", CntPersenter);

                Global.ShippingNumber = ssss;


                wndBoxInfoForPallet ss = new wndBoxInfoForPallet();
                ss.ShowDialog();
                ////Global.ssccNumber = "00181267" + mystring + temp;


                ////Global.TotalSKUQuantity = _Contro.GetTotalSKUByPalletNUmberCon(txtBoxNumberScanned.Text.Trim());
                ////Global.BOLNumber = txtBOLNumber.Text;
                ////Global.PRONumber = txtPRONumber.Text;
                ////Global.CarrierName = txtCarrierName.Text;
                ////Global.Carton = cartoncount++ + " of " + grdContent.Items.Count;
                ////txtBoxNumberScanned.Text = "";
                ////wndPalletInfo pall = new wndPalletInfo();
                ////// Global.palletnumber = Global.ShippingNumber;
                ////pall.ShowDialog();
            }
            catch (Exception)
            {
            }
        }
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
            //if (IsManualEntry == false)
            //{
            //    txtScannSKu.Focus();
            //}
            //else
            //{
            //    txtQunatity.Focus();
            //}
        }
        private void mbox_ok(object sender, RoutedEventArgs e)
        {
            try
            {
                /// brdMessage.Opacity = 1;
                /// brdMessage.Background = new SolidColorBrush(Colors.Transparent);
                brdMessage2.Visibility = System.Windows.Visibility.Hidden;
                dtLoadUpdate.Stop();
                // txtScannSKu.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndShipmentDetailPage - mbox_ok", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time") + "]" + Ex.ToString(), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time"), Global.LoggedUserId);
            }

        }
        private void btnadd_Click_1(object sender, RoutedEventArgs e)
        {
            int selectedindex = grdtest.SelectedIndex;
            DataGridRow rowContainer = GetRow(selectedindex);

            //// TextBox ProgressFlag = grdtest.Columns[1].GetCellContent(rowContainer) as TextBox;

            // TextBox myTextBlock = (TextBox)grdtest.  FindName("txtPRONumber");
            // string  kk = grdtest.Columns[2].GetCellContent(rowContainer).ToString();
            TextBlock pallet = grdtest.Columns[0].GetCellContent(rowContainer) as TextBlock;
            TextBlock Ship = grdtest.Columns[1].GetCellContent(rowContainer) as TextBlock;

            ContentPresenter CntPersenter = grdtest.Columns[2].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp = CntPersenter.ContentTemplate;
            TextBox Bol = (TextBox)DataTemp.FindName("txtstkTxt", CntPersenter);

            ContentPresenter CntPersenter2 = grdtest.Columns[3].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp2 = CntPersenter2.ContentTemplate;
            TextBox txtCarrier = (TextBox)DataTemp2.FindName("txtCarrier", CntPersenter2);

            ContentPresenter CntPersenter3 = grdtest.Columns[4].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp3 = CntPersenter3.ContentTemplate;
            TextBox txtPRONumber = (TextBox)DataTemp3.FindName("txtPRONumber", CntPersenter3);
            TextBox txtPRONumberPalletID = (TextBox)DataTemp3.FindName("txtPalletID", CntPersenter3);


            //ContentPresenter CntPersenter4 = grdtest.Columns[5].GetCellContent(rowContainer) as ContentPresenter;
            //DataTemplate DataTemp4 = CntPersenter4.ContentTemplate;
            //TextBox txtPalletID = (TextBox)DataTemp4.FindName("txtPalletID", CntPersenter4);

            List<cstPalletInfo> lspall = new List<cstPalletInfo>();
            cstPalletInfo cstpall = new cstPalletInfo();

            cstpall.PLT_BOL = Bol.Text;
            cstpall.PLT_Carrier = txtCarrier.Text;
            cstpall.PLT_PRO = txtPRONumber.Text;
            cstpall.Location = cmdLocalFile.ReadString("Location");
            cstpall.PalletID = new Guid(txtPRONumberPalletID.Text);
            cstpall.BOLCreatedDateTime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString());


            lspall.Add(cstpall);


            sm.UpdatePallet(lspall, new Guid(txtPRONumberPalletID.Text));


            ContentPresenter sp3 = grdtest.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate myDataTemplate213 = sp3.ContentTemplate;
            Button Print = (Button)myDataTemplate213.FindName("btnprint", sp3);


            Print.Visibility = Visibility.Visible;



            ShowMassagePopup("Information Updated Sucessfully", 3000);
        }

        private void btnprint_Click_1(object sender, RoutedEventArgs e)
        {

            int selectedindex = grdtest.SelectedIndex;
            DataGridRow rowContainer = GetRow(selectedindex);

            //// TextBox ProgressFlag = grdtest.Columns[1].GetCellContent(rowContainer) as TextBox;

            // TextBox myTextBlock = (TextBox)grdtest.  FindName("txtPRONumber");
            // string  kk = grdtest.Columns[2].GetCellContent(rowContainer).ToString();
            TextBlock pallet = grdtest.Columns[0].GetCellContent(rowContainer) as TextBlock;
            TextBlock Ship = grdtest.Columns[1].GetCellContent(rowContainer) as TextBlock;

            Global.ShipmentNumPalletforPrint = Ship.Text;

            ContentPresenter CntPersenter = grdtest.Columns[2].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp = CntPersenter.ContentTemplate;
            TextBox Bol = (TextBox)DataTemp.FindName("txtstkTxt", CntPersenter);

            ContentPresenter CntPersenter2 = grdtest.Columns[3].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp2 = CntPersenter2.ContentTemplate;
            TextBox txtCarrier = (TextBox)DataTemp2.FindName("txtCarrier", CntPersenter2);

            ContentPresenter CntPersenter3 = grdtest.Columns[4].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate DataTemp3 = CntPersenter3.ContentTemplate;
            TextBox txtPRONumber = (TextBox)DataTemp3.FindName("txtPRONumber", CntPersenter3);
            TextBox txtPRONumberPalletID = (TextBox)DataTemp3.FindName("txtPalletID", CntPersenter3);

            ContentPresenter sp3 = grdtest.Columns[7].GetCellContent(rowContainer) as ContentPresenter;
            DataTemplate myDataTemplate213 = sp3.ContentTemplate;
            Button Print = (Button)myDataTemplate213.FindName("btnprint", sp3);

            Print.Content = "Re-print";



            int Original = 0;
            int Original1 = 0;
            int Original2 = 0;
            int Original3 = 0;
            int Original4 = 0;
            int Original5 = 0;
            int Original6 = 0;

            int FinalNumber;


            string mystring = pallet.Text.Substring(pallet.Text.Length - 7);

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


            sm.GetSHNumberByBoxnumber1(Global.ssccNumber, pallet.Text);


            Global.TotalSKUQuantity = sm.GetTotalSKUByPalletNUmberCon(pallet.Text.Trim());
            Global.BOLNumber = Bol.Text;
            Global.PRONumber = txtPRONumber.Text;
            Global.CarrierName = txtCarrier.Text;

            int palletcount = cartoncount + 1;

            Global.Carton = palletcount + " of " + grdtest.Items.Count;
            // txtBoxNumberScanned.Text = "";
            wndPalletInfo pall = new wndPalletInfo();
            // Global.palletnumber = Global.ShippingNumber;
            pall.ShowDialog();
        }

        private void btnDefault_Click_1(object sender, RoutedEventArgs e)
        {
            PackingClassLibrary.ReportController ReportObj = new PackingClassLibrary.ReportController();

            if (Global.BOLProcessFlag == "MyBol")
            {
                ship = ReportObj.GetAllShipmentAndPalletPerStationRC(Global.controller.GetStationMasterByName(Global.StationName).StationID);

            }
            else
            {
                ship = ReportObj.GetAllShipmentAndPallet();
            }

            

            if (ship.Count > 0)
            {

                grdtest.ItemsSource = ship;


                //grdtest.ItemsSource = Customers;

                dtLoadUpdate1 = new DispatcherTimer();
                dtLoadUpdate1.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dtLoadUpdate1.Tick += dtLoadUpdate1_Tick;
                //start the dispacher.
                dtLoadUpdate1.Start();
                txtScanned.Text = "";
                btnDefault.Visibility = Visibility.Visible;

            }
            else
            {
                txtBol.Text = "No Peniding Shipment";
                //txtBol.Text = "No Shipment Found.";
                MessageBox.Show("No Peniding Shipment");
                txtScanned.Text = "";

            }
        }

       
        private void btnClose_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
