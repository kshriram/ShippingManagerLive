using System;
using System.Collections.Generic;
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
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.CustomEntity.SMEntitys;
using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.BusinessLogic;
using PackingNet.Classes;
using PackingClassLibrary;
using PackingClassLibrary.Commands;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndWayfair.xaml
    /// </summary>
    public partial class wndWayfair : Window
    {

        cmdShipment _cmdSageShipment = new cmdShipment();
        cmdCartonInfo _carton = new cmdCartonInfo();

        smController smObj = new smController();

        cstPackageTbl Localpkg = new cstPackageTbl();

        DispatcherTimer _threadPrint = new DispatcherTimer();
        public UPCA upc = null;

       
        string EBoxNumber = "";

        public wndWayfair()
        {
            InitializeComponent();
            //_threadPrint.Interval = new TimeSpan(0, 0, 1);
            //_threadPrint.Start();
            //_threadPrint.Tick += _threadPrint_Tick;
           // txtTextToAdd.Visibility = Visibility.Hidden;
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Box Information 
            String BoxNumber = Global.BoxNumberScanned;
            EBoxNumber = Global.BoxNumberScanned;

            Localpkg = smObj.GetPkgByBoxnumber(BoxNumber);


            //Package Information
            String ShippingNumber = Localpkg.ShippingNum; //Global.ShippingNumber;

            //Shipping information
            cstShippingTbl shippingTbl = Global.controller.GetShippingTbl(ShippingNumber);
            List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(BoxNumber);

            model_Shipment _shipment = Global.controller.getModelShipment(ShippingNumber);



            string upccode = Global.controller.SKUnameToUPCCode(_packingDetails[Global.counter].SKUNumber);
            //String UPC_Code = _shipment.ShipmentDetailSage.SingleOrDefault(i => i.SKU == item.UPCCode).UPCCode;
            txtaddress.Text = shippingTbl.ToAddressLine1 + shippingTbl.ToAddressLine2 + shippingTbl.ToAddressLine3 + shippingTbl.ToAddressCity + shippingTbl.ToAddressState + shippingTbl.ToAddressCountry + shippingTbl.ToAddressZipCode;
            txtponumber.Text = shippingTbl.CustomerPO;
            //  txtvendorname.Text = shippingTbl.VendorName;
            txtWH.Text = "3";
            txtupc.Text = upccode;
            txtQty.Text = _packingDetails[Global.counter].SKUQuantity.ToString();

            txtcompany.Text = shippingTbl.VendorName;

            txtPartNumber.Text = _packingDetails[Global.counter].SKUNumber.ToString();
           // txtCarton.Text = GetCarton(BoxNumber);
            txtBoxNumber.Text = GetBox(BoxNumber);
            UPCA upca = new UPCA();
            if (this.txtupc.Text.Length == 12)
            {
                this.txtupc.Text = this.txtupc.Text.Substring(0, 11) + upca.GetCheckSum(this.txtupc.Text).ToString();
                System.Drawing.Image img;
                img = upca.CreateBarCode(this.txtupc.Text, 3);

                var imges = new System.Drawing.Bitmap(img);

                var newimag = Imaging.CreateBitmapSourceFromHBitmap(imges.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());


                image.Source = newimag;
            }
            else
            {
                this.image.Source = null;
            }

            _threadPrint.Interval = new TimeSpan(0, 0, 2);
            _threadPrint.Start();
            _threadPrint.Tick += _threadPrint_Tick;



        }


        public String GetCarton(String BoxNumber)
        {
            String _return = "";
            int total = 0;
            int outOf = 1;

            List<cstCartonInfo> _CartonByShipmentNumber = _carton.GetCartonByShipmentNumberAndLocation(Localpkg.ShippingNum, cmdLocalFile.ReadString("Location"));
            cstCartonInfo _cartonBox = _CartonByShipmentNumber.FirstOrDefault(i => i.BOXNumber == BoxNumber);
            foreach (var item in _CartonByShipmentNumber)
            {
                if (item.CartonNumber == _cartonBox.CartonNumber)
                {
                    total = total + 1;

                    if (item.Printed != 0)
                        outOf = outOf + 1;
                    
                }
            }
            if (total < outOf)
                outOf = total - 1;

            _return = outOf.ToString() + " of " + total.ToString();
            return _return;
        }
        public String GetBox(String BoxNumber)
        {
            String _return = "";
            int outOf = 1;
            int total = 0;

            List<cstCartonInfo> _CartonByShipmentNumber = _carton.GetCartonByShipmentNumberAndLocation(Localpkg.ShippingNum, cmdLocalFile.ReadString("Location"));
            cstCartonInfo _cartonBox = _CartonByShipmentNumber.SingleOrDefault(i => i.BOXNumber == BoxNumber);
            foreach (var item in _CartonByShipmentNumber)
            {
                if (item.Printed != 0)
                    outOf = outOf + 1;
            }
            total = _CartonByShipmentNumber.Count();
            

            _return = outOf.ToString() + " of " + total.ToString();
            return _return;
        }

       
        private void _print()
        {
            try
            {

                PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                printDlg.PrintTicket.PageMediaSize = new PageMediaSize((Double)395.0, (Double)360.0);
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

            this.Close();
        }
    }
}
