using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
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
    /// Interaction logic for OnlyBoxSlipForLTL.xaml
    /// </summary>
    public partial class OnlyBoxSlipForLTL : Window
    {
        public OnlyBoxSlipForLTL()
        {
            InitializeComponent();
              _threadPrint.Interval = new TimeSpan(0, 0, 1);
            _threadPrint.Start();
            _threadPrint.Tick += _threadPrint_Tick;
        }
          DispatcherTimer _threadPrint = new DispatcherTimer();

        string EBoxNumber="";
            
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

        private void _print()
        {
            try
            {

                PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                printDlg.PrintTicket.PageMediaSize = new PageMediaSize((Double)395.0, (Double)200.0);
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            cstBoxPackage _boxInfo = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID);
            String BoxNumber = _boxInfo.BOXNUM;
            EBoxNumber = BoxNumber;

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            //Barcode for each label
            var sBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, BoxNumber, System.Drawing.Color.Black, System.Drawing.Color.Transparent, 400, 160);

            var bitmapBox = new System.Drawing.Bitmap(sBoxNumber);

            var bBoxSource = Imaging.CreateBitmapSourceFromHBitmap(bitmapBox.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            bitmapBox.Dispose();

            imgBoxNumber.Source = bBoxSource;
            lblBoxNumber.Content = BoxNumber;
            imgBoxNumber.Stretch = Stretch.Fill;

        }
    }
}
