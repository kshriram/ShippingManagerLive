using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.Commands;
using System.Threading;

namespace PackingClassLibrary.BusinessLogic
{
    /// <summary>
    /// Author : Avinash
    /// Version: Alfa.
    /// Model shipment calss.
    /// </summary>
    public class model_Shipment
    {
        #region Declarations

        public static String ShipmentNumber { get; set; }
        public  Boolean IsShipmentLocationValidInSage { get; set; }
        public Boolean IsShipmentValidated { get; set; }
        public Boolean IsMultiLocation { get; set; }
        public Boolean IsAlreadyPacked { get; set; }
        public List<String> PackedLocations { get; set; }
        public Boolean CanOverride { get; set; }
        public List<cstUserMasterTbl> UserInfo_ShipmentPackedBy { get; set; }
        public List<cstPackageTbl> PackingInfo { get; set; }
        public List<cstShipment> ShipmentDetailSage { get; set; }

        /// <summary>
        /// Shipment Fucnctions .
        /// </summary>
        public cmdShipment ShipmentFucntions = new cmdShipment();
        #endregion

        /// <summary>
        /// Model Constructor(Default assigns default values of class.)
        /// </summary>
        public model_Shipment() { }

        /// <summary>
        /// Model Constructor with parameter
        /// </summary>
        /// <param name="ShipmentNo">String Shiment number information set</param>
        public model_Shipment(string ShipmentNo)
        {
            ShipmentNumber = ShipmentNo;
            SetShipmentInformatio();
        }

        /// <summary>
        /// Set all information in constructor.
        /// </summary>
        private void SetShipmentInformatio()
        {
           
            cmdShipment _cmdSageShipment = new cmdShipment();
            cmdPackage _cmdPackingList = new cmdPackage();

            //Sage Shipment Information
            ShipmentDetailSage = _cmdSageShipment.GetShipmentInfoFromSage( ShipmentNumber, true);

            //Shipment Valid or not
            if (ShipmentDetailSage.Count > 0 || ShipmentDetailSage != null)
            {
                IsShipmentLocationValidInSage = true;

                #region Check Multilocation.
                IsMultiLocation = false;
                var GroupLocation = from sage in ShipmentDetailSage
                                    group sage by sage.Location into Gsage
                                    select Gsage;
                if (GroupLocation.Count() > 1)
                {
                    IsMultiLocation = true;
                }
                #endregion
            }
            else
            {
                IsShipmentLocationValidInSage = false;
            }

            //Set packing Detail if Packed
            PackingInfo = _cmdPackingList.ExecuteNoLocation(ShipmentNumber);
           
            if (PackingInfo == null || PackingInfo.Count <= 0)
            {
                //Remove this after checking
                IsAlreadyPacked = false;
            }
            else if (PackingInfo.Count>0)
            {
                IsAlreadyPacked = true;
                //new location List
                List<string> lsLocations = new List<string>();
                //Find Diffrent location and add to paked locations.
                var _packedLocations = from _packed in PackingInfo
                                      group _packed by _packed.ShipmentLocation into Gpacked
                                      select Gpacked;

                foreach (var _Packeditem in _packedLocations)
                {
                    lsLocations.Add(_Packeditem.Key.ToString());
                }
                 //Packed Locations 
                PackedLocations = lsLocations;

                //If packed then User Information set;
                SetCanOverrides();
            }

            //Validated Check
            //IsShipmentValidated = _cmdSageShipment.IsShipmentValidated(ShipmentNumber);
            IsShipmentValidated = true;
        }

        /// <summary>
        /// User Information and CanOverride set;
        /// </summary>
        private void SetCanOverrides()
        {
            cmdUser _UserCall = new cmdUser();
            CanOverride = true;
            List<cstUserMasterTbl> userInfo = new List<cstUserMasterTbl>();
           
            foreach (cstPackageTbl _packItem in PackingInfo)
            {
                userInfo.Add(_UserCall.GetUserMaster(_packItem.UserID)[0]);

                if (_packItem.MangerOverride == 1 || _packItem.MangerOverride == 2)
                {
                    CanOverride = false;
                    //UserInfo_ShipmentPackedBy = _UserCall.GetUserMaster().SingleOrDefault(i => i.UserID == _packItem.UserID);
                }
            }
            UserInfo_ShipmentPackedBy = userInfo;
        }

        

    }
}
