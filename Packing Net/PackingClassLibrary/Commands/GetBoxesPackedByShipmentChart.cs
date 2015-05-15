using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;

namespace PackingClassLibrary.Commands
{
    class GetBoxesPackedByShipmentChart : AbstractEntityCommand<KeyValue>
    {

        private int _userId;
        public GetBoxesPackedByShipmentChart(int userId)
        {
            _userId = userId;
        }

        List<KeyValue> keyValues;
        public override List<KeyValue> Execute()
        {
            /**
            local_x3v6Entities entities = new local_x3v6Entities();            
            var result = from pd in entities.PackingDetails 
                         join p in entities.Packings
                         on pd.PackingId = pd.PackingId                                                            
                         && (p.UserId == _userId)
                         group pd.PackingId by EntityFunctions.TruncateTime(pd.StartTime) into groupObject                        
                         select new 
                         {
                            Key = groupObject.Key,
                            Value = groupObject.Count()                            
                         };
           
            if (result != null)            
                keyValues = new List<KeyValue>();            
            foreach (var item in result)
            {
                KeyValue keyvalue = new KeyValue();
                keyvalue.Key = ((DateTime) item.Key).ToShortDateString();                
                keyvalue.Value = item.Value;
                keyValues.Add(keyvalue);
            }
             **/
            return keyValues;           
        }
    }
}
