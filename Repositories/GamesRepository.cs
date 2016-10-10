using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using webapi.Models;

namespace webapi.Repositories
{
    public class GamesRepository
    {
        private IDBContext dbContext;

        public GamesRepository(IDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public GameModel InsertGame(GameModel game)
        {
            var a = this.dbContext.Games.InsertOneAsync(game);
            a.Wait();
            //return this.Get(game._id.ToString());
            return game;
        }
        public GameModel Get(string id)
        {
            var a =  this.dbContext.Games.FindAsync<GameModel>(x => x.id == new ObjectId(id));
            a.Wait();
            return a.Result.SingleOrDefault();
        }

        public IEnumerable<GameModel> Get()
        {
            return this.dbContext.Games.Find(x => true).Limit(3).ToListAsync().Result;
        }


    }
}
