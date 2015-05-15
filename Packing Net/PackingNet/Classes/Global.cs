using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System.Windows.Threading;
using System.Globalization;
using System.Threading;
using System.Windows.Documents;
using PackingClassLibrary.Models;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System.Windows;

namespace Packing_Net.Classes
{
    /// <summary>
    /// Use Global Class to store Application Level variables.
    /// </summary>
    public class Global
    {
        public static Thread newWindowThread;

        public static SplashScreen splashScreen = new SplashScreen("1235.png");

        /// <summary>
        /// Controller object.
        /// </summary>
        public static smController controller = new smController();

        public static string deleteflag;

        public static string ShipmentNumPalletforPrint;

        public static Boolean FromRemove = false;

        public static string BOLProcessFlag;

        public static string CartonOn;

        #region Model Objects
        /// <summary>
        /// UserModel Object for global access.
        /// </summary>
        public static model_User LoggedUserModel;
        /// <summary>
        /// Station Model information for global Access.
        /// </summary>
        public static model_Station StationModel;
        #endregion

        ////For Unpacked Boxes
        public static string UnPacked;
        public static string shipmentclosed;
        ////For All Shipment packed and compler come from delete and remove from

        public static string FlagAllshippckd;
        ////end
        public static Boolean FromRemoveFlag;
        //variable add for Pallet 

        public static string PRONumber;
        public static string BOLNumber;
        public static string CarrierName;
        public static string Carton;
        public static string ssccNumber;
        public static string palletnumber;

        public static string PalletNoForReplace;

        public static string ActivePallet;

        public static int TotalSKUQuantity;

        public static Boolean CheckPalletPack;
        public static Boolean CheckRemoveButton;

        public static List<cstShipmentList> ListShipmnet = new List<cstShipmentList>();

        public static Boolean PalletPrint;
        public static Boolean BoxPrint;

        ///adding new box
        public static string flgBx;

        public static string BoxActive;

        public static Boolean IsActiveFlag;
        public static Boolean isAllpack;
        public static Guid PrintBox;
        /// <summary>
        /// For add  skus In User Selected Box
        /// </summary>
        public static string FlgaddInBox;
        public static Guid selectedBoxID;
        /// End
        public static string pkdStatus;
        ///for previous current box
        public static string previouscurrentbox;
        /// eend
        public static String ShipmentNumberforferguson;

        public static string CartonPallet;


        //Print Wayfair Box Lables

        public String ShipmentNumber;
        public static string BoxNumberScanned;
        public static int counter = 0;
        public static string WH;

        public static string LoginType;
        public static string LTLLogin;


        public static string ShippingNumber;
        public static int primaryKey;
        public static String WindowTopUserName;


        public static Guid PalletID;

        public static Guid PalletDetailID;

        //message box veriables.
        public static string MsgBoxTitle;
        public static string MsgBoxMessage;
        public static string MsgBoxType;
        public static String MsgBoxResult;
        public static String SKUName;

        //User Control Veriables.
        public static Guid LoggedUserId;
        public static string StationName;//For Station Name Dispaly..

        //LastLoginTime
        public static DateTime LastLoginDateTime;

        //Manager Override Scan ID
        public static Guid ManagerID;
        public static string ManagerName;
        public static string Mode;

        //Same User Repacking Mode Packing ID
        public static Guid SameUserpackingID;

        ///paking Detail
        public static Guid pkDetailID;

        //Timer Flag
        public static Boolean ISTimerRaised;
        public static Boolean ISTimerTickInitialised = false;
        public static String TimeOutUserName = "";

        //language File name for messageBox Translator
        public static String LanguageFileName;

        public static bool flgChkdeletebx;
        //StationDenied
        public static Boolean ISStateTimer = false;

        //Recently Packed Shipment ID
        public static Guid RecentlyPackedID;

        //Packing shipment GUID
        public static Guid PackingID;

        //Show Barcode in GridView radio button Status;
        public static String ISBarcodeShow = "Yes";

        //List scroll messages in Application
        public static List<Run> lsScroll = new List<Run>();

        //PrintBoxID
        public static Guid PrintBoxID = Guid.Empty;

        //for print Boxes
        //List<string> boxnumber = new List<string>();

        public static string formatDateTime(DateTime date, string culture)
        {
            return DateTime.UtcNow.ToString("MMM, dd yyyy hh:mm tt", CultureInfo.CreateSpecificCulture(culture));
        }

        public static List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet> Pallet = new List<PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet>();

        public static PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet PalletSingle = new PackingNet.Pages.wndBoxInfoForPallet.BoxAndPallet();

       // public static cstBoxNumberAndPallet BoxPall = new cstBoxNumberAndPallet();  

    }   
}