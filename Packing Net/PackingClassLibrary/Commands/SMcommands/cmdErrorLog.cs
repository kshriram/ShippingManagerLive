using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using System.IO;

namespace PackingClassLibrary.Commands
{
   public class cmdErrorLog
    {
       local_x3v6Entities entX3v6 = new local_x3v6Entities();

       #region Set Fucntions
       /// <summary>
       /// Save Error log
       /// </summary>
       /// <param name="lsErrorlog">detail error log info</param>
       /// <returns>Boolean Value</returns>
       public Boolean SaveLog(List<cstErrorLog> lsErrorlog)
       {
           Boolean _return = false;
           try
           {
               if (lsErrorlog.Count > 0)
               {
                   foreach (var _Erroritem in lsErrorlog)
                   {
                       ErrorLog _errorCustom = new ErrorLog();
                       _errorCustom.ErrorLogID = Guid.NewGuid();
                       _errorCustom.ErrorLocation = _Erroritem.ErrorLocation;
                       _errorCustom.ErrorDesc = _Erroritem.ErrorDesc;
                       _errorCustom.ErrorTime = Convert.ToDateTime(_Erroritem.ErrorTime);
                       _errorCustom.UserID = _Erroritem.UserID;
                       entX3v6.AddToErrorLogs(_errorCustom);
                       entX3v6.SaveChanges();
                       _return = true;
                   }
               }
           }
           catch (Exception)
           {
               ///Save the Exeption to the File Name Bellow
               String[] Lines = { "", "" }; ;
               Lines[0] = Environment.NewLine + "------------------------------------------------" + DateTime.UtcNow + "------------------------------------------------";
               Lines[1] = "Internet Connection Fail == " + lsErrorlog[0].ErrorLocation.ToString() + " == " + DateTime.UtcNow;
               File.AppendAllLines("C:\\ShipmentManagerErrorLog.sys", Lines);
           }
           return _return;
       }
       #endregion

       #region Get Functions.
       /// <summary>
       /// Get All error Log Table information.
       /// </summary>
       /// <returns>list og cstErrorLog Table.</returns>
       public List<cstErrorLog> GetErrorLogAll()
       {
           List<cstErrorLog> lsError = new List<cstErrorLog>();
           try
           {
               var v = from _error in entX3v6.ErrorLogs select _error;
               foreach (var Vitem in v)
               {
                   cstErrorLog _error = new cstErrorLog();
                   _error.ErrorLogID = Vitem.ErrorLogID;
                   _error.ErrorDesc = Vitem.ErrorDesc;
                   _error.ErrorLocation = Vitem.ErrorLocation;
                   _error.ErrorTime = Convert.ToDateTime(Vitem.ErrorTime);
                   Guid Userid = (Guid)Vitem.UserID;
                   _error.UserID = Userid;
                   lsError.Add(_error);
               }
           }
           catch (Exception)
           { }
           return lsError;
       }
       #endregion
    }
}
