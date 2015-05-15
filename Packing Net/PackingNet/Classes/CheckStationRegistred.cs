using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;

namespace Packing_Net.Classes
{
   public static class CheckStationRegistred
    {
       public static Boolean IsRegistered()
       {
           Boolean _return = false;

           try
           {
               List<cstStationMasterTbl> lsStation = Global.controller.GetStationMaster(Global.controller.getDeviceMAC());
               if (lsStation.Count>0 && lsStation[0].StationAlive !=0)
               {
                   _return = true;
               }
           }
           catch (Exception)
           {
           }
           return _return;
       }
    }
}
