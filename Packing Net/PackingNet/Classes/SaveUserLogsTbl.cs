using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System.Threading;

namespace Packing_Net.Classes
{
   public static class SaveUserLogsTbl
    {
       /// <summary>
       /// static method that call the saveuser method without shipmentID
       /// </summary>
       /// <param name="UserID">String</param>
       /// <param name="ActionType">string</param>
       /// <param name="ActionTime">string</param>
       /// <returns>Boolean value</returns>
       public static Boolean logThis(string UserID, String ActionType, String ActionTime)
       {
           Boolean _return = false;
           Thread t = new Thread(() =>
               {
                   try
                   {
                       List<cstAutditLog> _UserLog = new List<cstAutditLog>();
                       cstAutditLog _UserC = new cstAutditLog();
                       _UserC.UserLogID = Guid.Empty;
                       Guid TuserID = Guid.Empty;
                       Guid.TryParse(UserID.ToString(), out TuserID);
                       _UserC.UserID = TuserID;
                       _UserC.ActionType = ActionType;
                      /// _UserC.ActionTime = Convert.ToDateTime(ActionTime);
                      /// 8-12-2014
                       _UserC.ActionTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(ActionTime), "Eastern Standard Time");
                       //End
                       _UserLog.Add(_UserC);
                       _return = Global.controller.SaveUserLog(_UserLog);
                   }
                   catch (Exception Ex)
                   {
                       ErrorLoger.save("Classes\\AddToUserLogFirstFunction", Ex.InnerException.ToString());
                   }
               });
           return _return;
       }
       /// <summary>
       /// save User log with ShipmentID
       /// </summary>
       /// <param name="UserID">String</param>
       /// <param name="ActionType">String</param>
       /// <param name="ActionTime">String</param>
       /// <param name="ShipmentID">String</param>
       /// <returns></returns>
       public static Boolean logThis(string UserID, String ActionType, String ActionTime, String ShipmentID)
       {
           Boolean _return = false;
           Thread T = new Thread(() =>
             {
                 try
                 {
                     List<cstAutditLog> _UserLog = new List<cstAutditLog>();
                     cstAutditLog _UserC = new cstAutditLog();
                     _UserC.UserLogID = Guid.Empty;
                     Guid TUserID = Guid.Empty;
                     Guid.TryParse(UserID.ToString(), out TUserID);
                     _UserC.UserID = TUserID;
                     _UserC.ActionType = ActionType;
                   ///  _UserC.ActionTime = Convert.ToDateTime(ActionTime);
                     /// 8-12-2014
                     _UserC.ActionTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(ActionTime), "Eastern Standard Time");
                     //End
                     _UserC.ActionValue = ShipmentID;
                     _UserLog.Add(_UserC);
                     _return = Global.controller.SaveUserLog(_UserLog);
                 }
                 catch (Exception Ex)
                 {
                     ErrorLoger.save("Classes\\LogThis(2)", Ex.InnerException.ToString());
                 }
             });
           return _return;
       }
       /// <summary>
       /// Current time and Global UserID is the default vaues saved. with shipmentID
       /// </summary>
       /// <param name="ActionType">String</param>
       /// <param name="ShipmentID">String</param>
       /// <returns>Boolean true or false</returns>
       public static Boolean logThis(String ActionType,String ShipmentID)
       {
           Boolean _return = false;
           Thread T = new Thread(() =>
           {
               try
               {
                   List<cstAutditLog> _UserLog = new List<cstAutditLog>();
                   cstAutditLog _UserC = new cstAutditLog();
                   _UserC.UserLogID = Guid.Empty;
                   _UserC.UserID = Global.LoggedUserId;
                   _UserC.ActionType = ActionType;
                ////   _UserC.ActionTime = Convert.ToDateTime(DateTime.UtcNow.ToString());
                   /// 8-12-2014
                   _UserC.ActionTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                   //End
                   _UserC.ActionValue = ShipmentID;
                   _UserLog.Add(_UserC);
                   _return = Global.controller.SaveUserLog(_UserLog);
               }
               catch (Exception Ex)
               {
                   ErrorLoger.save("Classes\\LogThis(3)", Ex.InnerException.ToString());
               }
           });
           T.Start();
           return _return;
       }
    
    /// <summary>
       /// Current time , Global UserID is the default vaues saved. without shipment ID
       /// </summary>
       /// <param name="ActionType">String</param>
       /// <param name="ShipmentID">String</param>
       /// <returns>Boolean true or false</returns>
       public static Boolean logThis(String ActionType)
       {
           Boolean _return = false;
           Thread T = new Thread(() =>
          {

              try
              {
                  List<cstAutditLog> _UserLog = new List<cstAutditLog>();
                  cstAutditLog _UserC = new cstAutditLog();
                  _UserC.UserLogID = Guid.Empty;
                  _UserC.UserID = Global.LoggedUserId;
                  _UserC.ActionType = ActionType;
                //  _UserC.ActionTime = Convert.ToDateTime(DateTime.UtcNow.ToString());
                  /// 8-12-2014
                  _UserC.ActionTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                  //End

                  _UserLog.Add(_UserC);
                  _return = Global.controller.SaveUserLog(_UserLog);
              }
              catch (Exception Ex)
              {
                  ErrorLoger.save("Classes\\LogThis(4)", Ex.InnerException.ToString());
              }
          });
           return _return;
       }
   
    }
}
