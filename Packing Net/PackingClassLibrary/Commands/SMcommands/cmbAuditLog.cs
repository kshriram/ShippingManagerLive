using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using System.Data.Objects;

namespace PackingClassLibrary.Commands
{
   public class cmbAuditLog
    {
        local_x3v6Entities x3v6 = new local_x3v6Entities();
       /// <summary>
       /// UserLogs table Save method
       /// </summary>
       /// <param name="lsUserLog">list Of user Information(UserCustom)</param>
       /// <returns>Boolen true on success else false</returns>
        public Boolean SaveUserLog(List<cstAutditLog> lsUserLog)
        {
            Boolean _return = false;
            try
            {
                if (lsUserLog.Count > 0)
                {
                    foreach (var _UserLogitem in lsUserLog)
                    {
                        Audit _Userlog = new Audit();
                        _Userlog.UserLogID = Guid.NewGuid();
                        _Userlog.UserID = _UserLogitem.UserID;
                        _Userlog.ActionType = _UserLogitem.ActionType;
                        _Userlog.ActionTime = Convert.ToDateTime(_UserLogitem.ActionTime);
                        _Userlog.ActionValue = _UserLogitem.ActionValue;
                        x3v6.AddToAudits(_Userlog);

                    }
                    x3v6.SaveChanges();
                    _return = true;
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.SaveUserLog()", Ex.Message.ToString());
            }
            return _return;
        }
       /// <summary>
       /// Update the UserLogs information
       /// </summary>
       /// <param name="lsUserLog">Updated User Information list </param>
        /// <param name="UserID">User Id (Long)</param>
       /// <returns>Boolean Value</returns>
        public Boolean UpdateUserLog(List<cstAutditLog> lsUserLog, Guid UserID)
        {
            Boolean _return = false;
            try
            {
                if (lsUserLog.Count > 0)
                {
                    foreach (var _UserLogitem in lsUserLog)
                    {
                        Audit _Userlog = new Audit();
                        _Userlog.UserLogID = _UserLogitem.UserLogID;
                        _Userlog.UserID = _UserLogitem.UserID;
                        _Userlog.ActionType = _UserLogitem.ActionType;
                        _Userlog.ActionTime = Convert.ToDateTime(_UserLogitem.ActionTime);
                        _Userlog.ActionValue = _UserLogitem.ActionValue;
                    }
                    x3v6.SaveChanges();
                    _return = true;
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.UpdateUserLog()", Ex.Message.ToString());
            }
            return _return;
        }

       /// <summary>
       /// fetch all rows from the UserLogs table and return the list
       /// </summary>
       /// <returns>list of UserLogCustom type</returns>
        public List<cstAutditLog> GetUserLog()
        {
            List<cstAutditLog> _lsReturn = new List<cstAutditLog>();
            try
            {
                var UserLogsinfo = from Userl in x3v6.Audits select Userl;
                foreach (var _UserLogitem in UserLogsinfo)
                {
                    cstAutditLog _UserCustom = new cstAutditLog();
                    _UserCustom.UserLogID = _UserLogitem.UserLogID;
                    _UserCustom.UserID = _UserLogitem.UserID;
                    _UserCustom.ActionType = _UserLogitem.ActionType;
                    _UserCustom.ActionTime = Convert.ToDateTime(_UserLogitem.ActionTime);
                    _UserCustom.ActionValue = _UserLogitem.ActionValue;
                    _lsReturn.Add(_UserCustom);                    
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.GetUserLog(0)", Ex.Message.ToString());
            }
            return _lsReturn;
        }

       /// <summary>
       /// Fetch Selected User Information From the Userlogs table
       /// </summary>
       /// <param name="UserID">Long UserID </param>
       /// <returns>List of UserLogs of type UserLogCustom</returns>
        public List<cstAutditLog> GetUserLog(Guid UserID)
        {
            List<cstAutditLog> _lsReturn = new List<cstAutditLog>();
            try
            {
                var UserLoginfo = from userl in x3v6.Audits
                                  where userl.UserID == UserID
                                  select userl;
                foreach (var _UserLogitem in UserLoginfo)
                {
                    cstAutditLog _UserCustom = new cstAutditLog();
                    _UserCustom.UserLogID = _UserLogitem.UserLogID;
                    _UserCustom.UserID = _UserLogitem.UserID;
                    _UserCustom.ActionType = _UserLogitem.ActionType;
                    _UserCustom.ActionTime = Convert.ToDateTime(_UserLogitem.ActionTime);
                    _UserCustom.ActionValue = _UserLogitem.ActionValue;
                    _lsReturn.Add(_UserCustom);
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.GetUserLog(1)", Ex.Message.ToString());
            }
            return _lsReturn;
        }
        /// <summary>
        /// Fetch Selected User Information From the Userlogs table
        /// </summary>
        /// <param name="UserID">Long UserID </param>
        /// <param name="CurrentDate">Date time paramenter</param>
        /// <returns>List of UserLogs of type UserLogCustom</returns>
        public List<cstAutditLog> GetUserLog(Guid UserID,DateTime CurrentDate)
        {
            List<cstAutditLog> _lsReturn = new List<cstAutditLog>();
            try
            {
                DateTime Cdate = CurrentDate.Date;
                var UserLoginfo = from userl in x3v6.Audits
                                  where userl.UserID == UserID
                                  &&  EntityFunctions.TruncateTime(userl.ActionTime) == Cdate
                                  select userl;
                foreach (var _UserLogitem in UserLoginfo)
                {
                    cstAutditLog _UserCustom = new cstAutditLog();
                    _UserCustom.UserLogID = _UserLogitem.UserLogID;
                    _UserCustom.UserID = _UserLogitem.UserID;
                    _UserCustom.ActionType = _UserLogitem.ActionType;
                    _UserCustom.ActionTime = Convert.ToDateTime(_UserLogitem.ActionTime);
                    _UserCustom.ActionValue = _UserLogitem.ActionValue;
                    _lsReturn.Add(_UserCustom);
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.GetUserLog(1)", Ex.Message.ToString());
            }
            return _lsReturn;
        }

       /// <summary>
       /// Last User login datetime.
       /// </summary>
       /// <param name="UserID">Long UserID</param>
       /// <returns>DateTime </returns>
        public DateTime UserLastLogin(Guid UserID)
        {
            //DateTime _returnDateTime = DateTime.UtcNow;
            DateTime _returnDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
            try
            {
                String datee = csteActionType.Login.ToString();
                var DatetimeLast = from _Last in x3v6.Audits
                                   where _Last.UserID == UserID &&
                                   _Last.ActionType == datee
                                   group _Last by _Last.UserID into _NewLast
                                   select new
                                   {
                                       Lastdate = _NewLast.Max(i => i.ActionTime)
                                   };
            _returnDateTime =Convert.ToDateTime( DatetimeLast.FirstOrDefault().Lastdate.ToString());
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("UserLogAction.UserLastLogin()", Ex.Message.ToString());
            }

            return _returnDateTime;
        }
    }
}
