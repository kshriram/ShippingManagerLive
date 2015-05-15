using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdShippingInfo
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();

        #region Taking All information from Shipment
        public List<cstShippingInfo> GetShipmentInfoBySHNumber(String SHnumber)
        {
            List<cstShippingInfo> _ShipInfo = new List<cstShippingInfo>();

            try
            {


                var shipment = entx3v6.ExecuteStoreQuery<cstShippingInfo>(@"select distinct
                                                                        pk.PackingID, pk.ShippingNum, pk.ShipmentLocation,
                                                                        pkd.BoxNumber, plt.PalletID, plt.PalletNumber
                                                                        from
                                                                        Package pk inner join
                                                                        PackageDetail pkd on pk.PackingID = pkd.PackingID left join
                                                                        PalletDetail pltd on pkd.BoxNumber = pltd.BoxNumber left join
                                                                        PalletInfo plt on pltd.PalletID = plt.PalletID
                                                                        where pk.ShippingNum = '" + SHnumber + "' order by pkd.BoxNumber").ToList();

                foreach (var item in shipment)
                {
                    cstShippingInfo _ShipmentInfo = new cstShippingInfo();
                    _ShipmentInfo.BoxNumber = item.BoxNumber;
                    _ShipmentInfo.PackingID = item.PackingID;
                    _ShipmentInfo.PalletID = item.PalletID;
                    _ShipmentInfo.PalletNumber = item.PalletNumber;
                    _ShipmentInfo.ShipmentLocation = item.ShipmentLocation;
                    _ShipmentInfo.ShipmentNumber = item.ShipmentNumber;
                    _ShipInfo.Add(_ShipmentInfo);
                }

            }
            catch (Exception)
            {
            }
            return _ShipInfo;
        }



        public List<cstPackageDetails> GetShipmentInfoByPackageID(Guid PackingID, String location)
        {
            List<cstPackageDetails> _ShipInfo = new List<cstPackageDetails>();

            try
            {

                var shipment = entx3v6.ExecuteStoreQuery<cstPackageDetails>(@"SELECT
                                                                                                           CASE WHEN ROW_NUMBER() OVER (PARTITION BY BoxNumber ORDER BY BoxNumber) > 1 THEN NULL ELSE BoxNumber END AS BoxNumber,
                                                                                                           SKUNumber, SKUQuantity,SKUScanDateTime as PackingDetailStartDateTime,ShipmentLocation
                                                                                                           FROM PackageDetail pd
                                                                                                           where pd.PackingId='" + PackingID + "'AND pd.ShipmentLocation='" + location + "' ORDER BY PackingId, pd.BoxNumber").ToList();


                foreach (var item in shipment)
                {
                    cstPackageDetails _ShipmentInfo = new cstPackageDetails();
                    _ShipmentInfo.BoxNumber = item.BoxNumber;
                    _ShipmentInfo.SKUNumber = item.SKUNumber;
                    _ShipmentInfo.SKUQuantity = item.SKUQuantity;
                    _ShipInfo.Add(_ShipmentInfo);
                }

            }
            catch (Exception)
            {
            }
            return _ShipInfo;
        }

        public List<cstShippingInfo> GetShipmentInfoByShipment(String ShipmentNumber, String location)
        {
            List<cstShippingInfo> _ShipInfo = new List<cstShippingInfo>();

            try
            {

                var shipment = entx3v6.ExecuteStoreQuery<cstShippingInfo>(@"select
                                                                            case when ROW_NUMBER() over (partition by plt.PalletNumber order by plt.PalletNumber, pltd.BoxNumber) = 1 then plt.PalletNumber else null end PalletNumber,
                                                                            case when ROW_NUMBER() over (partition by plt.PalletNumber,pltd.BoxNumber order by pltd.BoxNumber) = 1 then pltd.BoxNumber else null end BoxNumber,
                                                                            pkd.SKUNumber, pkd.SKUQuantity as SKUQty
                                                                            from
                                                                            Package pk inner join
                                                                            PackageDetail pkd on pk.PackingID = pkd.PackingID inner join
                                                                            PalletDetail pltd on pkd.BoxNumber = pltd.BoxNumber inner join
                                                                            PalletInfo plt on pltd.PalletID = plt.PalletID where pltd.ShipmentNumber='" + ShipmentNumber + "' AND pk.ShipmentLocation='" + location + "'order by plt.PalletNumber, ROW_NUMBER() over (partition by plt.PalletNumber order by plt.PalletNumber, pltd.BoxNumber),pltd.BoxNumber, pkd.SKUNumber, pkd.SKUQuantity").ToList();


                foreach (var item in shipment)
                {
                    cstShippingInfo _ShipmentInfo = new cstShippingInfo();
                    _ShipmentInfo.PalletNumber = item.PalletNumber;
                    _ShipmentInfo.BoxNumber = item.BoxNumber;
                    _ShipmentInfo.SKUNumber = item.SKUNumber;
                    _ShipmentInfo.SKUQty = item.SKUQty;
                    _ShipInfo.Add(_ShipmentInfo);
                }

            }
            catch (Exception)
            {
            }
            return _ShipInfo;
        }


        #endregion
    }
}
