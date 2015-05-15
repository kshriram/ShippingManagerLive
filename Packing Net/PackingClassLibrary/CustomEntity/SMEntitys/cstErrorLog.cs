using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.CustomEntity
{
    public class cstErrorLog
    {
        public Guid ErrorLogID { get; set; }
        public Guid UserID { get; set; }
        public String ErrorLocation { get; set; }
        public String ErrorDesc { get; set; }
        public DateTime ErrorTime { get; set; }
        
    }
}
