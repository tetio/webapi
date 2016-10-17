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
            games.InsertOneAsync(game).Wait();
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
            var game = games.FindOneAndUpdateAsync<Game>(x => x.state == GameStates.Open && x.createdAt >= fiveMinutesAgo,
            Builders<Game>.Update.Set(x => x.state, "BLOCKED")).Result;

            player.joinedAt = DateTime.UtcNow;

            if (game != null)
            {
                if (game.players.Count >= game.maxPlayers - 1) {
                    game.state = "READY";
                } 
                var update = Builders<Game>.Update
                .Push("players", player)
                .Set("state", game.state);
                games.UpdateOneAsync<Game>(x => x.id == game.id, update).Wait();
                return game;
            }
            else
            {
                return this.InsertGame(createNewGame(player));
            }
        }


        public List<Game> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return games.Find<Game>(queryDoc).ToList();
        }

        public Game gameGenerate()
        {
            var numPlayers = new Bogus.DataSets.Commerce().Random.Int(1, 3);
            var numMovements = new Bogus.DataSets.Commerce().Random.Int(1, 5);
            var name = new Bogus.DataSets.Name();
            var word = new Bogus.DataSets.Hacker();
            var players = new List<Player>();
            for (var i = 0; i < numPlayers; i++)
            {
                var longitude = new Bogus.DataSets.Address().Longitude();
                var latitude = new Bogus.DataSets.Address().Latitude();
                var movements = new List<Movement>();
                for (var j = 0; j < numMovements; j++)
                {
                    movements.Add(new Movement() { playedAt = DateTime.UtcNow, word = word.Noun() });
                }
                players.Add(new Player() { username = name.FirstName(), joinedAt = DateTime.UtcNow, movements = movements, longitude = longitude, latitude = latitude });
            }
            return new Game() { owner = name.FirstName(), maxPlayers = 3, type = "TEST", createdAt = DateTime.UtcNow, players = players, state = GameStates.Open };
        }
        public Game createNewGame(Player player, int maxPlayers = 1) 
        {
            var players = new List<Player>();
            players.Add(player);
            return new Game() {owner = player.username, maxPlayers = maxPlayers, type = "TEST", createdAt = DateTime.UtcNow, players = players, state = GameStates.Open};
        }

    }
}
