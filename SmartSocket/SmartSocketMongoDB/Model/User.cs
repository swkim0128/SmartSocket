using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSocketMongoDB.Model
{
    public class User
    {
        [BsonId]
        public string ID { get; set; }
        [BsonElement("userName")]
        public string userName { get; set; }
        [BsonElement("protexsiveTax")]
        public int protexsiveTax { get; set; }
        [BsonElement("standard")]
        public Standard standard { get; set; }
        [BsonElement("measureProduct_id")]
        public string measureProduct_id { get; set; }

        public User()
        {
            standard = new Standard();
        }
    }

    public class Standard
    {
        [BsonElement("contractDate")]
        public int contractDate { get; set; }
        [BsonElement("contract")]
        public string contract { get; set; }
        [BsonElement("family")]
        public string family { get; set; }
        [BsonElement("welfare")]
        public string welfare { get; set; }
        [BsonElement("contractPower")]
        public int contractPower { get; set; }
        [BsonElement("receivingVoltage")]
        public string receivingVoltage { get; set; }
    }
}
