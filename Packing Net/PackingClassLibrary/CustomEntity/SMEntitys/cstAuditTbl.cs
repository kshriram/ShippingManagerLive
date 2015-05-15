using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
   public class cstAutditLog
    {
       public Guid UserLogID { get; set; }
       public Guid UserID { get; set; }
       public String ActionType { get; set; }
       public DateTime ActionTime { get; set; }
       public string ActionValue { get; set; }
    }
}
