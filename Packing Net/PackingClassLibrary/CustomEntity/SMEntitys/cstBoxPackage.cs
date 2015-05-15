using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
    public class cstBoxPackage
    {
        public Guid BoxID { get; set; }
        public Guid PackingID { get; set; }
        public string BoxType { get; set; }
        public Double BoxWeight { get; set; }
        public Double BoxLength { get; set; }
        public Double BoxHeight { get; set; }
        public Double BoxWidth { get; set; }
        public DateTime BoxCreatedTime { get; set; }
        public DateTime BoxMeasurementTime { get; set; }
        public int ROWID { get; set; }
        public string BOXNUM { get; set; }

        public DateTime BoxEndTime { get; set; }

    }
}
