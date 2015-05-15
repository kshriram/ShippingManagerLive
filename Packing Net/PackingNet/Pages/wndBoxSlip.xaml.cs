using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for wndBoxSlip.xaml
    /// </summary>
    public partial class wndBoxSlip : Window
    {
        //Dispatcher To print the after 3 sec.
        DispatcherTimer _threadPrint = new DispatcherTimer();

        string EBoxNumber="";

        public wndBoxSlip()
        {
            try
            {
                //ManagementScope scope = new ManagementScope(@"\\WIN-K99QHBEQB83");
                //scope.Connect();

                // Select Printers from WMI Object Collections
                ManagementObjectSearcher searcher = new
                 ManagementObjectSearcher("SELECT * FROM Win32_Printer where Default = true");

                string printerName = "";
                foreach (ManagementObject printer in searcher.Get())
                {
                    //printerName = printer["Name"].ToString().ToLower();
                    //if (printerName.Equals("\\\\win-k99qhbeqb83\\canon lbp2900"))
                    //{
                    //Console.WriteLine("Printer = " + printer["Name"]);  Availability

                    ////string ss = printer["Availability"].ToString();
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        // Console.WriteLine("Your Plug-N-Play printer is not connected.");
                        MessageBox.Show("Please Connect The Printer  " + printer["Name"] + "  Or Contact To help desk");
                        this.Close();
                    }
                    else
                    {
                        InitializeComponent();
                        //_threadPrint.Interval = new TimeSpan(0, 0, 0, 0, 90);
                        //_threadPrint.Start();
                        //_threadPrint.Tick += _threadPrint_Tick;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Connect The Printer Or Contact To help desk");
                this.Close();
            }
        }

        void _threadPrint_Tick(object sender, EventArgs e)
        {
            //Print functions.
            _print();
            //Stop Double priting 
            _threadPrint.Stop();
            //Close this window.
            this.Close();
            //i++;

            //if (i == 0)
            //{

            //}
            //if (i ==1)
            //{
            //    //close form after Print 


            //}
        }

        private void BoxSlip_Loaded(object sender, RoutedEventArgs e)
        {
            //Box Information 


            try
            {
                Guid PrintDemo = Global.PrintBoxID;

                cstBoxPackage _boxInfo = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID);
                String BoxNumber = _boxInfo.BOXNUM;
                EBoxNumber = BoxNumber;

                //Package Information
                cstPackageTbl packing = Global.controller.GetPackingList(_boxInfo.PackingID, true);
                String ShippingNumber = packing.ShippingNum;

                //Shipping information
                cstShippingTbl shippingTbl = Global.controller.GetShippingTbl(ShippingNumber);
                List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(packing.PackingId);

                //Sku Quantity.
                var SKUQty = from ls in _packingDetails
                             where ls.BoxNumber == BoxNumber
                             select new
                             {
                                 SkuCount = ls.SKUQuantity
                             };

                int SkuQuantity = SKUQty.Sum(i => i.SkuCount);
                //User Packing Shippment
                String Username = Global.controller.GetSelcetedUserMaster(packing.UserID)[0].UserName.ToString();

                //Grid Fill with SKU Name and Product Name
                var _packDetail = from ls in _packingDetails
                                  where ls.BoxNumber == BoxNumber
                                  select new
                                  {
                                      SKUNumber = ls.SKUNumber + " -" + ls.ProductName,
                                      SKUQuantity = ls.SKUQuantity
                                  };

                //Box Number(package Box Packing)
                int BoxCount = (from pd in _packingDetails
                                group pd by pd.BoxNumber into Gpd
                                select Gpd).Count();


                //Barcode Liabrary
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                //Barcode for each label
                var sBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, BoxNumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);
                var sBoxTopNumber = b.Encode(BarcodeLib.TYPE.CODE128, BoxNumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);
                var sSOoNumber = b.Encode(BarcodeLib.TYPE.CODE128, shippingTbl.OrderID, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);
                var sPCKNumber = b.Encode(BarcodeLib.TYPE.CODE128, packing.PCKROWID, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);
                var sShippingNumber = b.Encode(BarcodeLib.TYPE.CODE128, shippingTbl.ShippingNum, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);


                //Image Conversion
                var bitmapBox = new System.Drawing.Bitmap(sBoxNumber);
                var bitmapBoxTop = new System.Drawing.Bitmap(sBoxTopNumber);
                var bitmapShipping = new System.Drawing.Bitmap(sShippingNumber);
                var bitmapSO = new System.Drawing.Bitmap(sSOoNumber);
                var bitmapPCK = new System.Drawing.Bitmap(sPCKNumber);



                var bBoxSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBox.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                var bBoxTopSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBoxTop.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                var bShippingSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapShipping.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                var bShOSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapSO.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                var bSPCKSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapPCK.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                bitmapBox.Dispose();
                bitmapBoxTop.Dispose();
                bitmapShipping.Dispose();
                bitmapSO.Dispose();
                bitmapPCK.Dispose();

                //assign source to images
                imgBoxNumber.Source = bBoxSource;
                lblBoxNumber.Content = BoxNumber;
                imgBoxNumber.Stretch = Stretch.Fill;


                imgBOxNumTop.Source = bBoxTopSource;
                lblBoxTupNumber.Content = BoxNumber;

                imgShipping.Source = bShippingSource;
                lblShipment.Content = ShippingNumber;

                imgSO.Source = bShOSource;
                lblSoNumber.Content = shippingTbl.OrderID;

                //imgPackNum.Source = bSPCKSource;
                //lblPckNum.Content = packing.PCKROWID;


                //packing Detal Info
                tbPackageBox.Text = BoxCount.ToString() + " [SKU QTY: " + SkuQuantity + "]";
                tbCarrier.Text = shippingTbl.Carrier + " / " + shippingTbl.MDL_0;
                tbPoNum.Text = shippingTbl.CustomerPO.ToString();
                tbDealer.Text = shippingTbl.VendorName.ToString();
                tbWarehouse.Text = Global.controller.ApplicationLocation();
                tbUserName.Text = Username;
                dgSKUinfo.ItemsSource = _packDetail.ToList();
                tbPackingTime.Text = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString();
                //lblBDate.Content = DateTime.UtcNow.ToString("dd MMM, yyyy hh:mm tt").TrimStart('0').ToString();

                lblBDate.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ").TrimStart('0').ToString();


                // lblLastLoginTime.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString("MMM dd, yyyy h:mm tt ");


                _threadPrint.Interval = new TimeSpan(0, 0, 2);
                _threadPrint.Start();
                _threadPrint.Tick += _threadPrint_Tick;

                //_print();
                //this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


         
            
        }

        /// <summary>
        /// Print Entire contents from the page.
        /// </summary>
        private void _print()
        {
            try
            {

                PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                printDlg.PrintTicket.PageMediaSize = new PageMediaSize((Double)395.0, (Double)820.0);
                //printDlg.ShowDialog();

                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.Width, capabilities.PageImageableArea.ExtentHeight / this.Height);

                //Transform the Visual to scale
                this.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                this.Measure(sz);

                this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(this, "BoxSlip_KrausUSA_A");
            }
            catch (Exception ex)
            {
                ErrorLoger.save("Print Canceled: " + EBoxNumber + " ", ex.ToString());

            }
          }
    }
}