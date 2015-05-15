using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdStation
   {
       local_x3v6Entities entx3v6 = new local_x3v6Entities();
       Sage_x3v6Entities Sage = new Sage_x3v6Entities();

       #region Get functions

       /// <summary>
       /// list of station master.
       /// </summary>
       /// <returns>List<cstStationMasterTbl></returns>
       public List<cstStationMasterTbl> GetStationList()
       {
           List<cstStationMasterTbl> _lsReturn = new List<cstStationMasterTbl>();
           try
           {
               var stationMaster = from station in entx3v6.Stations select station;
               foreach (var station in stationMaster)
               {
                   cstStationMasterTbl TStaion = new cstStationMasterTbl();
                   TStaion.StationID = station.StationID;
                   TStaion.StationName = station.StationName;
                   TStaion.RequestedUserID = station.RequestedUserID;
                   TStaion.StationAlive = station.StationAlive;
                   TStaion.RegistrationDate = station.RegistrationDate;
                   TStaion.DeviceNumber = station.DeviceNumber;
                   TStaion.StaionLocation = station.StationLocation;
                   _lsReturn.Add(TStaion);
               }
           }
           catch (Exception)
           { }
           return _lsReturn;
       }

       /// <summary>
       /// Get station Information from the Device ID
       /// </summary>
       /// <param name="DevideID">String MAC Address </param>
       /// <returns>List<cstStationMasterTbl> </returns>
       public List<cstStationMasterTbl> GetStationList(String DevideNumber)
       {
           List<cstStationMasterTbl> _lsReturn = new List<cstStationMasterTbl>();
           try
           {
               var stationMaster = from station in entx3v6.Stations
                                   where station.DeviceNumber == DevideNumber
                                   select station;
               foreach (var station in stationMaster)
               {
                   cstStationMasterTbl TStaion = new cstStationMasterTbl();
                   TStaion.StationID = station.StationID;
                   TStaion.StationName = station.StationName;
                   TStaion.RequestedUserID = station.RequestedUserID;
                   TStaion.StationAlive = station.StationAlive;
                   TStaion.RegistrationDate = station.RegistrationDate;
                   TStaion.DeviceNumber = station.DeviceNumber;
                   TStaion.StaionLocation = station.StationLocation;
                   _lsReturn.Add(TStaion);
               }
           }
           catch (Exception)
           { }
           return _lsReturn;
       }

       /// <summary>
       /// Selected item from Station Table
       /// </summary>
       /// <param name="StationID">Guid StaionID</param>
       /// <returns>List<cstStationMasterTbl> Indication Station Information</returns>
       public cstStationMasterTbl GetStationList(Guid StationID)
       {
           cstStationMasterTbl TStaion = new cstStationMasterTbl();
           try
           {
               Station station = entx3v6.Stations.SingleOrDefault(i => i.StationID == StationID);
               TStaion.StationID = station.StationID;
               TStaion.StationName = station.StationName;
               TStaion.RequestedUserID = station.RequestedUserID;
               TStaion.StationAlive = station.StationAlive;
               TStaion.RegistrationDate = station.RegistrationDate;
               TStaion.DeviceNumber = station.DeviceNumber;
               TStaion.StaionLocation = station.StationLocation;
           }
           catch (Exception)
           { }
           return TStaion;
       }

       /// <summary>
       /// Station Information filter by station Name
       /// </summary>
       /// <param name="StationName">String Station Name</param>
       /// <returns>CstStationMasterTbl Information</returns>
       public cstStationMasterTbl GetStationInfo(String StationName)
       {
           cstStationMasterTbl _return = new cstStationMasterTbl();
           try
           {
               Station station = entx3v6.Stations.SingleOrDefault(i => i.StationName == StationName);
               cstStationMasterTbl TStaion = new cstStationMasterTbl();
               TStaion.StationID = station.StationID;
               TStaion.StationName = station.StationName;
               TStaion.RequestedUserID = station.RequestedUserID;
               TStaion.StationAlive = station.StationAlive;
               TStaion.RegistrationDate = station.RegistrationDate;
               TStaion.DeviceNumber = station.DeviceNumber;
               TStaion.StaionLocation = station.StationLocation;
               _return = TStaion;

           }
           catch (Exception)
           {}
           return _return;
       }
       #endregion

       #region Set functions
       /// <summary>
       /// Save data to the StationMaster Table.
       /// </summary>
       /// <param name="lsStationMaster">List<cstStationMasterTbl></param>
       /// <returns>Boolean </returns>
       public Boolean SaveSationMaster(List<cstStationMasterTbl> lsStationMaster)
       {
           Boolean _return = false;
           try
           {
               foreach (var Stationitem in lsStationMaster)
               {
                   Station _Station = new Station();
                   _Station.StationID = Guid.NewGuid();
                   _Station.StationName = Stationitem.StationName;
                   _Station.DeviceNumber = Stationitem.DeviceNumber;
                   _Station.RequestedUserID = Stationitem.RequestedUserID;
                   _Station.StationAlive = Stationitem.StationAlive;
                   _Station.StationLocation = Stationitem.StaionLocation;
                   _Station.RegistrationDate = Stationitem.RegistrationDate;
                   _Station.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                   _Station.CreatedBy = GlobalClasses.ClGlobal.UserID;
                   entx3v6.AddToStations(_Station);
               }
               entx3v6.SaveChanges();
               _return = true;
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Update table with Device ID
       /// </summary>
       /// <param name="lsStationMaster"></param>
       /// <param name="DeviceNumber"></param>
       /// <returns></returns>
       public Boolean SaveSationMaster(List<cstStationMasterTbl> lsStationMaster, String DeviceNumber)
       {
           Boolean _return = false;
           try
           {
               Station _Station = entx3v6.Stations.SingleOrDefault(i => i.DeviceNumber == DeviceNumber);
               _Station.StationID = lsStationMaster[0].StationID;
               _Station.StationName = lsStationMaster[0].StationName;
               _Station.RequestedUserID = lsStationMaster[0].RequestedUserID;
               _Station.StationAlive = lsStationMaster[0].StationAlive;
               _Station.StationLocation = lsStationMaster[0].StaionLocation;
               _Station.RegistrationDate = lsStationMaster[0].RegistrationDate;
               _Station.UpdatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
               _Station.Updatedby = GlobalClasses.ClGlobal.UserID;
               entx3v6.SaveChanges();
               _return = true;
           }
           catch (Exception)
           { }
           return _return;
       }
       #endregion

       #region Delete functions
       #endregion
   }
}
