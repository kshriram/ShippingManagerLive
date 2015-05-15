using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
    public class cmdPallet
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();

        #region setPallet
        public Guid SavePalletInfo(List<cstPalletInfo> lsPalletinfo)
        {
            Guid _return = Guid.Empty;

            try
            {
                foreach (var _palletitem in lsPalletinfo)
                {
                    PalletInfo _pallet = new PalletInfo();

                    _pallet.PalletID = Guid.NewGuid();
                    _pallet.PalletType = _palletitem.PalletType;
                    _pallet.PalletWeight = _palletitem.PalletWeight;
                    _pallet.PalletHeight = _palletitem.PalletHeight;
                    _pallet.PalletWidth = _palletitem.PalletWeight;
                    _pallet.palletCreatedTime = _palletitem.palletCreatedTime;
                    _pallet.PrintStatus = _palletitem.PrintStatus;

                    _pallet.PLT_Carrier = _palletitem.PLT_Carrier;
                    _pallet.PLT_BOL = _palletitem.PLT_BOL;
                    _pallet.PLT_PRO = _palletitem.PLT_PRO;

                    _pallet.Location = _palletitem.Location;

                    _pallet.SSCC_Code = _palletitem.SSCC_Code;

                    //_pallet.PalletEndDateTime = _palletitem.PalletEndDateTime;
                    //_pallet.BOLCreatedDateTime = _palletitem.BOLCreatedDateTime;



                    //if (_palletitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                    //{
                    //    _boxPackage.BoxMeasurementTime = _palletitem.BoxMeasurementTime;
                    //}
                    entx3v6.AddToPalletInfoes(_pallet);
                    _return = _pallet.PalletID;
                }
                entx3v6.SaveChanges();

            }
            catch (Exception)
            { }
            return _return;
        }   
        #endregion


        #region Update Pallet

        public Guid UpdatePalletInfo(List<cstPalletInfo> lsPalletinfo, Guid PalletID)
        {
            Guid _return = Guid.Empty;

            try
            {
                foreach (var _palletitem in lsPalletinfo)
                {


                    PalletInfo _packing = entx3v6.PalletInfoes.SingleOrDefault(i => i.PalletID == PalletID);

                    _packing.PalletID = PalletID;

                    //  _packing.PrintStatus = _palletitem.PrintStatus;
                    _packing.PLT_BOL = _palletitem.PLT_BOL;
                    _packing.PLT_Carrier = _palletitem.PLT_Carrier;
                    _packing.PLT_PRO = _palletitem.PLT_PRO;
                    _packing.Location = _palletitem.Location;
                    _packing.PrintStatus = 1;
                    _packing.BOLCreatedDateTime = _palletitem.BOLCreatedDateTime;




                    //if (_palletitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                    //{
                    //    _boxPackage.BoxMeasurementTime = _palletitem.BoxMeasurementTime;
                    //}
                    //entx3v6.AddToPalletInfoes(_pallet);
                    // _return = _pallet.PalletID;
                }
                entx3v6.SaveChanges();

            }
            catch (Exception)
            { }
            return _return;
        }

        public Guid UpdateEndTimePalletInfo(List<cstPalletInfo> lsPalletinfo, Guid PalletID)
        {
            Guid _return = Guid.Empty;

            try
            {
                foreach (var _palletitem in lsPalletinfo)
                {


                    PalletInfo _packing = entx3v6.PalletInfoes.SingleOrDefault(i => i.PalletID == PalletID);

                    _packing.PalletID = PalletID;

                    //  _packing.PrintStatus = _palletitem.PrintStatus;
                    _packing.PalletEndDateTime = _palletitem.PalletEndDateTime;





                    //if (_palletitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                    //{
                    //    _boxPackage.BoxMeasurementTime = _palletitem.BoxMeasurementTime;
                    //}
                    //entx3v6.AddToPalletInfoes(_pallet);
                    // _return = _pallet.PalletID;
                }
                entx3v6.SaveChanges();

            }
            catch (Exception)
            { }
            return _return;
        }


        public void UpdateSSCCNUmberInfo(String SSCC_code, String PalletNum)
        {


            try
            {


                PalletInfo _packing = entx3v6.PalletInfoes.SingleOrDefault(i => i.PalletNumber == PalletNum);


                _packing.SSCC_Code = SSCC_code;





                //if (_palletitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                //{
                //    _boxPackage.BoxMeasurementTime = _palletitem.BoxMeasurementTime;
                //}
                //entx3v6.AddToPalletInfoes(_pallet);
                // _return = _pallet.PalletID;

                entx3v6.SaveChanges();

            }
            catch (Exception)
            { }
            //return _return;
        }


        #endregion

        #region SerPalletDetail
        public Guid SavePalletDetailInfo(List<cstPalletDetails> lsPalletDetailinfo)
        {
            Guid _palletDetailID = Guid.Empty;

            try
            {
                foreach (var _palletdetailitem in lsPalletDetailinfo)
                {
                    PalletDetail _pallet = new PalletDetail();

                    _pallet.PalletDetailID = Guid.NewGuid();
                    _pallet.PalletID = _palletdetailitem.PalletID;
                    _pallet.ShipmentNumber = _palletdetailitem.ShipmentNumber;
                    _pallet.BoxNumber = _palletdetailitem.BoxNumber;
                    _pallet.CartonNumber = _palletdetailitem.CartonNumber;
                    _pallet.PrintStatus = _palletdetailitem.PrintStatus;

                  

                    //if (_palletitem.BoxMeasurementTime.Date != Convert.ToDateTime("01/01/001").Date)
                    //{
                    //    _boxPackage.BoxMeasurementTime = _palletitem.BoxMeasurementTime;
                    //}
                    entx3v6.AddToPalletDetails(_pallet);
                    _palletDetailID = _pallet.PalletDetailID;
                }
                entx3v6.SaveChanges();

            }
            catch (Exception)
            { }
            return _palletDetailID;
        }
        #endregion

        #region GetPalletInfo
        public cstPalletInfo GetSelectedByPalletID(Guid PalletID)
        {
            cstPalletInfo _return = new cstPalletInfo();
            try
            {
                PalletInfo _palletitem = entx3v6.PalletInfoes.SingleOrDefault(i => i.PalletID == PalletID);

                cstPalletInfo _pallet = new cstPalletInfo();
                _pallet.PalletID = _pallet.PalletID;//Guid.NewGuid();
                _pallet.PalletType = _palletitem.PalletType;
                _pallet.PalletWeight = Convert.ToDouble(_palletitem.PalletWeight);
                _pallet.PalletHeight = Convert.ToDouble(_palletitem.PalletHeight);
                _pallet.PalletWidth = Convert.ToDouble(_palletitem.PalletWidth);
                _pallet.palletCreatedTime = Convert.ToDateTime(_palletitem.palletCreatedTime);
                _pallet.RowID = _palletitem.RowID;
                _pallet.PrintStatus = _palletitem.PrintStatus;
                _pallet.PalletNumber = _palletitem.PalletNumber;

                _pallet.PLT_Carrier = _palletitem.PLT_Carrier;
                _pallet.PLT_BOL = _palletitem.PLT_BOL;
                _pallet.PLT_PRO = _palletitem.PLT_PRO;

                _pallet.Location = _palletitem.Location;

                _pallet.SSCC_Code = _palletitem.SSCC_Code;

                _pallet.BOLCreatedDateTime = Convert.ToDateTime(_palletitem.BOLCreatedDateTime);
                _pallet.PalletEndDateTime = Convert.ToDateTime(_palletitem.PalletEndDateTime);


                _return = _pallet;
            }
            catch (Exception)
            { }
            return _return;
        }
        #endregion

        #region GetPalletDetail
        public cstPalletDetails GetpalletDetailsByPalletDetailID(Guid PalletDetailID)
        {
            cstPalletDetails _palletDetail = new cstPalletDetails();
            try
            {
                PalletDetail _palletitem = entx3v6.PalletDetails.SingleOrDefault(i => i.PalletDetailID == PalletDetailID);

                cstPalletDetails _pallet = new cstPalletDetails();
                _pallet.PalletID = Guid.NewGuid();
                _pallet.PalletDetailID = _palletitem.PalletDetailID;
                _pallet.BoxNumber = _palletitem.BoxNumber;
                _pallet.CartonNumber = _palletitem.CartonNumber;
                _pallet.ShipmentNumber = _palletitem.ShipmentNumber;
                _palletDetail = _pallet;
            }
            catch (Exception)
            { }
            return _palletDetail;
        }

        public List<cstPalletDetails> GetpalletDetailsByBoxNumber(String BoxNumber)
        {
            List<cstPalletDetails> _palletDetail = new List<cstPalletDetails>();
            try
            {
                //  PalletDetail _palletitem = entx3v6.PalletDetails.SingleOrDefault(i => i.BoxNumber == BoxNumber);
                
                var count = from Pallet in entx3v6.PalletDetails
                            where Pallet.BoxNumber == BoxNumber
                            select Pallet;

                foreach (var item in count)
                {
                    cstPalletDetails _pallet = new cstPalletDetails();
                    _pallet.PalletID = (Guid)item.PalletID;
                    _pallet.PalletDetailID = item.PalletDetailID;
                    _pallet.BoxNumber = item.BoxNumber;
                    _pallet.CartonNumber = item.CartonNumber;
                    _pallet.ShipmentNumber = item.ShipmentNumber;
                    _palletDetail.Add(_pallet);

                }

            }
            catch (Exception)
            { }
            return _palletDetail;
        }










        #endregion

        #region PalletInfoBySHNumber
        public List<cstPalletInfo> GetPalletInfoBySHNumber(String SHnumber)
        {
           List<cstPalletInfo> _lspallet = new List<cstPalletInfo>();

           //cstPalletInfo palletInfo = new cstPalletInfo();
            try
            {


                var pallet = entx3v6.ExecuteStoreQuery<cstPalletInfo>(@"select 
                                                                        p.PalletNumber,p.PrintStatus,p.PalletID 
                                                                        from palletinfo p 
                                                                        join
                                                                        PalletDetail pd on p.PalletID = pd.palletID
                                                                        left join packagedetail pkd on pkd.boxnumber = pd.boxnumber
                                                                        where ShipmentNumber='" + SHnumber + "' and ShipmentLocation = '" + cmdLocalFile.ReadString("Location") + "'group by p.PalletNumber,p.PrintStatus,p.PalletID;").ToList();

                               
                foreach (var item in pallet)
                {
                    cstPalletInfo _palletInfo = new cstPalletInfo();
                    _palletInfo.PrintStatus = item.PrintStatus;
                    _palletInfo.PalletNumber = item.PalletNumber;
                    _palletInfo.PalletID = item.PalletID;
                   
                    //_lspallet
                    _lspallet.Add(_palletInfo);
                }
              
            }
            catch (Exception)
            {
            }
            return _lspallet;
        }

        public List<cstPalletInfo> GetPalletInfoBySHNumber1(String SHnumber)
        {
            List<cstPalletInfo> _lspallet = new List<cstPalletInfo>();

            //cstPalletInfo palletInfo = new cstPalletInfo();
            try
            {


                var pallet = entx3v6.ExecuteStoreQuery<cstPalletInfo>(@"select 
                                                                        p.PalletNumber,p.PrintStatus,p.PalletID 
                                                                        from palletinfo p 
                                                                        join
                                                                        PalletDetail pd on p.PalletID = pd.palletID
                                                                        left join packagedetail pkd on pkd.boxnumber = pd.boxnumber
                                                                        where ShipmentNumber='" + SHnumber + "' group by p.PalletNumber,p.PrintStatus,p.PalletID;").ToList();


                foreach (var item in pallet)
                {
                    cstPalletInfo _palletInfo = new cstPalletInfo();
                    _palletInfo.PrintStatus = item.PrintStatus;
                    _palletInfo.PalletNumber = item.PalletNumber;
                    _palletInfo.PalletID = item.PalletID;

                    //_lspallet
                    _lspallet.Add(_palletInfo);
                }

            }
            catch (Exception)
            {
            }
            return _lspallet;
        }



        public void RemoveBoxFromPallet(String Boxnumber)
        {
            try
            {
                PalletDetail tl = entx3v6.PalletDetails.SingleOrDefault(i => i.BoxNumber == Boxnumber);
                entx3v6.PalletDetails.DeleteObject(tl);
                entx3v6.SaveChanges();
            }
            catch (Exception)
            {


            }
        }

        public void DeletePallet(String Palletnumber)
        {
            try
            {

                Guid palletID = entx3v6.PalletInfoes.SingleOrDefault(i => i.PalletNumber == Palletnumber).PalletID;

                var palletDetail = from Detail in entx3v6.PalletDetails
                                   where Detail.PalletID == palletID
                                   select Detail;

                foreach (var item in palletDetail)
                {
                    local_x3v6Entities mm = new local_x3v6Entities();



                    //var tl = from n in mm.PalletDetails
                    //         where n.PalletID == palletID
                    //         select new { n.PalletID };

                   PalletDetail tl = mm.PalletDetails.SingleOrDefault(i => i.PalletDetailID ==item.PalletDetailID);

                   
                        
                        mm.PalletDetails.DeleteObject(tl);
                        mm.SaveChanges();
                                            //mm.PalletDetails.SingleOrDefault(i => i.PalletID == item.PalletID);


                   
                }
                
            }
            catch (Exception)
            {


            }
        }

        public List<cstPalletDetails> GetPalletDetailByPalletNumber(String PalletNumber)
        {
            List<cstPalletDetails> lstPallet = new List<cstPalletDetails>();

            try
            {
                var Boxnumber = from Pall in entx3v6.PalletDetails
                                join pallinfo in entx3v6.PalletInfoes
                                on Pall.PalletID equals pallinfo.PalletID
                                where pallinfo.PalletNumber == PalletNumber
                                select Pall;

                foreach (var item in Boxnumber)
                {
                    cstPalletDetails pal = new cstPalletDetails();
                    pal.BoxNumber = item.BoxNumber;
                    pal.CartonNumber = item.CartonNumber;
                    pal.PrintStatus = (int)item.PrintStatus;
                    pal.PalletID = (Guid)item.PalletID;
                    pal.PalletDetailID = item.PalletDetailID;
                    pal.ShipmentNumber = item.ShipmentNumber;
                    lstPallet.Add(pal);
                }
            }
            catch (Exception)
            {
            }

            return lstPallet;
        }



        public List<cstBoxPallet> GetBoxInforamtionByShipment(String ShipmentNumber)
        {
            List<cstBoxPallet> lstPallet = new List<cstBoxPallet>();

            try
            {
                var Boxnumber = from Pall in entx3v6.PalletInfoes
                                join pallinfo in entx3v6.PalletDetails
                                on Pall.PalletID equals pallinfo.PalletID
                                where pallinfo.ShipmentNumber == ShipmentNumber
                                select new 
                                {
                                    Pall.PalletNumber, 
                                    pallinfo.BoxNumber
                                };

                foreach (var item in Boxnumber)
                {
                    cstBoxPallet pal = new cstBoxPallet();
                    pal.boxnumber = item.BoxNumber;
                    pal.PalletNumber = item.PalletNumber;
                    lstPallet.Add(pal);
                }
            }
            catch (Exception)
            {
            }

            return lstPallet;
        }

        public string GetPalletInfoByBoxnumber(string BoxNumber)
        {
            string Pallet = "";

            try
            {
                var Pall = from info in entx3v6.PalletInfoes
                           join pal in entx3v6.PalletDetails
                           on info.PalletID equals pal.PalletID
                           where pal.BoxNumber == BoxNumber
                           select new { info.PalletNumber };  


                foreach (var item in Pall)
                {
                    //cstPalletInfo _palletInfo = new cstPalletInfo();
                    //_palletInfo.PrintStatus = item.PrintStatus;
                    //_palletInfo.PalletNumber = item.PalletNumber;
                    //Pallet.Add(_palletInfo);

                    Pallet = item.PalletNumber;

                }
            }
            catch (Exception)
            {
            }
            
            return Pallet;
        }


        public List<cstBoxnumberQuantity> GetBoxnumberandQuanity(String PalletNumber)
        {
            List<cstBoxnumberQuantity> _lspallet = new List<cstBoxnumberQuantity>();

            //cstPalletInfo palletInfo = new cstPalletInfo();
            try
            {


                var pallet = entx3v6.ExecuteStoreQuery<cstBoxnumberQuantity>(@"select 
                                                                                pd.BoxNumber, Sum(pkd.SKUQuantity) SKUQuantity
                                                                                from
                                                                                PalletInfo p
                                                                                inner join PalletDetail pd on p.PalletID = pd.PalletID
                                                                                inner join Package pk on pk.ShippingNum = pd.ShipmentNumber
                                                                                inner join PackageDetail pkd on pk.PackingID = pkd.PackingID and pkd.BoxNumber = pd.BoxNumber
                                                                                where p.PalletNumber = '" + PalletNumber + "' group by pd.BoxNumber order by pd.BoxNumber").ToList();


                foreach (var item in pallet)
                {
                    cstBoxnumberQuantity _palletInfo = new cstBoxnumberQuantity();
                    _palletInfo.BoxNumber = item.BoxNumber;
                    _palletInfo.SKUQuantity = item.SKUQuantity;
                   

                    //_lspallet
                    _lspallet.Add(_palletInfo);
                }

            }
            catch (Exception)
            {
            }
            return _lspallet;
        }


        public int GetSumFromPackageDetail(string ShipmentNumber, string ShipmentLocation)
        {
            int count = 0;

            try
            {
                var pallet = entx3v6.ExecuteStoreQuery<int>(@"select sum(SKUQuantity) Quantity from PackageDetail  where PackingID = (select PackingID from Package where ShippingNum = '" + ShipmentNumber + "' and ShipmentLocation = '" + ShipmentLocation + "')");

                foreach (var item in pallet)
                {
                    count = item;
                }

            }
            catch (Exception)
            {
            }

            return count;

        }


        public int GetSumFromShippingView(string ShipmentNumber, string ShipmentLocation)
        {
            int count = 0;

            try
            {
                var pallet = entx3v6.ExecuteStoreQuery<int>(@"SELECT sum(Quantity) Quantity FROM [Shipping Manager].[dbo].[Get_Shipping_Data] where ShipmentID = '" + ShipmentNumber + "' and LineType <> 6 and LocationCombined = '" + ShipmentLocation + "'");

                foreach (var item in pallet)
                {
                    count = item;
                }

            }
            catch (Exception)
            {
            }

            return count;

        }

        public int GetTotalSKUByPalletNUmber(String PalletNumber)
        {
            int total = 0;
            try
            {
               // var pallet = entx3v6.ExecuteStoreQuery<int>(@"select sum(SKUQuantity) as Quantity from Package p inner join PackageDetail pd on p.PackingID = pd.PackingID inner join PalletDetail pld on pd.BoxNumber = pld.BoxNumber inner join PalletInfo pl on pld.PalletID = pl.PalletID where pl.PalletNumber = '" + PalletNumber + "'");

                var pallet = entx3v6.ExecuteStoreQuery<int>(@"select 
                                                                count(distinct pd.BoxNumber) No_of_Boxes
                                                                from 
                                                                Package p 
                                                                inner join PackageDetail pd on p.PackingID = pd.PackingID 
                                                                inner join PalletDetail pld on pd.BoxNumber = pld.BoxNumber 
                                                                inner join PalletInfo pl on pld.PalletID = pl.PalletID 
                                                                where pl.PalletNumber = '" + PalletNumber + "'");


                foreach (var item in pallet)
                {
                    total = item;
                }
            }
            catch (Exception)
            {
            }

            return total;
        }

        public cstPalletInfo GetPalletInfoByPalletnumber(string palletnumber)
        {
            cstPalletInfo infopallet = new cstPalletInfo();

            try
            {
                var info = from Pallet in entx3v6.PalletInfoes
                           where Pallet.PalletNumber == palletnumber
                           select Pallet;


                foreach (var item in info)
                {
                    infopallet.palletCreatedTime = (DateTime)item.palletCreatedTime;
                    infopallet.PalletHeight = (Double)item.PalletHeight;
                    infopallet.PalletID = item.PalletID;
                    infopallet.PalletNumber = item.PalletNumber;
                    infopallet.PalletType = item.PalletType;
                    infopallet.PalletWeight = (Double)item.PalletWeight;
                    infopallet.PalletWidth = (Double)item.PalletWidth;
                    infopallet.PrintStatus = item.PrintStatus;
                    infopallet.RowID = item.RowID;

                    infopallet.PLT_Carrier = item.PLT_Carrier;
                    infopallet.PLT_BOL = item.PLT_BOL;
                    infopallet.PLT_PRO = item.PLT_PRO;

                    infopallet.Location = item.Location;
                    infopallet.PalletEndDateTime = (DateTime)item.PalletEndDateTime;

                    infopallet.SSCC_Code = item.SSCC_Code;

                    //infopallet.BOLCreatedDateTime = (DateTime)item.BOLCreatedDateTime;


                }
            }
            catch (Exception)
            {
            }

            return infopallet;
        }


        public string GetSHNumberByBoxnumber(string BoxNumber)
        {
            string SHnumber = "";

            try
            {
                var Pack = from info in entx3v6.Packages
                           join pal in entx3v6.PackageDetails
                           on info.PackingId equals pal.PackingId
                           where pal.BoxNumber == BoxNumber
                           select new { info.ShippingNum };


                foreach (var item in Pack)
                {
                    //cstPalletInfo _palletInfo = new cstPalletInfo();
                    //_palletInfo.PrintStatus = item.PrintStatus;
                    //_palletInfo.PalletNumber = item.PalletNumber;
                    //Pallet.Add(_palletInfo);

                    SHnumber = item.ShippingNum;

                }
            }
            catch (Exception)
            {
            }

            return SHnumber;
        }





        #endregion





    }
}
