using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSocketMongoDB.Model
{
    public class MeasureProduct
    {
        [BsonId]
        public string id { get; set; }
        [BsonElement("electricProduct_id")]
        public BsonArray electricProduct_id { get; set; }

        public MeasureProduct()
        {
            electricProduct_id = new BsonArray();
        }
    }
}
