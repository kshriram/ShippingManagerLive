using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackingClassLibrary.CustomEntity.SMEntitys;

namespace PackingClassLibrary.Commands.SMcommands
{
  public  class cmdGetSlipData
    {

        local_x3v6Entities entx3v6 = new local_x3v6Entities();
        public List<cstSlipData> SlipData(string BoxNumber)
        {
            List<cstSlipData> lsslipdata = new List<cstSlipData>();
            try
            {
                var info = from ship in entx3v6.Packages
                           join get in entx3v6.Shippings
                           on ship.ShippingID equals get.ShippingID
                           join viewdata in entx3v6.Get_Shipping_Data
                           on ship.ShippingNum equals viewdata.ShipmentID
                           join box in entx3v6.BoxPackages
                           on ship.PackingId equals box.PackingID
                           where box.BOXNUM == BoxNumber
                           select new
                           {
                               get.ToAddressLine1,
                               get.ToAddressLine2,
                               get.ToAddressLine3,
                               get.ToAddressState,
                               get.ToAddressCity,
                               get.ToAddressZipCode,
                               get.CustomerPO,
                               get.VendorName,
                               viewdata.UPCCode
                           };
                foreach (var item in info)
                {
                    cstSlipData slip=new cstSlipData();

                    slip.ToAddressLine1 = item.ToAddressLine1;
                    slip.ToAddressLine2 = item.ToAddressLine2;
                    slip.ToAddressLine3 = item.ToAddressLine3;
                    slip.UPCCode = item.UPCCode;
                    slip.VendorName = item.VendorName;
                    slip.ToAddressZipCode = item.ToAddressZipCode;
                    slip.ToAddressState = item.ToAddressState;
                    slip.CustomerPO = item.CustomerPO;

                    lsslipdata.Add(slip);
                }
            }
            catch (Exception)
            { }
            return lsslipdata;
        }


        public List<string> getSKUfromBoxNumber(string BoxNumber)
        {
            List<string> lssku = new List<string>();

            var sku = from b in entx3v6.PackageDetails
                      where b.BoxNumber == BoxNumber
                      select b.SKUNumber;

            foreach (var item in sku)
            {
                lssku.Add(item);
            }

            return lssku;
        }

    }
}
