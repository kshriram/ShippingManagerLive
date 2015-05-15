using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity.ReportEntitys
{
    public class cstUserShipmentCount
    {
        public Guid UserID { get; set; }
        public String UserName { get; set; }
        public DateTime Datepacked { get; set; }
        public int ShipmentCount { get; set; }

    }
}
