using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using System.Data.Objects;
namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdUser
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();
        Sage_x3v6Entities Sage = new Sage_x3v6Entities();

        #region Get Functions 
        public List<cstUserMasterTbl> GetUserMaster(string UserName)
        {
            List<cstUserMasterTbl> _lsUserCostom = new List<cstUserMasterTbl>();
            try
            {
                User _UserInfo = entx3v6.Users.SingleOrDefault(i => i.UserName == UserName);
                if (String.Compare(_UserInfo.UserName, UserName) == 0)
                {
                    cstUserMasterTbl _CustomUserMaster = new cstUserMasterTbl();
                    _CustomUserMaster.UserID = _UserInfo.UserID;
                    _CustomUserMaster.UserName = _UserInfo.UserName;
                    _CustomUserMaster.UserFullName = _UserInfo.UserFullName;
                    _CustomUserMaster.UserAddress = _UserInfo.UserAddress;
                    _CustomUserMaster.JoiningDate = Convert.ToDateTime(_UserInfo.UserJoiningDate);
                    _CustomUserMaster.Password = _UserInfo.UserPassword;
                    _CustomUserMaster.Role = _UserInfo.Role.RoleId;
                    _CustomUserMaster.RoleName = _UserInfo.Role.Name;

                    #region set permission code
                    cstPermissions permission = new cstPermissions();
                    string[] strPermission = _UserInfo.Role.Action.Split(new char[] { '&', '-' });
                    if (strPermission != null)
                    {
                        permission.viewUser = Boolean.Parse(strPermission[0]);
                        permission.newUser = Boolean.Parse(strPermission[1]);
                        permission.editUser = Boolean.Parse(strPermission[2]);
                        permission.deleteUser = Boolean.Parse(strPermission[3]);
                        permission.viewShipment = Boolean.Parse(strPermission[4]);
                        permission.scanShipment = Boolean.Parse(strPermission[5]);
                        permission.reScanShipment = Boolean.Parse(strPermission[6]);
                        permission.overrideShipment = Boolean.Parse(strPermission[7]);
                    }
                    _CustomUserMaster.Permission = permission;
                    #endregion
                    _lsUserCostom.Add(_CustomUserMaster);
                }

            }

            catch (Exception Ex)
            {
                if (Ex.Message.ToString() == "The underlying provider failed on Open.")
                {

                    Error_Loger.elAction.save("GetSelectedUserName.GetSelected(0)", "Network Problem");
                }
                else
                {
                    Error_Loger.elAction.save("GetSelectedUserName.GetSelected(0)", Ex.Message.ToString());
                }
            }
            return _lsUserCostom;
        }
        public List<cstUserMasterTbl> GetUserMaster(Guid UserID)
        {
            List<cstUserMasterTbl> _lsUserCostom = new List<cstUserMasterTbl>();
            try
            {
                User _UserInfo = new User();
                _UserInfo = entx3v6.Users.SingleOrDefault(i => i.UserID == UserID);
                cstUserMasterTbl _CustomUserMaster = new cstUserMasterTbl();
                _CustomUserMaster.UserID = _UserInfo.UserID;
                _CustomUserMaster.UserName = _UserInfo.UserName;
                _CustomUserMaster.UserFullName = _UserInfo.UserFullName;
                _CustomUserMaster.UserAddress = _UserInfo.UserAddress;
                _CustomUserMaster.JoiningDate = Convert.ToDateTime(_UserInfo.UserJoiningDate);
                _CustomUserMaster.Password = _UserInfo.UserPassword;
                _CustomUserMaster.Role = _UserInfo.RoleId;
                _CustomUserMaster.RoleName = entx3v6.Roles.SingleOrDefault(i => i.RoleId == _UserInfo.RoleId).Name.ToString();
                _lsUserCostom.Add(_CustomUserMaster);
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("GetSelectedUserName.GetSelectedShipment(1)", Ex.Message.ToString());
            }
            return _lsUserCostom;
        }
        public List<cstUserMasterTbl> GetUserMaster()
        {
            List<cstUserMasterTbl> _lsUserCostom = new List<cstUserMasterTbl>();
            try
            {
                var _UserInfovar = from V in entx3v6.Users select V;
                foreach (var _UserInfo in _UserInfovar)
                {
                    cstUserMasterTbl _CustomUserMaster = new cstUserMasterTbl();
                    _CustomUserMaster.UserID = _UserInfo.UserID;
                    _CustomUserMaster.UserName = _UserInfo.UserName;
                    _CustomUserMaster.UserFullName = _UserInfo.UserFullName;
                    _CustomUserMaster.UserAddress = _UserInfo.UserAddress;
                    _CustomUserMaster.JoiningDate = Convert.ToDateTime(_UserInfo.UserJoiningDate);
                    _CustomUserMaster.Password = _UserInfo.UserPassword;
                    _CustomUserMaster.Role = _UserInfo.RoleId;
                    _CustomUserMaster.RoleName = entx3v6.Roles.SingleOrDefault(i => i.RoleId == _UserInfo.RoleId).Name.ToString();
                    _lsUserCostom.Add(_CustomUserMaster);
                }


            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("GetSelectedUserName.GetSelectedShipment(2)", Ex.Message.ToString());
            }
            return _lsUserCostom;
        }
        public List<KeyValuePair<string, long>> GetTotalShipmentsPackedToday(Guid UserID)
        {
            List<KeyValuePair<string, long>> list = new List<KeyValuePair<string, long>>(); ;

            var result = (from p in entx3v6.Packages
                          where p.UserId == UserID && EntityFunctions.TruncateTime(p.StartTime) == (EntityFunctions.TruncateTime(DateTime.UtcNow))
                          select EntityFunctions.TruncateTime(p.StartTime)).Count();

            if (result > 0)
            {
                list.Add(new KeyValuePair<string, long>("Total Today", result));
            }
            else
            {
                list.Add(new KeyValuePair<string, long>("Total Today", 0));
            }
            return list;
        }
        #endregion

        #region Set Functions
        /// <summary>
        /// Save the User Information in UserMaster Table.
        /// </summary>
        /// <param name="lsUserMaster">List of User Information</param>
        /// <returns>If information saved Succesfuly then return True else False.</returns>
        public Boolean SetUserMaster(List<cstUserMasterTbl> lsUserMaster)
        {
            Boolean _return = false;
            try
            {

                foreach (var _userinfo in lsUserMaster)
                {
                    //UserMaster Table Object from the Entity.
                    User _UserMasterTbl = new User();
                    //add list values to the custom class boject
                    _UserMasterTbl.UserID = Guid.NewGuid();
                    _UserMasterTbl.UserName = _userinfo.UserName;
                    _UserMasterTbl.UserAddress = _userinfo.UserAddress;
                    _UserMasterTbl.UserPassword = _userinfo.Password;
                    _UserMasterTbl.UserJoiningDate = _userinfo.JoiningDate;
                    _UserMasterTbl.UserFullName = _userinfo.UserFullName;
                    _UserMasterTbl.RoleId = _userinfo.Role;
                    _UserMasterTbl.CreatedBy = GlobalClasses.ClGlobal.UserID;
                    _UserMasterTbl.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    //add Object to the entity.
                    entx3v6.AddToUsers(_UserMasterTbl);
                }
                // save the changes to the Entity.
                entx3v6.SaveChanges();
                _return = true;
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("SetUserInFormationCommand.Execute(0)", Ex.Message.ToString());
            }
            return _return;
        }
        /// <summary>
        /// This is Overrried method to update the record
        /// </summary>
        /// <param name="lsUserMaster">list of user information</param>
        /// <param name="UserID">User's ID which u  want to update</param>
        /// <returns>Transaction Saved the True else False</returns>
        public Boolean SetUserMaster(List<cstUserMasterTbl> lsUserMaster, Guid UserID)
        {
            Boolean _return = false;
            try
            {
                foreach (var _userinfo in lsUserMaster)
                {
                    //select User information form UserMaster Table whose UserID is passed to the Method.
                    User _userMaster = entx3v6.Users.SingleOrDefault(i => i.UserID == UserID);

                    //Assing Values to the userMaster table object.
                    _userMaster.UserName = _userinfo.UserName;
                    _userMaster.UserAddress = _userinfo.UserAddress;
                    _userMaster.UserPassword = _userinfo.Password;
                    _userMaster.UserJoiningDate = _userinfo.JoiningDate;
                    _userMaster.UserFullName = _userinfo.UserFullName;
                    _userMaster.RoleId = _userinfo.Role;
                    _userMaster.UpdatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    _userMaster.Updatedby = GlobalClasses.ClGlobal.UserID;
                    if (_userinfo.Role == Guid.Empty)
                    {

                        _userMaster.RoleId = entx3v6.Users.SingleOrDefault(i => i.UserID == UserID).RoleId;
                    }
                }
                //Dont add the Object just save the information to entity.
                entx3v6.SaveChanges();
                _return = true;
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("SetUserInFormationCommand.Execute(1)", Ex.Message.ToString());
            }
            return _return;
        }
        #endregion

       #region Delete Functions
        #endregion
    }
}
