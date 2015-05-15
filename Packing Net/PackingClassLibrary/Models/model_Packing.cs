using PackingClassLibrary.BusinessLogic;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.CustomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Models
{
    /// <summary>
    /// Author: Avinash.
    /// Versiom: Alfa.
    /// Packing Information model.
    /// </summary>
   public class model_Packing
    {
       public static Guid PackingID { get; set;}
       public cstPackageTbl PackageInfo { get; set; }
       public model_Shipment ShipmentInfo { get; set; }
       public Boolean IsMangerOverride { get; set; }
       public Boolean IsSelfOverride{get;set;}
       public int PackingStatus { get; set; }

       /// <summary>
       /// Package Functions.
       /// </summary>
       public cmdPackage PackageFunctions = new cmdPackage();

       /// <summary>
       /// Default constructor of Model Packing.
       /// </summary>
       public model_Packing() { }

       /// <summary>
       /// Parameterised Constructor of packing Model.
       /// </summary>
       /// <param name="PackingIDc">Guid Packing ID of the package table</param>
       public model_Packing(Guid PackingIDc)
       {
           PackingID = PackingIDc;
           setPackingInfo();
       }

       /// <summary>
       /// Set packing Information to the Packing object of the model class
       /// </summary>
       public void setPackingInfo()
       {
           cmdPackage _packing = new cmdPackage();
           PackageInfo = _packing.Execute().SingleOrDefault(i => i.PackingId == PackingID);

           //Set Packing Status.
           PackingStatus = PackageInfo.PackingStatus;
           IsMangerOverride = false;
           IsSelfOverride = false;
           if (PackageInfo.MangerOverride  ==1)
           {
               IsMangerOverride = true;
           }
           else if(PackageInfo.MangerOverride == 2)
           {
               IsSelfOverride = true;
           }

           //Packing Information Present then Fill its shipment information
           if (PackageInfo !=null)
           {
               SetShipmentInfo();
           }
       }

       /// <summary>
       /// set shipping information related to the packing model packing ID.
       /// </summary>
       public void SetShipmentInfo()
       {
           ShipmentInfo = new model_Shipment(PackageInfo.ShippingNum);
       }

    }
}
