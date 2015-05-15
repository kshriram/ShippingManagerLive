using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
   public class cstStationMasterTbl
    {
        public Guid StationID { get; set; }
        public string StationName { get; set; }
        public Guid RequestedUserID { get; set; }
        public int StationAlive { get; set; }
        public string DeviceNumber { get; set; }
        public String StaionLocation{ get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
