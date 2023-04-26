using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System;

namespace DemoGPTBot.Bots
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        //Request Or Response
        //[BsonElement("Type")]
        public string Type { get; set; }
        //[BsonElement("Message")]
        public string Message { get; set; }
        //[BsonElement("actiondate")]
        public DateTime actionDate { get; set; }
    }
}
