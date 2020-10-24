﻿using MongoDB.Driver;
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
            var servers = new List<MongoServerAddress>() { new MongoServerAddress("log430-shard-00-01.upspr.mongodb.net", 27017) };
            var credential = MongoCredential.CreateCredential("admin", "massyl", "Massyl-10");
            var mongoClientSettings = new MongoClientSettings()
            {
                ConnectionMode = ConnectionMode.Direct,
                Credential = credential,
                Servers = servers.ToArray(),
                ApplicationName = "LOG430",
            };

            //_mongoClient = new MongoClient(mongoClientSettings);
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
    }
}
