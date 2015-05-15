using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    public class cstShipmentQuary
    {
        public string ShipmentID { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string UPCcode { get; set; }
        public string Location { get; set; }
    }
}
