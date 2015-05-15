using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstTrackingTbl
    {
        public Guid TrackingID { get; set; }
        public Guid PackingID { get; set; }
        public Guid ShippingID { get; set; }
        public String BoxNum { get; set; }
        public String TrackingNum { get; set; }
        public DateTime TrackingDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public String VOIIND { get; set; }
        public Boolean ReadyToExport { get; set; }
        public Boolean Exported { get; set; }
        public String PCKCHG { get; set; }
        public String Weight { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Updatedby { get; set; }

    }
}
