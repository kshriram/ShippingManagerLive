using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity
{
    public class cstDeletedPalletSave
    {
        public Guid DeletedPalletID { get; set; }

        public string PalletNumber { get; set; }
        public string BoxNumber { get; set; }

        public Guid UserID { get; set; }

        public DateTime DeleteDateTime { get; set; }

        public string Location { get; set; }

        public string StationName { get; set; }

        public string ActionStatus { get; set; }
    }
}
