using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.Commands;
using PackingClassLibrary.Commands.ReportCommands;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.Commands.SMcommands;

namespace PackingClassLibrary
{
    public class ReportController
    {
        #region Shipping table Processing
        /// <summary>
        /// Get all Shipping ids information with Delivery provider name
        /// </summary>
        /// <returns>List<cstShippingInfoBPName></returns>
        public List<cstShippingInfoBPName> GetBpinfoOFShippingNum()
        {
            cmdBPNameShippingNum command = new cmdBPNameShippingNum();
            return command.GetBpinfoOFShippingNum();
        }

        /// <summary>
        /// get Delivery provider Name from the number
        /// </summary>
        /// <param name="BPCNUM">Strign Delivery provider Number</param>
        /// <returns>String Delivery provider Name</returns>
        public String GetDeliveryProviderNameFromBPCNUM(String BPCNUM)
        {
            cmdBPNameShippingNum command = new cmdBPNameShippingNum();
            return command.getBPNameFromBPNUM(BPCNUM);
        }
        #endregion

        #region Total Shipment Packed By user Per date
        /// <summary>
        /// User packed total shipment count per day per user
        /// </summary>
        /// <returns>List of cstUserShipmentCount</returns>
        public List<cstUserShipmentCount> GetUserTotalPakedPerDay()
        {
            cmdUserShipmentCount command = new cmdUserShipmentCount();
            return command.GetAllShipmentCountByUser();
        }

        #endregion

        /// <summary>
        /// Shipment number serch for information of packing status
        /// </summary>
        /// <param name="ShippingNumber">String Shipping Number</param>
        /// <returns>List<cstShipmentNumStatus> depending on location retuersn shipping number information</returns>
        #region Shipping Number with status and location

        public List<cstShipmentNumStatus> GetShippingStatus(String ShippingNumber)
        {
            cmdShippinNumStatus command = new cmdShippinNumStatus();
            return command.GetStaus(ShippingNumber);
        }

        #endregion

        #region Station Total Packed and Unpacked.
        /// <summary>
        /// Total Shipment packed per station and under packing Shipments per station
        /// </summary>
        /// <returns>List<cstStationToatlPacked>  information</returns>
        public List<cstStationToatlPacked> GetStationTotalPaked()
        {
            cmdStationTotalPacked command = new cmdStationTotalPacked();
            return command.GetEachStationPacked();
        }

        /// <summary>
        /// For Station Dashboard screen
        /// </summary>
        /// <param name="DatetimeNow"></param>
        /// <returns></returns>
        public List<cstDashBoardStion> GetStationDashboard(DateTime DatetimeNow)
        {
            cmdStationTotalPacked command = new cmdStationTotalPacked();
            return command.GetStationByReport(DatetimeNow);
        }

        /// <summary>
        /// Total Shipment packed per station and under packing Shipments per station on the given date
        /// </summary>
        /// <param name="ReportDate">Date Time Report Date</param>
        /// <returns>List<cstStationToatlPacked></returns>
        public List<cstStationToatlPacked> GetStationTotalPaked(DateTime ReportDate)
        {
            cmdStationTotalPacked command = new cmdStationTotalPacked();
            return command.GetEachStationPacked(ReportDate);
        }

        /// <summary>
        /// Station Total Packed Shipment Today by staion Name
        /// </summary>
        /// <param name="StationName">
        /// String Staion name.
        /// </param>
        /// <returns>
        /// inter total packed Shipment Count. else 0.
        /// </returns>
        public int TotalPackedTodayByStationID(String StationName)
        {
            cmdStationTotalPacked cmd = new cmdStationTotalPacked();
            return cmd.PackedTodayByStationID(StationName);
        }


        public String GetShippingNumByStation(String StationName)
        {
            cmdStationTotalPacked cmd = new cmdStationTotalPacked();
            return cmd.UnderPackingID(StationName);
        }
        #endregion

        public Guid GetBoxID(string Boxnumber)
        {
            cmdBox command = new cmdBox();
            return command.GetBoxIDByBoxNumber(Boxnumber);
        }

        public List<cstPackageDetails> GetPackingDetailsByBoxNumForReport(String ShipNumber)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetPackingDetailsByShipmentNumber(ShipNumber);
        }
        public List<PaaletDetail> GetAllShipmentAndPallet()
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPallet();
        }

        public List<PaaletDetail> GetAllShipmentAndPalletPerStationRC(Guid StationID)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPalletPerStation(StationID);
        }

        public List<PaaletDetail> GetAllShipmentAndPalletByShipmentRC(String ShipmentNumber)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPalletByShipment(ShipmentNumber);
        }

        public List<PaaletDetail> GetAllShipmentAndPalletByPalletRC(String PalletNumber)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPalletByPallet(PalletNumber);
        }

        public List<PaaletDetail> GetAllShipmentAndPalletByPalletWithStationRC(String PalletNumber, Guid StationID)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPalletByPalletWithStation(PalletNumber, StationID);
        }

        public List<PaaletDetail> GetAllShipmentAndPalletByShipmentWithStationRC(String ShipmentNumber, Guid StationID)
        {
            cmdPakingDetails package = new cmdPakingDetails();
            return package.GetAllShipmentAndPalletByShipmentWithStation(ShipmentNumber, StationID);
        }

    }
}
