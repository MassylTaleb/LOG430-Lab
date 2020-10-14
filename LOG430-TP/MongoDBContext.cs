using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class MongoDBContext: IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoDBContext()
        {
            _mongoClient = new MongoClient("mongodb+srv://massyl:<password>@log430.upspr.mongodb.net/<dbname>?retryWrites=true&w=majority");
            _db = _mongoClient.GetDatabase("LOG430");
        }

        public IMongoCollection<ApplicationMessage> ApplicationMessage
        {
            get
            {
                return _db.GetCollection<ApplicationMessage>("ApplicationMessage");
            }
        }
    }
}
