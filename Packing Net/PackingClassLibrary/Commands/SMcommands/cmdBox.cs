using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
    /// <summary>
    /// All fucntions related to the Box.
    /// </summary>
   public class cmdBox
   {
       local_x3v6Entities entx3v6 = new local_x3v6Entities();

       #region Get Fucntions
       /// <summary>
       /// Get all BoxPackage Table Information
       /// </summary>
       /// <returns>List<cstBoxPackage></returns>
       public List<cstBoxPackage> GetAll()
       {
           List<cstBoxPackage> lsreturn = new List<cstBoxPackage>();
           try
           {
               var allBoxInfo = from b in entx3v6.BoxPackages
                                select b;

               foreach (var _boxitem in allBoxInfo)
               {
                   cstBoxPackage _box = new cstBoxPackage();
                   _box.BoxID = Guid.NewGuid();
                   _box.PackingID = _boxitem.PackingID;
                   _box.BoxType = _boxitem.BoxType;
                   _box.BoxWeight =Convert.ToDouble( _boxitem.BoxWeight);
                   _box.BoxLength =Convert.ToDouble( _boxitem.BoxLength);
                   _box.BoxHeight = Convert.ToDouble(_boxitem.BoxHeight);
                   _box.BoxWidth = Convert.ToDouble(_boxitem.BoxWidth);
                   _box.BoxCreatedTime =Convert.ToDateTime( _boxitem.BoxCreatedTime);
                   _box.BoxMeasurementTime = Convert.ToDateTime(_boxitem.BoxMeasurementTime);
                   _box.ROWID = _boxitem.ROWID;
                   _box.BOXNUM = _boxitem.BOXNUM;

                   _box.BoxEndTime = Convert.ToDateTime(_boxitem.BoxEndTime);
                   lsreturn.Add(_box);
               }
           }
           catch (Exception)
           {}
           return lsreturn;

       }

       /// <summary>
       /// Selected single Row by filter Box id
       /// </summary>
       /// <param name="BoxID">Guid Box ID</param>
       /// <returns>cstBoxPackage Object</returns>
       public cstBoxPackage GetSelectedByBoxID(Guid BoxID)
       {
           cstBoxPackage _return = new cstBoxPackage();
           try
           {
               BoxPackage _boxitem = entx3v6.BoxPackages.SingleOrDefault(i => i.BoxID == BoxID); 

                   cstBoxPackage _box = new cstBoxPackage();
                   _box.BoxID = Guid.NewGuid();
                   _box.PackingID = _boxitem.PackingID;
                   _box.BoxType = _boxitem.BoxType;
                   _box.BoxWeight = Convert.ToDouble(_boxitem.BoxWeight);
                   _box.BoxLength = Convert.ToDouble(_boxitem.BoxLength);
                   _box.BoxHeight = Convert.ToDouble(_boxitem.BoxHeight);
                   _box.BoxWidth = Convert.ToDouble(_boxitem.BoxWidth);
                   _box.BoxCreatedTime = Convert.ToDateTime(_boxitem.BoxCreatedTime);
                   _box.BoxMeasurementTime = Convert.ToDateTime(_boxitem.BoxMeasurementTime);
                   _box.ROWID = _boxitem.ROWID;
                   _box.BOXNUM = _boxitem.BOXNUM;

                   _box.BoxEndTime =Convert.ToDateTime(_boxitem.BoxEndTime);

                   _return = _box;
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Get BoxPackage Table information sorted by BOXNUM
       /// </summary>
       /// <param name="BoxNumber">String Box Nuber</param>
       /// <returns>BoxPackage Object </returns>
       public cstBoxPackage GetSelectedByBoxNumber(String BoxNumber)
       {
           cstBoxPackage _return = new cstBoxPackage();
           try
           {
               BoxPackage _boxitem = entx3v6.BoxPackages.SingleOrDefault(i => i.BOXNUM == BoxNumber);
               cstBoxPackage _box = new cstBoxPackage();
               _box.BoxID = Guid.NewGuid();
               _box.PackingID = _boxitem.PackingID;
               _box.BoxType = _boxitem.BoxType;
               _box.BoxWeight = Convert.ToDouble(_boxitem.BoxWeight);
               _box.BoxLength = Convert.ToDouble(_boxitem.BoxLength);
               _box.BoxHeight = Convert.ToDouble(_boxitem.BoxHeight);
               _box.BoxWidth = Convert.ToDouble(_boxitem.BoxWidth);
               _box.BoxCreatedTime = Convert.ToDateTime(_boxitem.BoxCreatedTime);
               _box.BoxMeasurementTime = Convert.ToDateTime(_boxitem.BoxMeasurementTime);
               _box.ROWID = _boxitem.ROWID;
               _box.BOXNUM = _boxitem.BOXNUM;
               _box.BoxEndTime = Convert.ToDateTime(_boxitem.BoxEndTime);
               _return = _box;
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// list of information by Packing ID
       /// </summary>
       /// <param name="PackingID">Guid Packing ID</param>
       /// <returns>List<cstBoxPackage> </returns>
       public List<cstBoxPackage> GetSelectedByPackingID(Guid PackingID)
       {
           List<cstBoxPackage> lsreturn = new List<cstBoxPackage>();
           try
           {
               var allBoxInfo = from b in entx3v6.BoxPackages
                                where b.PackingID== PackingID
                                select b;

               foreach (var _boxitem in allBoxInfo)
               {
                   cstBoxPackage _box = new cstBoxPackage();
                   _box.BoxID = _boxitem.BoxID;
                   _box.PackingID = _boxitem.PackingID;
                   _box.BoxType = _boxitem.BoxType;
                   _box.BoxWeight = Convert.ToDouble(_boxitem.BoxWeight);
                   _box.BoxLength = Convert.ToDouble(_boxitem.BoxLength);
                   _box.BoxHeight = Convert.ToDouble(_boxitem.BoxHeight);
                   _box.BoxWidth = Convert.ToDouble(_boxitem.BoxWidth);
                   _box.BoxCreatedTime = Convert.ToDateTime(_boxitem.BoxCreatedTime);
                   _box.BoxMeasurementTime = Convert.ToDateTime(_boxitem.BoxMeasurementTime);
                   _box.ROWID = _boxitem.ROWID;
                   _box.BOXNUM = _boxitem.BOXNUM;
                   _box.BoxEndTime = Convert.ToDateTime(_boxitem.BoxEndTime);
                   lsreturn.Add(_box);
               }
           }
           catch (Exception)
           { }
           return lsreturn;
       }


       public int GetSKUQtyByBoxSKU(string skuNum,String BoxNum)
       {
           var  skuQty = (from pkd in entx3v6.PackageDetails
                        where pkd.SKUNumber == skuNum && pkd.BoxNumber == BoxNum
                        select pkd.SKUQuantity).Single();
           int ss = Convert.ToInt32(skuQty);
           return ss;
       }

       public Guid GetBoxIDByBoxNumber(string BoxNumber)
       {
           Guid BoxId = Guid.NewGuid();

           var BoxGuid = (from r in entx3v6.BoxPackages where r.BOXNUM == BoxNumber select r.BoxID).SingleOrDefault();

           BoxId = BoxGuid;

           return BoxId;
       }

       #endregion

       #region Set Functions
       /// <summary>
       /// Save Box information to the database
       /// </summary>
       /// <param name="lsBoxpackage">list of information of box</param>
       /// <returns>Guid of New Box Id</returns>
       public Guid SaveBoxPackage(List<cstBoxPackage> lsBoxpackage)
       {
           Guid _return = Guid.Empty;
               
           try
           {
               foreach (var _boxitem in lsBoxpackage)
               {
                   BoxPackage _boxPackage = new BoxPackage();

                   _boxPackage.BoxID = Guid.NewGuid();
                   _boxPackage.PackingID = _boxitem.PackingID;
                   _boxPackage.BoxType = _boxitem.BoxType;
                   _boxPackage.BoxWeight = _boxitem.BoxWeight;
                   _boxPackage.BoxLength = _boxitem.BoxLength;
                   _boxPackage.BoxHeight = _boxitem.BoxHeight;
                   _boxPackage.BoxWidth = _boxitem.BoxWidth;
                   _boxPackage.BoxCreatedTime = _boxitem.BoxCreatedTime;

                   if (_boxitem.BoxEndTime.Date != Convert.ToDateTime("01/01/001").Date)
                   {
                       _boxPackage.BoxEndTime = Convert.ToDateTime(_boxitem.BoxEndTime);
                   }


                   

                   if (_boxitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                   {
                       _boxPackage.BoxMeasurementTime = _boxitem.BoxMeasurementTime;
                   }
                   entx3v6.AddToBoxPackages(_boxPackage); 
                   _return = _boxPackage.BoxID;
               }
               entx3v6.SaveChanges();
              
           }
           catch (Exception)
           {}
           return _return;
       }
       #endregion



       #region Update
       public Boolean UpdateBoxPackage(DateTime EndTime,String BOXNUM)
       {
           Boolean flag = false;

           try
           {
             
                  // BoxPackage _boxPackage = new BoxPackage();


                   BoxPackage _BoxPacking = entx3v6.BoxPackages.SingleOrDefault(i => i.BOXNUM == BOXNUM);


                   _BoxPacking.BoxEndTime = EndTime;
                   //_boxPackage.PackingID = _boxitem.PackingID;
                   //_boxPackage.BoxType = _boxitem.BoxType;
                   //_boxPackage.BoxWeight = _boxitem.BoxWeight;
                   //_boxPackage.BoxLength = _boxitem.BoxLength;
                   //_boxPackage.BoxHeight = _boxitem.BoxHeight;
                   //_boxPackage.BoxWidth = _boxitem.BoxWidth;
                   //_boxPackage.BoxCreatedTime = _boxitem.BoxCreatedTime;

                   //_boxPackage.BoxEndTime = Convert.ToDateTime(_boxitem.BoxEndTime);

                   //if (_boxitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                   //{
                   //    _boxPackage.BoxMeasurementTime = _boxitem.BoxMeasurementTime;
                   //}
                   entx3v6.SaveChanges();
                   flag = true;
              
              

           }
           catch (Exception)
           { }
           return flag;
       }
       #endregion

   }
}
