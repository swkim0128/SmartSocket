using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using SmartSocketMongoDB.Model;

namespace SmartSocketMongoDB
{
    public class ElectricProductRepository : Repository
    {
        private IMongoCollection<ElectricProduct> _collection;

        public ElectricProductRepository()
            : base("mongodb://localhost")
        {
            _collection = _database.GetCollection<ElectricProduct>("WattMeasurement");
        }

        public async Task Insert(ElectricProduct product)
        {
            await _collection.InsertOneAsync(product);
        }

        public async Task<ElectricProduct> Find(string fieldName, string fieldValue)
        {
            var filter = Builders<ElectricProduct>.Filter.Eq(fieldName, fieldValue);
            var result = await _collection.Find(filter).SingleOrDefaultAsync();

            return result;
        }

        public async Task<bool> Update(ObjectId id, string updateFieldName, string updateFieldValue)
        {
            var filter = Builders<ElectricProduct>.Filter.Eq("_id", id);
            var update = Builders<ElectricProduct>.Update.Set(updateFieldName, updateFieldValue);
            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<bool> DeleteUserById(ObjectId id)
        {
            var filter = Builders<ElectricProduct>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }
    }
}
