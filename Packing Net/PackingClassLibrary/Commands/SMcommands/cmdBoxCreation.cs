using System;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackingClassLibrary.Commands.SMcommands
{
   public  class cmdBoxCreation
   {
       local_x3v6Entities entx3v6 = new local_x3v6Entities();
       public List<cstBoxCreation> GetOrSelectBoxNumber(Guid PackingID)
       {
           List<cstBoxCreation> lsreturn = new List<cstBoxCreation>();
           try
           {
               var box = entx3v6.ExecuteStoreQuery<cstBoxCreation>(@"select 
                                                max(bp.BoxNum) MaxBoxinBoxPackage,
                                                cast(case when max(pd.BoxNumber) = max(bp.BoxNum) then 1 else 0 end as bit) as BoxFLG
                                                from 
                                                Package p
                                                inner         join PackageDetail pd on p.PackingID = pd.PackingID
                                                inner join BoxPackage bp on bp.PackingID = p.PackingID
                                                where p.PackingID = '" + PackingID + "'group by p.PackingID order by p.PackingID").ToList();




               foreach (var item in box)
               {
                   cstBoxCreation bxcrt = new cstBoxCreation();
                   bxcrt.MaxBoxinBoxPackage = item.MaxBoxinBoxPackage;
                   bxcrt.BoxFLG = item.BoxFLG;
                   lsreturn.Add(bxcrt);
               }
           }
           catch (Exception)
           {
           }
           return lsreturn;
       }
       

           
       
    }
}
