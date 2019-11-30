using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartSocketMongoDB.Model
{
    public class ContractStandard
    {
        [BsonId]
        ObjectId id { get; set; }
        [BsonElement("contract")]
        public string contract { get; set; }
        [BsonElement("receiving")]
        public string receiving { get; set; }
        [BsonElement("basic_charge")]
        public BsonArray basic_charge { get; set; }
        [BsonElement("charge")]
        public Charge charge { get; set; }

        public ContractStandard()
        {
            basic_charge = new BsonArray();
            charge = new Charge();
        }

        public int getBasic_charge(int index)
        {
            return basic_charge[index].ToInt32();
        }
    }

    public class Charge
    {
        [BsonElement("basic")]
        public BsonArray basic { get; set; }
        [BsonElement("lightload")]
        public BsonArray lightload { get; set; }
        [BsonElement("middleload")]
        public BsonArray middleload { get; set; }
        [BsonElement("maximumload")]
        public BsonArray maximumload { get; set; }

        public double getBasic(int index)
        {
            return basic[index].ToDouble();
        }

        public double getLightload(int index)
        {
            return lightload[index].ToDouble();
        }

        public double getMiddleload(int index)
        {
            return middleload[index].ToDouble();
        }

        public double getMaximumload(int index)
        {
            return maximumload[index].ToDouble();
        }
    }
}
