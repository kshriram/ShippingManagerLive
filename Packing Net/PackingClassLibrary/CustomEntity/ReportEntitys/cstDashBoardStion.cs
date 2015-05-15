using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.ReportEntitys
{
   public class cstDashBoardStion
   {
       public String StationName { get; set; }
       public int ErrorCaught { get; set; }
       public int TotalPacked { get; set; }
       public String packagePerhr { get; set; }
       public String ShipmentNumber { get; set; }
       public String PackerName { get; set; }
    }
}
