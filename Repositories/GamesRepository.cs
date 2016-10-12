using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using webapi.Models;
using System;

namespace webapi.Repositories
{
    public class GamesRepository
    {
        private IDBContext dbContext;

        public GamesRepository(IDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Game InsertGame(Game game)
        {
            var a = this.dbContext.Games.InsertOneAsync(game);
            a.Wait();
            //return this.Get(game._id.ToString());
            return game;
        }
        public Game Get(string id)
        {
            var a =  this.dbContext.Games.FindAsync<Game>(x => x.id == new ObjectId(id));
            a.Wait();
            return a.Result.SingleOrDefault();
        }

        public IEnumerable<Game> Get()
        {
            return this.dbContext.Games.Find(x => true).Limit(3).ToListAsync().Result;
        }

        public IEnumerable<Game> GetOpenGames()
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            return this.dbContext.Games.Find(x => x.state == GameStates.Open && x.createdAt >= fiveMinutesAgo).Limit(3).ToListAsync().Result;
        }


        public List<Game> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return this.dbContext.Games.Find<Game>(queryDoc).ToList();
        }        
        
    }
}
