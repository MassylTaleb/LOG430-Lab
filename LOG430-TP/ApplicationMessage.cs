using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class ApplicationMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Topic { get; set; }

        public string Payload { get; set; }

        public int QualityOfServiceLevel { get; set; }

        public bool Retain { get; set; }
    }
}
