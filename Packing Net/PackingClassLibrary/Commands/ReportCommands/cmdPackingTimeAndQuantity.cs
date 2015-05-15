using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using System.Data.Objects.SqlClient;
using System.Data.Objects;
using PackingClassLibrary.Commands.SMcommands;

namespace PackingClassLibrary.Commands.ReportCommands
{
    public class cmdPackingTimeAndQuantity
    {
        local_x3v6Entities x3v6 = new local_x3v6Entities();
        /// <summary>
        /// Calculate all shipments toatal Quantity and Time Required to pack the saprate shipment
        /// </summary>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity()
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       select new
                                       {
                                           ShipmentID =Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber =cmdPackage.GetShippingNum( listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(Guid UserID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where packing.UserId == UserID
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(DateTime Fromdate, DateTime Todate)
        {


           List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                        EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(Guid UserID, DateTime Fromdate, DateTime Todate)
        {

            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                        EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                        packing.UserId == UserID
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }


        public List<cstPackingTime> GetPackingTimeAndQantity(int PackingStatus, Boolean PackingStaus)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where packing.PackingStatus == PackingStatus
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };

                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };









                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(Guid UserID, int PackingStatus)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where packing.PackingStatus == PackingStatus && packing.UserId == UserID
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.UserId == UserID &&
                                          packing.PackingStatus == PackingStatus

                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };

                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };












                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(DateTime Fromdate, DateTime Todate, int PackingStatus)
        { 
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                 try
                {
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                           EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                           packing.PackingStatus == PackingStatus
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                            EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                            packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };


                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };






                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantity(Guid UserID, DateTime Fromdate, DateTime Todate, int PackingStatus)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where packing.UserId == UserID &&
                                         EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                          EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                          packing.PackingStatus == PackingStatus
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.UserId == UserID &&
                                           EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                            EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                            packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                           //Quantity = 00
                                       };


                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };





                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }


        #region Station Wise new added

        /// <summary>
        /// Calculate all shipments toatal Quantity and Time Required to pack the saprate shipment
        /// </summary>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(Guid StationID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where packing.StationID == StationID && EntityFunctions.TruncateTime( packing.EndTime) == EntityFunctions.TruncateTime(dt)
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(Guid UserID, Guid StationID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where packing.UserId == UserID
                                       where packing.StationID == StationID && EntityFunctions.TruncateTime(packing.EndTime) == EntityFunctions.TruncateTime(dt)
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(DateTime Fromdate, DateTime Todate, Guid StationID)
        {


            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                        EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date && packing.StationID == StationID 
                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(Guid UserID, DateTime Fromdate, DateTime Todate, Guid StationID)
        {

            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            try
            {
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                var packingTimeAndID = from packing in x3v6.Packages
                                       join Packingdtl in x3v6.PackageDetails
                                       on packing.PackingId equals Packingdtl.PackingId
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                        EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                        packing.UserId == UserID &&
                                         packing.StationID == StationID 

                                       select new
                                       {
                                           ShipmentID = Packingdtl.PackingId,
                                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                       };
                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeSpend,
                                      Quantity = Pq.Quantity
                                  };
                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            catch (Exception)
            { }
            return _lsreturnPacingTime;
        }


        public List<cstPackingTime> GetPackingTimeAndQantityByStation(int PackingStatus, Boolean PackingStaus, Guid StationID)
        {
            DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {

                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where packing.PackingStatus == PackingStatus
                                           && packing.StationID == StationID && EntityFunctions.TruncateTime(packing.EndTime) == EntityFunctions.TruncateTime(dt)
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.PackingStatus == PackingStatus &&
                                       packing.StationID == StationID && EntityFunctions.TruncateTime(packing.EndTime) == EntityFunctions.TruncateTime(dt)
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };



                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };

















                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(Guid UserID, int PackingStatus, Guid StationID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    //var packingTimeAndID = from packing in x3v6.Packages
                    //                       join Packingdtl in x3v6.PackageDetails
                    //                       on packing.PackingId equals Packingdtl.PackingId
                    //                       where packing.PackingStatus == PackingStatus && packing.UserId == UserID
                    //                       where packing.StationID == StationID && EntityFunctions.TruncateTime(packing.EndTime) == EntityFunctions.TruncateTime(dt)
                    //                       select new
                    //                       {
                    //                           ShipmentID = Packingdtl.PackingId,
                    //                           TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                    //                       };

                    var packingTimeAndID = from packing in x3v6.Packages
                                           where packing.UserId == UserID &&
                                              packing.PackingStatus == PackingStatus
                                           select new
                                           {
                                               ShipmentID = packing.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                               // Quantity = 00
                                           };


                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.UserId == UserID &&
                                          packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };



                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };








                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(DateTime Fromdate, DateTime Todate, int PackingStatus, Guid StationID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                           EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                           packing.PackingStatus == PackingStatus
                                           &&   packing.StationID == StationID 
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                            EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                            packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };

                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };










                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeAndQantityByStation(Guid UserID, DateTime Fromdate, DateTime Todate, int PackingStatus, Guid StationID)
        {
            List<cstPackingTime> _lsreturnPacingTime = new List<cstPackingTime>();
            if (PackingStatus == 0)
            {
                try
                {
                    DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    var packingTimeAndID = from packing in x3v6.Packages
                                           join Packingdtl in x3v6.PackageDetails
                                           on packing.PackingId equals Packingdtl.PackingId
                                           where packing.UserId == UserID &&
                                         EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                          EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                          packing.PackingStatus == PackingStatus
                                          && packing.StationID == StationID 
                                           select new
                                           {
                                               ShipmentID = Packingdtl.PackingId,
                                               TimeSpend = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime)
                                           };
                    var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                          group packingDetail by packingDetail.PackingId into Gpd
                                          select new
                                          {
                                              ShipmentID = Gpd.Key,
                                              Quantity = Gpd.Sum(p => p.SKUQuantity)
                                          };
                    var packingJoin = from Pq in PackingQuantity
                                      join Pt in packingTimeAndID
                                      on Pq.ShipmentID equals Pt.ShipmentID
                                      select new
                                      {
                                          ShipmentID = Pq.ShipmentID,
                                          TimeRquare = Pt.TimeSpend,
                                          Quantity = Pq.Quantity
                                      };
                    var packingG = from pj in packingJoin
                                   group pj by pj.ShipmentID into GPj
                                   select GPj;

                    foreach (var listItem in packingG)
                    {
                        cstPackingTime _Packing = new cstPackingTime();
                        _Packing.PackingID = listItem.Key;
                        _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.Key);
                        _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                        string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                        _Packing.TimeSpend = answer;
                        _lsreturnPacingTime.Add(_Packing);
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                var packingTimeAndID = from packing in x3v6.Packages
                                       where packing.UserId == UserID &&
                                           EntityFunctions.TruncateTime(packing.EndTime.Value) >= Fromdate.Date &&
                                            EntityFunctions.TruncateTime(packing.EndTime.Value) <= Todate.Date &&
                                            packing.PackingStatus == PackingStatus
                                       select new
                                       {
                                           ShipmentID = packing.PackingId,
                                           TimeRquare = SqlFunctions.DateDiff("s", packing.StartTime, packing.EndTime),
                                          // Quantity = 00
                                       };

                var PackingQuantity = from packingDetail in x3v6.PackageDetails
                                      group packingDetail by packingDetail.PackingId into Gpd
                                      select new
                                      {
                                          ShipmentID = Gpd.Key,
                                          Quantity = Gpd.Sum(p => p.SKUQuantity)
                                      };
                var packingJoin = from Pq in PackingQuantity
                                  join Pt in packingTimeAndID
                                  on Pq.ShipmentID equals Pt.ShipmentID
                                  select new
                                  {
                                      ShipmentID = Pq.ShipmentID,
                                      TimeRquare = Pt.TimeRquare,
                                      Quantity = Pq.Quantity
                                  };








                var packingG = from pj in packingJoin
                               group pj by pj.ShipmentID into GPj
                               select GPj;

                foreach (var listItem in packingG)
                {
                    cstPackingTime _Packing = new cstPackingTime();
                    _Packing.PackingID = listItem.Key;
                    _Packing.ShippingNumber = cmdPackage.GetShippingNum(listItem.FirstOrDefault().ShipmentID);
                    _Packing.Quantity = Convert.ToInt32(listItem.FirstOrDefault().Quantity);
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(listItem.FirstOrDefault().TimeRquare.ToString()));
                    string answer = string.Format("{0:D2}H:{1:D2}M:{2:D2}S", t.Hours, t.Minutes, t.Seconds);
                    _Packing.TimeSpend = answer;
                    _lsreturnPacingTime.Add(_Packing);
                }
            }
            return _lsreturnPacingTime;
        }
        #endregion
    }
}
