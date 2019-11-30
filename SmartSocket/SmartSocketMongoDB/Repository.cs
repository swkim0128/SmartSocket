using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace SmartSocketMongoDB
{
    public abstract class Repository
    {
        private IMongoClient _client;
        protected IMongoDatabase _database;

        public Repository(string connectionString)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("smartsocket");
        }
    }
}
