using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using webapi.Models;

namespace webapi.Repository {
    public class GameRepository {
        protected static IMongoClient client;
        protected static IMongoDatabase db;
        protected  IMongoCollection<GameModel> collection;

        public GameRepository() {
            client = new MongoClient();
            db = client.GetDatabase("lw");
            collection = db.GetCollection<GameModel>("games");
        }
        public GameModel InsertGame(GameModel game) {
            this.collection.InsertOneAsync(game);
            return this.Get(game._id.ToString());
        }
        public GameModel Get(string id) {
            return this.collection.Find(new BsonDocument {{"_id", new ObjectId(id)}}).FirstAsync().Result;
        }
    }
}
