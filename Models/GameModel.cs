using MongoDB.Bson;

namespace webapi.Models
{
    public class GameModel
    {
        public ObjectId _id { get; set; }
        public int maxPlayers { get; set; }
        public string type { get; set; }
        // public DateTime createdAt { get; set; };

        // public string[] players { get; set; }

    }
}