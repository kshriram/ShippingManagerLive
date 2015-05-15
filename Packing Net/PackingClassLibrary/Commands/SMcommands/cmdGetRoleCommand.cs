using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
namespace PackingClassLibrary.Commands
{
    class cmdGetRoleCommand : cmdAbstractEntity<cstRoleTbl>
    {
        public cmdGetRoleCommand() { }
        public override List<cstRoleTbl> Execute()
        {
              local_x3v6Entities entiites = new local_x3v6Entities();
              var result = from r in entiites.Roles
                           select r;

              List<cstRoleTbl> list = new List<cstRoleTbl>();
              foreach (var role in result)
              {
                  cstRoleTbl item = new cstRoleTbl();
                  item.Name = role.Name;
                  item.RoleId = role.RoleId;
                  item.Action = role.Action;
                  list.Add(item);
              }
             
              return list;
        }

    }
}
