using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;


namespace Packing_Net.Classes
{
   public static class ErrorLoger
    {
       /// <summary>
       /// Save the error Log to the Database.
       /// </summary>
        /// <param name="ErrorLocation">String Form Name+Fuction Name </param>
       /// <param name="ErrorDesc">String error string</param>
       /// <param name="ErrorTime">DateTime of Error</param>
       /// <param name="UserID">long UserID</param>
       /// <returns>Boolean true if log saved else false</returns>
       public static Boolean save(string ErrorLocation, string ErrorDesc, DateTime ErrorTime, Guid UserID)
       {
           Boolean _return = false;
           try
           {
               List<cstErrorLog> _lsErrorCustom = new List<cstErrorLog>();
               cstErrorLog _error = new cstErrorLog();
               _error.ErrorLogID = Guid.Empty;
               _error.ErrorLocation = ErrorLocation;
               _error.ErrorDesc = ErrorDesc;
               //_error.ErrorTime = ErrorTime;
               _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(ErrorTime, "Eastern Standard Time");
               _error.UserID = UserID;
               _lsErrorCustom.Add(_error);
               _return = Global.controller.SaveErrorlog(_lsErrorCustom);
           }
           catch (Exception)
           {}
           return _return;
       }
       /// <summary>
       /// Save the error Log to the Database.with Global.UserID
       /// </summary>
       /// <param name="ErrorLocation">String Form Name+Fuction Name </param>
       /// <param name="ErrorDesc">String error string</param>
       /// <param name="ErrorTime">DateTime of Error</param>
       /// <returns>Boolean true if log saved else false</returns>
       public static Boolean save(string ErrorLocation, string ErrorDesc, DateTime ErrorTime)
       {
           Boolean _return = false;
           try
           {
               List<cstErrorLog> _lsErrorCustom = new List<cstErrorLog>();
               cstErrorLog _error = new cstErrorLog();
               _error.ErrorLogID = Guid.Empty;
               _error.ErrorLocation = ErrorLocation;
               _error.ErrorDesc = ErrorDesc;
              // _error.ErrorTime = ErrorTime;
               _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(ErrorTime, "Eastern Standard Time");
               _error.UserID = Global.LoggedUserId;
               _lsErrorCustom.Add(_error);
               _return = Global.controller.SaveErrorlog(_lsErrorCustom);
           }
           catch (Exception)
           { }
           return _return;
       }
       /// <summary>
       /// Save the error Log to the Database.with null UserID
       /// </summary>
       /// <param name="ErrorLocation">String Form Name+Fuction Name </param>
       /// <param name="ErrorDesc">String error string</param>
       /// <param name="ErrorTime">DateTime of Error</param>
       /// <param name="NullUserID">it takes null User ID true or false any Condition</param>
       /// <returns>Boolean true if log saved else false</returns>
       public static Boolean save(string ErrorLocation, string ErrorDesc, DateTime ErrorTime,Boolean NullUserID)
       {
           Boolean _return = false;
           try
           {
               List<cstErrorLog> _lsErrorCustom = new List<cstErrorLog>();
               cstErrorLog _error = new cstErrorLog();
               _error.ErrorLogID = Guid.Empty;
               _error.ErrorLocation = ErrorLocation;
               _error.ErrorDesc = ErrorDesc;
             //  _error.ErrorTime = ErrorTime;
               _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(ErrorTime, "Eastern Standard Time");
               _lsErrorCustom.Add(_error);
               _return = Global.controller.SaveErrorlog(_lsErrorCustom);
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Save error log with NullUserID and Current Datetime value
       /// </summary>
       /// <param name="ErrorLocation">String Form Name+Fuction Name </param>
       /// <param name="ErrorDesc">String error string</param>
       /// <param name="NullUserID">it takes null User ID true or false any Condition</param>
       /// <returns>Boolean true if log saved else false</returns>
       public static Boolean save(string ErrorLocation, string ErrorDesc,  Boolean NullUserID)
       {
           Boolean _return = false;
           try
           {
               List<cstErrorLog> _lsErrorCustom = new List<cstErrorLog>();
               cstErrorLog _error = new cstErrorLog();
               _error.ErrorLogID = Guid.Empty;
               _error.ErrorLocation = ErrorLocation;
               _error.ErrorDesc = ErrorDesc;
              // _error.ErrorTime = DateTime.UtcNow;
               _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
               _lsErrorCustom.Add(_error);
               _return = Global.controller.SaveErrorlog(_lsErrorCustom);
           }
           catch (Exception)
           { }
           return _return;
       }

       /// <summary>
       /// Save error log with Global UserID and Current Datetime value 
       /// </summary>
       /// <param name="ErrorLocation">String Form Name+Fuction Name </param>
       /// <param name="ErrorDesc">String error string</param>
       /// <returns>Boolean true if log saved else false</returns>
       public static Boolean save(string ErrorLocation, string ErrorDesc)
       {
           Boolean _return = false;
           try
           {
               List<cstErrorLog> _lsErrorCustom = new List<cstErrorLog>();
               cstErrorLog _error = new cstErrorLog();
               _error.ErrorLogID = Guid.Empty;
               _error.ErrorLocation = ErrorLocation;
               _error.ErrorDesc = ErrorDesc;
               //_error.ErrorTime = DateTime.UtcNow;

               _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" );
               _error.UserID = Global.LoggedUserModel.UserInfo.UserID;
               _lsErrorCustom.Add(_error);
              _return= Global.controller.SaveErrorlog(_lsErrorCustom);
           }
           catch (Exception)
           { }
           return _return;
       }
    }
}
