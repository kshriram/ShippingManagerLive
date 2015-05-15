using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands.ReportCommands
{
   public class cmdUserCurrentStationAndDeviceID
    {
        local_x3v6Entities ent = new local_x3v6Entities();

        /// <summary>
        /// All Users last Login Station
        /// </summary>
        /// <returns></returns>
        public List<cstUserCurrentStationAndDeviceID> LastLoginStationAllUsers()
        {
            List<cstUserCurrentStationAndDeviceID> lsUstation = new List<cstUserCurrentStationAndDeviceID>();
            try
            {
                var UserName = from user in ent.Users
                               join Us in ent.UserStations
                               on user.UserID equals Us.UserID
                               group Us by user.UserID into Gusers
                               select new
                               {
                                   User = Gusers.Key,
                                   StationTime = Gusers.Max(i => i.LoginDateTime),
                                   StaionID = Gusers.FirstOrDefault(i => i.UserID == Gusers.Key && i.LoginDateTime == Gusers.Max(j => j.LoginDateTime)).StationID
                               };
                var StaionName = from Station in UserName
                                 join Station2 in ent.Stations
                               on Station.StaionID equals Station2.StationID
                                 join User in ent.Users
                                 on Station.User equals User.UserID
                                 select new
                                 {
                                     UserID = User.UserID,
                                     UserName = User.UserFullName,
                                     Station.StationTime,
                                     Station2.StationName,
                                     Station2.DeviceNumber,
                                 };

                foreach (var Useritem in StaionName)
                {
                    cstUserCurrentStationAndDeviceID UserStation = new cstUserCurrentStationAndDeviceID();
                    UserStation.UserID = Useritem.UserID;
                    UserStation.UserName = Useritem.UserName;
                    UserStation.StationName = Useritem.StationName;
                    UserStation.Datetime = Useritem.StationTime.ToString("MMM dd, yyyy hh:mm tt");
                    UserStation.DeviceID = Useritem.DeviceNumber;
                    lsUstation.Add(UserStation);
                }
            }
            catch (Exception)
            { }
            return lsUstation;
        }
    }
}
