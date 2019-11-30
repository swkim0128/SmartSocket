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
    public class UserRepository : Repository
    {
        private IMongoCollection<User> _collection;

        public UserRepository()
            : base("mongodb://localhost")
        {
            _collection = _database.GetCollection<User>("User");
        }

        public Task Insert(User user)
        {
            return _collection.InsertOneAsync(user);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public Task<User> Find(string fieldName, string fieldValue)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, fieldValue);
            var result = _collection.Find(filter).SingleOrDefaultAsync();

            return result;
        }

        public bool Update(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.ID, id);
            var result = _collection.ReplaceOneAsync(filter, user);

            bool resultBool = result.Result.ModifiedCount != 0;
            return resultBool;
        }

        public async Task<bool> Update(ObjectId id, string updateFieldName, string updateFieldValue)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            var update = Builders<User>.Update.Set(updateFieldName, updateFieldValue);
            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<bool> DeleteUserById(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }

    }
}
