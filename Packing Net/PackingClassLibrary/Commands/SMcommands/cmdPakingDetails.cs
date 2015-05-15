using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
namespace PackingClassLibrary.Commands
{
    /// <summary>
    /// Avinash.
    /// Packing Detail class operations.
    /// </summary>
   public  class cmdPakingDetails
    {
       
      // x3v6 local entity database Object created.
       local_x3v6Entities entx3v6 = new local_x3v6Entities();

       #region Set Package Details Functions.
       /// <summary>
       /// Save the list values to the packing Detail table.
       /// </summary>
       /// <param name="lsPackingOb">list of values of packing Detail </param>
       /// <returns>Success if transaction Success else Fail.</returns>
             public string savePackageDetails(List< cstPackageDetails> lsPackingOb)
             {
                 string Retuen = "Fail";
                 try
                 {
                     foreach (var _PakingDetails in lsPackingOb)
                     {
                         PackageDetail _Packing = new PackageDetail();
                         _Packing.PackingDetailID = _PakingDetails.PackingDetailID;
                         _Packing.PackingId = _PakingDetails.PackingId;
                         _Packing.SKUNumber = _PakingDetails.SKUNumber;
                         _Packing.SKUQuantity = _PakingDetails.SKUQuantity;
                         _Packing.SKUScanDateTime = Convert.ToDateTime(_PakingDetails.PackingDetailStartDateTime);
                         _Packing.BoxNumber = _PakingDetails.BoxNumber;
                         _Packing.ShipmentLocation = _PakingDetails.ShipmentLocation;
                         _Packing.RowNumber = _PakingDetails.RowNumber;
                         //view added extra
                         _Packing.ItemName = _PakingDetails.ItemName;
                         _Packing.ProductName = _PakingDetails.ProductName;
                         _Packing.UnitOfMeasure = _PakingDetails.UnitOfMeasure;
                         _Packing.CountryOfOrigin = _PakingDetails.CountryOfOrigin;
                         _Packing.MAP_Price = _PakingDetails.MAP_Price;
                         _Packing.TCLCOD_0 = _PakingDetails.TCLCOD_0;
                         _Packing.TarrifCode = _PakingDetails.TarrifCode;
                         //created Time set
                         _Packing.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                         _Packing.CreatedBy = GlobalClasses.ClGlobal.UserID;
                         entx3v6.AddToPackageDetails(_Packing);
                     }
                     entx3v6.SaveChanges();
                     Retuen = "Success";
                 }
                 catch (Exception Ex)
                 {
                     Error_Loger.elAction.save("SetPakingDetailsCommand.Execute()", Ex.Message.ToString());
                 }
                 return Retuen;
             }

             public string UpdatePackageDetails(List<cstPackageDetails> lsPackingOb)
             {
                 string Retuen = "Fail";
                 try
                 {
                     foreach (var _PakingDetails in lsPackingOb)
                     {
                         PackageDetail _Packing = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == _PakingDetails.PackingDetailID);
                         _Packing.PackingId = _PakingDetails.PackingId;
                         _Packing.SKUNumber = _PakingDetails.SKUNumber;
                         _Packing.SKUQuantity = _PakingDetails.SKUQuantity;
                         _Packing.SKUScanDateTime = Convert.ToDateTime(_PakingDetails.PackingDetailStartDateTime);
                         _Packing.BoxNumber = _PakingDetails.BoxNumber;
                         _Packing.ShipmentLocation = _PakingDetails.ShipmentLocation;
                         _Packing.RowNumber = _PakingDetails.RowNumber;
                         //view added extra
                         _Packing.ItemName = _PakingDetails.ItemName;
                         _Packing.ProductName = _PakingDetails.ProductName;
                         _Packing.UnitOfMeasure = _PakingDetails.UnitOfMeasure;
                         _Packing.CountryOfOrigin = _PakingDetails.CountryOfOrigin;
                         _Packing.MAP_Price = _PakingDetails.MAP_Price;
                         _Packing.TCLCOD_0 = _PakingDetails.TCLCOD_0;
                         _Packing.TarrifCode = _PakingDetails.TarrifCode;
                         //created Time set
                         _Packing.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                         _Packing.CreatedBy = GlobalClasses.ClGlobal.UserID;
                     }
                     entx3v6.SaveChanges();
                     Retuen = "Success";
                 }
                 catch (Exception Ex)
                 {
                     Error_Loger.elAction.save("SetPakingDetailsCommand.Execute()", Ex.Message.ToString());
                 }
                 return Retuen;
             }
       #endregion

        #region get Package Details functions.
             /// <summary>
             /// packing detail table information with all rows.
             /// </summary>
             /// <returns>List<cstPackageDetails> information</returns>
             public List<cstPackageDetails> GetPackingDetails()
             {
                 //Return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();

                 try
                 {
                     var packingDeatils = from Pack in entx3v6.PackageDetails select Pack;
                     //fill list object cstPackageDetails and add to return list.
                     //Foreach loop for all recoreds in the packageDetail table
                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         _pd.PackingDetailID = lsitem.PackingDetailID;
                         _pd.PackingId = lsitem.PackingId;
                         _pd.SKUNumber = lsitem.SKUNumber;
                         _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;
                         _pd.ShipmentLocation = lsitem.ShipmentLocation;
                         _pd.ItemName = lsitem.ItemName;
                         _pd.ProductName = lsitem.ProductName;
                         _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         _pd.MAP_Price =Convert.ToDecimal( lsitem.MAP_Price);
                         _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         _pd.TarrifCode = lsitem.TarrifCode;
                         _pd.RowNumber = lsitem.RowNumber;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }

             /// <summary>
             /// Unique Boxnumbers.
             /// </summary>
             /// <param name="PackingID">Guid packingID of the shipment from packing table</param>
             /// <returns>List<cstPackageDetails> Information list</returns>
             //////List<string> boxnumber = new List<string>();
             //////public string  GetBoxnumber(Guid PackingID)
             //////{
             //////    try
             //////    {
             //////        var packingDeatils = (from Pack in entx3v6.PackageDetails
             //////                              where Pack.PackingId == PackingID
             //////                              select Pack).Distinct();
             //////      ///  boxnumber.Add(packingDeatils);
             //////        return packingDeatils;
             //////    }
             //////    catch (Exception)
             //////    { }
             //////}
             public List<cstPackageDetails> GetPackingDetailsBoxNumber(Guid PackingID)
             {
                 //return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = from Pack in entx3v6.PackageDetails
                     //                     where Pack.PackingId == PackingID
                     //                     select Pack;

                     ///New Chaged Query

                     string location = cmdLocalFile.ReadString("Location");

                     var packingDeatils = (from Pack in entx3v6.PackageDetails
                                           join packMaser in entx3v6.Packages on Pack.PackingId equals packMaser.PackingId
                                           where Pack.PackingId == PackingID && packMaser.ShipmentLocation == location
                                           select new { Pack.BoxNumber }).Distinct();     
                                           //Pack.BoxNumber).Distinct();



                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         ////_pd.PackingDetailID = lsitem.PackingDetailID;
                         ////_pd.PackingId = lsitem.PackingId;
                         ////_pd.SKUNumber = lsitem.SKUNumber;
                         ////_pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         ////_pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;

                         //Shipment Number assign to Shipment location For MUltiple shipmnet changes...

                        // _pd.ShipmentLocation = lsitem.ShippingNum;
                         ////_pd.ShipmentLocation = lsitem.ShipmentLocation;
                         ////_pd.ItemName = lsitem.ItemName;
                         ////_pd.ProductName = lsitem.ProductName;
                         ////_pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         ////_pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         ////_pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                         ////_pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         ////_pd.TarrifCode = lsitem.TarrifCode;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }
             ////Deepak 16-1-2015

             public List<cstPackageDetails> GetPackingDetailsByBOX(Guid PackingID)
             {
                 //return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = from Pack in entx3v6.PackageDetails
                     //                     where Pack.PackingId == PackingID
                     //                     select Pack;

                     ///New Chaged Query


                     //var packingDeatils = (from Pack in entx3v6.PackageDetails
                     //                     where Pack.PackingId == PackingID
                     //                     select Pack).Distinct();

                     List<cstPackageDetails> packingDeatils = entx3v6.ExecuteStoreQuery<cstPackageDetails>(@" SELECT
                                                                                                            CASE WHEN ROW_NUMBER() OVER (PARTITION BY BoxNumber ORDER BY BoxNumber) > 1 THEN NULL ELSE BoxNumber END AS BoxNumber,
                                                                                                            SKUNumber, SKUQuantity,SKUScanDateTime as PackingDetailStartDateTime,ShipmentLocation
                                                                                                            FROM PackageDetail pd
                                                                                                            where pd.PackingId='" + PackingID + "' ORDER BY PackingId, pd.BoxNumber").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         _pd.PackingDetailID = lsitem.PackingDetailID;
                         _pd.PackingId = lsitem.PackingId;
                         _pd.SKUNumber = lsitem.SKUNumber;
                         _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.PackingDetailStartDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;
                         _pd.ShipmentLocation = lsitem.ShipmentLocation;
                         _pd.ItemName = lsitem.ItemName;
                         _pd.ProductName = lsitem.ProductName;
                         _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         _pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                         _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         _pd.TarrifCode = lsitem.TarrifCode;
                         _pd.RowNumber = lsitem.RowNumber;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }

             ////End


             /// <summary>
             /// Packing detail information list with Packing ID filter.
             /// </summary>
             /// <param name="PackingID">Guid packingID of the shipment from packing table</param>
             /// <returns>List<cstPackageDetails> Information list</returns>
             public List<cstPackageDetails> GetPackingDetails(Guid PackingID)
             {
                 //return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = from Pack in entx3v6.PackageDetails
                     //                     where Pack.PackingId == PackingID
                     //                     select Pack;

                     ///New Chaged Query
                     

                     var packingDeatils = (from Pack in entx3v6.PackageDetails
                                          where Pack.PackingId == PackingID
                                          select Pack).Distinct();
                     


                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         _pd.PackingDetailID = lsitem.PackingDetailID;
                         _pd.PackingId = lsitem.PackingId;
                         _pd.SKUNumber = lsitem.SKUNumber;
                         _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;
                         _pd.ShipmentLocation = lsitem.ShipmentLocation;
                         _pd.ItemName = lsitem.ItemName;
                         _pd.ProductName = lsitem.ProductName;
                         _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         _pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                         _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         _pd.TarrifCode = lsitem.TarrifCode;
                         _pd.RowNumber = lsitem.RowNumber;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }

             ///GetPackingDetailTblbyskuandBoxnumber

             public Guid GetPackingDetailTblbyskuandBoxnumber(String SKU, String BoxNumber)
             {
                 //return list.
                // List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 Guid packingDeatils = new Guid();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = from Pack in entx3v6.PackageDetails
                     //                     where Pack.PackingId == PackingID
                     //                     select Pack;

                     ///New Chaged Query


                     packingDeatils = (from Pack in entx3v6.PackageDetails
                                       where Pack.BoxNumber == BoxNumber && Pack.SKUNumber == SKU
                                       select Pack.PackingDetailID).SingleOrDefault();



                     //foreach (var lsitem in packingDeatils)
                     //{
                     //    cstPackageDetails _pd = new cstPackageDetails();
                     //    _pd.PackingDetailID = lsitem.PackingDetailID;
                     //    _pd.PackingId = lsitem.PackingId;
                     //    _pd.SKUNumber = lsitem.SKUNumber;
                     //    _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                     //    _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                     //    _pd.BoxNumber = lsitem.BoxNumber;
                     //    _pd.ShipmentLocation = lsitem.ShipmentLocation;
                     //    _pd.ItemName = lsitem.ItemName;
                     //    _pd.ProductName = lsitem.ProductName;
                     //    _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                     //    _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                     //    _pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                     //    _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                     //    _pd.TarrifCode = lsitem.TarrifCode;
                     //    _lsReturn.Add(_pd);
                     //}
                 }
                 catch (Exception)
                 { }
                 return packingDeatils;
             }






       /// <summary>
       /// Filter Packing Detail Table by Box Number
       /// </summary>
       /// <param name="BoxNum">String Box Number</param>
       /// <returns>List of Packing Detail table Information.</returns>
             public List<cstPackageDetails> GetPackingDetailsByBoxNum(String BoxNum)
             {
                 //return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 try
                 {
                     //Filtring condition.
                     var packingDeatils = from Pack in entx3v6.PackageDetails
                                          where Pack.BoxNumber == BoxNum
                                          select Pack;

                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         _pd.PackingDetailID = lsitem.PackingDetailID;
                         _pd.PackingId = lsitem.PackingId;
                         _pd.SKUNumber = lsitem.SKUNumber;
                         _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;
                         _pd.ShipmentLocation = lsitem.ShipmentLocation;
                         _pd.ItemName = lsitem.ItemName;
                         _pd.ProductName = lsitem.ProductName;
                         _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         _pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                         _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         _pd.TarrifCode = lsitem.TarrifCode;
                         _pd.RowNumber = lsitem.RowNumber;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }
        #endregion

             public List<cstPackageDetails> GetPackingDetailsByShipmentNumber(String ShipmentNumber)
             {
                 //return list.
                 List<cstPackageDetails> _lsReturn = new List<cstPackageDetails>();
                 try
                 {
                     //Filtring condition.
                     var packingDeatils = from Pack in entx3v6.Packages
                                          join packageDetail in entx3v6.PackageDetails
                                          on Pack.PackingId equals packageDetail.PackingId
                                          where Pack.ShippingNum == ShipmentNumber
                                          select packageDetail;

                     foreach (var lsitem in packingDeatils)
                     {
                         cstPackageDetails _pd = new cstPackageDetails();
                         _pd.PackingDetailID = lsitem.PackingDetailID;
                         _pd.PackingId = lsitem.PackingId;
                         _pd.SKUNumber = lsitem.SKUNumber;
                         _pd.SKUQuantity = Convert.ToInt32(lsitem.SKUQuantity);
                         _pd.PackingDetailStartDateTime = Convert.ToDateTime(lsitem.SKUScanDateTime);
                         _pd.BoxNumber = lsitem.BoxNumber;
                         _pd.ShipmentLocation = lsitem.ShipmentLocation;
                         _pd.ItemName = lsitem.ItemName;
                         _pd.ProductName = lsitem.ProductName;
                         _pd.UnitOfMeasure = lsitem.UnitOfMeasure;
                         _pd.CountryOfOrigin = lsitem.CountryOfOrigin;
                         _pd.MAP_Price = Convert.ToDecimal(lsitem.MAP_Price);
                         _pd.TCLCOD_0 = lsitem.TCLCOD_0;
                         _pd.TarrifCode = lsitem.TarrifCode;
                         _pd.RowNumber = lsitem.RowNumber;
                         _lsReturn.Add(_pd);
                     }
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }

             public Boolean ckeckBoxIn(string BoxNumber,string Shipment)
             {
                 Boolean Flag = false;
                 try
                 {
                     string packingDeatils = (from Pack in entx3v6.Packages
                                              join packageDetail in entx3v6.PackageDetails
                                              on Pack.PackingId equals packageDetail.PackingId
                                              where Pack.ShippingNum == Shipment && packageDetail.BoxNumber == BoxNumber
                                              select packageDetail.BoxNumber).FirstOrDefault();

                     if (packingDeatils != "" && packingDeatils != null)
                     {
                         Flag = true;
                     }



                 }
                 catch (Exception)
                 {
                 }
                 return Flag;
             }
             /// <summary>
             /// /13-1-2015 For Shipment And Pallet Information
             /// </summary>
             /// <returns></returns>
             public List<PaaletDetail> GetAllShipmentAndPallet()
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@"                 
                                                                                        select 
                                                                                        case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                        plt.PalletNumber end PalletNumber, pk.ShippingNum as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                        from
                                                                                        Package pk inner join
                                                                                        PackageDetail pkd on pk.PackingID = pkd.PackingID inner join
                                                                                        PalletDetail pltd on pkd.BoxNumber = pltd.BoxNumber left join
                                                                                        PalletInfo plt on pltd.PalletID = plt.PalletID
                                                                                        where pk.LTL_flg = 1 and pk.Pallet_flg = 1 and (isnull(plt.PLT_PRO,'') = '' or isnull(plt.PLT_BOL,'') = '')
                                                                                        group by plt.PalletNumber, pk.ShippingNum, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                        order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pk.ShippingNum").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                     //foreach (var lsitem in packingDeatils)
                     //{
                     //    PaaletDetail _pd = new PaaletDetail();
                     //    _pd.shipmentNum = lsitem.ShipmentNumber;
                     //    _pd.PalletNum = lsitem.PalletNumber;

                     //    _lsReturn.Add(_pd);
                     //}
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }


             public List<PaaletDetail> GetAllShipmentAndPalletPerStation(Guid StationID)
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@"                 
                                                                                         select 
                                                                                         case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                         plt.PalletNumber end PalletNumber, pk.ShippingNum as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                         from Package pk inner join
                                                                                         PackageDetail pkd on pk.PackingID = pkd.PackingID inner join
                                                                                         PalletDetail pltd on pkd.BoxNumber = pltd.BoxNumber left join
                                                                                         PalletInfo plt on pltd.PalletID = plt.PalletID
                                                                                         where pk.LTL_flg = 1 and pk.Pallet_flg = 1 and pk.stationId ='" + StationID + "' and (isnull(plt.PLT_PRO,'') = '' or isnull(plt.PLT_BOL,'') = '') group by plt.PalletNumber, pk.ShippingNum, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pk.ShippingNum").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                     //foreach (var lsitem in packingDeatils)
                     //{
                     //    PaaletDetail _pd = new PaaletDetail();
                     //    _pd.shipmentNum = lsitem.ShipmentNumber;
                     //    _pd.PalletNum = lsitem.PalletNumber;

                     //    _lsReturn.Add(_pd);
                     //}
                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }


             public List<PaaletDetail> GetAllShipmentAndPalletByShipment(String ShipmentNumber)
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@"select 
                                                                                     case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                     plt.PalletNumber end PalletNumber, pltd.ShipmentNumber as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                     from 
                                                                                     PalletDetail pltd left join
                                                                                     PalletInfo plt on pltd.PalletID = plt.PalletID
                                                                                     where pltd.ShipmentNumber='" + ShipmentNumber + "' group by plt.PalletNumber, pltd.ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID  order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pltd.ShipmentNumber").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }


             public List<PaaletDetail> GetAllShipmentAndPalletByPallet(String PalletNumber)
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@"select 
                                                                                     case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                     plt.PalletNumber end PalletNumber, pltd.ShipmentNumber as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                     from 
                                                                                     PalletDetail pltd left join
                                                                                     PalletInfo plt on pltd.PalletID = plt.PalletID
                                                                                     where plt.PalletNumber='" + PalletNumber + "' group by plt.PalletNumber, pltd.ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pltd.ShipmentNumber").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }



             public List<PaaletDetail> GetAllShipmentAndPalletByPalletWithStation(String PalletNumber,Guid StationID)
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@" select 
                                                                                         case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                         plt.PalletNumber end PalletNumber, pltd.ShipmentNumber as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                         from 
                                                                                         PalletDetail pltd left join
                                                                                         PalletInfo plt on pltd.PalletID = plt.PalletID left join 
                                                                                         Package pk on pk.ShippingNum = pltd.ShipmentNumber
                                                                                         where plt.PalletNumber='" + PalletNumber + "' and pk.StationID='" + StationID + "' group by plt.PalletNumber, pltd.ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pltd.ShipmentNumber").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }


             public List<PaaletDetail> GetAllShipmentAndPalletByShipmentWithStation(String ShipmentNumber, Guid StationID)
             {
                 //return list.
                 List<PaaletDetail> _lsReturn = new List<PaaletDetail>();
                 try
                 {
                     //Filtring condition.
                     //var packingDeatils = (from PalletDetails in entx3v6.PalletDetails
                     //                     join PalletInfoes in entx3v6.PalletInfoes
                     //                     on PalletDetails.PalletID equals PalletInfoes.PalletID

                     //                     select new { PalletDetails.ShipmentNumber, PalletInfoes.PalletNumber }).Distinct();


                     var packingDeatils = entx3v6.ExecuteStoreQuery<PaaletDetail>(@" select 
                                                                                    case when row_number() over (partition by plt.PalletNumber order by plt.PalletNumber) > 1 then null else 
                                                                                    plt.PalletNumber end PalletNumber, pltd.ShipmentNumber as ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID
                                                                                    from 
                                                                                    PalletDetail pltd left join 
                                                                                    PalletInfo plt on pltd.PalletID = plt.PalletID left join 
                                                                                    Package pk on pk.ShippingNum = pltd.ShipmentNumber
                                                                                    where pk.ShippingNum='" + ShipmentNumber + "' and pk.StationID='" + StationID + "' group by plt.PalletNumber, pltd.ShipmentNumber, plt.PLT_Carrier,plt.PLT_BOL,plt.PLT_PRO,plt.Location,plt.PalletID order by plt.PalletNumber, row_number() over (partition by plt.PalletNumber order by plt.PalletNumber), pltd.ShipmentNumber").ToList();


                     foreach (var lsitem in packingDeatils)
                     {
                         PaaletDetail _pd = new PaaletDetail();
                         _pd.ShipmentNumber = lsitem.ShipmentNumber;
                         _pd.PalletNumber = lsitem.PalletNumber;
                         _pd.PLT_BOL = lsitem.PLT_BOL;
                         _pd.PLT_Carrier = lsitem.PLT_Carrier;
                         _pd.PLT_PRO = lsitem.PLT_PRO;
                         _pd.Location = lsitem.Location;
                         _pd.PalletID = lsitem.PalletID;
                         _lsReturn.Add(_pd);
                     }

                 }
                 catch (Exception)
                 { }
                 return _lsReturn;
             }



             #region DO NOT Delete Boxes which have Tracking Num
             public List<cstCheckTracking> CheckDeleteBoxFromPackingDetail(String BoxNumber)
             {

                 List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                 try
                 {

                     var validBox = entx3v6.ExecuteStoreQuery<cstCheckTracking>(@"select distinct
                                                                            pd.BoxNumber, TrackingNum, case when TrackingNum is null then 'Y' else 'N' end as DeleteFlag
                                                                            from
                                                                            PackageDetail pd left join Tracking t
                                                                            on pd.BoxNumber = t.BOXNUM and substring(isnull([TrackingNum_UPS],''),9,2) <> '90' and
                                                                            TrackingNum not in (Select TrackingNum from Tracking where VOIIND = 'Y')
                                                                            where pd.BoxNumber = '" + BoxNumber + "'");

                     foreach (var detls in validBox)
                     {
                         cstCheckTracking cs = new cstCheckTracking();
                         cs.BoxNumber = detls.BoxNumber;
                         cs.TrackingNum = detls.TrackingNum;
                         cs.DeleteFlag = detls.DeleteFlag;
                         lsttrcking.Add(cs);
                     }

                 }
                 catch (Exception)
                 {
                 }
                 return lsttrcking;
             }

             #endregion
             public List<cstCheckTracking> CheckDeleteBoxFromPackingDetail(String BoxNumber, string Location)
             {

                 List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                 try
                 {

                     var validBox = entx3v6.ExecuteStoreQuery<cstCheckTracking>(@"select distinct
                                                                            pd.BoxNumber, TrackingNum, case when TrackingNum is null then 'Y' else 'N' end as DeleteFlag
                                                                            from
                                                                            PackageDetail pd left join Tracking t
                                                                            on pd.BoxNumber = t.BOXNUM and substring(isnull([TrackingNum_UPS],''),9,2) <> '90' and
                                                                            TrackingNum not in (Select TrackingNum from Tracking where VOIIND = 'Y')
                                                                            where pd.BoxNumber = '" + BoxNumber + "'and pd.ShipmentLocation='" + Location + "'");

                     foreach (var detls in validBox)
                     {
                         cstCheckTracking cs = new cstCheckTracking();
                         cs.BoxNumber = detls.BoxNumber;
                         cs.TrackingNum = detls.TrackingNum;
                         cs.DeleteFlag = detls.DeleteFlag;
                         lsttrcking.Add(cs);
                     }

                 }
                 catch (Exception)
                 {
                 }
                 return lsttrcking;
             }



             #region delete BOX from Loacation
             #region DeleteBox and SKU From PackingDetail
             public void DeleteBoxFromPackingDetail(String BoxNumber, string Location)
             {
                 try
                 {
                     List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                     lsttrcking = CheckDeleteBoxFromPackingDetail(BoxNumber);

                     foreach (var valid in lsttrcking)
                     {
                         cstCheckTracking cs = new cstCheckTracking();
                         if (valid.DeleteFlag == "Y")
                         {
                             var _Packing = from detail in entx3v6.PackageDetails
                                            where detail.BoxNumber == BoxNumber
                                            where detail.ShipmentLocation == Location
                                            select detail;


                             foreach (var detls in _Packing)
                             {
                                 local_x3v6Entities mm = new local_x3v6Entities()
                                 {

                                     //PackageDetail tl = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                                     //entx3v6.PackageDetails.DeleteObject(tl);
                                     //entx3v6.SaveChanges();


                                 };

                                 PackageDetail tl = mm.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                                 mm.PackageDetails.DeleteObject(tl);
                                 mm.SaveChanges();
                             }
                         }
                     }
                     //                     context.Widgets.Where(w => w.WidgetId == widgetId)
                     //               .ToList().ForEach(context.Widgets.DeleteObject);
                     //context.SaveChanges()

                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID =).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                     //entx3v6.SaveChanges();

                     //var deleteobjects = from detl in entx3v6.PackageDetails
                     //                    where detl.BoxNumber == BoxNumber 

                     //                    select detl;

                     //foreach (var detls in deleteobjects)
                     //{
                     //    entx3v6.PackageDetails.DeleteObject(detls);
                     //    entx3v6.SaveChanges();
                     //}

                     //foreach (var item in _Packing)
                     //{
                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID == item.PackingDetailID).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                     //entx3v6.SaveChanges();

                     //entx3v6.AttachTo(,entx3v6);
                     //entx3v6.DeleteObject(item.PackingDetailID);
                     //entx3v6.SaveChanges(); 
                     //}
                     //entx3v6.DeleteObject(_Packing);
                     //entx3v6.SaveChanges();
                     //entx3v6.DeleteObject(_Packing);
                     //entx3v6.SaveChanges();
                 }
                 catch (Exception)
                 {
                 }
             }
             #endregion
             #endregion
        #region DeleteBox and SKU From PackingDetail
//             public void DeleteBoxFromPackingDetail(String BoxNumber)
//             {
//                 try
//                 { 
//                     var _Packing = from detail in entx3v6.PackageDetails
//                                    where detail.BoxNumber == BoxNumber
//                                    select detail;


//                      foreach (var detls in _Packing)
//                     {
//                         local_x3v6Entities mm = new local_x3v6Entities()
//                         {

//                             //PackageDetail tl = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
//                             //entx3v6.PackageDetails.DeleteObject(tl);
//                             //entx3v6.SaveChanges();
                             

//                         };

//                         PackageDetail tl = mm.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
//                         mm.PackageDetails.DeleteObject(tl);
//                         mm.SaveChanges();
//                 }
////                     context.Widgets.Where(w => w.WidgetId == widgetId)
////               .ToList().ForEach(context.Widgets.DeleteObject);
////context.SaveChanges()

//                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID =).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
//                     //entx3v6.SaveChanges();

//                     //var deleteobjects = from detl in entx3v6.PackageDetails
//                     //                    where detl.BoxNumber == BoxNumber 
                                         
//                     //                    select detl;

//                     //foreach (var detls in deleteobjects)
//                     //{
//                     //    entx3v6.PackageDetails.DeleteObject(detls);
//                     //    entx3v6.SaveChanges();
//                     //}
                   
//                     //foreach (var item in _Packing)
//                     //{
//                         //entx3v6.PackageDetails.Where(w => w.PackingDetailID == item.PackingDetailID).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
//                         //entx3v6.SaveChanges();

//                         //entx3v6.AttachTo(,entx3v6);
//                         //entx3v6.DeleteObject(item.PackingDetailID);
//                         //entx3v6.SaveChanges(); 
//                     //}
//                     //entx3v6.DeleteObject(_Packing);
//                     //entx3v6.SaveChanges();
//                     //entx3v6.DeleteObject(_Packing);
//                     //entx3v6.SaveChanges();
//                 }
//                 catch (Exception)
//                 {
//                 }
//             }

             public void DeleteBoxFromPackingDetail(String BoxNumber)
             {
                 try
                 {
                     List<cstCheckTracking> lsttrcking = new List<cstCheckTracking>();
                     lsttrcking = CheckDeleteBoxFromPackingDetail(BoxNumber);

                     foreach (var valid in lsttrcking)
                     {
                         cstCheckTracking cs = new cstCheckTracking();
                         if (valid.DeleteFlag == "Y")
                         {


                             var _Packing = from detail in entx3v6.PackageDetails
                                            where detail.BoxNumber == BoxNumber
                                            select detail;


                             foreach (var detls in _Packing)
                             {
                                 local_x3v6Entities mm = new local_x3v6Entities()
                                 {

                                     //PackageDetail tl = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                                     //entx3v6.PackageDetails.DeleteObject(tl);
                                     //entx3v6.SaveChanges();


                                 };

                                 PackageDetail tl = mm.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                                 mm.PackageDetails.DeleteObject(tl);
                                 mm.SaveChanges();
                             }
                         }
                         else
                         {

                         }
                     }
                     //                     context.Widgets.Where(w => w.WidgetId == widgetId)
                     //               .ToList().ForEach(context.Widgets.DeleteObject);
                     //context.SaveChanges()

                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID =).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                     //entx3v6.SaveChanges();

                     //var deleteobjects = from detl in entx3v6.PackageDetails
                     //                    where detl.BoxNumber == BoxNumber 

                     //                    select detl;

                     //foreach (var detls in deleteobjects)
                     //{
                     //    entx3v6.PackageDetails.DeleteObject(detls);
                     //    entx3v6.SaveChanges();
                     //}

                     //foreach (var item in _Packing)
                     //{
                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID == item.PackingDetailID).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                     //entx3v6.SaveChanges();

                     //entx3v6.AttachTo(,entx3v6);
                     //entx3v6.DeleteObject(item.PackingDetailID);
                     //entx3v6.SaveChanges(); 
                     //}
                     //entx3v6.DeleteObject(_Packing);
                     //entx3v6.SaveChanges();
                     //entx3v6.DeleteObject(_Packing);
                     //entx3v6.SaveChanges();
                 }
                 catch (Exception)
                 {
                 }
             }
             public void DeleteSingleBoxFromPackingDetail(String BoxNumber)
             {
                 try
                 {



                     var _Packing = from detail in entx3v6.PackageDetails
                                    where detail.BoxNumber == BoxNumber
                                    select detail;


                     foreach (var detls in _Packing)
                     {
                         local_x3v6Entities mm = new local_x3v6Entities()
                         {

                             //PackageDetail tl = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                             //entx3v6.PackageDetails.DeleteObject(tl);
                             //entx3v6.SaveChanges();


                         };

                         PackageDetail tl = mm.PackageDetails.SingleOrDefault(i => i.PackingDetailID == detls.PackingDetailID);
                         mm.PackageDetails.DeleteObject(tl);
                         mm.SaveChanges();
                     }


                 }
                 //                     context.Widgets.Where(w => w.WidgetId == widgetId)
                 //               .ToList().ForEach(context.Widgets.DeleteObject);
                 //context.SaveChanges()

                     //entx3v6.PackageDetails.Where(w => w.PackingDetailID =).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                 //entx3v6.SaveChanges();

                     //var deleteobjects = from detl in entx3v6.PackageDetails
                 //                    where detl.BoxNumber == BoxNumber 

                     //                    select detl;

                     //foreach (var detls in deleteobjects)
                 //{
                 //    entx3v6.PackageDetails.DeleteObject(detls);
                 //    entx3v6.SaveChanges();
                 //}

                     //foreach (var item in _Packing)
                 //{
                 //entx3v6.PackageDetails.Where(w => w.PackingDetailID == item.PackingDetailID).ToList().ForEach(entx3v6.PackageDetails.DeleteObject);
                 //entx3v6.SaveChanges();

                     //entx3v6.AttachTo(,entx3v6);
                 //entx3v6.DeleteObject(item.PackingDetailID);
                 //entx3v6.SaveChanges(); 
                 //}
                 //entx3v6.DeleteObject(_Packing);
                 //entx3v6.SaveChanges();
                 //entx3v6.DeleteObject(_Packing);
                 //entx3v6.SaveChanges();

                 catch (Exception)
                 {
                 }

             }
        #endregion
    }
   public class PaaletDetail
   {
       public string ShipmentNumber { get; set; }

       public string PalletNumber { get; set; }
       public string PLT_Carrier { get; set; }
       public string PLT_BOL { get; set; }
       public string PLT_PRO { get; set; }

       public string Location { get; set; }

       public Guid PalletID { get; set; }

   }
}
