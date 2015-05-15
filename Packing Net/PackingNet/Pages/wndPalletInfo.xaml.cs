using Packing_Net.Classes;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for wndPalletInfo.xaml
    /// </summary>
    public partial class wndPalletInfo : Window
    {

        DispatcherTimer _threadPrint = new DispatcherTimer();

        smController _Contro = new smController();

        public wndPalletInfo()
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
                        _threadPrint.Interval = new TimeSpan(0, 0, 3);
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

        void _threadPrint_Tick(object sender, EventArgs e)
        {
            //Print functions.
            _print();
            //Stop Double priting 
            _threadPrint.Stop();
            //Close this window.
            this.Close();

        }
        private void _print()
        {
            try
            {

                PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                printDlg.PrintTicket.PageMediaSize = new PageMediaSize((Double)410.0, (Double)581.0);
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
            catch (Exception)
            {

            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            cstShippingTbl lstshipping = new cstShippingTbl();

            lstshipping = _Contro.GetShippingTbl(Global.ShipmentNumPalletforPrint);

            List<cstPackageTbl> lstpackage = _Contro.GetPackingList(Global.ShipmentNumPalletforPrint, _Contro.ApplicationLocation());


            if (lstpackage.Count == 0)
            {
                MessageBox.Show("Location not matched.");
            }
            else
            {


                if (lstpackage[0].ShipmentLocation == "NYWH")
                {
                    lblFromAddress.Text = "Kraus USA Inc.12 Harbor Park Drive,Port Washington,New York US.11050";
                }
                else if (lstpackage[0].ShipmentLocation == "NYWT")
                {
                    lblFromAddress.Text = "Kraus USA Inc.300 Michael Drive Syosset New York US 11791";
                }

                // lblFromAddress.Text = lstshipping.FromAddressLine1 + " " + lstshipping.FromAddressLine2 + " " + lstshipping.FromAddressLine3 + " " + lstshipping.FromAddressCity + " " + lstshipping.FromAddressState + " " + lstshipping.FromAddressCountry + " " + lstshipping.FromAddressZipCode;

                lblPurchseNumber.Content = lstshipping.CustomerPO;

                lblbolnumber.Content = Global.BOLNumber;
                lblpronumber.Content = Global.PRONumber;
                lblcarriername.Content = Global.CarrierName;
                lblcarton.Content = "-- Of --";//Global.Carton;
                lblqty.Content = Global.TotalSKUQuantity;


                BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                var sBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, Global.ssccNumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);

                var bitmapBox = new System.Drawing.Bitmap(sBoxNumber);

                var bBoxSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBox.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                bitmapBox.Dispose();

                imgBoxNumber.Source = bBoxSource;
                imgBoxNumber.Stretch = Stretch.Fill;

                lblssccnumber.Content = Global.ssccNumber;


                Global.BOLNumber = "";
                Global.PRONumber = "";
                Global.CarrierName = "";
                Global.Carton = "";
                Global.ssccNumber = "";
                Global.TotalSKUQuantity = 0;

                lblToAddress.Text = lstshipping.CustomerName1 + " " + lstshipping.ToAddressLine1 + " " + lstshipping.ToAddressLine2 + " " + lstshipping.ToAddressLine3 + " " + lstshipping.ToAddressCity + " " + lstshipping.ToAddressState + " " + lstshipping.ToAddressCountry + " " + lstshipping.ToAddressZipCode;
            }
        }


    }
}
