using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdCartonInfo
    {

        local_x3v6Entities entx3v6 = new local_x3v6Entities();

        #region Get Fucntions
        public List<cstCartonInfo> GetAll()
        {
            List<cstCartonInfo> lsreturn = new List<cstCartonInfo>();
            try
            {
                var allBoxInfo = from b in entx3v6.CartonInfoes
                                 select b;

                foreach (var _caritem in allBoxInfo)
                {
                    cstCartonInfo _carton = new cstCartonInfo();
                    _carton.CartonID = _caritem.CartonID;
                    _carton.BOXNumber = _caritem.BOXNumber;
                    _carton.CartonNumber = _caritem.CartonNumber;
                    _carton.ShipmentNumber = _caritem.ShipmentNumber;
                    _carton.Printed = _caritem.Printed;
                    lsreturn.Add(_carton);
                }
            }
            catch (Exception)
            { }
            return lsreturn;

        }

        public List<cstCartonInfo> GetByBoxNumber(String BoxNumber)
        {
            List<cstCartonInfo> lsreturn = new List<cstCartonInfo>();
            try
            {
                var allBoxInfo = from b in entx3v6.CartonInfoes
                                 where b.BOXNumber == BoxNumber
                                 select new 
                                 {
                                     b.BOXNumber,
                                     b.CartonID,
                                     b.CartonNumber,
                                     b.Printed,
                                     b.ShipmentNumber

                                 };

                foreach (var _caritem in allBoxInfo)
                {
                    cstCartonInfo _carton = new cstCartonInfo();
                    _carton.CartonID = _caritem.CartonID;
                    _carton.BOXNumber = _caritem.BOXNumber;
                    _carton.CartonNumber = _caritem.CartonNumber;
                    _carton.ShipmentNumber = _caritem.ShipmentNumber;
                    _carton.Printed = _caritem.Printed;
                    lsreturn.Add(_carton);
                }
            }
            catch (Exception)
            { }
            return lsreturn;

        }

        public cstCartonInfo GetByCaronID(Guid CartonID)
        {
            cstCartonInfo lsreturn = new cstCartonInfo();
            try
            {
                CartonInfo allBoxInfo = entx3v6.CartonInfoes.FirstOrDefault(i => i.CartonID == CartonID);

                lsreturn.CartonID = allBoxInfo.CartonID;
                lsreturn.BOXNumber = allBoxInfo.BOXNumber;
                lsreturn.CartonNumber = allBoxInfo.CartonNumber;
                lsreturn.ShipmentNumber= allBoxInfo.ShipmentNumber;
                lsreturn.Printed = allBoxInfo.Printed;
            }
            catch (Exception)
            { }
            return lsreturn;

        }

        public List<cstCartonInfo> GetCartonByShipmentNumber(string ShipmentNumber)
        {
            List<cstCartonInfo> lsreturn = new List<cstCartonInfo>();
            try
            {
                var allBoxInfo = from b in entx3v6.CartonInfoes
                                 where b.ShipmentNumber == ShipmentNumber 
                                 select b;

                foreach (var _caritem in allBoxInfo)
                {
                    cstCartonInfo _carton = new cstCartonInfo();
                    _carton.CartonID = _caritem.CartonID;
                    _carton.BOXNumber = _caritem.BOXNumber;
                    _carton.CartonNumber = _caritem.CartonNumber;
                    _carton.ShipmentNumber = _caritem.ShipmentNumber;
                    _carton.Printed = _caritem.Printed;
                    lsreturn.Add(_carton);
                }
            }
            catch (Exception)
            { }
            return lsreturn;

        }
        public List<cstCartonInfo> GetCartonByShipmentNumberAndLocation(string ShipmentNumber,String Location)
        {
            List<cstCartonInfo> lsreturn = new List<cstCartonInfo>();
            try
            {
                //var allBoxInfo = from b in entx3v6.CartonInfoes
                //                 where b.ShipmentNumber == ShipmentNumber && b.loca
                //                 select b;

                var allBoxInfo = entx3v6.ExecuteStoreQuery<cstCartonInfo>(@"select distinct c.BoxNumber,c.ShipmentNUmber,c.cartonNumber,c.CartonID,c.Printed from CartonInfo c inner join (select
                                                                            distinct pd.BoxNumber,p.ShippingNum,  pd.ShipmentLocation
                                                                            from
                                                                            Package p inner join
                                                                            PackageDetail pd on p.PackingID = pd.PackingID
                                                                            ) f
                                                                            on f.BoxNumber=c.BoxNumber where f.ShippingNum = '" + ShipmentNumber + "' and f.ShipmentLocation = '" + Location + "'").ToList();



                foreach (var _caritem in allBoxInfo)
                {
                    cstCartonInfo _carton = new cstCartonInfo();
                    _carton.CartonID = _caritem.CartonID;
                    _carton.BOXNumber = _caritem.BOXNumber;
                    _carton.CartonNumber = _caritem.CartonNumber;
                    _carton.ShipmentNumber = _caritem.ShipmentNumber;
                    _carton.Printed = _caritem.Printed;
                    lsreturn.Add(_carton);
                }
            }
            catch (Exception)
            { }
            return lsreturn;

        }


        #endregion

        #region Set Functions
        /// <summary>
        /// Save Box information to the database
        /// </summary>
        /// <param name="lscartonInfo">list of information of box</param>
        /// <returns>Guid of New Box Id</returns>
        public Guid SaveCartonInfo(cstCartonInfo lscartonInfo)
        {
            Guid _return = Guid.Empty;

            try
            {
                CartonInfo _caton = new CartonInfo();
                _caton.CartonID = lscartonInfo.CartonID;
                _caton.BOXNumber = lscartonInfo.BOXNumber;
                _caton.CartonNumber= lscartonInfo.CartonNumber;
                _caton.ShipmentNumber = lscartonInfo.ShipmentNumber;
                _caton.Printed = lscartonInfo.Printed;
                entx3v6.AddToCartonInfoes(_caton);

                entx3v6.SaveChanges();
                _return = _caton.CartonID;
            }
            catch (Exception)
            { }
            return _return;
        }

        public Guid UpdateCartonInfo(cstCartonInfo lscartonInfo)
        {
            Guid _return = Guid.Empty;

            try
            {
                CartonInfo _caton = entx3v6.CartonInfoes.SingleOrDefault(i => i.BOXNumber == lscartonInfo.BOXNumber);
                _caton.Printed = lscartonInfo.Printed;
                entx3v6.SaveChanges();
                _return = _caton.CartonID;
            }
            catch (Exception)
            { }
            return _return;
        }
        #endregion

        public Boolean DeleteCartonByShippingNumber(String ShippingNumber)
        {
            Boolean _return = false;
            try
            {
                var lscartonInfo = (from ls in entx3v6.CartonInfoes
                                    where ls.ShipmentNumber == ShippingNumber
                                    select ls).ToList();
                foreach (var item in lscartonInfo)
                {
                    CartonInfo crton = (CartonInfo)item;
                    entx3v6.DeleteObject(crton);
                }
                entx3v6.SaveChanges();
            }
            catch (Exception)
            {
            }

            return _return;
        }
        public Boolean DeleteCartonByBoxNumber(String ShippingNumber, String BoxNum)
        {
            Boolean _return = false;
            try
            {
                var lscartonInfo = (from ls in entx3v6.CartonInfoes
                                    where ls.ShipmentNumber == ShippingNumber
                                    where ls.BOXNumber==BoxNum
                                    select ls).ToList();
                foreach (var item in lscartonInfo)
                {
                    CartonInfo crton = (CartonInfo)item;
                    entx3v6.DeleteObject(crton);
                }
                entx3v6.SaveChanges();
            }
            catch (Exception)
            {
            }

            return _return;
        }
        //public void ss()
        //{
        //   var dd= from  PackageDetail in entx3v6.PackageDetails
        //           from Package in entx3v6.Packages
        //           where 
        //}
    }
}
