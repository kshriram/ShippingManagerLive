using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity.ReportEntitys
{
    public class cstShippingInfoBPName
    {
        public Guid ShippingID { get; set; }
        public String ShippingNumner { get; set; }
        public String BusinessPartNo { get; set; }
        public String BPName { get; set; }
        public String ShippingStatus { get; set; }
    }
}
