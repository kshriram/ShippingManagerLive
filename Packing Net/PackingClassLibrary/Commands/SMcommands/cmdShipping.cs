using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands.SMcommands
{
    public class cmdShipping
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();
        Sage_x3v6Entities Sage = new Sage_x3v6Entities();

        #region set Methods
        public Boolean SaveShippingTbl(List<cstShippingTbl> lsShipping)
        {
            Boolean _return = false;
            try
            {
                if (lsShipping.Count() > 0)
                {
                    foreach (cstShippingTbl _shippingInfo in lsShipping)
                    {
                        Guid _tShipID = Guid.Empty;
                        try { _tShipID = entx3v6.Shippings.SingleOrDefault(i => i.ShippingNum == _shippingInfo.ShippingNum).ShippingID; }
                        catch (Exception) { }

                        //If shipping number is not saved previously then insert new record
                        if (_tShipID == Guid.Empty)
                        {
                            Shipping Ship = new Shipping();
                            Ship.ShippingID = Guid.NewGuid();
                            Ship.ShippingNum = _shippingInfo.ShippingNum;
                            Ship.ShippingStartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.ShippingEndTime = null;
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
                            Ship.Indexcode = _shippingInfo.Indexcode;
                            Ship.Contact = _shippingInfo.Contact;
                            Ship.PaymentTerms = _shippingInfo.PaymentTerms;
                            Ship.TotalPackages = _shippingInfo.TotalPackages;
                            Ship.Fax = _shippingInfo.Fax;
                            Ship.VendorName = _shippingInfo.VendorName;
                            Ship.MDL_0 = _shippingInfo.MDL_0;
                            Ship.XB_RESFLG_0 = _shippingInfo.XB_RESFLG_0;
                            Ship.CODCHG_0 = _shippingInfo.CODCHG_0;
                            Ship.INSVAL_0 = _shippingInfo.INSVAL_0;
                            Ship.ADDCODFRT_0 = _shippingInfo.ADDCODFRT_0;
                            Ship.BILLOPT_0 = _shippingInfo.BILLOPT_0;
                            Ship.HDLCHG_0 = _shippingInfo.HDLCHG_0;
                            Ship.DOWNFLG_0 = _shippingInfo.DOWNFLG_0;
                            Ship.BACCT_0 = _shippingInfo.BACCT_0;
                            Ship.TPBILL_0 = _shippingInfo.TPBILL_0;
                            Ship.CUSTBILL_0 = _shippingInfo.CUSTBILL_0;
                            Ship.CNTFULNAM_0 = _shippingInfo.CNTFULNAM_0;
                            Ship.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.CreatedBy = GlobalClasses.ClGlobal.UserID;
                            entx3v6.AddToShippings(Ship);
                        }
                        else //If shipping number is saved previously then updated old record
                        {
                            Shipping Ship = entx3v6.Shippings.SingleOrDefault(i => i.ShippingID == _tShipID);
                            Ship.ShippingNum = _shippingInfo.ShippingNum;
                            DateTime EndTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            DateTime.TryParse(_shippingInfo.ShippingEndTime.ToString(), out EndTime);
                            Ship.ShippingEndTime = null;
                            if (EndTime.Date != Convert.ToDateTime("01/01/0001"))
                            { Ship.ShippingEndTime = EndTime; }
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
                            Ship.Indexcode = _shippingInfo.Indexcode;
                            Ship.Contact = _shippingInfo.Contact;
                            Ship.PaymentTerms = _shippingInfo.PaymentTerms;
                            Ship.TotalPackages = _shippingInfo.TotalPackages;
                            Ship.Fax = _shippingInfo.Fax;
                            Ship.VendorName = _shippingInfo.VendorName;
                            Ship.MDL_0 = _shippingInfo.MDL_0;
                            Ship.XB_RESFLG_0 = _shippingInfo.XB_RESFLG_0;
                            Ship.CODCHG_0 = _shippingInfo.CODCHG_0;
                            Ship.INSVAL_0 = _shippingInfo.INSVAL_0;
                            Ship.ADDCODFRT_0 = _shippingInfo.ADDCODFRT_0;
                            Ship.BILLOPT_0 = _shippingInfo.BILLOPT_0;
                            Ship.HDLCHG_0 = _shippingInfo.HDLCHG_0;
                            Ship.DOWNFLG_0 = _shippingInfo.DOWNFLG_0;
                            Ship.BACCT_0 = _shippingInfo.BACCT_0;
                            Ship.TPBILL_0 = _shippingInfo.TPBILL_0;
                            Ship.CUSTBILL_0 = _shippingInfo.CUSTBILL_0;
                            Ship.CNTFULNAM_0 = _shippingInfo.CNTFULNAM_0;
                            Ship.UpdatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                            Ship.Updatedby = GlobalClasses.ClGlobal.UserID;
                        }

                    }
                    entx3v6.SaveChanges();
                    _return = true;
                }
            }
            catch (Exception)
            { }

            return _return;
        }
        #endregion

        #region Get Methods
        public List<cstShippingTbl> GetShipping()
        {
            List<cstShippingTbl> lsshippingInfo = new List<cstShippingTbl>();
            try
            {
                var ShippingInfo = from ls in entx3v6.Shippings select ls;

                foreach (var _shippingInfo in ShippingInfo.ToList())
                {
                    DateTime Edate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    DateTime.TryParse(_shippingInfo.ShippingEndTime.ToString(), out Edate);
                    cstShippingTbl Ship = new cstShippingTbl();
                    Ship.ShippingID = _shippingInfo.ShippingID;
                    Ship.ShippingNum = _shippingInfo.ShippingNum;
                    Ship.ShippingStartTime = Convert.ToDateTime(_shippingInfo.ShippingStartTime);
                    Ship.ShippingEndTime = Edate;
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
                    Ship.MDL_0 = _shippingInfo.MDL_0;
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
                    Ship.SHIPPINGROWID = _shippingInfo.SHIPPINGROWID;
                    lsshippingInfo.Add(Ship);
                }
            }
            catch (Exception)
            { }
            return lsshippingInfo;

        }


        public cstShippingTbl GetShippingByShippingNumber(String ShippingNum)
        {
            cstShippingTbl Ship = new cstShippingTbl();
            try
            {
                Shipping _shippingInfo = entx3v6.Shippings.SingleOrDefault(i => i.ShippingNum == ShippingNum);
                
                Ship.ShippingID = _shippingInfo.ShippingID;
                Ship.ShippingNum = _shippingInfo.ShippingNum;
                Ship.ShippingStartTime = Convert.ToDateTime(_shippingInfo.ShippingStartTime);
                Ship.ShippingEndTime = Convert.ToDateTime(_shippingInfo.ShippingEndTime); ;
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
                Ship.MDL_0 = _shippingInfo.MDL_0;
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
                Ship.SHIPPINGROWID = _shippingInfo.SHIPPINGROWID;

            }
            catch (Exception)
            { }
            return Ship;
        }
        #endregion

        #region set Delete

        #endregion
    }
}
