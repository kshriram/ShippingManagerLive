using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstPalletDetails
    {
        public Guid PalletID { get; set; }
        public Guid PalletDetailID { get; set; }
        public string BoxNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string CartonNumber { get; set; }
        public int PrintStatus { get; set; }


    }
}
