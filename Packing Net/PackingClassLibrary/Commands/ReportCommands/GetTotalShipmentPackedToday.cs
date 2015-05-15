using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.CustomEntity;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
namespace PackingClassLibrary.Commands.ReportCommands
{
   public class GetTotalShipmentPackedToday
    {

       local_x3v6Entities ent = new local_x3v6Entities();

       /// <summary>
       /// Current date packing details
       /// </summary>
       /// <returns></returns>
       public List<cstShipmentPackedTodayAndAvgTime> GetTotalShipmentPackedTime()
       {
           List<cstShipmentPackedTodayAndAvgTime> lsShipmentPacked = new List<cstShipmentPackedTodayAndAvgTime>();
           try
           {
               String CurrentTime = DateTime.UtcNow.ToString();

               var packingCount = from user in ent.Users
                                  join packing in ent.Packages
                                  on user.UserID equals packing.UserId
                                  where EntityFunctions.TruncateTime(packing.EndTime) == EntityFunctions.TruncateTime(DateTime.UtcNow)
                                  group packing by packing.UserId into Gpacking
                                  select new
                                  {
                                      userID = Gpacking.Key,
                                      TotalPacked = Gpacking.Count(i => i.PackingStatus == 0)
                                       
                                  };



               foreach (var pckitem in packingCount)
               {
                   cstShipmentPackedTodayAndAvgTime _cspck = new cstShipmentPackedTodayAndAvgTime();
                   _cspck.UserID = pckitem.userID;
                   _cspck.Packed =Convert.ToInt32( pckitem.TotalPacked);
                   lsShipmentPacked.Add(_cspck);
               }

           }
           catch (Exception)
           { }
           return lsShipmentPacked;
       }
    }
}
