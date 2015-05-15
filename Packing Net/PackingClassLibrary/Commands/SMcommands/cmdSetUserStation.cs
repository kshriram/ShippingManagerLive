using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;

namespace PackingClassLibrary.Commands
{
   public class cmdSetUserStation
    {
       local_x3v6Entities ent = new local_x3v6Entities();

       /// <summary>
       /// Save UserStation transaction Table.
       /// </summary>
       /// <param name="lsUserStation">List<cstUserStationTbl></param>
       /// <returns>Boolean</returns>
       public Boolean SaveUserStation(List<cstUserStationTbl> lsUserStation)
       {
           bool _return = false;
           try
           {
               foreach (var UserStationitem in lsUserStation)
               {
                   UserStation _Ust = new UserStation();
                   _Ust.UserStationID = Guid.NewGuid();
                   _Ust.StationID = UserStationitem.StationID;
                   _Ust.UserID = UserStationitem.UserID;
                   _Ust.LoginDateTime = UserStationitem.LoginDateTime;
                   _Ust.CreatedBy = GlobalClasses.ClGlobal.UserID;
                   _Ust.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                   ent.AddToUserStations(_Ust);
               }
               ent.SaveChanges();
               _return = true;
           }
           catch (Exception)
           {}
           return _return;

       }
      
    }
}
