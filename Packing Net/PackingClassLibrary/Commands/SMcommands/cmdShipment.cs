using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.Commands;
using PackingClassLibrary.Error_Loger;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdShipment 
   {
       local_x3v6Entities entx3v6 = new local_x3v6Entities();
       Sage_x3v6Entities Sage = new Sage_x3v6Entities();

       #region Get Functions
       //Return List.
       List<cstShipment> _lsShipment_SPAKD = new List<cstShipment>();

     /// <summary>
     /// Get Shipment information from the sage
     /// </summary>
     /// <param name="ShipmentID">String shipment number</param>
       /// <returns>List<cstShipment></returns>
       public List<cstShipment> GetShipmentInfoFromSage(string ShipmentID)
       {
           try
           {
               String _location = cmdLocalFile.ReadString("Location");
               //selection operation that fetch the data from the sage DB for the perticular id
               //linq query
               var _Shipment = from Ship in entx3v6.Get_Shipping_Data.OrderBy(i => i.Row_Number)
                               where Ship.ShipmentID == ShipmentID &&
                               Ship.LocationCombined == _location
                               select new
               {
                   Ship.SKU,
                   Ship.Quantity,
                   Ship.ProductName,
                   Ship.UPCCode,
                   Ship.LocationCombined,
                   Ship.LineType,
                   Ship.Row_Number

               };

               //Add each shipment information row in to the list
               foreach (var _shipentItem in _Shipment)
               {
                   cstShipment _ship = new cstShipment();
                   _ship.SKU = _shipentItem.SKU;
                   _ship.Quantity = Convert.ToInt64(_shipentItem.Quantity);
                   _ship.ProductName = _shipentItem.ProductName;
                   _ship.UPCCode = _shipentItem.UPCCode;
                   _ship.Location = _shipentItem.LocationCombined;
                   _ship.LineType = Convert.ToInt32(_shipentItem.LineType);
                   _ship.RowNumber = (long)_shipentItem.Row_Number;
                   _lsShipment_SPAKD.Add(_ship);
               }
           }
           catch (Exception Ex)
           {
               Error_Loger.elAction.save("GetShipmentDetails_command.Execute()", Ex.Message.ToString());
           }
           return _lsShipment_SPAKD;
       }

       /// <summary>
       /// Get Shipment Information From the sage
       /// </summary>
       /// <param name="ShipmentID">String Shiment Number</param>
       /// <param name="LocationNull">Boolean loacation filer true</param>
       /// <returns>List<cstShipment></returns>
       public List<cstShipment> GetShipmentInfoFromSage(string ShipmentID, Boolean LocationNull)
       {
           try
           {
               //selection operation that fetch the data from the sage DB for the perticular id
               //linq query
               var _Shipment = from Ship in entx3v6.Get_Shipping_Data.OrderBy(i => i.Row_Number)
                               where Ship.ShipmentID == ShipmentID
                               select new
                               {
                                    
                   Ship.SKU,
                   Ship.Quantity,
                   Ship.ProductName,
                   Ship.UPCCode,
                   Ship.LocationCombined,
                   Ship.LineType,
                   Ship.Row_Number

               };
                               

               //Add each shipment information row in to the list
               foreach (var _shipentItem in _Shipment)
               {
                   cstShipment _ship = new cstShipment();
                   _ship.SKU = _shipentItem.SKU;
                   _ship.Quantity = Convert.ToInt64(_shipentItem.Quantity);
                   _ship.ProductName = _shipentItem.ProductName;
                   _ship.UPCCode = _shipentItem.UPCCode;
                   _ship.Location = _shipentItem.LocationCombined;
                   _ship.LineType =Convert.ToInt32 (_shipentItem.LineType);
                   _ship.RowNumber = (long)_shipentItem.Row_Number;
                   _lsShipment_SPAKD.Add(_ship);
               }
           }
           catch (Exception Ex)
           {
               Error_Loger.elAction.save("GetShipmentDetails_command.Execute()", Ex.Message.ToString());
           }
           return _lsShipment_SPAKD;
       }

       /// <summary>
       /// check that shipment is packed at the application location or not.
       /// </summary>
       /// <param name="PackedLocations">List of string that show paked locations of the shipment.</param>
       /// <param name="ApplicationLocation">string Application Current Location</param>
       /// <param name="IsAlreadyPacked">boolean Value the shipment is already packed or not.</param>
       /// <returns></returns>
       public Boolean IsShipmentPackedAtSameLocation(List<string> PackedLocations, String ApplicationLocation, bool IsAlreadyPacked)
       {
           Boolean _return = false;
           try
           {
               if (IsAlreadyPacked)
               {
                   foreach (string Location in PackedLocations)
                   {
                       if (Location == ApplicationLocation)
                       {
                           _return = true;
                       }
                   }
               }

           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Check shipment Validated flag is 1 or 2 
       /// </summary>
       /// <param name="ShipmentID">String Shipment number</param>
       /// <returns>True if 1 else flase</returns>
       public Boolean IsShipmentValidated(string ShipmentID)
       {
           Boolean _return = false;
           try
           {
               var _Shipment = from Ship in entx3v6.Get_Shipping_Data.OrderBy(i => i.Row_Number)
                               where Ship.ShipmentID == ShipmentID && Ship.ValidationFLG == 1
                               select Ship;

                   if (_Shipment.Count()>0)
                   {
                       _return = true;
                   }
           }
           catch (Exception)
           {}
           return _return;
       }

       #endregion
       
       #region set Functions
       
       #endregion

       #region Delete Functions
       /// <summary>
       /// Delte shipment Information from packingDetails table and Packing table
       /// </summary>
       /// <param name="ShippingNum">String ShipmentID</param>
       /// <returns>If sucess then true else false</returns>
       public Boolean DeleteShipment(Guid PackingID)
       {
           Boolean _return = false;
           try
           {
               //Delete from Packing Detail table first.
               var _PackingDetails = from _PckDetails in entx3v6.PackageDetails
                                     where _PckDetails.PackingId == PackingID
                                     select _PckDetails;

               foreach (var _PackingDetailsitem in _PackingDetails)
               {
                   PackageDetail _packDetails = entx3v6.PackageDetails.SingleOrDefault(i => i.PackingDetailID == _PackingDetailsitem.PackingDetailID);
                   entx3v6.PackageDetails.DeleteObject(_packDetails);
               }
               entx3v6.SaveChanges();
               // delete from the Packing table
               Package _packing = entx3v6.Packages.SingleOrDefault(i => i.PackingId == PackingID);
               entx3v6.Packages.DeleteObject(_packing);
               entx3v6.SaveChanges();
                
               _return = true;
           }
           catch (Exception Ex)
           {
               elAction.save("DeleteShipmentInfo.DeleteShipment()", Ex.ToString());
           }
           return _return;
       }
       #endregion
   }
}
