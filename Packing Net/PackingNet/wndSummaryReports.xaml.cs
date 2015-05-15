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
using System.Windows.Shapes;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using Packing_Net.Classes;
using Packing_Net;
using Packing_Net.Pages;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System.Printing;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;


namespace PackingNet
{
    /// <summary>
    /// Interaction logic for wndSummaryReports.xaml
    /// </summary>
    public partial class wndSummaryReports : Window
    {
        smController ConObj = new smController();
        List<cstPackageDetails> lsPackingDetails = new List<cstPackageDetails>();
        public wndSummaryReports()
        {
            InitializeComponent();
           // cstPackingTime packingItem = (cstPackingTime)dgvSummaryReports.SelectedItem;
            ////  List<cstPackageDetails> lsPackingDetails = Global.controller.GetPackingDetailTbl(packingItem.PackingID);

            //////16-01-2015
           lsPackingDetails = Global.controller.GetPackingDetailsByBOX(Global.PackingID);

            dgvSummaryReports.ItemsSource = lsPackingDetails;

            lsPackingDetails = Global.controller.GetPackingDetailsByBOX(Global.PackingID);

            dgvSummaryReports.ItemsSource = lsPackingDetails;
            string ss = Global.ShippingNumber;
            cstShippingTbl st = new cstShippingTbl();
            List<cstShippingTbl> se = new List<cstShippingTbl>();
            ///se = Global.controller.GetShippingTbl(ss);
            cstShippingTbl Ship = new cstShippingTbl();
            Ship = Global.controller.GetShippingTbl(ss);
            List<cstPackageTbl> pkstatus = ConObj.GetPackingListByShippingNumber(Global.ShippingNumber);
            ///// Boolean flg;
            lblShipmentNo.Content = " Shipment Number : " + Global.ShippingNumber;
            lblPONumber.Content = " PO Number :" + Ship.CustomerPO;
            var pk = (from v in pkstatus
                      select v.PackingStatus).FirstOrDefault();
            if (pkstatus.Count > 0)
            {
                if (pk == 0)
                {
                    txtStatus.Text = "Shipment Status :Packed";
                }
                else if (pk == 1)
                {
                    txtStatus.Text = "Shipment Status :Partially Packed.";
                }
            }
            txtScannSKu.Focus();
        }

        private void btnPrint_Click_1(object sender, RoutedEventArgs e)
        {
            _print();
            //foreach (var item in lsPackingDetails)
            //{
            //    //String BoxNumber = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID).BOXNUM;
            //    Global.PrintBoxID = Global.controller.GetBoxPackageByBoxNumber(item.BoxNumber).BoxID;
              
              

            //    String BoxNumber = item.BoxNumber;
            //    wndBoxSlip _boxSlip = new wndBoxSlip();
            //    _boxSlip.ShowDialog();
            //    this.Dispatcher.Invoke(new Action(() => { _boxSlip.Hide(); }));
            //}
        }

        private void _print2()
        {
            //double allocatedSpace = 0;

            ////Measure the page header
            //ContentControl pageHeader = new ContentControl();
            //pageHeader.Content = pageHeader;
            //allocatedSpace = MeasureHeight(pageHeader);

            ////Measure the page footer
            //ContentControl pageFooter = new ContentControl();
            //pageFooter.Content = pageFooter;
            //allocatedSpace += MeasureHeight(pageFooter);

            ////Measure the table header
            //ContentControl tableHeader = new ContentControl();
            //tableHeader.Content = CreateTable(false);
            //allocatedSpace += MeasureHeight(tableHeader);

            ////Include any margins
            //allocatedSpace += this.PageMargin.Bottom + this.PageMargin.Top;





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
               /// Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                Size sz = new Size(1280, 760);
                //update the layout of the visual to the printer page size.
                this.Measure(sz);

             //////   this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(this, "BoxSlip_KrausUSA_A");
            }
            catch (Exception ex)
            {
               // ErrorLoger.save("Print Canceled: " + EBoxNumber + " ", ex.ToString());

            }
        }

        private void btnCancelShipment_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //_saveClick();
                this.Close();
                //Not To Add New Box
                Global.flgBx = "NotAddBx";
            }
            catch (Exception)
            {
            }
        }

        private void txtScannSKu_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
                {
                    if (txtScannSKu.Text.Contains("#"))
                    {
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
                                /// ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                                //string ss = "Can not allowed to cancel";
                                ///////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                                ////simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
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


    }
}
