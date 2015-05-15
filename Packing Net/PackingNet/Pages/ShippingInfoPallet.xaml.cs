using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Packing_Net;
using Packing_Net.Classes;
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
using Packing_Net.Pages;
using Microsoft.Windows.Controls.Primitives;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for ShippingInfoPallet.xaml
    /// </summary>
    public partial class ShippingInfoPallet : Window
    {
        public ShippingInfoPallet()
        {
            InitializeComponent();
        }


        #region Declaration
        PackingClassLibrary.smController sm = new PackingClassLibrary.smController();
        List<cstShippingInfo> ship = new List<cstShippingInfo>();
        List<cstPalletInfo> shipforpallet = new List<cstPalletInfo>();

        DispatcherTimer timer2;


        // List<SKU_ProductName_QTY> aqeww = new List<SKU_ProductName_QTY>();
        List<string> locationFlag = new List<string>();
        public Boolean _locationFlag = false;
        TreeViewItem treeItem = new TreeViewItem();
        String ApplicationLocation = Global.controller.ApplicationLocation();
        public Boolean box = false;
        public Boolean box1 = false;
        public Boolean pallet = false;
        public Boolean pallet1 = false;
        DispatcherTimer timer;
        List<cstPackageDetails> _package = new System.Collections.Generic.List<cstPackageDetails>();
        //List<Guid> _packingID = new System.Collections.Generic.List<System.Guid>();
        Guid _packingID = new System.Guid();
        //public string ShipmentNumber = "";
        #endregion

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            dtPalletsBoxes.Visibility = Visibility.Hidden;
        }

        private void txtShipmentId_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //List<cstPalletInfo> shipforpallet = new List<cstPalletInfo>();
            //grdContentPallet.ItemsSource = shipforpallet;
            List<cstPackageDetails> _package = new System.Collections.Generic.List<cstPackageDetails>();
            dtPalletsBoxes.ItemsSource = _package;
            dtPalletsBoxes.Visibility = Visibility.Hidden;
            //grdContent.Visibility = Visibility.Hidden;
            btnGo.Visibility = Visibility.Visible;
            //shipforpallet.Clear();
            //grdBoxSku.Visibility = Visibility.Hidden;
            //lblMessage.Content = "";
            txtShipmentScan.Text = "";

            //if (box1 == true)
            //{
            //    Global.BoxPrint = true;
            //}
            //else if (pallet1 == true)
            //{
            //    Global.PalletPrint = true;
            //}
        }

        //private void btnView_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    grdBoxSku.Visibility = Visibility.Visible;

        //    if (box == true)
        //    {
        //        string EBoxNumber = "";
        //        Guid BoxID = new Guid();
        //        int selectedindex = grdContent.SelectedIndex;
        //        DataGridRow rowContainer = GetRow(selectedindex);
        //        TextBlock ProgressFlag = grdContent.Columns[2].GetCellContent(rowContainer) as TextBlock;
        //        string ssss = ProgressFlag.Text;
        //        foreach (var item in ship)
        //        {
        //            if (item.BoxNumber == ssss)
        //            {
        //                cstBoxPackage boxinfo = new cstBoxPackage();
        //                Guid boxid = new System.Guid();
        //                boxid = sm.GetBoxIDByBoxNumber(ssss);
        //                BoxID = boxid;
        //            }
        //        }

        //        //Box Information                

        //        cstBoxPackage _boxInfo = Global.controller.GetBoxPackageByBoxID(BoxID);
        //        String BoxNumber = _boxInfo.BOXNUM;
        //        EBoxNumber = BoxNumber;

        //        //Package Information
        //        cstPackageTbl packing = Global.controller.GetPackingList(_boxInfo.PackingID, true);
        //        String ShippingNumber = packing.ShippingNum;

        //        //Shipping information
        //        cstShippingTbl shippingTbl = Global.controller.GetShippingTbl(ShippingNumber);
        //        List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(packing.PackingId);

        //        //Sku Quantity.
        //        var SKUQty = from ls in _packingDetails
        //                     where ls.BoxNumber == BoxNumber
        //                     select new
        //                     {
        //                         SkuCount = ls.SKUQuantity
        //                     };

        //        int SkuQuantity = SKUQty.Sum(i => i.SkuCount);

        //        var _packDetail = (from ls in _packingDetails
        //                          where ls.BoxNumber == BoxNumber
        //                          select new
        //                          {
        //                              SKUNumber = ls.SKUNumber ,//+ " -" + ls.ProductName,
        //                              SKUQuantity = ls.SKUQuantity
        //                          });


        //        grdBoxSku.ItemsSource = _packDetail;
        //    }
        //}


        //public class SKU_ProductName_QTY
        //{
        //    public String SKU { get; set; }
        //    public String Qty { get; set; }
        //    //public String FromAddressLine3 { get; set; }
        //}

        private void btnGO_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                btnGo.Visibility = Visibility.Hidden;
                dtPalletsBoxes.Visibility = Visibility.Hidden;
                _package.Clear();
                // grdContent.Visibility = Visibility.Hidden;

                #region Simple Reprinting
                //ship.Clear();
                //ship = sm.GetShipmentInfo(txtShipmentScan.Text.Trim());
                //String application location 
                ship = sm.GetShipmentInfo12(txtShipmentScan.Text.Trim(), ApplicationLocation);
                //_packingID = sm.GetPackingNum1(txtShipmentScan.Text.Trim(), ApplicationLocation);

                //foreach (var item in sm.GetShipmentInfo1(_packingID, ApplicationLocation))
                //{
                //    //_package = sm.GetShipmentInfo(item);
                //    cstPackageDetails A1 = new cstPackageDetails();
                //    A1.BoxNumber = item.BoxNumber;
                //    A1.SKUNumber = item.SKUNumber;
                //    A1.SKUQuantity = item.SKUQuantity;
                //    _package.Add(A1);
                //}

                // List<cstPalletInfo> lspalletInfo = sm.GetPalletInfoBySHNumber1(txtShipmentScan.Text.Trim());

                //List<string> lspalletInfo1 = Control.GetPalletInfoBySHNumber(Global.ShippingNumber);


                //List<cstShippingInfo> shippalletchk = new List<cstShippingInfo>();





                if (ship.Count != 0)
                {
                    //if (Global.BoxPrint == true)
                    //{
                    //lblMessage.Content = "";
                    box = true;
                    //box1 = true;
                    //Global.BoxPrint = false;
                    dtPalletsBoxes.Visibility = Visibility.Visible;
                    //grdContent.Visibility = Visibility.Visible;
                    dtPalletsBoxes.ItemsSource = ship;



                    timer2 = new DispatcherTimer();
                    timer2.Tick += timer2_Tick;
                    timer2.Interval = new TimeSpan(0, 0, 0, 1);
                    timer2.Start();





                    // }
                    //else if (Global.PalletPrint == true)
                    //{
                    //   // string ShipmentNumber = txtShipmentId.Text.Trim();
                    //    //lblMessage.Content = "";
                    //    //List<cstShippingInfo> shippalletchk = new List<cstShippingInfo>();
                    //    //foreach (var item in ship)
                    //    //{

                    //    //}
                    //    Boolean chk=false;
                    //    shipforpallet.Clear();
                    //    pallet = true;
                    //    pallet1 = true;
                    //    Global.PalletPrint = false;
                    //grdContent.Visibility = Visibility.Hidden;
                    //grdContentPallet.Visibility = Visibility.Visible;

                    //string pallet;

                    //foreach (var item in lspalletInfo)
                    //{
                    //    //  cstPalletInfo plt = new cstPalletInfo();
                    //    // plt.PalletNumber = ;

                    //    //pallet = item.PalletNumber;

                    //    //shipforpallet.Add(item);
                    //}
                    //foreach (var item in ship)
                    //{
                    //    //shippalletchk.Add(item);
                    //    if (chk == true)
                    //    {
                    //        foreach (var item1 in shipforpallet)
                    //        {
                    //            if (item.PalletNumber != null)
                    //            {
                    //                shipforpallet.Add(item);
                    //            }
                    //        }
                    //    }
                    //    else if (chk != true)
                    //    {
                    //        chk = true;
                    //        shipforpallet.Add(item);
                    //    }

                    //}
                    //grdContentPallet.ItemsSource = shipforpallet;

                    // }
                }
                else
                {
                    //if (Global.BoxPrint == true)
                    //{
                    //lblMessage.Content = "This Shipment not having any Box";
                    //MsgBox.Show("Confirm", "Caution", "This Shipment not having any Box.");
                    MessageBox.Show("This Shipment not having any Pallet.");
                    //ship.Clear();
                    dtPalletsBoxes.Visibility = Visibility.Hidden;
                    //grdContent.Visibility = Visibility.Hidden;
                    btnGo.Visibility = Visibility.Visible;
                    //lblMessage.Content = "";
                    txtShipmentScan.Text = "";

                    //if (box1 == true)
                    //{
                    //    Global.BoxPrint = true;
                    //}
                    //else if (pallet1 == true)
                    //{
                    //    Global.PalletPrint = true;
                    //}
                    // }
                    //else if (Global.PalletPrint == true)
                    //{
                    //    //lblMessage.Content = "This Shipment not having any Pallet";
                    //    // MsgBox.Show("Confirm", "Caution", "This Shipment not having any Pallet.");
                    //    MessageBox.Show("This Shipment not having any Pallet.");
                    //    shipforpallet.Clear();
                    //    grdContentPallet.Visibility = Visibility.Hidden;
                    //    grdContent.Visibility = Visibility.Hidden;
                    //    btnGO.Visibility = Visibility.Visible;
                    //    //lblMessage.Content = "";
                    //    txtShipmentId.Text = "";

                    //    if (box1 == true)
                    //    {
                    //        Global.BoxPrint = true;
                    //    }
                    //    else if (pallet1 == true)
                    //    {
                    //        Global.PalletPrint = true;
                    //    }
                    //}

                }
            }
            catch (Exception)
            {
            }
                #endregion
        }


        void timer2_Tick(object sender, EventArgs e)
        {
            foreach (DataGridRow row in GetDataGridRows(dtPalletsBoxes))
            {
                TextBlock txtBOXnumber = dtPalletsBoxes.Columns[1].GetCellContent(row) as TextBlock;
                if (txtBOXnumber.Text == null || txtBOXnumber.Text == "")
                {
                    // DataGridCell cell = GetCell(row.GetIndex(), 0);
                    ContentPresenter CntPersenter = dtPalletsBoxes.Columns[0].GetCellContent(row) as ContentPresenter;
                    DataTemplate DataTemp = CntPersenter.ContentTemplate;

                    //TextBox txtReturnGuid = (TextBox)DataTemp.FindName("btnPrint", CntPersenter);
                    Button btnPrint = DataTemp.FindName("btnPrint", CntPersenter) as Button;

                    //Button btnPrint = dtPalletsBoxes.Columns[0].GetCellContent(row).FindName("btnPrint") as Button;

                    btnPrint.Visibility = Visibility.Hidden;

                }
            }

            timer2.Stop();

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

        //public List<cstShippingInfo> GetShipmentInfo(String ShippingNumber)
        //{
        //    cmdShippingInfo com = new cmdShippingInfo();
        //    return com.GetShipmentInfoBySHNumber(ShippingNumber);
        //}

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (box == true)
                //{
                //int selectedindex = dtPalletsBoxes.SelectedIndex;
                //DataGridRow rowContainer = GetRow(selectedindex);
                //TextBlock ProgressFlag = dtPalletsBoxes.Columns[1].GetCellContent(rowContainer) as TextBlock;
                //string ssss = ProgressFlag.Text;
                //foreach (var item in _package)
                //{
                //    if (item.BoxNumber == ssss)
                //    {
                //        cstBoxPackage boxinfo = new cstBoxPackage();
                //        Guid boxid = new System.Guid();
                //        boxid = sm.GetBoxIDByBoxNumber(ssss);
                //        Global.PrintBoxID = boxid;
                //    }
                //}
                //wndBoxSlip Box = new wndBoxSlip();
                //Box.ShowDialog();
                //}
                //else if (pallet == true)
                //{
                    int selectedindex2 = dtPalletsBoxes.SelectedIndex;
                    DataGridRow rowContainer = GetRow(selectedindex2);
                    TextBlock ProgressFlag = dtPalletsBoxes.Columns[1].GetCellContent(rowContainer) as TextBlock;
                    string ssss = ProgressFlag.Text;
                    foreach (var item in ship)
                    {
                        if (item.PalletNumber == ssss)
                        {
                            Global.palletnumber = ssss;
                            Global.ShippingNumber = txtShipmentScan.Text;
                            // Global.ShippingNumber = item.ShipmentNumber;
                        }
                    }
                    wndPalletSlip Box = new wndPalletSlip();
                    Box.ShowDialog();
                //}
            }
            catch (Exception)
            { }
        }

        void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Global.BoxPrint = false;
            //Global.PalletPrint = false;
        }

        private void grdContent_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void grdContent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
                    dtPalletsBoxes.ScrollIntoView(rowContainer, dtPalletsBoxes.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
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
            if (box == true)
            {
                DataGridRow row = (DataGridRow)dtPalletsBoxes.ItemContainerGenerator.ContainerFromIndex(index);
                if (row == null)
                {
                    dtPalletsBoxes.UpdateLayout();
                    dtPalletsBoxes.ScrollIntoView(dtPalletsBoxes.Items[index]);
                    row = (DataGridRow)dtPalletsBoxes.ItemContainerGenerator.ContainerFromIndex(index);
                }
                return row;
            }
            else if (pallet == true)
            {
                DataGridRow row = (DataGridRow)dtPalletsBoxes.ItemContainerGenerator.ContainerFromIndex(index);
                if (row == null)
                {
                    dtPalletsBoxes.UpdateLayout();
                    dtPalletsBoxes.ScrollIntoView(dtPalletsBoxes.Items[index]);
                    row = (DataGridRow)dtPalletsBoxes.ItemContainerGenerator.ContainerFromIndex(index);
                }
                return row;
            }
            return null;
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
