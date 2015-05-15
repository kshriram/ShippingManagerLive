using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.Commands;
using System.IO;

namespace PackingClassLibrary.Error_Loger
{
   public static class elAction
    {
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
                _error.ErrorLocation ="PackingCustom."+ ErrorLocation;
                _error.ErrorDesc = ErrorDesc;
                _error.ErrorTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                _error.UserID = Guid.Empty;
                _lsErrorCustom.Add(_error);
                cmdErrorLog _Error = new cmdErrorLog();
                _return = _Error.SaveLog(_lsErrorCustom);
            }
            catch (Exception )
            {
                
            }
            return _return;
        }
    }
}
