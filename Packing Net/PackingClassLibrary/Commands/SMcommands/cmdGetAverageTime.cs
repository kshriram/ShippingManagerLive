using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Data.Linq.SqlClient;
using System.Data.Objects.SqlClient;
namespace PackingClassLibrary.Commands
{
    public class cmdGetAverageTime : cmdAbstractEntity<KeyValuePair<string, float>>
    {
        Guid _userId;
        public cmdGetAverageTime(Guid userId)
        {
            _userId = userId;
        }

        public override List<KeyValuePair<string, float>> Execute()
        {
            List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();
            DateTime currentDate = DateTime.UtcNow;

              local_x3v6Entities entities = new local_x3v6Entities();
              var result = from pd in entities.PackageDetails
                           join p in entities.Packages on pd.PackingId equals p.PackingId
                           join u in entities.Users on p.UserId equals u.UserID                          
                           where p.UserId == _userId && EntityFunctions.TruncateTime(p.StartTime) == (EntityFunctions.TruncateTime(currentDate))                           
                           select new 
                              {
                                  difference = SqlFunctions.DateDiff("s", p.StartTime, p.EndTime)
        
                              };          

            int total = 0;
            foreach( var data in result)
            {
                int diff = (int)data.difference;
                total = total + diff;
            }

            if (total > 0)
            {
                float averageTime = (total/ result.Count());
                list.Add(new KeyValuePair<string, float>("Average Time", averageTime));
            }
            else
            {
                list.Add(new KeyValuePair<string,float>("Average Time", 0));
            }
            
            return list;
        }
    }    
}
