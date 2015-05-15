using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands.SMcommands
{
    /// <summary>
    /// Author: Avinash
    /// Version: Alfa.
    /// Sage Get oprtaions.
    /// </summary>
   public class cmdSageOperations
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();
        Sage_x3v6Entities entSage = new Sage_x3v6Entities();

        /// <summary>
        /// Call the Sql query to and collect shipment locations
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>Boolean Value that shows this shipment packing on multiple locations or not(true/false)</returns>
        public Boolean getShipmentOnMultipleLocation(String ShipmentID)
        {
            Boolean _return = false;
            try
            {
                var LocationCount = (from _shipview in entx3v6.Get_Shipping_Data.OrderBy(i=> i.Row_Number).AsParallel()
                                     where _shipview.ShipmentID == ShipmentID
                                     group _shipview by _shipview.LocationCombined into Gship
                                     select Gship).ToList();
                if (LocationCount.Count() > 1)
                {
                    _return = true;
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("ShipmentLocation.ShipmentOnMultipleLocation()", Ex.Message.ToString());
            }
            return _return;
        }

        /// <summary>
        /// list of Shipment id and its locations
        /// that is list of ShipmentLocationNameCustomEntity
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>list of Shipment id and its locations</returns>
        public List<cstShipmentLocationWise> getLocationListOFShipment(String ShipmentID)
        {
            List<cstShipmentLocationWise> _lsShList = new List<cstShipmentLocationWise>();
            try
            {
                var LocationCount = from _shipview in entx3v6.Get_Shipping_Data.OrderBy(i=> i.Row_Number)
                                     where _shipview.ShipmentID == ShipmentID
                                     group _shipview by _shipview.LocationCombined  into Gship
                                     select Gship;

                foreach (var _loca in LocationCount)
                {
                    cstShipmentLocationWise Shipmentloc = new cstShipmentLocationWise();
                    Shipmentloc.ShipmentID = ShipmentID;
                    Shipmentloc.LocationName = _loca.Key;
                    _lsShList.Add(Shipmentloc);
                }

            }

            catch (Exception Ex)
            {
                Error_Loger.elAction.save("ShipmentLocation.LocationListOFShipment()", Ex.Message.ToString());
            }

            return _lsShList;
        }
        public List<cstShippingTbl> getShippingInfo(String ShippingNumber)
        {
            List<cstShippingTbl> _lsShippingInfo = new List<cstShippingTbl>();
            try
            {
                var ShippingInfo = from ls in entx3v6.getShippingDetails
                                   where ls.ShippingNum == ShippingNumber
                                   select ls;
                //AND [SDELIVERY].[SDHNUM_0]='" + ShippingNumber + "';").ToList();
                if (ShippingInfo != null)
                {
                    foreach (var _shippingInfo in ShippingInfo.ToList())
                    {
                        cstShippingTbl Ship = new cstShippingTbl();
                        // Ship.ShippingID = _shippingInfo.ShippingID;
                        Ship.ShippingNum = _shippingInfo.ShippingNum;
                        Ship.ShippingStartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                        Ship.ShippingEndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                        Ship.DeliveryProvider = _shippingInfo.DeliveryProvider;
                        Ship.DeliveryMode = _shippingInfo.DeliveryMode;
                        Ship.FromAddressLine1 = _shippingInfo.FromAddressLine1;
                        Ship.FromAddressLine2 = _shippingInfo.FromAddressLine2;
                        Ship.FromAddressLine3 = _shippingInfo.FromAddressLine3;
                        Ship.FromAddressCity = _shippingInfo.FromAddressCity;
                        Ship.FromAddressState = _shippingInfo.FromAddressState;
                        Ship.FromAddressCountry = _shippingInfo.FromAddressCountry;
                        Ship.FromAddressZipCode = _shippingInfo.FromAddressZipCode;
                        Ship.ToAddressLine1 = _shippingInfo.ToAddressLine1;
                        Ship.ToAddressLine2 = _shippingInfo.ToAddressLine2;
                        Ship.ToAddressLine3 = _shippingInfo.ToAddressLine3;
                        Ship.ToAddressCity = _shippingInfo.ToAddressCity;
                        Ship.ToAddressState = _shippingInfo.ToAddressState;
                        Ship.ToAddressCountry = _shippingInfo.ToAddressCountry;
                        Ship.ToAddressZipCode = _shippingInfo.ToAddressZipCode;
                        Ship.ShipmentStatus = _shippingInfo.ShipmentStatus;
                        Ship.OrderID = _shippingInfo.OrderID;
                        Ship.CustomerPO = _shippingInfo.CustomerPO;
                        Ship.ShipToAddress = _shippingInfo.ShipToAddress;
                        Ship.OurSupplierNo = _shippingInfo.OurSupplierNo;
                        Ship.CustomerName1 = _shippingInfo.CustomerName1;
                        Ship.CustomerName2 = _shippingInfo.CustomerName2;
                        Ship.WebAddress = _shippingInfo.WebAddress;
                        Ship.FreightTerms = _shippingInfo.FreightTerms;
                        Ship.Carrier = _shippingInfo.Carrier;
                        Ship.DeliveryContact = _shippingInfo.DeliveryContact;
                        Ship.Indexcode = Convert.ToInt16(_shippingInfo.Indexcode);
                        Ship.Contact = _shippingInfo.Contact;
                        Ship.PaymentTerms = _shippingInfo.PaymentTerms;
                        Ship.TotalPackages = Convert.ToInt32(_shippingInfo.TotalPackages);
                        Ship.Fax = _shippingInfo.Fax;
                        Ship.VendorName = _shippingInfo.VendorName;
                        Ship.MDL_0 = _shippingInfo.DeliveryMode;
                        Ship.XB_RESFLG_0 = Convert.ToByte(_shippingInfo.XB_RESFLG_0);
                        Ship.CODCHG_0 = _shippingInfo.CODCHG_0;
                        Ship.INSVAL_0 = Convert.ToDecimal(_shippingInfo.INSVAL_0);
                        Ship.ADDCODFRT_0 = Convert.ToByte(_shippingInfo.ADDCODFRT_0);
                        Ship.BILLOPT_0 = _shippingInfo.BILLOPT_0;
                        Ship.HDLCHG_0 = _shippingInfo.HDLCHG_0;
                        Ship.DOWNFLG_0 = Convert.ToByte(_shippingInfo.DOWNFLG_0);
                        Ship.BACCT_0 = _shippingInfo.BACCT_0;
                        Ship.TPBILL_0 = Convert.ToByte(_shippingInfo.TPBILL_0);
                        Ship.CUSTBILL_0 = Convert.ToByte(_shippingInfo.CUSTBILL_0);
                        Ship.CNTFULNAM_0 = _shippingInfo.CNTFULNAM_0;
                        _lsShippingInfo.Add(Ship);
                    }
                }
            }
            catch (Exception)
            { }
            return _lsShippingInfo;
        }
       /// <summary>
       /// Shipping table information From sage.
       /// </summary>
       /// <param name="ShippingNumber">String Shippign Number</param>
        /// <returns> List of cstShippingTbl  information.</returns>
        public List<cstShippingTbl> getShippingInfo(String ShippingNumber, string Flag)
        {
            List<cstShippingTbl> _lsShippingInfo = new List<cstShippingTbl>();
            if (Flag == "UPSFEDEX")
            {
                try
                {
                    var ShippingInfo = from ls in entx3v6.getShippingDetails
                                       where ls.ShippingNum == ShippingNumber
                                       select ls;
                    //AND [SDELIVERY].[SDHNUM_0]='" + ShippingNumber + "';").ToList();
                    if (ShippingInfo != null)
                    {
                        foreach (var _shippingInfo in ShippingInfo.ToList())
                        {
                            cstShippingTbl Ship = new cstShippingTbl();
                            // Ship.ShippingID = _shippingInfo.ShippingID;
                            Ship.ShippingNum = _shippingInfo.ShippingNum;
                            Ship.ShippingStartTime =TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.ShippingEndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.DeliveryProvider = _shippingInfo.DeliveryProvider;
                            Ship.DeliveryMode = _shippingInfo.DeliveryMode;
                            Ship.FromAddressLine1 = _shippingInfo.FromAddressLine1;
                            Ship.FromAddressLine2 = _shippingInfo.FromAddressLine2;
                            Ship.FromAddressLine3 = _shippingInfo.FromAddressLine3;
                            Ship.FromAddressCity = _shippingInfo.FromAddressCity;
                            Ship.FromAddressState = _shippingInfo.FromAddressState;
                            Ship.FromAddressCountry = _shippingInfo.FromAddressCountry;
                            Ship.FromAddressZipCode = _shippingInfo.FromAddressZipCode;
                            Ship.ToAddressLine1 = _shippingInfo.ToAddressLine1;
                            Ship.ToAddressLine2 = _shippingInfo.ToAddressLine2;
                            Ship.ToAddressLine3 = _shippingInfo.ToAddressLine3;
                            Ship.ToAddressCity = _shippingInfo.ToAddressCity;
                            Ship.ToAddressState = _shippingInfo.ToAddressState;
                            Ship.ToAddressCountry = _shippingInfo.ToAddressCountry;
                            Ship.ToAddressZipCode = _shippingInfo.ToAddressZipCode;
                            Ship.ShipmentStatus = _shippingInfo.ShipmentStatus;
                            Ship.OrderID = _shippingInfo.OrderID;
                            Ship.CustomerPO = _shippingInfo.CustomerPO;
                            Ship.ShipToAddress = _shippingInfo.ShipToAddress;
                            Ship.OurSupplierNo = _shippingInfo.OurSupplierNo;
                            Ship.CustomerName1 = _shippingInfo.CustomerName1;
                            Ship.CustomerName2 = _shippingInfo.CustomerName2;
                            Ship.WebAddress = _shippingInfo.WebAddress;
                            Ship.FreightTerms = _shippingInfo.FreightTerms;
                            Ship.Carrier = _shippingInfo.Carrier;
                            Ship.DeliveryContact = _shippingInfo.DeliveryContact;
                            Ship.Indexcode = Convert.ToInt16(_shippingInfo.Indexcode);
                            Ship.Contact = _shippingInfo.Contact;
                            Ship.PaymentTerms = _shippingInfo.PaymentTerms;
                            Ship.TotalPackages = Convert.ToInt32(_shippingInfo.TotalPackages);
                            Ship.Fax = _shippingInfo.Fax;
                            Ship.VendorName = _shippingInfo.VendorName;
                            Ship.MDL_0 = _shippingInfo.DeliveryMode;
                            Ship.XB_RESFLG_0 = Convert.ToByte(_shippingInfo.XB_RESFLG_0);
                            Ship.CODCHG_0 = _shippingInfo.CODCHG_0;
                            Ship.INSVAL_0 = Convert.ToDecimal(_shippingInfo.INSVAL_0);
                            Ship.ADDCODFRT_0 = Convert.ToByte(_shippingInfo.ADDCODFRT_0);
                            Ship.BILLOPT_0 = _shippingInfo.BILLOPT_0;
                            Ship.HDLCHG_0 = _shippingInfo.HDLCHG_0;
                            Ship.DOWNFLG_0 = Convert.ToByte(_shippingInfo.DOWNFLG_0);
                            Ship.BACCT_0 = _shippingInfo.BACCT_0;
                            Ship.TPBILL_0 = Convert.ToByte(_shippingInfo.TPBILL_0);
                            Ship.CUSTBILL_0 = Convert.ToByte(_shippingInfo.CUSTBILL_0);
                            Ship.CNTFULNAM_0 = _shippingInfo.CNTFULNAM_0;
                            _lsShippingInfo.Add(Ship);
                        }
                    }
                }

                catch (Exception)
                { }
            }
            else if (Flag == "LTL")
            {
                try
                {
                    var ShippingInfo = from ls in entx3v6.getShippingDetails_LTL_Non_FEDEX_UPS
                                       where ls.ShippingNum == ShippingNumber
                                       select ls;
                    //AND [SDELIVERY].[SDHNUM_0]='" + ShippingNumber + "';").ToList();
                    if (ShippingInfo != null)
                    {
                        foreach (var _shippingInfo in ShippingInfo.ToList())
                        {
                            cstShippingTbl Ship = new cstShippingTbl();
                            // Ship.ShippingID = _shippingInfo.ShippingID;
                            Ship.ShippingNum = _shippingInfo.ShippingNum;
                            Ship.ShippingStartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.ShippingEndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.DeliveryProvider = _shippingInfo.DeliveryProvider;
                            Ship.DeliveryMode = _shippingInfo.DeliveryMode;
                            Ship.FromAddressLine1 = _shippingInfo.FromAddressLine1;
                            Ship.FromAddressLine2 = _shippingInfo.FromAddressLine2;
                            Ship.FromAddressLine3 = _shippingInfo.FromAddressLine3;
                            Ship.FromAddressCity = _shippingInfo.FromAddressCity;
                            Ship.FromAddressState = _shippingInfo.FromAddressState;
                            Ship.FromAddressCountry = _shippingInfo.FromAddressCountry;
                            Ship.FromAddressZipCode = _shippingInfo.FromAddressZipCode;
                            Ship.ToAddressLine1 = _shippingInfo.ToAddressLine1;
                            Ship.ToAddressLine2 = _shippingInfo.ToAddressLine2;
                            Ship.ToAddressLine3 = _shippingInfo.ToAddressLine3;
                            Ship.ToAddressCity = _shippingInfo.ToAddressCity;
                            Ship.ToAddressState = _shippingInfo.ToAddressState;
                            Ship.ToAddressCountry = _shippingInfo.ToAddressCountry;
                            Ship.ToAddressZipCode = _shippingInfo.ToAddressZipCode;
                            Ship.ShipmentStatus = _shippingInfo.ShipmentStatus;
                            Ship.OrderID = _shippingInfo.OrderID;
                            Ship.CustomerPO = _shippingInfo.CustomerPO;
                            Ship.ShipToAddress = _shippingInfo.ShipToAddress;
                            Ship.OurSupplierNo = _shippingInfo.OurSupplierNo;
                            Ship.CustomerName1 = _shippingInfo.CustomerName1;
                            Ship.CustomerName2 = _shippingInfo.CustomerName2;
                            Ship.WebAddress = _shippingInfo.WebAddress;
                            Ship.FreightTerms = _shippingInfo.FreightTerms;
                            Ship.Carrier = _shippingInfo.Carrier;
                            Ship.DeliveryContact = _shippingInfo.DeliveryContact;
                            Ship.Indexcode = Convert.ToInt16(_shippingInfo.Indexcode);
                            Ship.Contact = _shippingInfo.Contact;
                            Ship.PaymentTerms = _shippingInfo.PaymentTerms;
                            Ship.TotalPackages = Convert.ToInt32(_shippingInfo.TotalPackages);
                            Ship.Fax = _shippingInfo.Fax;
                            Ship.VendorName = _shippingInfo.VendorName;
                            Ship.MDL_0 = _shippingInfo.DeliveryMode;
                            Ship.XB_RESFLG_0 = Convert.ToByte(_shippingInfo.XB_RESFLG_0);
                            Ship.CODCHG_0 = _shippingInfo.CODCHG_0;
                            Ship.INSVAL_0 = Convert.ToDecimal(_shippingInfo.INSVAL_0);
                            Ship.ADDCODFRT_0 = Convert.ToByte(_shippingInfo.ADDCODFRT_0);
                            Ship.BILLOPT_0 = _shippingInfo.BILLOPT_0;
                            Ship.HDLCHG_0 = _shippingInfo.HDLCHG_0;
                            Ship.DOWNFLG_0 = Convert.ToByte(_shippingInfo.DOWNFLG_0);
                            Ship.BACCT_0 = _shippingInfo.BACCT_0;
                            Ship.TPBILL_0 = Convert.ToByte(_shippingInfo.TPBILL_0);
                            Ship.CUSTBILL_0 = Convert.ToByte(_shippingInfo.CUSTBILL_0);
                            Ship.CNTFULNAM_0 = _shippingInfo.CNTFULNAM_0;
                            _lsShippingInfo.Add(Ship);
                        }
                    }
                }

                catch (Exception)
                { }
            }
            return _lsShippingInfo;
        }
    }
}
