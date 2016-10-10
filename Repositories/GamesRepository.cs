using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using webapi.Models;

namespace webapi.Repositories
{
    public class GamesRepository
    {
        protected static IMongoClient client;
        protected static IMongoDatabase db;
        protected IMongoCollection<GameModel> collection;

        public GamesRepository()
        {
            client = new MongoClient();
            db = client.GetDatabase("lw");
            collection = db.GetCollection<GameModel>("games");
        }
        public GameModel InsertGame(GameModel game)
        {
            var a = this.collection.InsertOneAsync(game);
            a.Wait();
            //return this.Get(game._id.ToString());
            return game;
        }
        public GameModel Get(string id)
        {
            // return this.collection.Find(new BsonDocument {{"_id", new ObjectId(id)}}).FirstAsync().Result;
            return this.collection.Find(x => x._id == new ObjectId(id)).FirstAsync().Result;

        }

        public IEnumerable<GameModel> Get()
        {
            return collection.Find(x => true).Limit(3).ToListAsync().Result;
        }


    }
}
