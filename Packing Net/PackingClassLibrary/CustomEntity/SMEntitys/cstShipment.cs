using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    /// <summary>
    /// All veriables that are needed for shipment screen declred eith their properties.
    /// </summary>
    public class cstShipment : IEditableObject, INotifyPropertyChanged
    {
        //Variables for shipment screen
        public string SKU { get; set; }
        public long Quantity { get; set; }
        public int Packed { get; set; }
        public string ProductName { get; set; }
        public string PackedDate { get; set; }
        public string UPCCode { get; set; }
        public string Location { get; set; }
        public int LineType { get; set; }
        public int ComboID { get; set; }
        public long RowNumber { get; set; }

        #region Begin Edit handeler
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupShipment = this.MemberwiseClone() as cstShipment;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Packed = backupShipment.Packed;
            backupShipment = null;
        }
        
        public void EndEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            backupShipment = null;
        }

        //Handler Created for DataGrid
     public event PropertyChangedEventHandler PropertyChanged;
     #endregion

     public cstShipment backupShipment { get; set; }
    }
}
