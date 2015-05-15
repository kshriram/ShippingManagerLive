using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity.ReportEntitys
{
    public class cstStationToatlPacked
    {
        public Guid StationID { get; set; }
        public String StationName { get; set; }
        public int TotalPacked { get; set; }
        public int PartiallyPacked { get; set; }
    }
}
