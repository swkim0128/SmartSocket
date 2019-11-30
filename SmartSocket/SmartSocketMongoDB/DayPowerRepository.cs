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
    public class DayPowerRepository : Repository
    {
        private IMongoCollection<DayPower> _collection;

        public DayPowerRepository()
            : base("mongodb://localhost")
        {
            _collection = _database.GetCollection<DayPower>("WattMeasurement");
        }

        public async Task Insert(DayPower dayPower)
        {
            await _collection.InsertOneAsync(dayPower);
        }

        public DayPower Find(DateTime id, string measureProduct_id)
        {
            var filter = Builders<DayPower>.Filter.And(
                Builders<DayPower>.Filter.Eq(x => x.id.measureProduct_id, measureProduct_id),
                Builders<DayPower>.Filter.Eq(x => x.id.date, id)
                );
            var result = _collection.Find(filter).SingleOrDefault();

            return result;
        }

        public async Task<DayPower> Find(string fieldName, string fieldValue)
        {
            var filter = Builders<DayPower>.Filter.Eq(fieldName, fieldValue);
            var result = await _collection.Find(filter).SingleOrDefaultAsync();

            return result;
        }

        public Task<List<DayPower>> FindListDayPower(DateTime id, string measureProduct_id)
        {
            var filter = Builders<DayPower>.Filter.And(
                Builders<DayPower>.Filter.Gte(x => x.id.date, id),
                Builders<DayPower>.Filter.Eq(x => x.id.measureProduct_id, measureProduct_id)
                );
            var result = _collection.Find(filter).ToListAsync();

            return result;
        }
        public async Task Add(DayPowerID id, string filedName, string filedValue)
        {
            var filter = Builders<DayPower>.Filter.Eq("_id", id);
            var update = Builders<DayPower>.Update.Push(filedName, BsonValue.Create(filedValue));
            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<bool> Update(DayPowerID id, string fieldName, string fieldValue)
        {
            var filter = Builders<DayPower>.Filter.Eq("_id", id);
            var update = Builders<DayPower>.Update.Set(fieldName, fieldValue);
            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<bool> DeleteUserById(ObjectId id)
        {
            var filter = Builders<DayPower>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }
    }
}
