using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    /// <summary>
    /// Avinash
    /// Packing table all veriales property set
    /// </summary>
    public class cstPackageDetails
    {
        public Guid PackingDetailID { get; set; }
        public Guid PackingId { get; set; }
        public String SKUNumber { get; set; }
        public int SKUQuantity { get; set; }
        public DateTime PackingDetailStartDateTime { get; set; }
        public String BoxNumber { get; set; }
        public String ShipmentLocation { get; set; }
        public String ItemName { get; set; }
        public string ProductName { get; set; }
        public String UnitOfMeasure { get; set; }
        public string CountryOfOrigin { get; set; }
        public Decimal MAP_Price { get; set; }
        public String TCLCOD_0 { get; set; }
        public String TarrifCode { get; set; }
        public int? RowNumber { get; set; }
    }
}
