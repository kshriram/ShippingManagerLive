using Packing_Net.Classes;
using PackingClassLibrary.BusinessLogic;
using PackingClassLibrary.CustomEntity;
using System;
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

namespace PackingNet.Pages.ReportPages
{
    /// <summary>
    /// Interaction logic for ReportForm.xaml
    /// </summary>
    public partial class ReportForm : Window
    {

        PackingClassLibrary.ReportController ReportObj = new PackingClassLibrary.ReportController();

        public ReportForm()
        {
            InitializeComponent();
            _reportviewer.Load += _reportviewer_Load;
        }
        private bool _isReportViewerLoaded;
        public void _reportviewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                Microsoft.Reporting.WinForms.ReportDataSource rd = new Microsoft.Reporting.WinForms.ReportDataSource();

                Shipping_ManagerDataSet dataset = new Shipping_ManagerDataSet();

                dataset.BeginInit();

                rd.Name = "DataSet1";

                rd.Value = ReportObj.GetPackingDetailsByBoxNumForReport(Global.ShippingNumber);

                //rd.Value = dataset.PackageDetail;
                this._reportviewer.LocalReport.DataSources.Add(rd);

                this._reportviewer.LocalReport.ReportEmbeddedResource = "PackingNet.Pages.ReportPages.Report_Shipment.rdlc";
                dataset.EndInit();

                Shipping_ManagerDataSetTableAdapters.PackageDetailTableAdapter product = new Shipping_ManagerDataSetTableAdapters.PackageDetailTableAdapter();
                product.ClearBeforeFill = true;

                //ReportObj.GetPackingDetailsByBoxNumForReport("23556")

                // product.Fill(dataset.PackageDetail);
                _reportviewer.RefreshReport();
                _isReportViewerLoaded = true;

            }
            txtScannSKu.Focus();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            lblshipment.Content = Global.ShippingNumber;

            model_Shipment modelshipment = Global.controller.getModelShipment(Global.ShippingNumber);

            List<cstPackageTbl> _lsPck = modelshipment.PackingInfo;

            if (_lsPck[0].PackingStatus == 0)
            {
                lblstatus.Content = "Status :- Shipment is closed.";
            }
            else if (_lsPck[0].PackingStatus == 1)
            {
                lblstatus.Content = "Status :- Shipment is under packing.";
            }
            txtScannSKu.Focus();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtScannSKu_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtScannSKu.Text.Trim() != "")
            {
                if (txtScannSKu.Text.Contains("#"))
                {
                    if (txtScannSKu.Text == "#close" || txtScannSKu.Text == "#CLOSE")
                    {
                        txtScannSKu.Text = "";

                        if (btnClose.IsVisible)
                        {
                            ButtonAutomationPeer peer = new ButtonAutomationPeer(btnClose);
                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            invokeProv.Invoke();
                        }
                        else
                        {
                            /// ScrollMsg("You can not add new box.", Color.FromRgb(222, 87, 24));
                            // string ss = "Can not allowed to cancel";
                            /////  simpledelegate sm = new simpledelegate(ShowMassagePopup);
                            //simpledelegate(ShowMassagePopup("You can not add new box.", 4000));
                            //MessageDelegate mm = new MessageDelegate(ShowMassagePopup);
                            //mm(ss, 3000);

                        }
                    }
                }
            }
        }

    }
}
