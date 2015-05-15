using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
   public class cstShippingInfo
    {
        public Guid PackingID { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentLocation { get; set; }
        public string BoxNumber { get; set; }
        public Guid? PalletID { get; set; }
        public string PalletNumber { get; set; }
        public string SKUNumber { get; set; }
        public int SKUQty { get; set; }
    }
}
