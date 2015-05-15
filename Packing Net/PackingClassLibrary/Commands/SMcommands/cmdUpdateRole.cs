using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;

namespace PackingClassLibrary.Commands
{
    class cmdUpdateRole : cmdAbstractEntity<cstRoleTbl>
    {
        csteActionenum _action;
        cstRoleTbl _role;
        Guid _id;

        public cmdUpdateRole() { }

        public cmdUpdateRole(cstRoleTbl role, csteActionenum action)
        {
            _role = role;
            _action = action;
        }
        public cmdUpdateRole(Guid Id, csteActionenum action)
        {
            _id = Id;
            _action = action;
        }
        public override List<cstRoleTbl> Execute()
        {
            List<cstRoleTbl> roleList = new List<cstRoleTbl>();

            local_x3v6Entities context = new local_x3v6Entities();
            switch (_action)
            {
                case csteActionenum.New:
                    Role role = new Role();
                    role.RoleId = Guid.NewGuid();
                    role.Name = _role.Name;
                    role.Action = _role.Action;
                    role.CreatedBy = GlobalClasses.ClGlobal.UserID;
                    role.CreatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    context.AddToRoles(role);
                    context.SaveChanges();
                    break;

                case csteActionenum.Update:
                    Role roleTypeUpdate = context.Roles.First(i => i.RoleId == _role.RoleId);
                    roleTypeUpdate.Name = _role.Name;
                    roleTypeUpdate.Action = _role.Action;
                    roleTypeUpdate.Updatedby = GlobalClasses.ClGlobal.UserID;
                    roleTypeUpdate.UpdatedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                    context.SaveChanges();
                    break;                       

                case csteActionenum.Delete:
                    break;
                
                case csteActionenum.Get:
                    var result = from r in context.Roles
                                  where r.RoleId == _id
                                  select r;
                    foreach (var varRole in result)
                    {
                        cstRoleTbl objRole = new cstRoleTbl();
                        objRole.RoleId = varRole.RoleId;
                        objRole.Name = varRole.Name;
                        objRole.Action = varRole.Action;
                        roleList.Add(objRole);
                    }
                    break;               
            }

            return roleList;
        }


    }
}
