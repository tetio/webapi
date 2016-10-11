using System;
using MongoDB.Bson;
using System.Collections.Generic;
// using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;

namespace webapi.Models
{
    public class Game
    {
        // [BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
        public ObjectId id { get; set; }
        public int maxPlayers { get; set; }
        public string type { get; set; }

        public string owner { get; set; }
        public DateTime createdAt { get; set; }

        public List<Player> players { get; set; }

        [BsonIgnore]
        public string myId { get {return id.ToString();} }
    }
}