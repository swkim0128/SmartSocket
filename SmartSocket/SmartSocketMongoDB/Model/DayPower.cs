using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSocketMongoDB.Model
{
    public class DayPower
    {
        [BsonId]
        public DayPowerID id { get; set; }
        [BsonElement("usagePower")]
        public double usagePower { get; set; }
        [BsonElement("standbyPower")]
        public double standbyPower { get; set; }
        [BsonElement("power")]
        public BsonArray power { get; set; }

        public DayPower()
        {
            id = new DayPowerID();
            power = new BsonArray();
        }

        public string[] getPowerArray()
        {
            string temp = power.ToString();
            temp = temp.Replace("[", "");
            temp = temp.Replace("]", "");
            string[] result = temp.Split(',');
            
            return result;
        }
    }

    public class DayPowerID
    {
        [BsonElement("date")]
        public DateTime date { get; set; }
        [BsonElement("measureProduct_id")]
        public string measureProduct_id { get; set; }
    }
}
