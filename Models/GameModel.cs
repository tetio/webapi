using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;

namespace webapi.Models
{
    public class GameModel
    {
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
        public string id { get; set; }
        public int maxPlayers { get; set; }
        public string type { get; set; }

        public string owner { get; set; }
        public DateTime createdAt { get; set; }

        // public string[] players { get; set; }

        // public string mId { get {return _id.ToString();} }
    }
}