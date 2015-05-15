using Packing_Net.Classes;
using PackingClassLibrary;
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


namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndPalletSlip.xaml
    /// </summary>
    /// 

    

    public partial class wndPalletSlip : Window
    {
        DispatcherTimer _threadPrint = new DispatcherTimer();
        smController Controller = new smController();



        public wndPalletSlip()
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
                        _threadPrint.Interval = new TimeSpan(0, 0, 1);
                        _threadPrint.Start();
                        _threadPrint.Tick += _threadPrint_Tick;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Connect The Printer Or Contact To help desk");
                this.Close();
            }
        }

        private void Border_Loaded_1(object sender, RoutedEventArgs e)
        {


           // cstShippingTbl shippingTbl = Global.controller.GetShippingTbl(Global.ShippingNumber);
           // List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(packing.PackingId);


            dgSKUinfo.ItemsSource = Controller.GetBoxnumberAndQuantity(Global.palletnumber);

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            var sBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, Global.palletnumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);
            var sBoxTopNumber = b.Encode(BarcodeLib.TYPE.CODE128, Global.palletnumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);
          //  var sSOoNumber = b.Encode(BarcodeLib.TYPE.CODE128, shippingTbl.OrderID, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);
           // var sPCKNumber = b.Encode(BarcodeLib.TYPE.CODE128, packing.PCKROWID, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);
           // var sShippingNumber = b.Encode(BarcodeLib.TYPE.CODE128, shippingTbl.ShippingNum, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 380, 50);
           

            
            var bitmapBox = new System.Drawing.Bitmap(sBoxNumber);
            var bitmapBoxTop = new System.Drawing.Bitmap(sBoxTopNumber);
          //  var bitmapShipping = new System.Drawing.Bitmap(sShippingNumber);
           // var bitmapSO = new System.Drawing.Bitmap(sSOoNumber);
          //  var bitmapPCK = new System.Drawing.Bitmap(sPCKNumber);


            var bBoxSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBox.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            var bBoxTopSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBoxTop.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
           // var bShippingSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapShipping.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
           // var bShOSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapSO.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //var bSPCKSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapPCK.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            bitmapBox.Dispose();
            bitmapBoxTop.Dispose();
         //   bitmapShipping.Dispose();
          // bitmapSO.Dispose();

            imgPalletNumber.Source = bBoxSource;

         
           // bitmapPCK.Dispose();

            imgPalletNumber.Stretch = Stretch.Fill;


            imgBOxNumTop.Source = bBoxTopSource;
            lblBoxTupNumber.Content = Global.palletnumber;

         //   imgShipping.Source = bShippingSource;
            lblShipment.Content =Global.ShippingNumber;

        //    imgSO.Source = bShOSource;
         //   lblSoNumber.Content = shippingTbl.OrderID;

            lblBDate.Content = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");//DateTime.UtcNow.ToString("dd MMM, yyyy hh:mm tt").TrimStart('0').ToString();
            tbPackingTime.Text = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToString();

            lblPalletNumber.Content = Global.palletnumber;

            tbPackageBox.Text = Convert.ToString(dgSKUinfo.Items.Count);
           // tbCarrier.Text = shippingTbl.Carrier + " / " + shippingTbl.MDL_0;
           // tbPoNum.Text = shippingTbl.CustomerPO.ToString();
           // tbDealer.Text = shippingTbl.VendorName.ToString();
            tbWarehouse.Text = Global.controller.ApplicationLocation();

        }

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
               // ErrorLoger.save("Print Canceled: " + EBoxNumber + " ", ex.ToString());

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
    }
}
