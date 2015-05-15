using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary;
using PackingClassLibrary.Commands;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.Commands.ReportCommands;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.BusinessLogic;
using PackingClassLibrary.Models;

namespace PackingClassLibrary
{
    /// <summary>
    /// Avinash
    /// Main Librabry class that contains all menthod declared throuth all classses.
    /// </summary>
    public class smController
    {

        /// <summary>
        /// Static Class that Return That columsn View Need
        /// </summary>
        /// <param name="ShippingNum">String Shipping Number</param>
        /// <param name="SKU">String SKU Name</param>
        /// <returns>cstViewExtraColumns</returns>
        public cstViewExtraColumns GetViewColumnInfo(string ShippingNum, String SKU)
        {
           return cmdViewExtra.GetExtraColumns(ShippingNum, SKU);
        }
        
        #region Tracking Functions

        /// <summary>
        /// All Table Withaout any filter
        /// </summary>
        /// <returns>List<cstTrackingTbl>  Inforamtion</returns>
        public List<cstTrackingTbl> GetTrackingTbl()
        {
            cmdTracking command = new cmdTracking();
            return command.GetTrackTbl();
        }

        /// <summary>
        /// Tracking Table information With filter Packing ID
        /// </summary>
        /// <param name="PackingID">Guid Packing ID</param>
        /// <returns>List<cstTrackingTbl>  Inforamtion</returns>
        public List<cstTrackingTbl> GetTrackingTbl(Guid PackingID)
        {
            cmdTracking command = new cmdTracking();
            return command.GetTrackTbl(PackingID);
        }

        /// <summary>
        /// get the information from tracking Table about Box number.
        /// </summary>
        /// <param name="BoxNumber">
        /// String Box Number.
        /// </param>
        /// <returns>
        /// list Of cstTrackingTbl filtered by Box number.
        /// </returns>
        public List<cstTrackingTbl> GetTrackingTbl(String  BoxNumber)
        {
            cmdTracking command = new cmdTracking();
            return command.GetTrackTbl(BoxNumber);
        }

        /// <summary>
        /// Tracking Table information With filter Packing ID and Shipping ID
        /// </summary>
        /// <param name="PackingID">Guid Packing Table ID</param>
        /// <param name="ShippingID">Guid Shipping Tabel ID</param>
        /// <returns> List<cstTrackingTbl> Information</returns>
        public List<cstTrackingTbl> GetTrackingTbl(Guid PackingID, Guid ShipppingID)
        {
            cmdTracking command = new cmdTracking();
            return command.GetTrackTbl(PackingID,ShipppingID);
        }

        /// <summary>
        /// Get Tracking Table information by Tracking Number
        /// </summary>
        /// <param name="TrackingNumber">
        /// String Tracking Number.
        /// </param>
        /// <returns>
        /// CstTrackingTbl information Object.
        /// </returns>
        public cstTrackingTbl GetTrackingTblByTrackingNumber(String TrackingNumber)
        {
            cmdTracking Command  = new cmdTracking();
            return Command.GetTrackingByTrackingNumber(TrackingNumber);
        }

        /// <summary>
        /// Check that box has tracking number or not
        /// </summary>
        /// <param name="BoxNum"></param>
        /// <returns></returns>
        public String IsTrackingNum(string BoxNum)
        {
            cmdTracking _tracking = new cmdTracking();
            return _tracking.IschecckTrackingNumberPresent(BoxNum);

        }

        /// <summary>
        /// Tracking Table information With filter Packing ID and Shipping ID
        /// </summary>
        /// <param name="ShippingID"> Shipping Table Guid</param>
        /// <returns> List<cstTrackingTbl> Information</returns>
        public List<cstTrackingTbl> GetTrackingTblShippingID(Guid ShippingID)
        {
            cmdTracking command = new cmdTracking();
            return command.GetTrackTblShippingIDOnly(ShippingID);
        }

        /// <summary>
        /// Update Table Tracking For Ready to export flag.
        /// </summary>
        /// <param name="TrackingNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Flag_ReadyTOExport">
        /// Booelan Value
        /// </param>
        /// <returns></returns>
        public Boolean UpdateTrackingReadyTOExport(String TrackingNumber, String BoxNumber, Boolean Flag_ReadyTOExport)
        {
            cmdTracking command = new cmdTracking();
           return command.UpdateReadyTOExport(TrackingNumber, BoxNumber, Flag_ReadyTOExport);
        }

        #endregion

        #region Shipment Functions
        /// <summary>
        /// shipment model object with all informatin instance 
        /// </summary>
        /// <param name="ShipmentNumber">String Shipment number</param>
        /// <returns>Object of shipment Model (mShipment)</returns>
        public model_Shipment getModelShipment(String ShipmentNumber)
        {
            model_Shipment _command = new model_Shipment(ShipmentNumber);
            return _command;
        }

        public List<cstPalletInfo> GetPalletInfoBySHNumber1(String SHNUmber)
        {
            cmdPallet _Pallet = new cmdPallet();
            return _Pallet.GetPalletInfoBySHNumber1(SHNUmber);
        }

        public String GetSHNumberByBoxnumber1(String BoxNUmber)
        {
            cmdPallet _Pallet = new cmdPallet();
            return _Pallet.GetSHNumberByBoxnumber(BoxNUmber);
        }

        public void GetSHNumberByBoxnumber1(String SSCC_code, string PalletNumber)
        {
            cmdPallet _Pallet = new cmdPallet();
            _Pallet.UpdateSSCCNUmberInfo(SSCC_code, PalletNumber);
        }





        /// <summary>
        /// This filter the shipment from sage with default location set in flat file
        /// </summary>
        /// <param name="ShippingNumber">String Shipment Number</param>
        /// <returns>List<cstShipment></returns>
        public List<cstShipment> GetShipment_SPCKD(string ShippingNumber)
        {
            cmdShipment command = new cmdShipment();
            return command.GetShipmentInfoFromSage(ShippingNumber);
        }

        public Guid GetPackingNum1(String ShippingNumber, string Location)
        {
            return cmdPackage.GetPackingNum1(ShippingNumber, Location);
        }

        public List<cstPackageDetails> GetShipmentInfo1(Guid PackingID, string location)
        {
            cmdShippingInfo com1 = new cmdShippingInfo();

            return com1.GetShipmentInfoByPackageID(PackingID, location);
        }

        public List<cstShippingInfo> GetShipmentInfo12(String ShipmentNumber, string location)
        {
            cmdShippingInfo com1 = new cmdShippingInfo();

            return com1.GetShipmentInfoByShipment(ShipmentNumber, location);
        }

        /// <summary>
        /// Get shipment number information without location filter
        /// </summary>
        /// <param name="ShippingNumber">Shipment number</param>
        /// <param name="NullLocation"> Booean must be always true</param>
        /// <returns></returns>
        public List<cstShipment> GetShipment_SPCKD(string ShippingNumber, Boolean NullLocation)
        {
            cmdShipment command = new cmdShipment();
            //return command.GetShipmentInfoFromSage(ShippingNumber);
            return command.GetShipmentInfoFromSage(ShippingNumber, NullLocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PackingID"></param>
        /// <returns></returns>
        public List<cstPackageDetails> GetPackingDetailsByBOX(Guid PackingID)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetailsByBOX(PackingID);
        }

        /// <summary>
        /// Delte shipment Information from packingDetails table and Packing table
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>If sucess then true else false</returns>
        public Boolean DeleteShipmentAll(Guid PackingID)
        {
            cmdShipment command = new cmdShipment();
            return command.DeleteShipment(PackingID);
        }

        #endregion

        #region User Functions

        /// <summary>
        /// user information Properites model
        /// </summary>
        /// <param name="UserID">Guid User id </param>
        /// <returns>mUser Model object</returns>
        public model_User getModelUser(Guid UserID)
        {
            model_User _command = new model_User(UserID);
            return _command;
        }

        /// <summary>
        /// Blank Object Of User Model.
        /// </summary>
        /// <returns> model object instance.</returns>
        public model_User getModelUser()
        {
            model_User _command = new model_User();
            return _command;
        }

             /// <summary>
        /// get all information from the UserMaster table and return it as a list of userInformation_CustomEntity
        /// </summary>
        /// <returns>list of user Information Table rows</returns>
        public List<cstUserMasterTbl> GetUserInfoList()
        {
            cmdUser command = new cmdUser();
            return command.GetUserMaster();
        }

        /// <summary>
        /// UserMaster Save Command .
        /// </summary>
        /// <param name="lsUserInformation">list Of User Information.</param>
        /// <returns>Boolen value True is Save Success else False.</returns>
        public Boolean SetUserMaster(List<cstUserMasterTbl> lsUserInformation)
        {
            cmdUser command = new cmdUser();
            return command.SetUserMaster(lsUserInformation);
        }

        /// <summary>
        /// User Master Update command
        /// Override method to Execute 
        /// </summary>
        /// <param name="lsUserInformation">list of user information to be updated.</param>
        /// <param name="UserID">User ID whose Information been updated.</param>
        /// <returns>Boolen Value True if updation successful else fail</returns>
        public Boolean SetUserMaster(List<cstUserMasterTbl> lsUserInformation, Guid UserID)
        {
            cmdUser command = new cmdUser();
            return command.SetUserMaster(lsUserInformation, UserID);
        }

        /// <summary>
        /// Selected User Information from user Master Table.
        /// (Login Screen)
        /// </summary>
        /// <param name="UserName">Which User Information Requested</param>
        /// <returns>List OF User Information Values.</returns>
        public List<cstUserMasterTbl> GetSelcetedUserMaster(String UserName)
        {
            cmdUser command = new cmdUser();
            return command.GetUserMaster(UserName);
        }

        /// <summary>
        /// Search the user information in UserMaster Table
        /// </summary>
        /// <param name="UserID">long</param>
        /// <returns>list of cstUserMaster</returns>
        public List<cstUserMasterTbl> GetSelcetedUserMaster(Guid UserID)
        {
            cmdUser command = new cmdUser();
            return command.GetUserMaster(UserID);
        }
        
        #endregion

        #region Shipping Functions

        /// <summary>
        /// Upsert information to the shipping table
        /// </summary>
        /// <param name="lsShippingInfo">cstShippingTbl list of information</param>
        /// <returns>Boean value</returns>
        public Boolean SetShippingTbl(List<cstShippingTbl> lsShippingInfo)
        {
            cmdShipping commnad = new cmdShipping();
            return commnad.SaveShippingTbl(lsShippingInfo);
        }

        /// <summary>
        /// Shipping Table By SHippingNumber.
        /// </summary>
        /// <param name="ShippingNum">String Shipping Number</param>
        /// <returns>cstShippingTbl</returns>
        public cstShippingTbl GetShippingTbl(String ShippingNum)
        {
            cmdShipping commnad = new cmdShipping();
            return commnad.GetShippingByShippingNumber(ShippingNum);
        }

        /// <summary>
        /// Get Shipping table required information from the *sage database
        /// </summary>
        /// <param name="ShippingNumber">String Shipping Number </param>
        /// <returns>List of cstShippingTbl that contints the shipping information of passed string Shipping Number</returns>
        public List<cstShippingTbl> GetShippingInfoFromSage(String ShippingNumber, string flag)
        {
            cmdSageOperations command = new cmdSageOperations();
            return command.getShippingInfo(ShippingNumber, flag);
        }

        /// <summary>
        /// Shipping table All data;
        /// </summary>
        /// <returns></returns>
        public List<cstShippingTbl> GetShippingTbl()
        {
            cmdShipping command = new cmdShipping();
            return command.GetShipping();
        }
        #endregion

        public List<cstShippingInfo> GetShipmentInfo(String ShippingNumber)
        {
            cmdShippingInfo com = new cmdShippingInfo();
            return com.GetShipmentInfoBySHNumber(ShippingNumber);
        }

        #region package Functions
        /// <summary>
        /// Package information model object
        /// </summary>
        /// <param name="PackingID">Guid packing id</param>
        /// <returns></returns>
        public model_Packing getModelPackage(Guid PackingID)
        {
            model_Packing _command = new model_Packing(PackingID);
            return _command;
        }

        /// <summary>
        /// Avinash
        /// This method call the function that set values to the database of table Packing
        /// </summary>
        /// <param name="lsPackingTbl">list of PakingCustom entity class</param>
        /// <returns>String value the Fail if save or update fail else Success returened</returns>
        public Guid SetPackingTable(List<cstPackageTbl> lsPackingTbl)
        {
            cmdPackage command = new cmdPackage();
            return command.setPacking(lsPackingTbl);
        }

        /// <summary>
        /// Update the packing table data
        /// </summary>
        /// <param name="lsPackingTbl">list of paking table custom entity</param>
        /// <param name="ShippingNumber">String </param>
        /// <param name="Location">String (clGlobal.LocationName)</param>
        /// <returns>String success or Fail</returns>
        public String SetPackingTable(List<cstPackageTbl> lsPackingTbl, Guid ShippingNumber)
        {
            cmdPackage command = new cmdPackage();
            return command.setPacking(lsPackingTbl, ShippingNumber);
        }

        /// <summary>
        /// Avinash
        /// delete shipment Information from Packing and PackingDetails tables.
        /// </summary>
        /// <param name="ShipmentID">Shipmenet ID whose Information to be rollback</param>
        /// <returns>true or False</returns>
        public Boolean RollBackPakingMaster(String ShipmentID)
        {
            cmdPackage command = new cmdPackage();
            return command.RollBack(ShipmentID);
        }

        /// <summary>
        /// Get packing GUID from PCKROWID
        /// </summary>
        /// <param name="PCKROWID">
        /// String PCKROWID
        /// </param>
        /// <returns>
        /// GUID PackingID
        /// </returns>
        public Guid GetPackageIDFromROWID(String PCKROWID)
        {
            cmdPackage command = new cmdPackage();
            return command.GetPackingID(PCKROWID);
        }

        /// <summary>
        /// get all information from local database about PackingID from Packing Table.
        /// </summary>
        /// <param name="ShippingNumber">String PackingID</param>
        /// <param name="Location">Location of packed Shipment</param>
        /// <returns>list of PackingCustom a</returns>
        public List<cstPackageTbl> GetPackingList(string ShippingNumber, String Location)
        {
            cmdPackage command = new cmdPackage();
            return command.Execute(ShippingNumber, Location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShippingNum">String PackingID</param>
        /// <param name="Location">Location of packed Shipment</param>
        /// <param name="MangerOverride">int Manager Override flag</param>
        /// <returns></returns>
        public List<cstPackageTbl> GetPackingList(string ShippingNum, String Location, int MangerOverride)
        {
            cmdPackage command = new cmdPackage();
            return command.Execute(ShippingNum, Location, MangerOverride);
        }

        /// <summary>
        /// Pakage Table Row with packing ID
        /// </summary>
        /// <param name="PackingID">Guid</param>
       ///<param name="Flag">nothing User search by packing ID only</param>
        /// <returns>List<cstPackingTbl></returns>
        public cstPackageTbl GetPackingList(Guid PackingID, Boolean Flag)
        {
            cmdPackage command = new cmdPackage();
            return command.ExecutePacking(PackingID);
        }
        /// <summary>
        /// get all information from local database about PackingID from Packing Table.
        /// </summary>
        /// <param name="ShippingNumber">String PackingID</param>
        /// <returns>list of PackingCustom a</returns>
        public List<cstPackageTbl> GetPackingListByShippingNumber(string ShippingNumber)
        {
            cmdPackage Command = new cmdPackage();
            return Command.ExecuteNoLocation(ShippingNumber);
        }

        /// <summary>
        /// Packing Tabel With all rows
        /// </summary>
        /// <returns>List<cstPackingTbl>list<cstpackingTbl></returns>
        public List<cstPackageTbl> GetPackingTbl()
        {
            cmdPackage command = new cmdPackage();
            return command.Execute();
        }

        /// <summary>
        /// Overload method with parameter Long UserId
        /// </summary>
        /// <param name="UserID">long UserID</param>
        /// <returns>List<cstPackingTbl>list<cstpackingTbl></returns>
        public List<cstPackageTbl> GetPackingTbl(Guid UserID)
        {
            cmdPackage command = new cmdPackage();
            return command.Execute(UserID);
        }

        /// <summary>
        /// Overload wiht long UserID and DateTime
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="Date"> Datetime Date</param>
        /// <returns>List<cstPackingTbl>list<cstpackingTbl></returns>
        public List<cstPackageTbl> GetPackingTbl(Guid UserID, DateTime Date)
        {
            cmdPackage command = new cmdPackage();
            return command.Execute(UserID, Date);
        }

        /// <summary>
        /// get information from packag table
        /// </summary>
        /// <param name="PackingID">Guid PackingID</param>
        /// <returns>String ShippingNum</returns>
        public String GetShippingNumber(Guid PackingID)
        {
            return cmdPackage.GetShippingNum(PackingID);
        }

        /// <summary>
        /// get information from packag table
        /// </summary>
        /// <param name="ShippingNumber">String Shippingn Number</param>
        /// <returns>List of Guid That indicates the PackingID</returns>
        public List<Guid> GetPackingNum(String ShippingNumber)
        {
            return cmdPackage.GetPackingNum(ShippingNumber);
        }

        /// <summary>
        /// Get Infromation from Package table 
        /// </summary>
        /// <param name="ShippingNumber">String Shipment Number</param>
        /// <param name="OverrideMode">int manager Override</param>
        /// <param name="Location">String Location</param>
        /// <returns>Guid as a packing</returns>
        public Guid GetPackingNum(String ShippingNumber, int OverrideMode, String Location)
        {
            return cmdPackage.GetPackageDI(ShippingNumber, OverrideMode, Location);
        }
        #endregion

        /// <summary>
        /// Convert the inpute string to language maintioned in the file 
        /// defult value is the same input string passed.
        /// </summary>
        /// <param name="InputString">String to convert</param>
        /// <param name="FileName">File from convert</param>
        /// <returns>Converted string or default same string</returns>
        public string ConvetLanguage(String InputString, String FileName)
        {
           return cmdLanguageTranslator.Convert(InputString, FileName);

        }
        
        /// <summary>
        /// Avinash
        /// Save values to the Packing Detail table
        /// </summary>
        /// <param name="lsPackingDetails">list of packing information</param>
        /// <returns>Success if transaction done Else Fail</returns>
        public string SetPackingDetails(List<cstPackageDetails> lsPackingDetails)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.savePackageDetails(lsPackingDetails);
        }
        public string UpdatePackingDetails(List<cstPackageDetails> lsPackingDetails)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.UpdatePackageDetails(lsPackingDetails);
        }
        
        /// <summary>
        /// Avinash 
        /// Returns the maximun Id Of the Packing Table
        /// </summary>
        /// <returns>String that represents the Shipment ID</returns>
        public string GetMaxShipmentID()
        {
            cmdPackage command = new cmdPackage();
           return command.getMaxPackageID();
        }
       
        /// <summary>
        /// Fing the image url from SkuUrl table 
        /// </summary>
        /// <param name="SKuName">String SKU Name</param>
        /// <returns>String URL path</returns>
        public string GetSKuUrl(String SKuName)
        {
            cmdSkuImages command = new cmdSkuImages();
            return command.getUrlByName(SKuName);
        }

        /// <summary>
        /// Show Barcode For applied SKU or not flag
        /// </summary>
        /// <param name="SkuName">
        /// String SKU name to be check
        /// </param>
        /// <returns>
        /// Boolean value from Databse Table SKUImages.
        /// </returns>
        public Boolean IsSKUShowBarcode(string SkuName)
        {
            cmdSkuImages command = new cmdSkuImages();
            return command.getBarcodeShowFlag(SkuName);
        }

        /// <summary>
        /// Total packed shipment count
        /// </summary>
        /// <param name="userId">long User ID </param>
        /// <returns>list of valur pair veriable</returns>
        public List<KeyValuePair<string, long>> GetTotalToday(Guid userId)
        {
            cmdUser command = new cmdUser();
            return command.GetTotalShipmentsPackedToday(userId);
        }

        public List<KeyValuePair<string, float>> GetAverageTime(Guid userId)
        {
            cmdGetAverageTime command = new cmdGetAverageTime(userId);
            return command.Execute();
       }

        public List<cstRoleTbl> GetRole()
        {
            cmdGetRoleCommand command = new cmdGetRoleCommand();
            return command.Execute();
        }

        /// <summary>
        /// Check that user is Admin or Not. that is its Supre user that has all permissions.
        /// </summary>
        /// <param name="UserID">
        /// Guid UserID
        /// </param>
        /// <returns>
        /// Boolen value that true or false.
        /// </returns>
        public Boolean IsSuperUser(Guid UserID)
        {
            cmdRole role = new cmdRole();
            return role.IsSuperUser(UserID);
        }

        /// <summary>
        /// check that user can override that shipment.
        /// </summary>
        /// <param name="UserID">
        /// Guid UserID
        /// </param>
        /// <returns>
        /// Boolean Value true Or Flase.
        /// </returns>
        public Boolean CanOverrideShipment(Guid UserID)
        {
            cmdRole command = new cmdRole();
            return command.CanOverride(UserID);
        }
        /// <summary>
        /// Update RoleType Table
        /// </summary>
        /// <param name="role">int </param>
        /// <param name="action">ActionType enum</param>
        /// <returns>list of table information of RoleType</returns>
        public List<cstRoleTbl> UpdateRole(cstRoleTbl role, csteActionenum action)
        {
            cmdUpdateRole command = new cmdUpdateRole(role, action);
            return command.Execute();
        }

        public List<cstRoleTbl> GetRoleById(Guid Id, csteActionenum action)
        {
            cmdUpdateRole command = new cmdUpdateRole(Id, action);
            return command.Execute();
        }

        /// <summary>
        /// Update Role Permissons
        /// </summary>
        /// <param name="RoleID">
        /// Guid RoleID to be update
        /// </param>
        /// <param name="RoleName">
        /// </param>
        /// <param name="IsSuperUser"></param>
        /// <param name="View"></param>
        /// <param name="Scan"></param>
        /// <param name="Rescan"></param>
        /// <param name="Override"></param>
        /// <returns></returns>
        public Boolean UpdateRole(Guid RoleID, String RoleName, Boolean IsSuperUser, Boolean View, Boolean Scan, Boolean Rescan, Boolean Override)
        {
            cmdRole command = new cmdRole();
            return command.SetRole(RoleID, RoleName, IsSuperUser, View, Scan, Rescan, Override);
        }
        /// <summary>
        /// Class has static method that accept the UPC Code and give its respective SKU Name(ITMREF1_0)
        /// </summary>
        /// <param name="UPCCode">UPC Code of SKU</param>
        /// <returns>Sting Value of SKU Name</returns>
        public string UPCtoSKUName(String UPCCode)
        {
           return( cmdUPCConverter.UPCCodeToSKU(UPCCode).ToString());

        }

        /// <summary>
        /// Find UPc Code for the SKu   
        /// </summary>
        /// <param name="SKuName">String SKU Name(ITMDES1_0)</param>
        /// <returns></returns>
        public string SKUnameToUPCCode(string SKuName)
        {
            return cmdUPCConverter.SKUNameToSku(SKuName);
        }

        /// <summary>
        /// Call the Sql query to and collect shipment locations
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>Boolean Value that shows this shipment packing on multiple locations or not(true/false)</returns>
        public Boolean ISShipMultiLocationExist(String ShipmentID)
        {
            cmdSageOperations command = new cmdSageOperations();
           return command.getShipmentOnMultipleLocation(ShipmentID);
        }
        
        /// <summary>
        /// list of Shipment id and its locations
        /// that is list of ShipmentLocationNameCustomEntity
        /// </summary>
        /// <param name="ShipmentID">String ShipmentID</param>
        /// <returns>list of Shipment id and its locations</returns>
        public List<cstShipmentLocationWise> ShipmentLocationList(String ShipmentID)
        {
            cmdSageOperations command = new cmdSageOperations();
           return command.getLocationListOFShipment(ShipmentID);
        }
        
        /// <summary>
        /// This functions retuns the location of application saved in the falt file.
        /// </summary>
        /// <returns>Sting Value of location</returns>
        public String ApplicationLocation()
        {
            return cmdLocalFile.ReadString("Location");
        }
        
        /// <summary>
        /// Read From file according to paramenter
        /// </summary>
        /// <param name="WhatToRead">String what do you want to read from the file</param>
        /// <returns>String</returns>
        public string ReadFromLocalFile(String WhatToRead)
        {
            return cmdLocalFile.ReadString(WhatToRead);
        }
        
        /// <summary>
        /// Overrite the new string to the local file setting
        /// </summary>
        /// <param name="Locationname">String </param>
        /// <param name="TimeString">String in hh:mm:ss Format</param>
        /// <param name="LanguageName">string</param>
        /// <returns></returns>
        public Boolean WriteStringTofile(String Locationname, String TimeString, String LanguageName, String IsBarcodeShow)
        {
            return cmdLocalFile.WriteString(Locationname, TimeString, LanguageName, IsBarcodeShow);
        }
        
        /// <summary>
        /// UserLogs table Save method
        /// </summary>
        /// <param name="lsUserLog">list Of user Information(UserCustom)</param>
        /// <returns>Boolen true on success else false</returns>
        public Boolean SaveUserLog(List<cstAutditLog> lsUserLogInfo)
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.SaveUserLog(lsUserLogInfo);
        }
        
        /// <summary>
        /// Update the UserLogs information
        /// </summary>
        /// <param name="lsUserLog">Updated User Information list </param>
        /// <param name="UserID">User Id (Long)</param>
        /// <returns>Boolean Value</returns>
        public Boolean UpdateUserLog(List<cstAutditLog> lsUserLogInfo, Guid UserID)
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.UpdateUserLog(lsUserLogInfo,UserID);
        }
        
        /// <summary>
        /// fetch all rows from the UserLogs table and return the list
        /// </summary>
        /// <returns>list of UserLogCustom type</returns>
        public List<cstAutditLog> GetUserLogAll()
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.GetUserLog();
        }
        
        /// <summary>
        /// Fetch Selected User Information From the Userlogs table
        /// </summary>
        /// <param name="UserID">Long UserID </param>
        /// <returns>List of UserLogs of type UserLogCustom</returns>
        public List<cstAutditLog> GetUserLogAll(Guid UserID)
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.GetUserLog(UserID);
        }
        /// <summary>
        /// Fetch Selected User Information From the Userlogs table
        /// </summary>
        /// <param name="UserID">Long UserID </param>
        /// /// <param name="CurrentDate">Date time paramenter</param>
        /// <returns>List of UserLogs of type UserLogCustom</returns>
        public List<cstAutditLog> GetUserLogAll(Guid UserID, DateTime DateOFLog)
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.GetUserLog(UserID, DateOFLog);
        }
        
        /// <summary>
        /// Last User login datetime.
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>DateTime </returns>
        public DateTime UserLastLogin(Guid UserID)
        {
            cmbAuditLog command = new cmbAuditLog();
            return command.UserLastLogin(UserID);
        }
        
        /// <summary>
        /// Save Error log
        /// </summary>
        /// <param name="lsErrorlog">detail error log info</param>
        /// <returns>Boolean Value</returns>
        public Boolean SaveErrorlog(List<cstErrorLog> lsError)
        {
            cmdErrorLog command = new cmdErrorLog();
            return command.SaveLog(lsError);
        }
      
        /// <summary>
        /// Calculate all shipments toatal Quantity and Time Required to pack the saprate shipment
        /// </summary>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity()
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity();
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(Guid UserID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(UserID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(Guid UserID,DateTime FromDate, DateTime ToDate)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(UserID,FromDate,ToDate);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(DateTime FromDate, DateTime ToDate)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(FromDate, ToDate);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(Guid UserID, int PackingStatus)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(UserID, PackingStatus);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(Guid UserID, DateTime FromDate, DateTime ToDate, int PackingStatus)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(UserID, FromDate, ToDate, PackingStatus);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(DateTime FromDate, DateTime ToDate, int PackingStatus)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity(FromDate, ToDate, PackingStatus);
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <param name="PackingStatusOnly">Boolean True</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity(int PackingStatus,Boolean PackingStatusOnly)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantity( PackingStatus,PackingStatusOnly);
        }



        /// <summary>
        /// Calculate all shipments toatal Quantity and Time Required to pack the saprate shipment
        /// </summary>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(Guid UserID, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(UserID, StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(Guid UserID, DateTime FromDate, DateTime ToDate, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(UserID, FromDate, ToDate, StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(DateTime FromDate, DateTime ToDate, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(FromDate, ToDate, StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity up to current Date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(Guid UserID, int PackingStatus, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(UserID, PackingStatus, StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(Guid UserID, DateTime FromDate, DateTime ToDate, int PackingStatus, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(UserID, FromDate, ToDate, PackingStatus, StationID);
        }

        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(DateTime FromDate, DateTime ToDate, int PackingStatus, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(FromDate, ToDate, PackingStatus, StationID);
        }
        /// <summary>
        /// Shipment With its Time And SKu Quantity on specified date
        /// </summary>
        /// <param name="UserID">Long UserID</param>
        /// <param name="date"> DateTime For Filter</param>
        /// <param name="PackingStatus">int Packing Status 0/1/2</param>
        /// <param name="PackingStatusOnly">Boolean True</param>
        /// <returns>List<cstPackingTime></returns>
        public List<cstPackingTime> GetPackingTimeQuantity_ByStation(int PackingStatus, Boolean PackingStatusOnly, Guid StationID)
        {
            cmdPackingTimeAndQuantity command = new cmdPackingTimeAndQuantity();
            return command.GetPackingTimeAndQantityByStation(PackingStatus, PackingStatusOnly, StationID);
        }


        /// <summary>
        /// All packing table Information
        /// </summary>
        /// <returns>List<cstPackingDetailTbl></returns>
        public List<cstPackageDetails> GetPackingDetailTbl()
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetails();
        }

        /// <summary>
        /// packing Details Of Specific ShipmentID
        /// </summary>
        /// <param name="ShipmentId">String ShipmentID</param>
        /// <returns>List<cstPackingDetailTbl></returns>
        public List<cstPackageDetails> GetPackingDetailTbl(Guid PackingID)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetails(PackingID);
        }


        /// <summary>
        /// packing Details Of Specific sku and Boxnumber
        /// </summary>
        /// <param name="ShipmentId">String ShipmentID</param>
        /// <returns>List<cstPackingDetailTbl></returns>
        public Guid GetPackingDetailTblbyskuandBoxnumber(String SKUNumber, String BoxNumber)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetailTblbyskuandBoxnumber(SKUNumber,BoxNumber);
        }

        /// <summary>
        /// Get Packing Detail table information by Box Number 
        /// </summary>
        /// <param name="BoxNumber">String Box Number</param>
        /// <returns>list of packing Detail table information</returns>
        public List<cstPackageDetails> GetPackingDetailTbl(String BoxNumber)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetailsByBoxNum(BoxNumber);
        }

        /// <summary>
        /// Save data to the StationMaster Table.
        /// </summary>
        /// <param name="lsStationMaster">List<cstStationMasterTbl></param>
        /// <returns>Boolean </returns>
        public Boolean SaveStationMaster(List<cstStationMasterTbl> lsStationMaster)
        {
            cmdStation command = new cmdStation();
            return command.SaveSationMaster(lsStationMaster);
        }

        /// <summary>
        /// Upadte Station table with the Device ID record
        /// </summary>
        /// <param name="lsStationMaster">List <cstStationMasterTbl></param>
        /// <param name="DeviceID">String Device ID</param>
        /// <returns></returns>
        public Boolean SaveStationMaster(List<cstStationMasterTbl> lsStationMaster,String DeviceID)
        {
            cmdStation command = new cmdStation();
            return command.SaveSationMaster(lsStationMaster, DeviceID);
        }

        /// <summary>
        /// Save UserStation transaction Table.
        /// </summary>
        /// <param name="lsUserStation">List<cstUserStationTbl></param>
        /// <returns>Boolean</returns>
        public Boolean SaveUserStation(List<cstUserStationTbl> lsUserStation)
        {
            cmdSetUserStation command = new cmdSetUserStation();
            return command.SaveUserStation(lsUserStation);
        }

        /// <summary>
        /// Select All staion list from the tblStationmster Table
        /// </summary>
        /// <returns></returns>
        public List<cstStationMasterTbl> GetStationMaster()
        {
            cmdStation command = new cmdStation();
           return command.GetStationList();
        }

        /// <summary>
        /// Station information filtered by Station Name    
        /// </summary>
        /// <param name="Stationame">String Station name</param>
        /// <returns>cst StationMasterTbl Information</returns>
        public cstStationMasterTbl GetStationMasterByName(string Stationame)
        {
            cmdStation Command = new cmdStation();
            return Command.GetStationInfo(Stationame);
        }

        public List<cstPackageDetails> GetPackingDetailTblByShipment(String shipm)
        {
            cmdPakingDetails command = new cmdPakingDetails();
            return command.GetPackingDetailsByShipmentNumber(shipm);
        }
        /// <summary>
        /// Get Shipping table required information from the *sage database
        /// </summary>
        /// <param name="ShippingNumber">String Shipping Number </param>
        /// <returns>List of cstShippingTbl that contints the shipping information of passed string Shipping Number</returns>
        public List<cstShippingTbl> GetShippingInfoFromSage(String ShippingNumber)
        {
            cmdSageOperations command = new cmdSageOperations();
            return command.getShippingInfo(ShippingNumber);
        }

        /// <summary>
        /// Get station Information from the Device ID
        /// </summary>
        /// <param name="DevideID">String MAC Address </param>
        /// <returns>List<cstStationMasterTbl> </returns>
        public List<cstStationMasterTbl> GetStationMaster(String DeviceID)
        {
            cmdStation command = new cmdStation();
            return command.GetStationList(DeviceID);
        }

        /// <summary>
        /// List of Last login Station Of all users
        /// </summary>
        /// <returns></returns>
        public List<cstUserCurrentStationAndDeviceID> GetlastLoginStationAllUsers()
        {
            cmdUserCurrentStationAndDeviceID command = new cmdUserCurrentStationAndDeviceID();
            return command.LastLoginStationAllUsers();
        }

        /// <summary>
        /// Total packed Shipment Today , Current Packing Shipment nad Respective User
        /// </summary>
        /// <returns>List<cstShipmentPackedTodayAndAvgTime></returns>
        public List<cstShipmentPackedTodayAndAvgTime> GetPackingCountCurrentShipmentUserName()
        {
            GetTotalShipmentPackedToday command = new GetTotalShipmentPackedToday();
            return command.GetTotalShipmentPackedTime();
        }

        /// <summary>
        /// Show the All rows of Error log
        /// </summary>
        /// <returns>List<cstErrorLog></returns>
        public List<cstErrorLog> GetErrorLog()
        {
            cmdErrorLog command = new cmdErrorLog();
           return command.GetErrorLogAll();
        }
        
        /// <summary>
        ///  Selected item from Station Table
        /// </summary>
        /// <param name="StationID">Guid StaionID</param>
        /// <returns>Indication Station Information</returns>
        public cstStationMasterTbl GetStationMaster(Guid StationID)
        {
            cmdStation command = new cmdStation();
            return command.GetStationList(StationID);
        }

        /// <summary>
        /// Set UserID for created by and update by responsible operations in all application for Audit table
        /// </summary>
        /// <param name="UserID">Guid UserID</param>
        public void SetCreatedOrUpdatedUserID(Guid UserID)
        {
            GlobalClasses.ClGlobal.UserID = UserID;
        }

        /// <summary>
        /// get the Device unique MAC address.
        /// </summary>
        /// <returns>String MAC Address.</returns>
        public String getDeviceMAC()
        {
            cmdDeviceMACaddress command = new cmdDeviceMACaddress();
            return command.DeviceNumber();
        }

        /// <summary>
        /// Save Box information to database. adds Box to the table
        /// </summary>
        /// <param name="lsboxPackageInfo">List of infrmation of box</param>
        /// <returns>Boolean Value if saved then True else false</returns>
        public Guid SetBox(List<cstBoxPackage> lsboxPackageInfo)
        {
            cmdBox _box = new cmdBox();
            return _box.SaveBoxPackage(lsboxPackageInfo);
        }


        public Boolean UpdateBox(DateTime EndTime, string BOXNUM)
        {
            cmdBox _box = new cmdBox();
            return _box.UpdateBoxPackage(EndTime, BOXNUM);
        }





        public Guid SetBox(List<cstBoxPackage> lsboxPackageInfo, Guid packingID)
        {
            Guid bxID = Guid.Empty;
            List<cstBoxCreation> lsbox = new List<cstBoxCreation>();
            cmdBoxCreation cmdbx = new cmdBoxCreation();
            cmdBox _box = new cmdBox();

            lsbox = cmdbx.GetOrSelectBoxNumber(packingID);
            if (lsbox.Count == 0)
            {
                bxID = _box.SaveBoxPackage(lsboxPackageInfo);


            }
            else
            {
                foreach (var itm in lsbox)
                {
                    //cstBoxCreation bxx = new cstBoxCreation();
                    //bxx.BoxFLG = itm.BoxFLG;
                    //bxx.MaxBoxinBoxPackage = itm.MaxBoxinBoxPackage;
                    //lsbox.Add(bxx);

                    if (itm.BoxFLG)
                    {
                        bxID = _box.SaveBoxPackage(lsboxPackageInfo);
                    }
                    else
                    {
                        string boxnum = itm.MaxBoxinBoxPackage;
                        bxID = _box.GetBoxIDByBoxNumber(boxnum);

                    }
                }
            }

            return bxID;
        }

        ///Avoid unnessry Box Creation 19-12-2014
        ///
        public List<cstBoxCreation> GetorSelectBox(Guid packingID)
        {
            cmdBoxCreation bxcration = new cmdBoxCreation();
            return bxcration.GetOrSelectBoxNumber(packingID);
        }

        ///End
        /// <summary>
        /// get all table of Box Package.
        /// </summary>
        /// <returns>List<cstBoxPackage> </returns>
        public List<cstBoxPackage> GetBoxPackageAll()
        {
            cmdBox _box = new cmdBox();
            return _box.GetAll();
        }

        /// <summary>
        /// Selected Packing Id information From Box Package table
        /// </summary>
        /// <param name="PackingID">Guid Packing ID</param>
        /// <returns>List<cstBoxPackage></returns>
        public List<cstBoxPackage> GetBoxPackageByPackingID(Guid PackingID)
        {
            cmdBox _box = new cmdBox();
            return _box.GetSelectedByPackingID(PackingID);
        }


        public Guid GetBoxIDByBoxNumber(string bxNum)
        {
            cmdBox _bx = new cmdBox();
            return _bx.GetBoxIDByBoxNumber(bxNum);
        }
        /// <summary>
        /// Selected Box ID information From BoxPackage Table
        /// </summary>
        /// <param name="BoxID">Guid Box ID</param>
        /// <returns>cstBoxPackage Object</returns>
        public cstBoxPackage GetBoxPackageByBoxID(Guid BoxID)
        {
            cmdBox _box = new cmdBox();
            return _box.GetSelectedByBoxID(BoxID);    
        }

        //For Getting SKU Qty From SKUName And BoxNumber 11-14-2014

        public int GetSKUQtyByBoxSKU(string skuNum,string BoxNum)
        {
            cmdBox _bo = new cmdBox();
            return _bo.GetSKUQtyByBoxSKU(skuNum,BoxNum);
        }
        //End


        /// <summary>
        /// Sort BoxPackage Table Information by Box number
        /// </summary>
        /// <param name="BoxNumber">
        /// string Box Number
        /// </param>
        /// <returns>
        /// box Package table Object with information
        /// </returns>
        public cstBoxPackage GetBoxPackageByBoxNumber(String BoxNumber)
        {
            cmdBox _box = new cmdBox();
            return _box.GetSelectedByBoxNumber(BoxNumber);
        }


        public List<cstPalletInfo> GetPalletInfoBySHNumber(String SHNUmber)
        {
            cmdPallet _Pallet = new cmdPallet();
            return _Pallet.GetPalletInfoBySHNumber(SHNUmber);
        }

        public void RemoveBoxFromPalletCon(String Boxnumber)
        {
            cmdPallet _pallet = new cmdPallet();
            _pallet.RemoveBoxFromPallet(Boxnumber);
        }

        public void DeletePalletCon(String Palletnumber)
        {
            cmdPallet _pallet = new cmdPallet();
            _pallet.DeletePallet(Palletnumber);
        }

        public List<cstBoxPallet> GetBoxInforamtionByShipmentCon(string ShipmentNumber)
        {
            cmdPallet _pallet = new cmdPallet();
            return _pallet.GetBoxInforamtionByShipment(ShipmentNumber);
        }



        public Guid SetPallet(List<cstPalletInfo> lspalletInfo)
        {
            cmdPallet _box = new cmdPallet();
            return _box.SavePalletInfo(lspalletInfo);
        }

        public Guid UpdatePallet(List<cstPalletInfo> lspalletInfo,Guid PalletID)
        {
            cmdPallet _box = new cmdPallet();
            return _box.UpdatePalletInfo(lspalletInfo, PalletID);
        }

        public Guid UpdateEndTimePallet(List<cstPalletInfo> lspalletInfo, Guid PalletID)
        {
            cmdPallet _box = new cmdPallet();
            return _box.UpdateEndTimePalletInfo(lspalletInfo, PalletID);
        }

        public Guid SetPalletDetails(List<cstPalletDetails> lspalletDetailInfo)
        {
            cmdPallet _box = new cmdPallet();
            return _box.SavePalletDetailInfo(lspalletDetailInfo);
        }
        public cstPalletInfo GetPalletInfoByPalletID(Guid PalletID)
        {
            cmdPallet _box = new cmdPallet();
            return _box.GetSelectedByPalletID(PalletID);
        }


        public List<cstBoxnumberQuantity> GetBoxnumberAndQuantity(String PalletNUmber)
        {
            cmdPallet _pallet = new cmdPallet();
            return _pallet.GetBoxnumberandQuanity(PalletNUmber);
        }


        public int GetTotalQuantityPackageDetailCon(string ShipmentNumber, string Location)
        {
            cmdPallet pallet = new cmdPallet();
            return pallet.GetSumFromPackageDetail(ShipmentNumber, Location);
        }


        public int GetTotalQuantityShipmentVIEWCon(string ShipmentNumber, string Location)
        {
            cmdPallet pallet = new cmdPallet();
            return pallet.GetSumFromShippingView(ShipmentNumber, Location);
        }

        public int GetTotalSKUByPalletNUmberCon(string PalletNumber)
        {
            cmdPallet pallet = new cmdPallet();
            return pallet.GetTotalSKUByPalletNUmber(PalletNumber);
        }



        #region Pallet
        public List<cstPalletDetails> GetpalletDetailsByBoxNumberCon(String BoxNUmber)
        {
            cmdPallet _box = new cmdPallet();
            return _box.GetpalletDetailsByBoxNumber(BoxNUmber);
        }

        public List<cstPalletDetails> GetPalletDetailByPalletNumberCon(String PalletNumber)
        {
            cmdPallet _box = new cmdPallet();
            return _box.GetPalletDetailByPalletNumber(PalletNumber);
        }

        public string GetPalletInfoByBoxnumberCon(string Boxnumber)
        {
            cmdPallet _pallet = new cmdPallet();
            return _pallet.GetPalletInfoByBoxnumber(Boxnumber);
        }


        public cstPalletInfo GetPalletInfoByPalletnumberCon(string Palletnumber)
        {
            cmdPallet _box = new cmdPallet();
            return _box.GetPalletInfoByPalletnumber(Palletnumber);
        }

        #endregion


        public Boolean ckeckBoxInCon(String BoxNumber, string ShipmentNumber)
        {
            cmdPakingDetails cmdPacking = new cmdPakingDetails();
            return cmdPacking.ckeckBoxIn(BoxNumber, ShipmentNumber);
        }

        #region CartonCommands
        public List<cstCartonInfo> GetAllCartonInfo()
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.GetAll();
        }

        public List<cstCartonInfo> GetAllCartonInfoByBoxNumber(String BoxNumber)
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.GetByBoxNumber(BoxNumber);
        }

        public cstCartonInfo GetCartonInfoByCartonID(Guid CartonID)
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.GetByCaronID(CartonID);
        }

        public Guid SaveCartonInfo(cstCartonInfo _CatonInfo)
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.SaveCartonInfo(_CatonInfo);
        }

        public Boolean DeleteCartonInfo(String ShipmentNumber)
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.DeleteCartonByShippingNumber(ShipmentNumber);
        }
        //DeleteCartonByBoxNumber
        public Boolean DeleteCartonInfoByBoxNum(String ShipmentNumber,string BOXNUM)
        {
            cmdCartonInfo cmd = new cmdCartonInfo();
            return cmd.DeleteCartonByBoxNumber(ShipmentNumber, BOXNUM);
        }
        #endregion



        #region Delete

        public void deleteSKUFromBoxsm(Guid PackingID)
        {
            cmdPackage delete = new cmdPackage();
            delete.deleteSKUFromBox(PackingID);
        }

        #endregion


        #region Update
        public void updateSKUFromBoxsm(Guid PackingID,int qty)
        {
            cmdPackage update = new cmdPackage();
            update.updateSKUFromBox(PackingID, qty);
        }
        #endregion

        public string SetDeletedBoxDetails(List<cstDeletedBoxSave> lsBoxDetails)
        {
            cmdDeleteBoxSave command = new cmdDeleteBoxSave();
            return command.saveDeletedBox(lsBoxDetails);
        }

        public string SetDeletedPalletDetails(List<cstDeletedPalletSave> lsPalletDetails)
        {
            cmdDeletedPalletSave command = new cmdDeletedPalletSave();
            return command.saveDeletedPallet(lsPalletDetails);
        }


        public cstPackageTbl GetPkgByBoxnumber(String Boxnumber)
        {
            cmdPackage pkg = new cmdPackage();
            return pkg.GetPackageByBoxnumber(Boxnumber);

        }

    }

}
