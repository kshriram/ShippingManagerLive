using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
   public class cstSlipData
    {
        public string  ToAddressLine1 { get; set; }
        public string  ToAddressLine2 { get; set; }
        public string ToAddressLine3 { get; set; }
        public string  ToAddressState { get; set; }
        public string ToAddressCity { get; set; }
        public string  ToAddressZipCode { get; set; }
        public string CustomerPO { get; set; }
        public string VendorName { get; set; }
        public string UPCCode { get; set; }
    }
}
