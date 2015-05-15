using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.CustomEntity.SMEntitys
{
   public class cstViewExtraColumns
    {
        public String ItemName { get; set; }
        public String UnitOfMeasure { get; set; }
        public string CountryOfOrigin { get; set; }
        public Decimal MAP_Price { get; set; }
        public String TCLCOD_0 { get; set; }
        public String TarrifCode { get; set; }
    }
}
