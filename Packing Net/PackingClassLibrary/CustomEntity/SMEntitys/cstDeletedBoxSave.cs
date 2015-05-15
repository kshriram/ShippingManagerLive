using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstDeletedBoxSave
    {
        public Guid DeletedBoxID { get; set; }

        public string BoxNumber { get; set; }
        public string SKUName { get; set; }
        public int SKUQuantity { get; set; }
        public Guid UserID { get; set; }

        public DateTime DeleteDateTime { get; set; }

        public string Location { get; set; }

        public string StationName { get; set; }

        public string ActionStatus { get; set; }
    }
}
