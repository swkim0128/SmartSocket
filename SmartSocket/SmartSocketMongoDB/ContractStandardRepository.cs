using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using SmartSocketMongoDB.Model;

namespace SmartSocketMongoDB
{
    public class ContractStandardRepository : Repository
    {
        private IMongoCollection<ContractStandard> _collection;
        public ContractStandardRepository()
            : base("mongodb://localhost")
        {
            _collection = _database.GetCollection<ContractStandard>("ContractStandard");
        }

        public async Task Insert(ContractStandard standard)
        {
            await _collection.InsertOneAsync(standard);
        }

        public async Task<ContractStandard> Find(string fieldName, string fieldValue)
        {
            var filter = Builders<ContractStandard>.Filter.Eq(fieldName, fieldValue);
            var result = await _collection.Find(filter).SingleOrDefaultAsync();

            return result;
        }

        public Task<ContractStandard> Find(ContractStandard standard)
        {
            var filter = Builders<ContractStandard>.Filter.And(
                Builders<ContractStandard>.Filter.Eq(x => x.contract, standard.contract),
                Builders<ContractStandard>.Filter.Eq(x => x.receiving, standard.receiving));
            var result = _collection.Find(filter).SingleOrDefaultAsync();

            return result;
        }
    }
}
