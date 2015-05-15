using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.Commands;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace PackingClassLibrary.Models
{
    /// <summary>
    /// User Model.
    /// </summary>
    public class model_User
    {

        public static Guid UserID { get; set; }
        public cstUserMasterTbl UserInfo { get; set; }
        public cstPermissions Permissions { get; set; }
        public cstRoleTbl UserRole { get; set; }
        public DateTime LastLoginTime { get; set; }
        public List<KeyValuePair<string, float>> AvragePackingTime { get; set; }

        /// <summary>
        /// User Related Get,set,Delete Methods object.
        /// </summary>
        public cmdUser UserFunctions = new cmdUser();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public model_User() { }

        /// <summary>
        /// Parameterised Constructor overload.
        /// </summary>
        /// <param name="UserIDc"> Guid User ID.</param>
        public model_User(Guid UserIDc)
        {
            UserID = UserIDc;
            _setUserInfo();
            _getAvgPackingTime();
        }

        /// <summary>
        /// User Inforamation set  To class object.
        /// </summary>
        private void _setUserInfo()
        {
            cmdUser _user = new cmdUser();
            cmbAuditLog command = new cmbAuditLog();
            UserInfo = _user.GetUserMaster(UserID)[0];
            if (UserInfo != null)
            {
                //Set User last login;
                LastLoginTime = command.UserLastLogin(UserID);
                //Set User Permissions
                Permissions = _user.GetUserMaster(UserID)[0].Permission;
                //Set Role of user
                _setRole();
            }
        }

        /// <summary>
        /// Set User Role Information.
        /// </summary>
        private void _setRole()
        {
            cmdGetRoleCommand _userRole = new cmdGetRoleCommand();
            UserRole = _userRole.Execute().FirstOrDefault(i => i.RoleId == UserInfo.Role);
        }

        /// <summary>
        /// Avarage Packing time Of the User.
        /// </summary>
        private void _getAvgPackingTime()
        {
           cmdGetAverageTime Avgtime  = new cmdGetAverageTime(UserID);
           AvragePackingTime = Avgtime.Execute();
        }

    }
}
