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
        public MongoDBContext()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            _db = _mongoClient.GetDatabase("LOG430");
        }

        public IMongoCollection<ApplicationMessage> ApplicationMessage
        {
            get
            {
                return _db.GetCollection<ApplicationMessage>("ApplicationMessage");
            }
        }

        public IMongoCollection<AggregatorModel> AggregatorModel
        {

            get
            {
                return _db.GetCollection<AggregatorModel>("AggregatorModel");
            }
        }
    }
}
