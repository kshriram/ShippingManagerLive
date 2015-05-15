using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity.SMEntitys;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdTracking
    {
       local_x3v6Entities lent = new local_x3v6Entities();
       
       /// <summary>
       /// All Table Withaout any filter
       /// </summary>
       /// <returns>List<cstTrackingTbl>  Inforamtion</returns>
       public List<cstTrackingTbl> GetTrackTbl()
       {
           List<cstTrackingTbl> _lsTrackingTbl = new List<cstTrackingTbl>();
           try
           {
               var TblTracking = from tbl in lent.Trackings select tbl;
               foreach (var item in TblTracking)
               {
                   cstTrackingTbl Track = new cstTrackingTbl();
                   Track.TrackingID = item.TrackingID;
                   if (item.PackingID != null)
                       Track.PackingID = (Guid)item.PackingID;
                   if (item.ShippingID != null)
                       Track.ShippingID = (Guid)item.ShippingID;
                   Track.TrackingNum = item.TrackingNum;
                   Track.BoxNum = item.BOXNUM;
                   Track.TrackingDate =Convert.ToDateTime( item.TrackingDate);
                   if(item.CreatedBy!=null)
                   Track.CreatedBy =(Guid) item.CreatedBy;
                   Track.VOIIND = item.VOIIND;
                   Track.ReadyToExport = (Boolean)item.ReadyToExport;
                   Track.Exported = (Boolean)item.Exported;
                   if (item.Updatedby != null)
                       Track.Updatedby = (Guid)item.Updatedby;
                   if (Track.UpdatedDateTime!=null)
                       Track.UpdatedDateTime = Convert.ToDateTime(item.UpdatedDateTime);
                   if (Track.CreatedDateTime!=null)
                        Track.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime);
                   Track.PCKCHG = item.PCKCHG;
                   Track.Weight = item.Weight;
                   _lsTrackingTbl.Add(Track);
               }
           }
           catch (Exception)
           {}
           return _lsTrackingTbl;
       }

       /// <summary>
       /// Tracking Table information With filter Packing ID
       /// </summary>
       /// <param name="PackingID">Guid Packing ID</param>
       /// <returns></returns>
       public List<cstTrackingTbl> GetTrackTbl(Guid PackingID)
       {
           List<cstTrackingTbl> _lsTrackingTbl = new List<cstTrackingTbl>();
           try
           {
               var TblTracking = from tbl in lent.Trackings 
                                 where tbl.PackingID == PackingID
                                 select tbl;
               foreach (var item in TblTracking)
               {
                   cstTrackingTbl Track = new cstTrackingTbl();
                   Track.TrackingID = item.TrackingID;
                   if (item.PackingID != null)
                       Track.PackingID = (Guid)item.PackingID;
                   if (item.ShippingID != null)
                       Track.ShippingID = (Guid)item.ShippingID;
                   Track.BoxNum = item.BOXNUM;
                   Track.TrackingNum = item.TrackingNum;
                   Track.TrackingDate = Convert.ToDateTime(item.TrackingDate);
                   if (item.CreatedBy != null)
                       Track.CreatedBy = (Guid)item.CreatedBy;
                   Track.VOIIND = item.VOIIND;
                   Track.ReadyToExport = (Boolean)item.ReadyToExport;
                   Track.Exported = (Boolean)item.Exported;
                   if (item.Updatedby != null)
                       Track.Updatedby = (Guid)item.Updatedby;
                   if (Track.UpdatedDateTime != null)
                       Track.UpdatedDateTime = Convert.ToDateTime(item.UpdatedDateTime);
                   if (Track.CreatedDateTime != null)
                       Track.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime);
                   Track.PCKCHG = item.PCKCHG;
                   Track.Weight = item.Weight;
                   _lsTrackingTbl.Add(Track);
               }
           }
           catch (Exception)
           { }
           return _lsTrackingTbl;
       }

       /// <summary>
       /// Box Number tracking Table Filter.
       /// </summary>
       /// <param name="BoxNumber">
       /// String Box Number
       /// </param>
       /// <returns>
       /// list of Tracking Table information.
       /// </returns>
       public List< cstTrackingTbl> GetTrackTbl(String BoxNumber)
       {
           List<cstTrackingTbl> _return = new List<cstTrackingTbl>();
           try
           {
                
                var TblTracking = from tbl in lent.Trackings 
                                 where tbl.BOXNUM == BoxNumber
                                 select tbl;
                foreach (var item in TblTracking)
                {
                    cstTrackingTbl Track = new cstTrackingTbl();
                    Track.TrackingID = item.TrackingID;
                    if (item.PackingID != null)
                        Track.PackingID = (Guid)item.PackingID;
                    if (item.ShippingID != null)
                        Track.ShippingID = (Guid)item.ShippingID;
                    Track.BoxNum = item.BOXNUM;
                    Track.VOIIND = item.VOIIND;
                    Track.ReadyToExport = (Boolean)item.ReadyToExport;
                    Track.Exported = (Boolean)item.Exported;
                    Track.TrackingNum = item.TrackingNum;
                    Track.TrackingDate = Convert.ToDateTime(item.TrackingDate);
                    if (item.CreatedBy != null)
                        Track.CreatedBy = (Guid)item.CreatedBy;
                    if (item.Updatedby != null)
                        Track.Updatedby = (Guid)item.Updatedby;
                    if (Track.UpdatedDateTime != null)
                        Track.UpdatedDateTime = Convert.ToDateTime(item.UpdatedDateTime);
                    if (Track.CreatedDateTime != null)
                        Track.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime);
                    Track.PCKCHG = item.PCKCHG;
                    Track.Weight = item.Weight;
                    _return.Add(Track);
                }
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Tracking Table information With filter Packing ID and Shipping ID
       /// </summary>
       /// <param name="PackingID">Guid Packing Table ID</param>
       /// <param name="ShippingID">Guid Shipping Tabel ID</param>
       /// <returns> List<cstTrackingTbl> Information</returns>
       public List<cstTrackingTbl> GetTrackTbl(Guid PackingID, Guid ShippingID)
       {
           List<cstTrackingTbl> _lsTrackingTbl = new List<cstTrackingTbl>();
           try
           {
               var TblTracking = from tbl in lent.Trackings
                                 where tbl.PackingID == PackingID &&
                                 tbl.ShippingID == ShippingID
                                 select tbl;
               foreach (var item in TblTracking)
               {
                   cstTrackingTbl Track = new cstTrackingTbl();
                   Track.TrackingID = item.TrackingID;
                   if (item.PackingID != null)
                       Track.PackingID = (Guid)item.PackingID;
                   if (item.ShippingID != null)
                       Track.ShippingID = (Guid)item.ShippingID;
                   Track.BoxNum = item.BOXNUM;
                   Track.TrackingNum = item.TrackingNum;
                   Track.TrackingDate = Convert.ToDateTime(item.TrackingDate);
                   if (item.CreatedBy != null)
                       Track.CreatedBy = (Guid)item.CreatedBy;
                   Track.VOIIND = item.VOIIND;
                   Track.ReadyToExport = (Boolean)item.ReadyToExport;
                   Track.Exported = (Boolean)item.Exported;
                   if (item.Updatedby != null)
                       Track.Updatedby = (Guid)item.Updatedby;
                   if (item.UpdatedDateTime != null)
                       Track.UpdatedDateTime = Convert.ToDateTime(item.UpdatedDateTime);
                   if (item.CreatedDateTime != null)
                       Track.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime);
                   Track.PCKCHG = item.PCKCHG;
                   Track.Weight = item.Weight;
                   _lsTrackingTbl.Add(Track);

               }
           }
           catch (Exception)
           { }
           return _lsTrackingTbl;
       }

       /// <summary>
       /// Tracking Table information With filter Packing ID and Shipping ID
       /// </summary>
       /// <param name="ShippingID"> Shipping Table Guid</param>
       /// <returns> List<cstTrackingTbl> Information</returns>
       public List<cstTrackingTbl> GetTrackTblShippingIDOnly(Guid ShippingID)
       {
           List<cstTrackingTbl> _lsTrackingTbl = new List<cstTrackingTbl>();
           try
           {
               var TblTracking = from tbl in lent.Trackings
                                 where tbl.ShippingID == ShippingID
                                 select tbl;
               foreach (var item in TblTracking)
               {
                   cstTrackingTbl Track = new cstTrackingTbl();
                   Track.TrackingID = item.TrackingID;
                   if (item.PackingID != null)
                       Track.PackingID = (Guid)item.PackingID;
                   if (item.ShippingID != null)
                       Track.ShippingID = (Guid)item.ShippingID;
                   Track.TrackingNum = item.TrackingNum;
                   Track.TrackingDate = Convert.ToDateTime(item.TrackingDate);
                   if (item.CreatedBy != null)
                       Track.CreatedBy = (Guid)item.CreatedBy;
                   Track.VOIIND = item.VOIIND;
                   Track.ReadyToExport = (Boolean)item.ReadyToExport;
                   Track.Exported = (Boolean)item.Exported;
                   if (item.Updatedby != null)
                       Track.Updatedby = (Guid)item.Updatedby;
                   if (Track.UpdatedDateTime != null)
                       Track.UpdatedDateTime = Convert.ToDateTime(item.UpdatedDateTime);
                   if (Track.CreatedDateTime != null)
                       Track.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime);
                   Track.PCKCHG = item.PCKCHG;
                   Track.Weight = item.Weight;
                   _lsTrackingTbl.Add(Track);
               }
           }
           catch (Exception)
           { }
           return _lsTrackingTbl;
       }

       /// <summary>
       /// Check tracking Number is Present For given box Number. 
       /// if present then Reaturn the Tracking Number string
       /// </summary>
       /// <param name="BoxNum">
       /// String Box Number
       /// </param>
       /// <returns>
       /// string Tracking Number.
       /// </returns>
       public String IschecckTrackingNumberPresent(String BoxNum)
       {
           String _return = "";
           try
           {
                _return = lent.Trackings.FirstOrDefault(i => i.BOXNUM == BoxNum).TrackingNum;
           }
           catch (Exception)
           {}
           return _return;
       }

       /// <summary>
       /// Tracking Table Object by Tracking Number
       /// </summary>
       /// <param name="TrackingNumber">
       /// String Tracking Number
       /// </param>
       /// <returns>  
       /// cstTrackingTbl Object.
       /// </returns>
       public cstTrackingTbl GetTrackingByTrackingNumber(String TrackingNumber)
       {
           cstTrackingTbl _Return = new cstTrackingTbl();
           try
           {
               Tracking TrackingInfo = lent.Trackings.FirstOrDefault(i => i.TrackingNum == TrackingNumber);
               _Return.BoxNum = TrackingInfo.BOXNUM;
               _Return.Exported = (Boolean)TrackingInfo.Exported;
               _Return.PCKCHG = TrackingInfo.PCKCHG;
               _Return.ReadyToExport = (Boolean)TrackingInfo.ReadyToExport;
               _Return.TrackingID = TrackingInfo.TrackingID;
               _Return.TrackingNum = TrackingInfo.TrackingNum;
               _Return.VOIIND = TrackingInfo.VOIIND;
               _Return.Weight = TrackingInfo.Weight;

               if (TrackingInfo.CreatedBy != null)
                   _Return.CreatedBy = (Guid)TrackingInfo.CreatedBy;
               if (TrackingInfo.CreatedDateTime != null)
                   _Return.CreatedDateTime = Convert.ToDateTime(TrackingInfo.CreatedDateTime);
               if (TrackingInfo.PackingID != null)
                   _Return.PackingID = (Guid)TrackingInfo.PackingID;
               if (TrackingInfo.ShippingID != null)
                   _Return.ShippingID = (Guid)TrackingInfo.ShippingID;
               if (TrackingInfo.TrackingDate != null)
                   _Return.TrackingDate = Convert.ToDateTime(TrackingInfo.TrackingDate);
               if (TrackingInfo.Updatedby != null)
                   _Return.Updatedby = (Guid)TrackingInfo.Updatedby;
               if (TrackingInfo.UpdatedDateTime != null)
                   _Return.UpdatedDateTime = Convert.ToDateTime(TrackingInfo.UpdatedDateTime);

           }
           catch (Exception)
           { }

           return _Return;
       }

       public Boolean UpdateReadyTOExport(String TrackingNo, String BoxNumber, Boolean ReadyTOExportflag)
       {
           Boolean _retutn = false;
           try
           {
               Tracking tracking = lent.Trackings.FirstOrDefault(i => i.TrackingNum == TrackingNo && i.BOXNUM == BoxNumber);
               tracking.ReadyToExport = ReadyTOExportflag;
               lent.SaveChanges();
               _retutn = true;
           }
           catch (Exception)
           {}
           return _retutn;
       }
    }
}
