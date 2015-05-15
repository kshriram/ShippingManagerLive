using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    /// <summary>
    /// Avinash.
    /// Packing main tables veriables property Declaretion.
    /// </summary>
    public class cstPackageTbl
    {
        public Guid PackingId{ get; set; }
        public Guid UserID { get; set; }
        public Guid StationID{ get; set; }
        public String ShippingNum{ get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PackingStatus { get; set; }
        public String ShipmentLocation { get; set; }
        public Guid ShippingID { get; set; }
        public int MangerOverride { get; set; }
        public string PCKROWID { get; set; }

        public int UPS_FEDEX_flg { get; set; }
        public int LTL_flg { get; set; }
        public int pallet_flg { get; set; }
        public string PLT_carrier { get; set; }
        public string PLT_BOL { get; set; }
        public string PLT_PRO { get; set; }
      
    }
}
