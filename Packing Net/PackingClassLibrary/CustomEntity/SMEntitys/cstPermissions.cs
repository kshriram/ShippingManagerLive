using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    public class cstPermissions
    {
        public bool viewUser { get; set; }
        public bool newUser { get; set; }
        public bool editUser { get; set; }
        public bool deleteUser { get; set; }
        public bool viewShipment { get; set; }
        public bool scanShipment { get; set; }
        public bool reScanShipment { get; set; }
        public bool overrideShipment { get; set; }

        public cstPermissions GetShallowCopy()
        {
            return (cstPermissions)this.MemberwiseClone();
        }
    }
}
