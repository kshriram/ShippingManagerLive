using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.Commands.SMcommands;
using System.Data.Objects;

namespace PackingClassLibrary.Commands.ReportCommands
{
   public  class cmdUserShipmentCount
    {
       local_x3v6Entities lent = new local_x3v6Entities();


       /// <summary>
       /// for each user its total packed shipments and its dates
       /// </summary>
       /// <returns>List of cstUserShipmentCount</returns>
       public List<cstUserShipmentCount> GetAllShipmentCountByUser()
       {
           List<cstUserShipmentCount> _lsUserShipmentCount = new List<cstUserShipmentCount>();
           try
           {
               var Shipments = from shp in lent.Packages
                               group shp by new { shp.UserId, Stime = EntityFunctions.TruncateTime(shp.StartTime) } into Gship
                               select new
                               {
                                   Userid = Gship.Key.UserId,
                                   PackingDate = Gship.Key.Stime,
                                   ShipmentCount = Gship.Count(i => i.ShippingID != null)
                               };
               foreach (var item in Shipments)
               {
                   cstUserShipmentCount Uship = new cstUserShipmentCount();
                   Uship.UserID = item.Userid;
                   Uship.UserName = lent.Users.SingleOrDefault(i=> i.UserID == item.Userid).UserFullName.ToString();
                   Uship.ShipmentCount =Convert.ToInt32( item.ShipmentCount);
                   Uship.Datepacked = Convert.ToDateTime(item.PackingDate);
                   _lsUserShipmentCount.Add(Uship);
               }

           }
           catch (Exception)
           {}
           return _lsUserShipmentCount;
       }
    }
}
