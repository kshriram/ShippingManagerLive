using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    public class cstUserStationTbl
    {
        public Guid UserStationID { get; set; }
        public Guid StationID { get; set; }
        public Guid UserID { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}
