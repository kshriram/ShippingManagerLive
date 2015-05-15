using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
   public static class cmdViewExtra 
    {
       

       public static cstViewExtraColumns GetExtraColumns(String ShippingNumber, String SKUName)
       {local_x3v6Entities entlocal = new local_x3v6Entities();
           cstViewExtraColumns _return = new cstViewExtraColumns();
           try
           {
               var Columns = from view in entlocal.Get_Shipping_Data
                             where view.SKU == SKUName && view.ShipmentID == ShippingNumber
                             select new
                             {
                                 view.TarrifCode,
                                 view.TCLCOD_0,
                                 view.ItemName,
                                 view.UnitOfMeasure,
                                 view.CountryOfOrigin,
                                 view.MAP_Price
                             };

               foreach (var _viewitem in Columns)
               {
                   _return.TarrifCode = _viewitem.TarrifCode;
                   _return.TCLCOD_0 = _viewitem.TCLCOD_0;
                   _return.ItemName = _viewitem.ItemName;
                   _return.UnitOfMeasure = _viewitem.UnitOfMeasure;
                   _return.CountryOfOrigin = _viewitem.CountryOfOrigin;
                   _return.MAP_Price = _viewitem.MAP_Price;
               }
           }
           catch (Exception)
           {
               
               
           }
           return _return;
 
       }

    }
}
