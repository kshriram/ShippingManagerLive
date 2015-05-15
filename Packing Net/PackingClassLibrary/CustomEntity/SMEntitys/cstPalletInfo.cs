using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstPalletInfo
    {
        public Guid PalletID { get; set; }
        public string PalletType { get; set; }
        public Double PalletWeight { get; set; }
        public Double PalletHeight { get; set; }
        public Double PalletWidth { get; set; }
        public int? PrintStatus { get; set; }
        public DateTime palletCreatedTime { get; set; }
        public int RowID { get; set; }
        public string PalletNumber { get; set; }

        public string PLT_Carrier { get; set; }
        public string PLT_BOL { get; set; }
        public string PLT_PRO { get; set; }

        public string Location { get; set; }

        public DateTime PalletEndDateTime { get; set; }

        public DateTime BOLCreatedDateTime { get; set; }

        public string SSCC_Code { get; set; }

    }
}
