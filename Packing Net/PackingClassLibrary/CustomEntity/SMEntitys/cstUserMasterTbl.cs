using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    /// <summary>
    /// Avinash
    /// All fields that are belongs to the UserMaster table are declared with their properties
    /// </summary>
   public class cstUserMasterTbl
    {
       public Guid UserID { get; set; }
       public string UserName { get; set; }
       public String UserAddress { get; set; }
       public DateTime JoiningDate { get; set; }
       public String Password {get; set;}
       public String UserFullName { get; set; }
       public String RoleName { get; set; }
       public Guid Role { get; set; }
       public cstPermissions Permission { get; set; }       
    }
}
