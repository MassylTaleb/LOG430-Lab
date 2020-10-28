using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LOG430_TP
{
    public class AggregatorModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public float Value { get; set; }
        
        public string Type { get; set; }

        public string Topic { get; set; }

        public DateTime DateTime { get; set; }
    }
}
