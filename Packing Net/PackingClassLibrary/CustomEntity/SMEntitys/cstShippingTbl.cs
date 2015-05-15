using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    public class cstShippingTbl
    {
        public Guid ShippingID { get; set; }
        public String ShippingNum { get; set; }
        public DateTime ShippingStartTime { get; set; }
        public DateTime ShippingEndTime { get; set; }
        public String DeliveryProvider { get; set; }
        public String DeliveryMode { get; set; }
        public String FromAddressLine1 { get; set; }
        public String FromAddressLine2 { get; set; }
        public String FromAddressLine3 { get; set; }
        public String FromAddressCity { get; set; }
        public String FromAddressState { get; set; }
        public string FromAddressCountry { get; set; }
        public String FromAddressZipCode { get; set; }
        public String ToAddressLine1 { get; set; }
        public String ToAddressLine2 { get; set; }
        public String ToAddressLine3 { get; set; }
        public String ToAddressCity { get; set; }
        public String ToAddressState { get; set; }
        public String ToAddressCountry { get; set; }
        public String ToAddressZipCode { get; set; }
        public String ShipmentStatus { get; set; }
        public String OrderID { get; set; }
        public String CustomerPO { get; set; }
        public String ShipToAddress { get; set; }
        public String OurSupplierNo { get; set; }
        public String CustomerName1 { get; set; }
        public String CustomerName2 { get; set; }
        public String WebAddress { get; set; }
        public String FreightTerms { get; set; }
        public String Carrier { get; set; }
        public String DeliveryContact { get; set; }
        public Int16 Indexcode { get; set; }
        public String Contact { get; set; }
        public String PaymentTerms { get; set; }
        public int TotalPackages { get; set; }
        public String Fax { get; set; }
        public String VendorName { get; set; }
        public string MDL_0 { get; set; }
        public String YCARSRV_0 { get; set; }
        public Byte XB_RESFLG_0 { get; set; }
        public String CODCHG_0 { get; set; }
        public Decimal INSVAL_0 { get; set; }
        public Byte ADDCODFRT_0 { get; set; }
        public String BILLOPT_0 { get; set; }
        public String HDLCHG_0 { get; set; }
        public Byte DOWNFLG_0 { get; set; }
        public String BACCT_0 { get; set; }
        public Byte TPBILL_0 { get; set; }
        public Byte CUSTBILL_0 { get; set; }
        public String CNTFULNAM_0 { get; set; }
        public String SHIPPINGROWID { get; set; }
       
    }
}