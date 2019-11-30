using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSocketMongoDB.Model
{
    public class ElectricProduct
    {
        [BsonId]
        public ObjectId id { get; set; }
        [BsonElement("productName")]
        public string productName { get; set; }
        [BsonElement("consumePower")]
        public double consumePower { get; set; }
        [BsonElement("usagePower")]
        public double usagePower { get; set; }
        [BsonElement("standbyPower")]
        public double standbyPower { get; set; }
        [BsonElement("usageTime")]
        public double usageTime { get; set; }
    }
}
