using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstCartonInfo
    {
        public Guid CartonID { get; set; }
        public String BOXNumber { get; set; }
        public String ShipmentNumber { get; set; }
        public int CartonNumber { get; set; }
        public int Printed { get; set; }
    }
}
