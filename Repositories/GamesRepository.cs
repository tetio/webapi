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
        IMongoCollection<Game> games;
        public GamesRepository(IDBContext dbContext)
        {
            this.dbContext = dbContext;
            games = this.dbContext.Games;
        }

        public Game InsertGame(Game game)
        {
            var a = games.InsertOneAsync(game);
            a.Wait();
            //return this.Get(game._id.ToString());
            return game;
        }
        public Game Get(string id)
        {
            var a = games.FindAsync<Game>(x => x.id == new ObjectId(id));
            a.Wait();
            return a.Result.SingleOrDefault();
        }

        public IEnumerable<Game> Get()
        {
            return games.Find(x => true).Limit(3).ToListAsync().Result;
        }

        public IEnumerable<Game> GetOpenGames()
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            return games.Find(x => x.state == GameStates.Open && x.createdAt >= fiveMinutesAgo).Limit(3).ToListAsync().Result;
        }


        public Game FindCreatedAndBlock(string jsonPlayer)
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            var player = BsonSerializer.Deserialize<Player>(jsonPlayer);
            // var update = "{$set: { state: 'BLOCKED' }}";
            // var sort = "{ createdAt: -1 }";
            var game = games.FindOneAndUpdateAsync<Game>(x => x.players.Count < x .maxPlayers && x.state == GameStates.Open && x.createdAt >= fiveMinutesAgo,
            Builders<Game>.Update.Set(x => x.state, "BLOCKED")).Result;

            if (game != null) {
                // TODO  add player to game
            } else {
                // TODO create a new game
            }

            return game;
        }


    public List<Game> Filter(string jsonQuery)
    {
        var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
        return games.Find<Game>(queryDoc).ToList();
    }

}
}
