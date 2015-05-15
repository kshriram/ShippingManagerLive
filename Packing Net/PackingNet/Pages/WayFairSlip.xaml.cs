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

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for WayFairSlip.xaml
    /// </summary>
    public partial class WayFairSlip : Window
    {
      //  cmdGetSlipData slip = new cmdGetSlipData();
        model_Shipment _shipment = Global.controller.getModelShipment(Global.ShippingNumber);
        string EBoxNumber = "";

        public WayFairSlip()
        {
            InitializeComponent();
        }

        //public void getSlipdata(string BoxNumber)
        //{
        //  List<cstSlipData> slp = new List<cstSlipData>();
        //    try
        //    {
        //        slp = slip.SlipData(BoxNumber);

        //        foreach (var item in slp)
        //        {
        //            txtponumber.Text = item.CustomerPO;
        //            txtaddress.Text = item.ToAddressLine1 + item.ToAddressLine2 + item.ToAddressLine3 + item.ToAddressCity + item.ToAddressState + item.ToAddressZipCode;
        //            txtUPC.Text = item.UPCCode;
        //            txtvendor.Text = item.VendorName;
        //        }


        //    }
        //    catch (Exception)
        //    {}
          
        //}

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Box Information 
            cstBoxPackage _boxInfo = Global.controller.GetBoxPackageByBoxID(Global.PrintBoxID);
            String BoxNumber = _boxInfo.BOXNUM;
            EBoxNumber = BoxNumber;

            //Package Information
            cstPackageTbl packing = Global.controller.GetPackingList(_boxInfo.PackingID, true);
            String ShippingNumber = packing.ShippingNum;

            //Shipping information
            cstShippingTbl shippingTbl = Global.controller.GetShippingTbl(ShippingNumber);
            List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(packing.PackingId);

           //List<cstShippingTbl> ship = Global.controller.GetShippingInfoFromSage(ShippingNumber);

            cmdGetSlipData slip = new cmdGetSlipData();
            List<string> skulist=slip.getSKUfromBoxNumber(BoxNumber);

            foreach (var item in skulist)
            {
                 String UPC_Code = _shipment.ShipmentDetailSage.FirstOrDefault(i => i.SKU ==item).UPCCode;
                 txtaddress.Text = shippingTbl.ToAddressLine1 + shippingTbl.ToAddressLine2 + shippingTbl.ToAddressLine3 + shippingTbl.ToAddressCity + shippingTbl.ToAddressState + shippingTbl.ToAddressCountry + shippingTbl.ToAddressZipCode;
                 txtponumber.Text = shippingTbl.CustomerPO;
                 //txtvendor.Text = shippingTbl.VendorName;
            }

        }

    }
}
