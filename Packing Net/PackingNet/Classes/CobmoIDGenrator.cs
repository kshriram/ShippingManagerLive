using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingNet.Classes
{
   public class CobmoIDGenrator
    {
       private int PreviousLineType=1;
       
       private int GetComboNumber(int LineType)
       {
           if (LineType != 7)
               PreviousLineType = PreviousLineType + 1;
               
           return PreviousLineType;
       }


       public  List<cstShipment> SetComboNumbers(List<cstShipment> lsShipmentNumbers)
       {
           foreach (var item in lsShipmentNumbers)
           {
              item.ComboID= GetComboNumber(item.LineType);
           }
           return lsShipmentNumbers;
       }

       

    }
}
